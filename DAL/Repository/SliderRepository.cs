using DAL.Model;
using DAL.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class SliderRepository
    {
        internal AnyeLabelEntities db = new AnyeLabelEntities();
        internal Slider GetImgSliderById(int id)
        {
            return db.Slider.Where(x => x.id_immagine == id).FirstOrDefault();
        }

        internal List<Slider> GetImmaginiSlider()
        {
            return db.Slider.Where(x => x.eliminato != true).ToList();
        }

        public string getPromotionalMessage(string lang)
        {
            string result = "";
            Slider slider = db.Slider.Where(x => x.posizione == 10 && x.slider_num == 1).FirstOrDefault();
            if (slider != null)
            {
                if(lang == "ita") result = slider.Descrizione;
                if (lang == "eng") result = slider.Descrizione_eng;
            }

            return result;
        }
        public void AddSlider(Slider s)
        {
            try
            {
                db.Slider.Add(s);
                db.SaveChanges();
            } catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public void ResetSliderConfiguration(int num, int position)
        {
            foreach(Slider s in db.Slider.Where(x => x.slider_num == num && x.posizione == position).ToList())
            {
                try
                {
                    if(s.type == "cus")
                    {
                        DeleteFile(s.path_img_slider);
                    }

                    db.Entry(s).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                } catch(Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }
        public List<Slider> getSliderByPosition(int slider, int position)
        {
            return db.Slider.Where(x => x.slider_num == slider && x.posizione == position).ToList();
        }

        internal static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }


        //Immagini Slider

        public string GetSliderImgPathFromProdId(int id, int position)
        {
            return db.Immagini_catProd.Where(x => x.id_prod == id && x.id_tipo_immagine == position).FirstOrDefault().path;
        }

        public string GetSliderImgPathFromCatId(int id, int position)
        {
            return db.Immagini_catProd.Where(x => x.id_cat == id && x.id_tipo_immagine == position).FirstOrDefault().path;
        }
         public string GetSliderImgPathFromSottCatId(int id, int position)
        {
            return db.Immagini_catProd.Where(x => x.id_sottocategoria == id && x.id_tipo_immagine == position).FirstOrDefault().path;
        }

        public List<Prodotti> GetProdottiSlider(int position)
        {
            List<Prodotti> result = new List<Prodotti>();
            foreach (Immagini_catProd el in db.Immagini_catProd.Where(x => x.id_tipo_immagine == position && x.id_prod != null).ToList())
            {
                Prodotti p = db.Prodotti.Where(t => t.id_prodotto == el.id_prod).FirstOrDefault();
                if (p != null) result.Add(p);
            }
            return result;
        }

        public List<Categorie> GetCategorieSlider(int position)
        {
            List<Categorie> result = new List<Categorie>();
            foreach (Immagini_catProd el in db.Immagini_catProd.Where(x => x.id_tipo_immagine == position && x.id_cat != null).ToList())
            {
                Categorie c = db.Categorie.Where(t => t.id_categorie == el.id_cat).FirstOrDefault();
                if (c != null) result.Add(c);
            }
            return result;
        }
        public List<Sottocategorie> GetSottocategorieSlider(int position)
        {
            List<Sottocategorie> result = new List<Sottocategorie>();
            foreach (Immagini_catProd el in db.Immagini_catProd.Where(x => x.id_tipo_immagine == position && x.id_sottocategoria != null).ToList())
            {
                Sottocategorie c = db.Sottocategorie.Where(t => t.id_sottocategoria == el.id_sottocategoria).FirstOrDefault();
                if (c != null) result.Add(c);
            }
            return result;
        }

        public void deleteSlider(int id)
        {
            Slider s = db.Slider.Where(x => x.id_immagine == id).FirstOrDefault();
            if (s != null)
            {

                db.Entry(s).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public List<Slider> getSliderByCat(int id_cat)
        {
            return db.Slider.Where(x => x.id_categoria == id_cat).ToList();
        }
    }
}
