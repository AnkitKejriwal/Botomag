using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;
using System.Data.Entity;

using Botomag.BLL.Contracts;
using Botomag.DAL;
using Botomag.DAL.Model;
using TelegramBot.Core.Services.Contracts;
using TelegramBot.Core.Types.ResponseTypes;
using TelegramBot.Core.Types.RequestTypes;
using Botomag.BLL.Models;

namespace Botomag.BLL.Implementations
{
    /// <summary>
    /// Serve messages to bots and form response to clients
    /// </summary>
    public class BotService : BaseService, IBotService
    {
        #region Properties and Fields

        public static readonly int INITSTATE = 0;

        protected ITelegramBotService _telegramBotService;

        #endregion Properties and Fields

        #region Constructors

        public BotService(IUnitOfWork unitOfWork, IMapper mapper, ITelegramBotService telegramBotService) : base(unitOfWork, mapper) 
        {
            _telegramBotService = telegramBotService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Process update belongs to appropriate bot identified by token
        /// </summary>
        /// <param name="token">Unique bot token</param>
        /// <param name="stream">Stream of webhook request</param>
        /// <returns>Object for response</returns>
        public Task<string> ProcessUpdateAsync(Guid botId, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream.");
            }

            return Task<string>.Factory.StartNew(() => _ProcessUpdate(botId, stream));
        }

        /// <summary>
        /// Get bot of appropriate user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="skip">Number of skipped items in result sequence</param>
        /// <param name="take">Number of taked items in result sequence</param>
        /// <returns>Task which must return result sequence</returns>
        public Task<IEnumerable<BotModel>> GetBotsNamesByUserIdAsync(Guid userId, int? skip = null, int? take = null)
        {
            return Task<IEnumerable<BotModel>>.Factory.StartNew(() => _GetBotsNamesByUserId(userId, skip, take));
        }

        /// <summary>
        /// Get the number of all bots of appropriate user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>number of all bots</returns>
        public Task<int> GetBotsCountByUserIdAsync(Guid userId)
        {
            return Task<int>.Factory.StartNew(() => _GetBotsCountByUserId(userId));
        }

        /// <summary>
        /// Get Bot with appropriate Id
        /// </summary>
        /// <param name="botId">Bot Id</param>
        /// <returns>Bot Model</returns>
        public Task<BotModel> GetBotByIdAsync(Guid botId)
        {
            return Task<BotModel>.Factory.StartNew(() => _GetBotById(botId));
        }

        #endregion Public Methods

        #region Private Methods

        private string _ProcessUpdate(Guid botId, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream.");
            }

            IRepository<Bot, Guid> botRepo = _unitOfWork.GetRepository<Bot, Guid>();

            Bot bot = botRepo.Find(botId);

            if (bot == null)
            {
                throw new InvalidOperationException(string.Format("bot with id {0} doesn`t exist.", botId));
            }

            // Take update object
            UpdateResponse update = _telegramBotService.ReadMessage<UpdateResponse>(stream);

            // Search last update with such chat_id in response
            LastUpdate lastUpdate = bot.LastUpdates.Where(n => n.ChatId == update.Message.Chat.Id).FirstOrDefault();

            List<Command> commands = new List<Command>();

            int expectedState = BotService.INITSTATE;

            if (lastUpdate == null)
            {
                // There is no conversation for this chat
                // so we search command with current state = initial state
                commands = bot.Commands.Where(n => n.CurrentState == expectedState).ToList();
            }
            else
            {

                //we have conversation with this user 
                //check if there is repeat
                if (lastUpdate.UpdateId == update.Update_Id)
                {
                    return null;
                }

                //check if there is continuation of last conversation
                if (lastUpdate.UpdateId == update.Update_Id - 1)
                {
                    //there is the same conversation, continue
                    expectedState = lastUpdate.CurrentState;
                    commands = bot.Commands.Where(n => n.CurrentState == expectedState).ToList();
                }
                else
                {
                    //there is new conversation
                    commands = bot.Commands.Where(n => n.CurrentState == expectedState).ToList();
                }
            }

