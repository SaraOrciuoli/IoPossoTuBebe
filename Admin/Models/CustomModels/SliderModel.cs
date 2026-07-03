using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class SliderModel
    {
        
        public List<Prodotti> Prodotti { get; set; }
        public List<Categorie> Categorie { get; set; }
        public List<Sottocategorie> Sottocategorie { get; set; }
        public List<Prodotti> ProdottiSlider { get; set; }
        public List<Categorie> CategorieSlider { get; set; }
        public List<Prodotti> ProdottiSide { get; set; }
        public List<Categorie> CategorieSide { get; set; }
        public Slider s_p1 { get; set; }
        public Slider s_p2 { get; set; }
        public Slider s_p3 { get; set; }
        public Slider s_p4 { get; set; }
        public List<Slider> main_s_p1 { get; set; }
        public List<string> main_s_p1_paths { get; set; }
        public List<int> main_s_p1_prod_ids { get; set; }
        public List<int> main_s_p1_cat_ids { get; set; }

        public string promotion_msg_ita { get; set; }
        public string promotion_msg_eng { get; set; }
    }

}