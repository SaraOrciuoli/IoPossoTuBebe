namespace ServizioStampaAdmin.FiscalPrinter
{

    public class Client
    {

        public string name { get; set; }

        // ean13 = code 1 ; code39 con check digit = code 3,code39 = code 2; code128 = code 120 ; qrcode = code 158 ;
        public string fiscal_code { get; set; }


    }
}