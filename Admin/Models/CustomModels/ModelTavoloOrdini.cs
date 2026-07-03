using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelTavoloOrdini
    {
        public List<Tavoli> ListaTavoli { get; set; }
        public Tavoli Tavolo { get; set; }
        public GestioneTavoli GestioneTavolo { get; set; }
        public List<DettaglioGestioneTavoli> ListaStorico { get; set; }
        public Ordine Ordine { get; set; }
        public List<TipoPagamento> ListaTipoPagamento { get; set; }
        public string TipoPagamento { get; set; }
        public TavoliOrdini TavoloOrdine { get; set; }
        public List<TabellaOrdiniTavolo> ListaProdottiOrdine { get; set; }
        public List<StatoTavolo> ListaStatoTavolo { get; set; }
        public string IdStatoOrdine { get; set; }
        public List<DettaglioTavoli> ListaGestioneTavolo { get; set; }
        public int Count { get; set; }
        public string end_point { get; set; }

    }
}