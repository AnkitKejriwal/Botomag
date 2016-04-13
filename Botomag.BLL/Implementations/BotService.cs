using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AutoMapper;

using Botomag.BLL.Contracts;
using Botomag.DAL;
using Botomag.DAL.Model;
using TelegramBot.Core.Services.Contracts;
using TelegramBot.Core.Types.ResponseTypes;
using TelegramBot.Core.Types.RequestTypes;

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

        #region Methods

        /// <summary>
        /// Process update belongs to appropriate bot identified by token
        /// </summary>
        /// <param name="token">Unique bot token</param>
        /// <param name="stream">Stream of webhook request</param>
        /// <returns>Object for response</returns>
        public object ProcessUpdate(string token, Stream stream)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream.");
            }

            IRepository<Bot, Guid> botRepo = _unitOfWork.GetRepository<Bot, Guid>();
            Bot bot = botRepo.Get(n => n.Token == token).FirstOrDefault();

            if (bot == null)
            {
                throw new InvalidOperationException(string.Format("bot with token {0} doesn`t exist.", token));
            }

            // Take update object
            UpdateResponse response = _telegramBotService.ReadMessage<UpdateResponse>(stream, token);

            // Search last update with such chat_id in response
            LastUpdate lastUpdate = bot.LastUpates.Where(n => n.ChatId == response.Message.Chat.Id).FirstOrDefault();

            List<Command> commands = new List<Command>();

            int expectedState = BotService.INITSTATE;

            if (lastUpdate == null)
            {
                // There is no conversation for this chat
                // so we search command with current state = 0 - initial state
                commands = bot.Commands.Where(n => n.CurrentState == expectedState).ToList();
            }
            else
            {

                //we have conversation with this user 
                //check if there is repeat
                if (lastUpdate.UpdateId == response.Update_Id)
                {
                    return null;
                }

                //check if there is continuation of last conversation
                if (lastUpdate.UpdateId == response.Update_Id - 1)
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
                throw new InvalidOperationException(string.Format("bot with token {0} has no at least one command with expected state {1}.", token, expectedState));
            }

            // now try parse request and get appropriate response to user
            string splitRegExStr = @"[^\s]+";
            Regex splitRegEx = new Regex(splitRegExStr);
            MatchCollection words = splitRegEx.Matches(response.Message.Text);

            // check if there at least one word
            List<string> wordsList = new List<string>();
            if (words.Count > 0)
            {
                foreach (Match word in words)
                {
                    wordsList.Add(word.Value);
                }

                // get bot name
                Response<UserResponse> botInfo = _telegramBotService.Post<UserResponse>(token, new Request(new GetMeRequest()));
                Regex botNameRegEx = new Regex(@"^@" + botInfo + @"$");

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

            // normalize input
            string normalizeInput = string.Join(" ", wordsList);

            // search match command
            List<Command> matchCommandList = new List<Command>();

            foreach(Command command in commands)
            {
                if (command.CommandType == CommandTypes.Literal)
                {
                    // command must interpretate as literal object
                    if (command.Name == normalizeInput)
                    {
                        matchCommandList.Add(command);
                    }
                }
                if (command.CommandType == CommandTypes.RegularExpression)
                {
                    // command is regular expression
                    Regex commandRegEx = new Regex(command.Name);
                    if (commandRegEx.IsMatch(normalizeInput))
                    {
                        matchCommandList.Add(command);
                    }
                }
            }

            if (matchCommandList.Count == 0)
            {
                return new Request(new SendMessageRequest
                    {
                        Chat_Id = response.Message.Chat.Id,
                        Text = string.Format("Неизвестная команда: {0}", normalizeInput)
                    });
            }

            // we need just one command in list
            if (matchCommandList.Count > 1)
            {
                throw new InvalidOperationException(
                    string.Format("there is ambiguity between commands: {0} of bot with token: {1}",
                    string.Join(", ", matchCommandList.Select(n => n.Name), token)));
            }

            //ok, at this point we have single match!
            // at this point update or create LastUpdate object
            Command matchCommand = matchCommandList.First();
            if (lastUpdate == null)
            {
                lastUpdate = new LastUpdate
                {
                    BotId = bot.Id,
                    ChatId = response.Message.Chat.Id.Value,
                    CurrentState = matchCommand.NextState,
                    UpdateId = response.Update_Id.Value,
                    Id = Guid.NewGuid()
                };
                bot.LastUpates.Add(lastUpdate);
            }
            else
            {
                lastUpdate.CurrentState = matchCommand.NextState;
                lastUpdate.UpdateId = response.Update_Id.Value;
            }
            _unitOfWork.Save();

            //and return response to user!
            return new Request(new SendMessageRequest
                {
                    Chat_Id = response.Message.Chat.Id,
                    Text = matchCommand.Response.Text
                });
        }

        #endregion Methods
    }
}
