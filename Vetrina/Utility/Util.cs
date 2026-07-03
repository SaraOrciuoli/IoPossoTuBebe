using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Vetrina.Utility
{
    public class Util
    {
        public static void CheckCulture()
        {
            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
            }

        }
    }
}