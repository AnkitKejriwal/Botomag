using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text;

using Botomag.BLL.Contracts;
using Botomag.Web.Infrastructure;

namespace Botomag.Web.Controllers
{

    public class HomeController : BaseController
    {

        #region Properties and Fields

        IMailService _mailService;
        IBotService _botService;

        #endregion Properties and Fields

        #region Constructors

        public HomeController(
            IBotService botService, 
            IMailService mailService)
        {
            _botService = botService;
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
        /// <param name="id">Id is token which was set when setup webhook
        /// it identifies appropriate bot</param>
        /// <returns>Json result with message response</returns>
        //[RequireHttps]
        [HttpPost]
        public void PostMessage(Guid? id = null)
        {
            if (id.HasValue)
            {
                string result = _botService.ProcessUpdate(id.Value, Request.InputStream);
                HttpContext.Response.ContentType = "application/json";
                HttpContext.Response.ContentEncoding = Encoding.UTF8;
                HttpContext.Response.Write(result);
            }
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