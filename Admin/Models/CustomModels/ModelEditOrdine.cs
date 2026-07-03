using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelEditOrdine
    {
        public Ordine ordine { get; set; }
        public List<ProdottiOrdine> prodottiOrdine { get; set; }
        public DettagliOrdine DettagliOrdine { get; set; }
        public List<DettaglioProdottiOrdine> DettagliProdottiOrdine { get; set; }
        public List<Stato_Ordine> StatiOrdine { get; set; }
        public List<Stato_Pagamento> StatiPagamento { get; set; }
            public List<Comuni> ListaComuniIsole { get; set; }
        public List<Prodotti> prodotti
        {
            get
            {
                OrdiniRepository repo = new OrdiniRepository();
                return repo.GetProdottiForOrdine();
            }
        }
        public List<Prodotti> servizi { 
            get 
            {
                ProdottiRepository repo = new ProdottiRepository();
                return repo.GetServizi();
            } 
        }
        public List<Taglie> taglie { get; set; }

        public List<Utenti> Clienti { 
            get
            {
                UtentiRepository repo = new UtentiRepository();
                return repo.GetClienti();
            } 
        }
        public List<TipoOrdine> tipi_ordine 
        { 
            get {
                OrdiniRepository repo = new OrdiniRepository();
                return repo.GetTipiOrdine();
            }
        }
        public List<Stato_Ordine> stati_ordine 
        { 
            get {
                OrdiniRepository repo = new OrdiniRepository();
                return repo.GetStatiOrdine();
            }
        }public List<Stato_Pagamento> stati_pagamento
        { 
            get {
                OrdiniRepository repo = new OrdiniRepository();
                return repo.GetStatiPagamento();
            }
        }
        public List<TipoPagamento> tipi_pagamento 
        { 
            get {
                OrdiniRepository repo = new OrdiniRepository();
                return repo.GetTipiPagamento();
            }
        }

    }
}