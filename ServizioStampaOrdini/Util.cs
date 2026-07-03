using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServizioStampaOrdini
{
    class Util
    {
        public List<string> SplitByLength(string str, int maxLength)
        {
            List<string> lista = new List<string>();
            for (int index = 0; index < str.Length; index += maxLength)
            {
                lista.Add( str.Substring(index, Math.Min(maxLength, str.Length - index)));
            }
            return lista;
        }

        public static string GeneraCodiceRandom()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[8];
            var random = new Random();
            string result = string.Empty;

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }
            result = new String(Charsarr);
            return result;
        }
       
    }
}
