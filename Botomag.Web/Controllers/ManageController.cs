using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AutoMapper;

using Botomag.BLL.Contracts;

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

        public async Task<ActionResult> Index()
        {
            if (!UserId.HasValue)
            {
                return RedirectToAction("Error", "Base");
            }
            IEnumerable<string> botNames = null;
            try
            {
                botNames = await _botService.GetBotNamesByUserIdAsync(UserId.Value);
            }
            catch
            {
                return RedirectToAction("Error", "Base");
            }
            return View(botNames);
        }

        #endregion Public Methods
    }
}