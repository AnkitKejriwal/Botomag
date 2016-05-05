using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Cryptography;

using Botomag.Web.Models.Account;

namespace Botomag.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // at this point check 
            }
            return View(model);
        }

        #region Private Methods



        #endregion Private Methods
    }
}