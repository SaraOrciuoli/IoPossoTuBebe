using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CategorieRepository
    {
        public static int CountCategorie()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Categorie.Where(x => x.eliminato != true && x.is_collezione == false).Count();
        }

        public IEnumerable<object> GetCategorie(/*bool showAll*/)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Categorie> result = new List<Categorie>();
            foreach(Categorie el in db.Categorie.Where(x => x.eliminato != true && x.MostraWeb == true && x.is_collezione == false).ToList())
            {
                el.ListaSottocategorie = new List<Sottocategorie>();
                foreach(int id_sub in db.Cat_SubCat.Where(s => s.id_categoria == el.id_categorie).Select(s => s.id_sottocategoria).ToList())
                {
                    Sottocategorie sub = db.Sottocategorie.Where(x => x.id_sottocategoria == id_sub).FirstOrDefault();
                    if(sub != null)
                    {
                        el.ListaSottocategorie.Add(sub);
                    }
                }  
                result.Add(el);
            }
            return result;
        }
        public List<Categorie> getCategorieAdmin()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Categorie.Where(x => x.eliminato != true && x.is_collezione == false).ToList();
        }

        public List<Categorie> GetSottocategorie(int categoriaId)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();

            return db.Categorie.Where(c => c.id_categorie == categoriaId).ToList();
        }

        public List<Sottocategorie> GetSottocategorieByCat(int idCat)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Sottocategorie> sottocategories = new List<Sottocategorie>();
            foreach( Cat_SubCat sot in db.Cat_SubCat.Where(x=> x.id_categoria == idCat).ToList())
            {
                var sottomessototalmente = getSottocategoriaById(sot.id_sottocategoria);
                if (sottomessototalmente != null)
                {
                    sottocategories.Add(sottomessototalmente);
                }
            
            }
            return sottocategories;
        }



        public int GetAssociazioniCatSubcat(int IdSottocat)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Cat_SubCat.Where(x => x.id_sottocategoria == IdSottocat).FirstOrDefault().id_categoria;
        }
        public Immagini_catProd getImgCatProdById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();

            return db.Immagini_catProd.Where(img => img.id_immagini == id).FirstOrDefault();
        }
        public void updateCategorie(Categorie cat)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Entry(cat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public int AddCategoria(Categorie m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            int id_cat = 0;
            try
            {
                db.Categorie.Add(m);
                db.SaveChanges();
                id_cat = m.id_categorie;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return id_cat;
        }



        public void UpdateCategoria(Categorie c)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public void deleteCategoria(Categorie c)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                c.eliminato = true;
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }
        public List<Immagini_catProd> getImgCategoryByIdCat(int idCat)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Immagini_catProd.Where(p => p.id_cat == idCat).ToList();
        }


        public Immagini_catProd GetImmagineSottocategoria(int idSottocategoria)
        {
            using (var context = new AnyeLabelEntities()) // Metti il nome corretto del tuo DbContext
            {
                return context.Immagini_catProd.FirstOrDefault(x => x.id_sottocategoria == idSottocategoria);
            }
        }
        public void deleteImgCategory(Immagini_catProd img)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            /*  var imgCat = db.Immagini_catProd.First(i => i.id_immagini == img);*/
            try
            {
                db.Entry(img).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                string exception2 = dbEx.Message;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }
        public object getImgById(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Immagini_catProd.Where(x => x.id_immagini == key).FirstOrDefault();
        }



        public IEnumerable<object> getImgCategory()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Immagini_catProd.ToList();
        }

        public void updateImgCategory(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var img = db.Immagini_catProd.Where(c => c.id_immagini == key).FirstOrDefault(); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, img);
            try
            {
                db.Entry(img).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public void addImgCategory(Immagini_catProd img_CatProd)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {

                db.Immagini_catProd.Add(img_CatProd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public Categorie getCategoriaById(string id)
        {
            int idcat = Convert.ToInt32(id);
            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                return db.Categorie.FirstOrDefault(p => p.id_categorie == idcat && p.eliminato == false);
            }
        }

        public List<Tipi_Immagini> GetTipoImgCat()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Tipi_Immagini.Where(t => t.Categoria == true).ToList();
        }

        public Immagini_catProd getImgCatById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Immagini_catProd.Where(img => img.id_immagini == id).FirstOrDefault();
        }

        public Categorie getCategoriaByNome(string nome)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Categorie.Where(x => x.Descrizione == nome).FirstOrDefault();
        }

        public List<Sottocategorie> getSottocategorie()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Sottocategorie.ToList();
        }

        public Sottocategorie getSottocategoriaById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Sottocategorie.Where(x => x.id_sottocategoria == id).FirstOrDefault();
        }
        public void addImgSottocat(Immagini_catProd img_CatProd)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();

            try
            {
                db.Immagini_catProd.Add(img_CatProd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void updateImgCatSottocat(Immagini_catProd el)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();

            db.Entry(el).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void AddSottocategoria(Sottocategorie m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Sottocategorie.Add(m);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }



        public void UpdateSottocategoria(Sottocategorie c)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public void deleteSottocategoria(Sottocategorie c)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Entry(c).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        public void deleteCatSubCat(int id) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            foreach(Cat_SubCat s in db.Cat_SubCat.Where(x => x.id_categoria == id).ToList())
            {
                try
                {
                    db.Entry(s).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
            }
        }
        public List<int> getSottoCategorieIdsByIdCat(int id) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<int> result = new List<int>();
            foreach(Cat_SubCat s in db.Cat_SubCat.Where(x => x.id_categoria == id).ToList())
            {
                result.Add(s.id_sottocategoria);
            }
            return result;
        }
        public void AddCat_SubCat(Cat_SubCat s) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Cat_SubCat.Add(s);
                db.SaveChanges();
            } catch(Exception e)
            {
                string ex = e.Message;
            }
        }

        public List<Sottocategorie> getSottocategorieByIdCat(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Sottocategorie> result = new List<Sottocategorie>();
            
            foreach(Cat_SubCat s in db.Cat_SubCat.Where(x => x.id_categoria == id).ToList())
            {
                result.Add(db.Sottocategorie.Where(x => x.id_sottocategoria == s.id_sottocategoria).FirstOrDefault());
            }
            
            return result;
        }

        public List<Immagini_catProd> getImmaginiSubCat(int id) {
            AnyeLabelEntities db = new AnyeLabelEntities();

            return db.Immagini_catProd.Where(x => x.id_sottocategoria == id).ToList();
        }

        


    }
}
