using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Botomag.Web.Infrastructure
{
    /// <summary>
    /// Extensions method for UrlHelper class
    /// </summary>
    public static class UrlHelperExtensions
    {
        public static string AbsoluteContent(this UrlHelper urlHelper, string relativePath = null)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException("urlHelper.");
            }
            relativePath = relativePath == null ? "" : relativePath;
            string url = urlHelper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            return string.Format("{0}{1}", url, urlHelper.Content(relativePath));
        }
    }
}