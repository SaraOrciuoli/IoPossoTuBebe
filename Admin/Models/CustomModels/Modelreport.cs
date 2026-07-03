using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class Modelreport
    {
        public DateTime DataSceltaDa { get; set; }
        public DateTime DataSceltaA { get; set; }
        public string valoreIncassato { get; set; }
        public string valoreContanti { get; set; }
        public string valoreCarte { get; set; }
        public string valoreServizi { get; set; }
        public string valoreProdotti { get; set; }


        public List<ProdottiReport> prodottiEsaurimento { get; set; }
        public List<ProdottiReport> prodottiVenduti { get; set; }

        public List<DettaglioReportProdotti> prodottiPiuVenduti { get; set; }

    }
}