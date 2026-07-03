using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelProdotti
    {
        public List<Marche> Marche { get; set; }
        public List<Categorie> Categorie { get; set; }
        public List<Modello> Modelli { get; set; }
        public List<Prodotti> Prodotti { get; set; }
        public Marche Marca { get; set; }
        public Categorie Categoria { get; set; }
        public Modello Modello { get; set; }
        public Prodotti Prodotto { get; set; }

        public bool Ita { get; set; }
        public bool Eng {  get; set; }
        
    }
}