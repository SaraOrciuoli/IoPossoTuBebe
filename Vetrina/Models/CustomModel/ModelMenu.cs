using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{

    public class ModelMenu    {
        public int idCategoria { get; set; }
        public int idSottocategoria { get; set; }
        public List<Categorie> listaCategorie { get; set; }
        public List<Categorie> listaCollezioni { get; set; }

        public List<Sottocategorie> ListaSottocategorie { get; set; }
        public List<Prodotti> listaProdotti { get; set; }
        public List<View_ProdPiuVenduti> listaProdottiPiuVenduti { get; set; }
        public string nomeCategoria { get; set; }
        public string nomeCategoriaEng { get; set; }
        public string sottoCategoriaNome { get; set; }
        public string sottoCategoriaNomeEng { get; set; }
        public Prodotti Prodotto { get; set; }
        public Ordine Ordine { get; set; }
        public string importo { get; set; }
        public string MetodoPagamento { get; set; }
        public string Categorie { get; set; }
        public string Utente { get; set; }
    }
}