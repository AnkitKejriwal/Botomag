using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using AutoMapper;


using Botomag.Web.Models.Account;
using Botomag.BLL.Contracts;
using Botomag.BLL.Models;

namespace Botomag.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region Properties

        private IUserService _userService;

        #endregion Properties

        private IAuthenticationManager _authManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        #region Constructors

        public AccountController(IUserService userService, IMapper mapper) : base(mapper)
        {
            _userService = userService;
        }

        #endregion Constuctors

        #region Public Methods

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // try to create new user
                UserModel newUser = _mapper.Map<UserModel>(model);
                CreateUserResult result = _userService.CreateUser(newUser);

                if (result.IsSuccess)
                {
                    // sign in user and redirect
                    SignIn(result.User, false);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(model);
                }
            }
            return View(model);
        }

        #endregion Public Methods

        #region Private Methods

        private void SignIn(UserModel user, bool isPersistent)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            _authManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    AllowRefresh = true
                }, identity);
        }

        #endregion Private Methods
    }
}