using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vetrina.Pos.Classes
{

    public class PrintConfig
    {

        public POS pos { get; set; }
        public SellerInfo sellerInfo { get; set; }
        public IEnumerable<PosItem> items { get; set; }
        public Barcode Barcode { get; set; }
        public Client client { get; set; }
        public string Courtesy_message { get; set; }
        public enum payment_type { CASH, CARD, ASSIGN }
        public payment_type payment { get; set; }


    }
}