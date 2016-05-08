using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using System.Security.Claims;

[assembly: OwinStartup(typeof(Botomag.AuthConfig))]
namespace Botomag
{
    public class AuthConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions 
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie, 
                LoginPath = new PathString("/Account/Login")
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}