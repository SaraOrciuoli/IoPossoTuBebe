using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Admin.Classi
{
   

    public static class UtilDoc
    {
        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),"-",
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

        public static bool IsMedico(int Tipo)
        {
            bool retvalue = false;
            if (Tipo == 2) retvalue = true;
            return retvalue;
        }

        public static bool IsCorso(int Tipo)
        {
            bool retvalue = false;
            if (Tipo == 1) retvalue = true;
            return retvalue;
        }
    }

    public class Configuration : DbConfiguration
    {
        public Configuration()
        {
            //var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

            //AddInterceptor(transactionHandler);

            //var cachingPolicy = new CachingPolicy();

            //Loaded +=
            //  (sender, args) => args.ReplaceService<DbProviderServices>(
            //    (s, _) => new CachingProviderServices(s, transactionHandler,
            //      cachingPolicy));
        }

        public static class UtilString
        {
            public static string CleanString(string start)
            {
                string retvalue = string.Empty;
                retvalue = start.Replace('/', ' ');

                return retvalue;
            }
        }

        public class EnumRuoli
        {
            public enum Ruoli
            {
                Admin = 1,
                Cliente = 2,
                Cameriere = 3,
                Test = 2001
            }
        }

        public static class GenericUtil
        {
            public static string RandomPassword(int size = 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                return builder.ToString();
            }

            public static string RandomString(int size, bool lowerCase)
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }

            public static int RandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }

            public class EnumTipoVendita
            {
                public enum TipoVendita
                {
                    Cassa = 1,
                    Camerieri = 2,
                    Ritiro = 3,
                    Cosegna = 4,
                    Tavolo = 5,
                    Ombrelloni = 6

                }
            }
        }
    }
}