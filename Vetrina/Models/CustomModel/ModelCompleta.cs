using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{

    public class ModelCompleta   {
        public List<DettaglioProdottiOrdine> listaProdotti { get; set; }
        public Ordine Ordine { get; set; }
        public string totale { get; set; }
        public string MetodoPagamento { get; set; }
        public string quantita { get; set; }
        public string Resto { get; set; }
        public string Prodotti { get; set; }
        public bool MostraServizio { get; set; }
        public bool MostraPulsante { get; set; }

        public string MostraBancomat { get; set; }
        public string CostoServizio { get; set; }

        public string minimo { get; set; }
        public string TotaleConServizio { get; set; }

    }
}