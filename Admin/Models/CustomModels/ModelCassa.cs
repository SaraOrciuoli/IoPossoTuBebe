using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelCassa
    {
        public List<Categorie> listaCategorie { get; set; }

        public List<Prodotti> listaProdotti { get; set; }

        public List<Ordine> listaOrdini { get; set; }

        public List<DettaglioProdottiTaglie> listaProdTaglie { get; set; }

        public string  importo { get; set; }
        public string importoCliente { get; set; }
        public string resto { get; set; }

        public string Operatore { get; set; }

        public string Categorie { get; set; }

        public string end_point { get; set; }

        public ModelEditUtente ModelEditUtente { get; set; }
      

    }
}