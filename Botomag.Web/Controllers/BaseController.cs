using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace Botomag.Web.Controllers
{
    /// <summary>
    /// Base controller for all controllers in application
    /// </summary>
    public class BaseController : Controller
    {

        #region Properties and Fields

        protected IAuthenticationManager AuthManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        #endregion Properties and Fields

        #region Constructors

        #endregion Constructors

        #region Methods

        #endregion Methods
    }
}