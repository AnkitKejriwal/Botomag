using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text;

using Botomag.BLL.Contracts;
using Botomag.BLL.Model;
using Botomag.Web.Infrastructure;
using TelegramBot.Core.Services.Contracts;
using TelegramBot.Core.Types.ReturnTypes;

namespace Botomag.Web.Controllers
{

    public class HomeController : BaseController
    {

        #region Properties and Fields

        IBaseBotService _botService;
        ISplitDecisionBotService _splitDecisionService;
        IMailService _mailService;

        #endregion Properties and Fields

        #region Constructors

        public HomeController(
            IBaseBotService botService, 
            ISplitDecisionBotService splitDecisionService, 
            IMailService mailService)
        {
            _botService = botService;
            _splitDecisionService = splitDecisionService;
            _mailService = mailService;
        }

        #endregion Constructors

        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Processing updates from telegram bot API webhook
        /// </summary>
        /// <param name="id">Id is token which was set when setup webhook</param>
        /// <returns>Json result with message response</returns>
        //[RequireHttps]
        [HttpPost]
        public async Task<ActionResult> PostMessages(Guid? id = null)
        {
            if (id.HasValue)
            {
                Guid telegramPostToken = await CacheHelper.GetOrSetAsync<Guid>(
                    CacheKeys.TelegramPostToken,
                    HttpContext.Application,
                    () => AppConfigHelper.GetValue<Guid>(AppConfigKeys.TelegramPostToken, str => Guid.Parse(str)));

                if (id == telegramPostToken)
                {
                    Update update = await _botService.ReadMessageAsync<Update>(Request.InputStream);
                    object result = await WebhookHelper.ProcessMessageAsync(update, HttpContext.Application, _botService, _splitDecisionService);
                    if (result != null)
                    {
                        return new JsonResult { Data = result };
                    }
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(string email, string message, string recipient)
        {
            await _mailService.SendMessageAsync(recipient, message, email);
            return new JsonResult();
        }

        #endregion Methods
    }
}