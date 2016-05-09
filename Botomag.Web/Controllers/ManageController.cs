using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;

using Botomag.BLL.Contracts;
using Botomag.BLL.Models;

namespace Botomag.Web.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {

        #region Properties and Fields

        private IBotService _botService { get; set; }

        #endregion Properties and Fields

        #region Constructors

        public ManageController(IMapper mapper, IBotService botService) : base(mapper) 
        {
            _botService = botService;
        }

        #endregion Constructors

        #region Public Methods

        public ActionResult Index()
        {

            return View();
        }

        public async Task<JsonResult> GetBots(int page, int rows)
        {
            if (!UserId.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            IEnumerable<BotModel> bots = null;
            int count;
            try
            {

                bots = await _botService.GetBotsByUserIdAsync(UserId.Value, (page - 1) * rows, rows);
                count = await _botService.GetBotsCountByUserIdAsync(UserId.Value);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult 
            { 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet, 
                Data = new 
                {
                    // current page
                    page = page,
                    // param number of pages
                    total = (int)Math.Ceiling((double)count / rows),
                    // param number of total rows
                    records = count,
                    rows = bots.ToArray()
                } 
            };
        }

        public async Task<JsonResult> GetBotDetails(Guid? id)
        {
            return new JsonResult 
            { 
                Data = new 
                {
                    rows = new object[] { }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion Public Methods
    }
}