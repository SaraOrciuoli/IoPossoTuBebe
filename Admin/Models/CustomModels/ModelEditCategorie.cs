using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelEditCategorie
    {
        public Categorie categoria { get; set; }
        public List<Categorie> ListaCategoria { get; set; }

        public List<Sottocategorie> ListaSottoCategorie { get; set; }

        public List<int> SottoCategorie_ids { get; set; }
        public int Count { get; set; }
        public bool Ita { get; set; }
        public bool Eng { get; set; }
        public Immagini_catProd immagini_CatProd { get; set; }
        public List<Immagini_catProd> ListaImmagini_CatProd { get; set; }
        public List<Tipi_Immagini> ListaTipi_Immagini { 
            get {
                AnyeLabelEntities db = new AnyeLabelEntities();
                return db.Tipi_Immagini.Where(x => x.Categoria == true).ToList();  
            } 
        }
        public List<int> posizioni
        {
            get {
                return new List<int>() { 1, 2, 3, 4, 5, 6};
            }
        }


    }
}