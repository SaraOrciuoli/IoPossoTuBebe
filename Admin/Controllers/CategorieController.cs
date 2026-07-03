using Admin.Models.CustomModels;
using DAL.Model;
using DAL.Repository;
using DevExpress.DataAccess.Native.Json;
using DevExpress.DataAccess.Native.Web;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using System.Xml.Linq;
using JObject = Newtonsoft.Json.Linq.JObject;

namespace Admin.Controllers
{
    public class CategorieController : Controller
    {
        public ActionResult GetCategorie(DataSourceLoadOptions loadOptions)
        {
            CategorieRepository repository = new CategorieRepository();
            var result = DataSourceLoader.Load(repository.getCategorieAdmin(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult GetSottocategorie(DataSourceLoadOptions loadOptions)
        {
            CategorieRepository repository = new CategorieRepository();
            var richionaze = repository.getSottocategorie();
            var result = DataSourceLoader.Load(repository.getSottocategorie(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        
        public ActionResult IndexCategorie()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditCategorie model = new ModelEditCategorie();
                model.Count = CategorieRepository.CountCategorie();
                model.Ita = true;
                model.Eng = false;
                if (Session["SessioneLingua"] == null)
                {
                    Session["SessioneLingua"] = "Ita";
                }
                if (Session["SessioneLingua"].ToString() == "Ita")
                {
                    model.Ita = true;
                    model.Eng = false;
                }
                else
                {
                    model.Ita = false;
                    model.Eng = true;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult EditCategoria(string id)
        {
            Session["Categoria"] = null;

            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditCategorie model = new ModelEditCategorie();
                CategorieRepository repository = new CategorieRepository();
                model.categoria = new Categorie();
                model.ListaImmagini_CatProd = new List<Immagini_catProd>();
                model.ListaSottoCategorie = repository.getSottocategorie();
                model.SottoCategorie_ids = new List<int>();
                Session["Categoria"] = id;
                if (reqCookies["IdRuolo"] == "2001")
                {
                    return View("EditCategoria", model);
                }
                //campi per modifica di categoria esistente
                if (!String.IsNullOrEmpty(id))
                {
                    model.categoria = repository.getCategoriaById(id);
                    model.ListaImmagini_CatProd = (List<Immagini_catProd>)repository.getImgCategoryByIdCat(Convert.ToInt32(id));
                    model.SottoCategorie_ids = repository.getSottoCategorieIdsByIdCat(Convert.ToInt32(id));
                }
                return View("EditCategoria", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult IndexSottocategorie()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                Sottocategorie model = new Sottocategorie();
                model.listaTipi_Immagini = new CategorieRepository().GetTipoImgCat();
                return View(model);
            } else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult AddImgSottocat(int id_sottocat, string values)
        {
            CategorieRepository repository = new CategorieRepository();
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(values);
            List<Immagini_catProd> ListaImmaginiSession = new List<Immagini_catProd>();
            Immagini_catProd img_CatProd = repository.getImgCatProdById(Convert.ToInt32(data.id_immagini));
            if (img_CatProd == null)
            {
                img_CatProd = new Immagini_catProd();
            }
            if (data.id_immagini != null) img_CatProd.id_immagini = data.id_immagini;
            if (data.Descrizione != null) img_CatProd.Descrizione = data.Descrizione;
            if (data.id_tipo_immagine != null) img_CatProd.id_tipo_immagine = data.id_tipo_immagine;
            if (data.path != null) img_CatProd.path = data.path;
            if (data.Alt != null) img_CatProd.Alt = data.Alt;
            if (data.Link != null) img_CatProd.Link = data.Link;


            if (id_sottocat != 0)
            {
                //salvataggio su DB delle immagini prodotto

                Sottocategorie sottoCat = repository.getSottocategoriaById(id_sottocat);

                if (sottoCat != null)
                {
                    img_CatProd.id_sottocategoria = sottoCat.id_sottocategoria;
                    if (img_CatProd.id_immagini == 0)
                    {
                        repository.addImgSottocat(img_CatProd);
                    }
                    else
                    {
                        repository.updateImgCatSottocat(img_CatProd);
                    }
                }
            }
            else
            {
                // salvataggio in sessione delle immagini categoria
                ListaImmaginiSession = Session["ImgsSubCat"] == null ? new List<Immagini_catProd>() : Session["ImgsSubCat"] as List<Immagini_catProd>;
                Immagini_catProd el = ListaImmaginiSession.Where(x => x.path == img_CatProd.path).FirstOrDefault();
                if (el != null)
                {
                    ListaImmaginiSession.Remove(el);
                }
                ListaImmaginiSession.Add(img_CatProd);
                Session["ImgsSubCat"] = ListaImmaginiSession;
            }
            return Content(data.path.ToString());
        }

        [HttpPost]
        public string UploadImgSottocat()
        {
            string retvalue = string.Empty;
            var myFile = Request.Files["myFile2"];
            var targetLocation = Server.MapPath("/Content/ImgSottocat/");
            var path = string.Empty;
            string new_fileName = string.Empty;
            try
            {
                string img_name = myFile.FileName.Replace(" ", "_").Split('.')[0];
                string img_format = myFile.ContentType.Split('/')[1];

                new_fileName = img_name + "_" + (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString().Replace(",", "_") + "." + img_format;
                path = Path.Combine(targetLocation, new_fileName);
                myFile.SaveAs(path);
            }
            catch
            {
                Response.StatusCode = 400;
            }

            retvalue = Path.Combine("/Content/ImgSottocat/", new_fileName);

            return retvalue;
        }

        public void SaveSessionSottocatImgs(int id_sottocat)
        {
            CategorieRepository repo = new CategorieRepository();
            List<Immagini_catProd> immagini_prod = (List<Immagini_catProd>)Session["ImgsProd"];
            if (immagini_prod == null) immagini_prod = new List<Immagini_catProd>();
            foreach (Immagini_catProd img in immagini_prod)
            {
                img.id_prod = id_sottocat;
                repo.addImgSottocat(img);
            }
            Session["ImgsProd"] = null;
        }
        public ActionResult DeleteCategoria(int key)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                SliderRepository sl_repo = new SliderRepository();
                CategorieRepository repository = new CategorieRepository();
                Categorie cat = repository.getCategoriaById(key.ToString());
                
                if(cat != null)
                {
                    foreach (Immagini_catProd img in repository.getImgCategoryByIdCat(key))
                    {
                        repository.deleteImgCategory(img);
                    }
                    foreach(Slider sl in sl_repo.getSliderByCat(key))
                    {
                        sl_repo.deleteSlider(sl.id_immagine);
                    }
                }
                
                repository.deleteCategoria(cat);
                return RedirectToAction("IndexCategorie");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult SalvaCategoria(string key, string values)
        {
            CategorieRepository repository = new CategorieRepository();
            dynamic data = JObject.Parse(values);
            Categorie cat = repository.getCategoriaById(key);
            if(cat == null)
            {
                cat = new Categorie();
            }
            cat.Servizio = data.servizio ?? false;
            cat.MostraWeb = data.web ?? false;
            cat.Descrizione = data.descrizione_ita;
            cat.Descrizione_eng = data.descrizione_eng;
            cat.posizione = data.posizione;
            cat.PathIcona = data.immagine;
            cat.Colore_categoria = data.colore;
            cat.eliminato = false;
            cat.is_collezione = false;
            int id_categoria = cat.id_categorie;
            try
            {
                if(cat.id_categorie == 0)
                {
                    id_categoria = repository.AddCategoria(cat);
                    this.SaveSessionCatImgs(id_categoria);
                } else
                {
                    repository.updateCategorie(cat);
                    repository.deleteCatSubCat(cat.id_categorie);
                }
                foreach (int id in data.sottocategorie)
                {
                    Cat_SubCat cat_SubCat = new Cat_SubCat();
                    cat_SubCat.id_categoria = id_categoria;
                    cat_SubCat.id_sottocategoria = id;
                    repository.AddCat_SubCat(cat_SubCat);
                }
                return Json("Salvataggio completato con successo!");
            } catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }


        public void SaveSessionCatImgs(int id_cat) 
        {
            CategorieRepository repo = new CategorieRepository();
            List<Immagini_catProd> immagini_cat = (List<Immagini_catProd>)Session["Imgs"]; 
            if(immagini_cat != null)
            {
                foreach(Immagini_catProd img in immagini_cat)
                {
                    img.id_cat = id_cat;
                    repo.addImgCategory(img);
                }
                Session["Imgs"] = null;
            }
            
        }



        public ActionResult AddImgCategory(string values)
        {
            CategorieRepository repository = new CategorieRepository();
            Immagini_catProd img_CatProd = new Immagini_catProd();
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(values);
            if (data.id_immagini != null) img_CatProd.id_immagini = data.id_immagini;
            if (data.Descrizione != null) img_CatProd.Descrizione = data.Descrizione;
            if (data.id_tipo_immagine != null) img_CatProd.id_tipo_immagine = data.id_tipo_immagine;
            if (data.path != null) img_CatProd.path = data.path;
            if (data.Alt != null) img_CatProd.Alt = data.Alt;
            if (data.Link != null) img_CatProd.Link = data.Link;
            img_CatProd.id_cat = data.id_categoria;
            string id_cat = Session["Categoria"] != null ? Session["Categoria"].ToString() : data.id_categoria;
            
            if (Convert.ToInt32(id_cat) != 0)
            {
                //salvataggio su DB delle immagini categoria
                Categorie cat = repository.getCategoriaById(id_cat);
                if (cat != null)
                {
                    img_CatProd.id_cat = cat.id_categorie;
                    repository.addImgCategory(img_CatProd);
                }
            }
            else
            {
                // salvataggio in sessione delle immagini categoria
                List<Immagini_catProd> ListaImmaginiSession = Session["Imgs"] as List<Immagini_catProd> ?? new List<Immagini_catProd>();
                ListaImmaginiSession.Add(img_CatProd);
                Session["Imgs"] = ListaImmaginiSession;
            }
            return Content("OK");
        }


        public ActionResult GetImgCategoryByIdCat(string idCat, DataSourceLoadOptions loadOptions)
        {
            CategorieRepository repository = new CategorieRepository();
            LoadResult result = new LoadResult();
            
            if(!String.IsNullOrEmpty(idCat) && Convert.ToInt32(idCat) != 0)
            {
                result = DataSourceLoader.Load(repository.getImgCategoryByIdCat(Convert.ToInt32(idCat)), loadOptions);
            } else
            {
                List<Immagini_catProd> ListaImmaginiSession = Session["Imgs"] as List<Immagini_catProd> ?? new List<Immagini_catProd>();
                result = DataSourceLoader.Load(ListaImmaginiSession, loadOptions);
            }
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult GetTipoImgCat(DataSourceLoadOptions loadOptions)
        {
            CategorieRepository repository = new CategorieRepository();
            var result = DataSourceLoader.Load(repository.GetTipoImgCat(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult DeleteImgCategory(string id, string path)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                CategorieRepository repository = new CategorieRepository();

                if (!String.IsNullOrEmpty(id) && Convert.ToInt32(id) != 0)
                {
                    repository.deleteImgCategory(repository.getImgCatById(Convert.ToInt32(id)));
                }
                else
                {
                    List<Immagini_catProd> ListaImmaginiSession = Session["Imgs"] as List<Immagini_catProd> ?? new List<Immagini_catProd>();

                    foreach (Immagini_catProd img in ListaImmaginiSession)
                    {
                        if (img.path == path)
                        {
                            ListaImmaginiSession.Remove(img);
                            break;
                        }
                    }
                    Session["Imgs"] = ListaImmaginiSession;
                }
                return Content("OK");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult UpdateSottocategorie(int key, string values)
        {
            CategorieRepository repo = new CategorieRepository();
            dynamic data = JObject.Parse(values);
            Sottocategorie sub = repo.getSottocategoriaById(key);
            if (data.Nome != null) sub.Nome = data.Nome;
            if(data.Nome_eng != null) sub.Nome_eng = data.Nome_eng;
            if (data.Costo_Spedizione != null) sub.Costo_Spedizione = data.Costo_Spedizione;

            repo.UpdateSottocategoria(sub);
            return RedirectToAction("IndexSottocategorie");
        }
        [HttpPost]
        public ActionResult AddSottocategorie(string values)
        {
            CategorieRepository repo = new CategorieRepository();
            Sottocategorie sub = JsonConvert.DeserializeObject<Sottocategorie>(values);
            //dynamic data = JObject.Parse(values);
            //if (data.id_sottocategoria != null) sub.id_sottocategoria = data.id_sottocategoria;
            //if (data.Nome != null) sub.Nome = data.Nome;
            if (sub.Nome_eng == null) sub.Nome_eng = "";
            repo.AddSottocategoria(sub);
            SaveSessionSottocatImgs(sub.id_sottocategoria);
            return RedirectToAction("IndexSottocategorie");
        }
        [HttpPost]
        public ActionResult DeleteSottocategorie(int key) 
        { 
            CategorieRepository repo = new CategorieRepository();
            repo.deleteSottocategoria(repo.getSottocategoriaById(key));
            return RedirectToAction("IndexSottocategorie");
        }
        [HttpPost]
        public string UploadImg()
        {
            string retvalue = string.Empty;
            var myFile = Request.Files["myFile2"];
            var targetLocation = Server.MapPath("/Content/ImgCat/");
            var path = string.Empty;
            string new_fileName=string.Empty;
            try

            {
                string img_name = myFile.FileName.Replace(" ", "_").Split('.')[0];
                string img_format = myFile.ContentType.Split('/')[1];

                new_fileName = img_name + "_" + (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString().Replace(",", "_") + "." + img_format;
                path = Path.Combine(targetLocation, new_fileName);
                myFile.SaveAs(path);
            }
            catch
            {
                Response.StatusCode = 400;
            }

            retvalue = Path.Combine("/Content/ImgCat/", new_fileName);

            return retvalue;
        }

        public ActionResult getImmaginiSottocategoria(int idSubCat, DataSourceLoadOptions options)
        {
            CategorieRepository repo = new CategorieRepository();
            List<Immagini_catProd> response = new List<Immagini_catProd>();
            if(idSubCat != 0)
            {
                response = repo.getImmaginiSubCat(idSubCat);
            } else
            {
                response = Session["ImgsSubCat"] as List<Immagini_catProd> ?? new List<Immagini_catProd>();
            }

            var result = DataSourceLoader.Load(response, options);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        public ActionResult GetImgSubCatByIdSubCat(string idSubCat)
        {
            CategorieRepository repo = new CategorieRepository();
            List<Immagini_catProd> response = new List<Immagini_catProd>();
            if (!String.IsNullOrEmpty(idSubCat) && Convert.ToInt32(idSubCat) != 0)
            {
                response = repo.getImmaginiSubCat(Convert.ToInt32(idSubCat));
            }
            else
            {
                response = Session["ImgsSubCat"] as List<Immagini_catProd> ?? new List<Immagini_catProd>();
            }
            return Json(response);
        }
        [HttpPost]
        public ActionResult DeleteImgSubCat(string id)
        {
            
            int id_img;
            List<Immagini_catProd> ImmaginiSession = new List<Immagini_catProd>();

            DettagliProdottiRepository repository = new DettagliProdottiRepository();

            if (!String.IsNullOrEmpty(id))
            {
                if (int.TryParse(id, out id_img))
                {
                    repository.deleteImgProduct(repository.getImgCatProdById(id_img));
                }
                else 
                {
                    ImmaginiSession = Session["ImgsSubCat"] as List<Immagini_catProd>;
                    foreach (Immagini_catProd img in ImmaginiSession)
                    {
                        if (img.path.Contains(id))
                        {
                            ImmaginiSession.Remove(img);
                            break;
                        }
                    }
                    Session["ImgsSubCat"] = ImmaginiSession;
                }

            }
            return Content("OK");
            
        }

        public ActionResult EmptySessionImgSubCat()
        {
            string result = "OK";
            try
            {
                Session["ImgsSubCat"] = null;
            }catch(Exception ex)
            {
                result = ex.Message;
            }
            return Content(result);
        }

    }

}