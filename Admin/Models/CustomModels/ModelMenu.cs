using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModel
{

    public class ModelMenu    {
        public List<Categorie> listaCategorie { get; set; }
        public List<Prodotti> listaProdotti { get; set; }
        public Ordine Ordine { get; set; }
        public string importo { get; set; }
        public string MetodoPagamento { get; set; }
        public string Categorie { get; set; }
        public string Utente { get; set; }
    }
}