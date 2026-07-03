using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServizioStampaAdmin.FiscalPrinter
{
    public class Barcode
    {

        public string code { get; set; }

        // ean13 = code 1 ; code39 con check digit = code 3,code39 = code 2; code128 = code 120 ; qrcode = code 158 ;
        public BarcodeType type { get; set; }
        public enum BarcodeType { ean13, code39_chk_dgt, code39, code128, qrcode }
    }
}
