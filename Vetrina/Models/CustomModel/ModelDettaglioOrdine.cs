using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class ModelDettaglioOrdine
    {
        public List<ProdottiOrdine> DettagliOrdine { get; set; }
        public ProdottiOrdine ProdottiOrdine { get; set; }
        public List<DettaglioProdottiOrdine> Prodotti { get; set; }
        public Utenti utente { get; set; }
    }
}