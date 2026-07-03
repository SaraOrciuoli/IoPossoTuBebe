using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class ModelPagamento
    {
        public List<DettaglioProdottiOrdine> Prodotti { get; set; }
        public Utenti Utente { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public string idUtente { get; set; }
        public List<Comuni> ListaComuniIsole { get; set; }

        public double SpeseSpedizioneTotali { get; set; }


    }
}