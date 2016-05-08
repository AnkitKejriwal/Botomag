using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using AutoMapper;
using System.Security.Claims;

namespace Botomag.Web.Controllers
{
    /// <summary>
    /// Base controller for all controllers in application
    /// </summary>
    public class BaseController : Controller
    {

        #region Properties and Fields

        protected IMapper _mapper { get; private set; }

        private string _userEmail;

        protected string UserEmail
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    if (_userEmail == null)
                    {
                        ClaimsPrincipal claimsPrincipal = HttpContext.User as ClaimsPrincipal;
                        if (claimsPrincipal != null)
                        {
                            Claim claimEmail = claimsPrincipal.FindFirst(ClaimTypes.Email);
                            if (claimEmail != null)
                            {
                                _userEmail = claimEmail.Value;
                            }
                        }
                    }
                    return _userEmail;
                }
                else
                {
                    return null;
                }

            }
            private set { _userEmail = value; }
        }

        private Guid? _userId;

        protected Guid? UserId
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    if (!_userId.HasValue)
                    {
                        ClaimsPrincipal claimsPrincipal = HttpContext.User as ClaimsPrincipal;
                        if (claimsPrincipal != null)
                        {
                            Claim claimIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                            if (claimIdString != null)
                            {
                                Guid userId;
                                if (Guid.TryParse(claimIdString.Value, out userId) == true)
                                {
                                    _userId = userId;
                                }
                            }
                        }
                    }
                    return _userId;
                }
                else
                {
                    return null;
                }
            }
            private set { _userId = value; }
        }

        #endregion Properties and Fields

        #region Constructors

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        #endregion Constructors

        #region Public Methods

        public ActionResult Error()
        {
            return View();
        }

        #endregion Public Methods
    }
}