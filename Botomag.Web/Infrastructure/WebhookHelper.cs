using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

using TelegramBot.Core.Services.Contracts;
using TelegramBot.Core.Types.ReturnTypes;
using Botomag.BLL.Model;
using Botomag.BLL.Contracts;

namespace Botomag.Web.Infrastructure
{
    /// <summary>
    /// Contains method for working with telegram webhook
    /// </summary>
    public class WebhookHelper
    {
        public static object ProcessMessage(
            Update update, 
            HttpApplicationStateBase applicationState, 
            IBaseBotService botService,
            ISplitDecisionBotService fightService)
        {
            if (update == null)
            {
                throw new ArgumentNullException("update");
            }
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            if (botService == null)
            {
                throw new ArgumentNullException("botService");
            }
            if (fightService == null)
            {
                throw new ArgumentNullException("fightService");
            }

            // Get bot name
            string botName = CacheHelper.GetOrSet<string>(
                CacheKeys.BotName,
                applicationState,
                () => 
                {
                    Response<User> response = botService.getMe();
                    return response.Result.UserName;
                });

            if (update.Message.Text.StartsWith("@" + botName, true, System.Globalization.CultureInfo.InvariantCulture))
            {
                update.Message.Text = update.Message.Text.Substring(("@" + botName).Length);
            }

            decimal factor;

            if (decimal.TryParse(update.Message.Text, out factor) == true)
            {
                IEnumerable<FightModel> fights = fightService.GetFights(factor);
                string result = "По вашему запросу ничего не найдено.";
                if (fights.Count() > 0)
                {
                    StringBuilder strb = new StringBuilder();
                    strb.AppendLine("Результат:");
                    foreach (FightModel fight in fights)
                    {
                        strb.AppendLine(string.Format("<b>Дата:</b> {0} <b>Организация:</b> {1}, <b>Тип:</b> {2}, <b>Бой:</b> {3}", fight.Date, fight.Organization.Title, fight.BetType.Title, fight.Bet));
                    }
                    string link = CacheHelper.GetOrSet<string>(
                        CacheKeys.PartnerLink,
                        applicationState,
                        () => AppConfigHelper.GetValue<string>(AppConfigKeys.PartnerLink, str => str));
                    strb.AppendLine("Вы можете сделать ставку, например, <a href=\"" + link + "\">здесь</a>");
                    result = strb.ToString();
                }
                return new  
                {
                        method = "sendMessage",
                        text = result,
                        chat_id = update.Message.Chat.Id,
                        reply_to_message_id = update.Message.Message_Id,
                        parse_mode = "HTML"
                };
            }
            return new 
            {
                    method = "sendMessage",
                    text = "Не могу обработать запрос, вы должны ввести число.",
                    chat_id = update.Message.Chat.Id,
                    reply_to_message_id = update.Message.Message_Id,
                    parse_mode = "HTML"
            };
        }

        public static async Task<object> ProcessMessageAsync(
            Update update,
            HttpApplicationStateBase applicationState,
            IBaseBotService botService,
            ISplitDecisionBotService fightService)
        {
            if (update == null)
            {
                throw new ArgumentNullException("update");
            }
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            if (botService == null)
            {
                throw new ArgumentNullException("botService");
            }
            if (fightService == null)
            {
                throw new ArgumentNullException("fightService");
            }

            // Get bot name
            string botName = await CacheHelper.GetOrSetAsync<string>(
                CacheKeys.BotName,
                applicationState,
                () =>
                {
                    Response<User> response = botService.getMe();
                    return response.Result.UserName;
                });

            if (update.Message.Text.StartsWith("@" + botName, true, System.Globalization.CultureInfo.InvariantCulture))
            {
                update.Message.Text = update.Message.Text.Substring(("@" + botName).Length);
            }

            decimal factor;

            if (decimal.TryParse(update.Message.Text, out factor) == true)
            {
                IEnumerable<FightModel> fights = await fightService.GetFightsAsync(factor);
                string result = "По вашему запросу ничего не найдено.";
                if (fights.Count() > 0)
                {
                    StringBuilder strb = new StringBuilder();
                    strb.AppendLine("Результат:");
                    foreach (FightModel fight in fights)
                    {
                        strb.AppendLine(string.Format("<b>Дата:</b> {0} <b>Организация:</b> {1}, <b>Тип:</b> {2}, <b>Бой:</b> {3}", fight.Date, fight.Organization.Title, fight.BetType.Title, fight.Bet));
                    }
                    string link = await CacheHelper.GetOrSetAsync<string>(
                        CacheKeys.PartnerLink,
                        applicationState,
                        () => AppConfigHelper.GetValue<string>(AppConfigKeys.PartnerLink, str => str));
                    strb.AppendLine("Вы можете сделать ставку, например, <a href=\"" + link + "\">здесь</a>");
                    result = strb.ToString();
                }
                return new
                {
                    method = "sendMessage",
                    text = result,
                    chat_id = update.Message.Chat.Id,
                    reply_to_message_id = update.Message.Message_Id,
                    parse_mode = "HTML"
                };
            }
            return new
            {
                method = "sendMessage",
                text = "Не могу обработать запрос, вы должны ввести число.",
                chat_id = update.Message.Chat.Id,
                reply_to_message_id = update.Message.Message_Id,
                parse_mode = "HTML"
            };
        }
    }
}