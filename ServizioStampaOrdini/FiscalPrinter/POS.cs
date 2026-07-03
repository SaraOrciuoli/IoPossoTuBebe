namespace ServizioStampaAdmin.FiscalPrinter
{

    public class POS
    {

        public string name { get; set; }
        public enum pos_type { EPSON, RCH, DITRONET }
        public string ip { get; set; }
        public bool https { get; set; }
        public pos_type type { get; set; }


    }
}