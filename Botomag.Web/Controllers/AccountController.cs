using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // try to create new user
                UserModel newUser = _mapper.Map<UserModel>(model);
                CreateUserResult result = await _userService.CreateUserAsync(newUser);

                if (result.IsSuccess)
                {
                    // sign in user and redirect
                    _SignIn(newUser.Id.ToString(), newUser.Email, model.IsPersistent);
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //check user
                UserModel user = _mapper.Map<UserModel>(model);
                VerifyUserResult result = await _userService.IsUserValidAsync(user);
                if (result.IsValid == true)
                {
                    _SignOut();
                    _SignIn(result.User.Id.ToString(), result.User.Email, model.IsPersistent);
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
                    ModelState.AddModelError("", "Вы ввели неверный пароль.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion Public Methods

        #region Private Methods

        private void _SignIn(string id, string email, bool isPersistent)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
            claims.Add(new Claim(ClaimTypes.Email, email));
            ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            _authManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    AllowRefresh = true
                }, identity);
        }

        private void _SignOut()
        {
            _authManager.SignOut();
        }

        #endregion Private Methods
    }
}