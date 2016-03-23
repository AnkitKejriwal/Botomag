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
        ISplitDecisionBotService _fightService;

        #endregion Properties and Fields

        #region Constructors

        public HomeController(IBaseBotService botService, ISplitDecisionBotService fightService)
        {
            _botService = botService;
            _fightService = fightService;
        }

        #endregion Constructors

        #region Methods

        public ActionResult Index()
        {
            return null;
        }

        /// <summary>
        /// Processing updates from telegram bot API webhook
        /// </summary>
        /// <param name="id">Id is token which was set when setup webhook</param>
        /// <returns>Json result with message response</returns>
        [RequireHttps]
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
                    object result = await WebhookHelper.ProcessMessageAsync(update, HttpContext.Application, _botService, _fightService);
                    if (result != null)
                    {
                        return new JsonResult { Data = result };
                    }
                }
            }

            return null;
        }

        #endregion Methods
    }
}