            if (commands.Count == 0)
            {
                // there is no at least one command with such state
                throw new InvalidOperationException(string.Format("bot with id {0} has no at least one command with expected state {1}.", botId, expectedState));
            }

            // now try parse request and get appropriate response to user
            string splitRegExStr = @"[^\s]+";
            Regex splitRegEx = new Regex(splitRegExStr);
            MatchCollection words = splitRegEx.Matches(update.Message.Text);

            // check if there at least one word
            List<string> wordsList = new List<string>();
            if (words.Count > 0)
            {
                foreach (Match word in words)
                {
                    wordsList.Add(word.Value);
                }

                // get bot name
                _RefreshBotName(ref bot);
                string botName = bot.Name;

                Regex botNameRegEx = new Regex(@"^@" + botName + @"$");

                //if first word is the name of bot, just delete it
                if (botNameRegEx.IsMatch(wordsList[0]))
                {
                    wordsList.RemoveAt(0);
                }
                //if collection is empty just add empty string
                if (wordsList.Count == 0)
                {
                    wordsList.Add(string.Empty);
                }
            }

            // search match command
            Command matchCommand = null;

            foreach (Command command in commands)
            {
                // command must interpretate as literal object
                if (command.Name == wordsList[0])
                {
                    matchCommand = command;
                    break;
                }
            }

            if (matchCommand == null)
            {
                Request output = null;

                if (bot.InvalidCommandResponse == null)
                {
                    output = new Request(new SendMessageRequest
                    {
                        Chat_Id = update.Message.Chat.Id,
                        Text = string.Format("Неизвестная команда: {0}", wordsList[0])
                    });
                }
                else
                {
                    output = new Request(new SendMessageRequest
                    {
                        Chat_Id = update.Message.Chat.Id,
                        Text = bot.InvalidCommandResponse.Text
                    });
                }

                return _telegramBotService.SerializeRequest(output);
            }

            //ok, at this point we have match!
            //we just ignore all commands if there is several ones and get only first
            //at this point update or create LastUpdate object

            //check if there paramaters
            Parameter matchParam = null;
            if (wordsList.Count > 1)
            {
                //normalize parameters
                wordsList.RemoveAt(0);
                string normParams = string.Join(" ", wordsList);

                foreach (Parameter param in matchCommand.Parameters)
                {
                    if (param.Type == ParameterTypes.Literal)
                    {
                        if (param.Expression == normParams)
                        {
                            matchParam = param;
                            break;
                        }
                    }
                    if (param.Type == ParameterTypes.RegularExpression)
                    {
                        Regex regex = new Regex(param.Expression);
                        if (regex.IsMatch(normParams))
                        {
                            matchParam = param;
                            break;
                        }
                    }
                }

                if (matchParam == null)
                {
                    Request output = null;
                    if (matchCommand.InvalidParameterResponse == null)
                    {
                        output = new Request(new SendMessageRequest
                        {
                            Chat_Id = update.Message.Chat.Id,
                            Text = string.Format("Неверно заданы параметры: {0} для команды: {1}", normParams, wordsList[0])
                        });
                    }
                    else
                    {
                        output = new Request(new SendMessageRequest
                        {
                            Chat_Id = update.Message.Chat.Id,
                            Text = matchCommand.InvalidParameterResponse.Text
                        });
                    }
                    return _telegramBotService.SerializeRequest(output);
                }
            }
            else
            {
                matchParam = matchCommand.Parameters.Where(n => string.IsNullOrEmpty(n.Expression)).FirstOrDefault();
                if (matchParam == null)
                {
                    Request output = null;
                    if (matchCommand.InvalidParameterResponse == null)
                    {
                        output = new Request(new SendMessageRequest
                        {
                            Chat_Id = update.Message.Chat.Id,
                            Text = string.Format("Неверно заданы параметры для команды: {0}", wordsList[0])
                        });
                    }
                    else
                    {
                        output = new Request(new SendMessageRequest
                        {
                            Chat_Id = update.Message.Chat.Id,
                            Text = matchCommand.InvalidParameterResponse.Text
                        });
                    }
                    return _telegramBotService.SerializeRequest(output);
                }
            }


