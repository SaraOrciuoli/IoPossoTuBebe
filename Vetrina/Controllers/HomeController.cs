using DAL.Repository;
using DAL.Model;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vetrina.Models.CustomModel;
using Vetrina.Utility;
using System.Threading;
using System.Globalization;

namespace Vetrina.Controllers
{
    public class HomeController : Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Util.CheckCulture();
            base.Initialize(requestContext);

            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            CollezioniRepository collRepo = new CollezioniRepository();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.listaCollezioni = (List<Categorie>)collRepo.getCollezioniAdmin();

            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }

            ViewBag.MenuModel = model.MenuModel;
        }
        public void RedirectToCulture(string culture)
        {
            Response.Cookies.Remove("Language");

            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            if (languageCookie == null) languageCookie = new HttpCookie("Language");

            languageCookie.Value = culture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);

            languageCookie.Expires = DateTime.Now.AddDays(10);

            Response.SetCookie(languageCookie);

            Response.Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Index(int? id)
        {
            CombinedViewModel model = new CombinedViewModel();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            CategorieRepository catRepo = new CategorieRepository();
            ProdottiRepository prodRepo = new ProdottiRepository();
            ScontoSpecialeRepository scontoRepo = new ScontoSpecialeRepository();
            CollezioniRepository collRepo = new CollezioniRepository();
            model.Prodotto = prodRepo.GetProdottoById(id);
            model.SliderModel = new ModelSlider();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.listaCollezioni = (List<Categorie>)collRepo.getCollezioniAdmin();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            model.MenuModel.listaProdotti = scontoRepo.CheckOfferta(DateTime.Now);
            model.ListaTestiHome = repo.GetListaTestiHome();
            model.ScontiSpeciali = scontoRepo.GetScontoSpecialeAttivo();
            model.bestSellers = collRepo.GetProdottiByCollezioneWeb(10);

            model.ProdottiPopup = new List<ProdottiPopup>();
            foreach (Prodotti p in prodRepo.GetPopupProdotti())
            {
                Categorie cat = catRepo.getCategoriaById(p.id_categoria.ToString()) ?? new Categorie();
                Sottocategorie sottocat = catRepo.getSottocategoriaById(p.id_sottocategoria ?? 0) ?? new Sottocategorie();
                model.ProdottiPopup.Add(new ProdottiPopup()
                {
                    Prodotto = p,
                    Categorie = cat,
                    Sottocategorie = sottocat
                });
            }


            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }

            return View(model);



        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Servizi()
        {
            CombinedViewModel model = new CombinedViewModel();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            CategorieRepository catRepo = new CategorieRepository();
            ProdottiRepository prodRepo = new ProdottiRepository();
            ScontoSpecialeRepository scontoRepo = new ScontoSpecialeRepository();

            model.SliderModel = new ModelSlider();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            model.MenuModel.listaProdotti = scontoRepo.CheckOfferta(DateTime.Now);
            model.ListaTestiHome = repo.GetListaTestiHome();
            model.ScontiSpeciali = scontoRepo.GetScontoSpecialeAttivo();
            model.ProdottiPopup = new List<ProdottiPopup>();
            foreach (Prodotti p in prodRepo.GetPopupProdotti())
            {
                Categorie cat = catRepo.getCategoriaById(p.id_categoria.ToString()) ?? new Categorie();
                Sottocategorie sottocat = catRepo.getSottocategoriaById(p.id_sottocategoria ?? 0) ?? new Sottocategorie();
                model.ProdottiPopup.Add(new ProdottiPopup()
                {
                    Prodotto = p,
                    Categorie = cat,
                    Sottocategorie = sottocat
                });
            }


            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            return View(model);
        }


        public ActionResult Eventi()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult InviaMail(ModelContatti con)
        //{
        //    SendEmail.SendMail(con.Messaggio, con.Nome, con.Email, con.Telefono);
        //    ViewBag.Message = "Messaggio inviato correttamente!";
        //    return View("Eventi");
        //}

        public ActionResult ListaEventi()
        {
            ModelEventi model = new ModelEventi();
            EventiRepository repository = new EventiRepository();
            model.Lista = (List<Eventi>)repository.GetEventi();

            return View(model);
        }




    }
}