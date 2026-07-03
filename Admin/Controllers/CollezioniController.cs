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
    public class CollezioniController : Controller
    {
        public ActionResult GetCollezioni(DataSourceLoadOptions loadOptions)
        {
            CollezioniRepository repository = new CollezioniRepository();
            var result = DataSourceLoader.Load(repository.getCollezioniAdmin(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        public ActionResult IndexCollezioni()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditCategorie model = new ModelEditCategorie();
                model.Count = CollezioniRepository.CountCollezioni();
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


        public ActionResult EditCollezione(string id)
        {
            Session["Collezione"] = null;

            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditCategorie model = new ModelEditCategorie();
                CategorieRepository repository = new CategorieRepository();
                model.categoria = new Categorie();
                model.ListaImmagini_CatProd = new List<Immagini_catProd>();

                Session["Collezione"] = id;

                if (reqCookies["IdRuolo"] == "2001")
                {
                    return View("EditCollezione", model);
                }
                if (!String.IsNullOrEmpty(id))
                {
                    model.categoria = repository.getCategoriaById(id);
                    model.ListaImmagini_CatProd = (List<Immagini_catProd>)repository.getImgCategoryByIdCat(Convert.ToInt32(id));
                }
                return View("EditCollezione", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult GetProdottiPerCollezione(DataSourceLoadOptions loadOptions, int id_collezione)
        {
            CollezioniRepository repository = new CollezioniRepository();
            var result = DataSourceLoader.Load(repository.getProdottiPerCollezione(id_collezione), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult AggiungiProdottoACollezione(string values)
        {
            var nuovaAssociazione = new Associazione_Prodotto_Collezione();
            JsonConvert.PopulateObject(values, nuovaAssociazione);

            CollezioniRepository repo = new CollezioniRepository();
            repo.AggiungiProdottoACollezione(nuovaAssociazione);

            return new HttpStatusCodeResult(200);
        }

        [HttpDelete]
        public ActionResult RimuoviProdottoDaCollezione(int key)
        {
            CollezioniRepository repo = new CollezioniRepository();
            repo.RimuoviProdottoDaCollezione(key);

            return new HttpStatusCodeResult(200);
        }
        public ActionResult DeleteCollezione(int key)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                SliderRepository sl_repo = new SliderRepository();
                CategorieRepository repository = new CategorieRepository();
                Categorie cat = repository.getCategoriaById(key.ToString());

                if (cat != null)
                {
                    foreach (Immagini_catProd img in repository.getImgCategoryByIdCat(key))
                    {
                        repository.deleteImgCategory(img);
                    }
                    foreach (Slider sl in sl_repo.getSliderByCat(key))
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


        public void SaveSessionCatImgs(int id_cat)
        {
            CategorieRepository repo = new CategorieRepository();
            List<Immagini_catProd> immagini_cat = (List<Immagini_catProd>)Session["Imgs"];
            if (immagini_cat != null)
            {
                foreach (Immagini_catProd img in immagini_cat)
                {
                    img.id_cat = id_cat;
                    repo.addImgCategory(img);
                }
                Session["Imgs"] = null;
            }

        }

        public ActionResult SalvaCollezione(string key, string values)
        {
            CategorieRepository repository = new CategorieRepository();
            CollezioniRepository collRepo = new CollezioniRepository(); 
            dynamic data = JObject.Parse(values);
            Categorie cat = repository.getCategoriaById(key);
            if (cat == null)
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
            cat.is_collezione = true;
            int id_categoria = cat.id_categorie;
            try
            {
                if (cat.id_categorie == 0)
                {
                    id_categoria = repository.AddCategoria(cat);
                    this.SaveSessionCatImgs(id_categoria);
                }
                else
                {
                    repository.updateCategorie(cat);
                    repository.deleteCatSubCat(cat.id_categorie);
                }
               
                return Json("Salvataggio completato con successo!");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
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
            string id_cat = Session["Collezione"] != null ? Session["Collezione"].ToString() : data.id_categoria;

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

            if (!String.IsNullOrEmpty(idCat) && Convert.ToInt32(idCat) != 0)
            {
                result = DataSourceLoader.Load(repository.getImgCategoryByIdCat(Convert.ToInt32(idCat)), loadOptions);
            }
            else
            {
                List<Immagini_catProd> ListaImmaginiSession = Session["Imgs"] as List<Immagini_catProd> ?? new List<Immagini_catProd>();
                result = DataSourceLoader.Load(ListaImmaginiSession, loadOptions);
            }
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult GetTipoImgColl(DataSourceLoadOptions loadOptions)
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
    }
}