            if (lastUpdate == null)
            {
                lastUpdate = new LastUpdate
                {
                    BotId = bot.Id,
                    ChatId = update.Message.Chat.Id.Value,
                    CurrentState = matchCommand.NextState,
                    UpdateId = update.Update_Id.Value,
                    Id = Guid.NewGuid()
                };
                bot.LastUpdates.Add(lastUpdate);
            }
            else
            {
                lastUpdate.CurrentState = matchCommand.NextState;
                lastUpdate.UpdateId = update.Update_Id.Value;
            }
            bot.BotStat.Requests = bot.BotStat.Requests + 1;
            _unitOfWork.Save();

            //and return response to user!
            Request request = new Request(new SendMessageRequest
            {
                Chat_Id = update.Message.Chat.Id,
                Text = matchParam.Response.Text
            });

            return _telegramBotService.SerializeRequest(request);
        }

        private Bot[] _GetBotsByUserId(Guid userId, int? skip = null, int? take = null, bool copy = false)
        {
            IRepository<Bot, Guid> botRepo = _unitOfWork.GetRepository<Bot, Guid>();

            IQueryable<Bot> botsQuery = botRepo.Get().Where(n => n.UserId == userId);

            botsQuery = botsQuery.OrderBy(n => n.Id);

            if (skip.HasValue)
            {
                botsQuery = botsQuery.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                botsQuery = botsQuery.Take(take.Value);
            }
            
            if (copy)
            {
                botsQuery = botsQuery.AsNoTracking();
            }

            Bot[] botsArray = botsQuery.ToArray();

            if (copy)
            {
                botRepo.AttachRange(botsArray);

                _unitOfWork.Save();
            }

            return botsArray;
        }

        private IEnumerable<BotModel> _GetBotsNamesByUserId(Guid userId, int? skip = null, int? take = null)
        {
            Bot[] botsArray = _GetBotsByUserId(userId, skip, take, true);

            for (int i = 0; i < botsArray.Length; ++i)
            {
                botsArray[i].BotStat = null;
                botsArray[i].Commands = null;
                botsArray[i].InvalidCommandResponse = null;
                botsArray[i].LastUpdates = null;
                botsArray[i].User = null;
            }

            BotModel[] botMapped = _mapper.Map<BotModel[]>(botsArray);

            return botMapped; 
        }

        private IEnumerable<BotModel> _GetBotsNamesByUserId(Guid userId)
        {
            IRepository<Bot, Guid> botRepo = _unitOfWork.GetRepository<Bot, Guid>();
            return Get<Bot, BotModel, Guid>(botRepo, n => n.UserId == userId, null, n => n.Name, n => n.Id);

        }

        private void _RefreshBotName(ref Bot bot)
        {
            if (!(bot.LastUpdate.HasValue && DateTime.Now - bot.LastUpdate.Value < TimeSpan.FromHours(3) && !string.IsNullOrEmpty(bot.Name)))
            {
                // refresh bot name every 3 hours
                Response<UserResponse> botInfo = _telegramBotService.Post<UserResponse>(bot.Token, new Request(new GetMeRequest()));
                if (botInfo.Ok == false)
                {
                    throw new InvalidOperationException(string.Format("it seems that bot with id {0} has invalid token {1}", bot.Id, bot.Token));
                }
                bot.Name = botInfo.Result.UserName;
                bot.LastUpdate = DateTime.Now;  
            }
        }

        private int _GetBotsCountByUserId(Guid userId)
        {
            return _unitOfWork.GetRepository<Bot, Guid>().Get().Where(n => n.UserId == userId).Count();
        }

        private BotModel _GetBotById(Guid botId)
        {
            Bot bot = _unitOfWork.GetRepository<Bot, Guid>().Get().Where(n => n.Id == botId).
                Include(n => n.BotStat).
                Include(n => n.Commands.Select(p => p.Parameters.Select(q => q.Response))).
                Include(n => n.InvalidCommandResponse).FirstOrDefault();
            BotModel model = _mapper.Map<BotModel>(bot);
            return model;
        }

        #endregion Private Methods
    }
}
