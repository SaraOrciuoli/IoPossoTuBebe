using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Admin.Utility
{
    public class Util
    {
        public static void CheckCulture()
        {
            HttpCookie languageCookie = HttpContext.Current.Request.Cookies["Language"];

            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["Language"]);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["Language"]);
            }



        }

        public static string GeneraPassword(int lunghezza = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var bytes = new byte[lunghezza];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var password = new StringBuilder(lunghezza);
            foreach (var b in bytes)
            {
                password.Append(chars[b % chars.Length]);
            }

            return password.ToString();
        }

    }
}