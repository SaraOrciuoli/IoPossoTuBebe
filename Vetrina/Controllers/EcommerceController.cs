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
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using System.Windows.Forms;
using DevExpress.XtraRichEdit.Model;
using DevExpress.Utils.Text.Internal;
using DevExpress.PivotGrid.OLAP;
using System.Data.Entity;
using DevExpress.XtraScheduler.Native;

namespace Vetrina.Controllers
{
    public class EcommerceController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Util.CheckCulture();
            base.Initialize(requestContext);

            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
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


            model.Prodotto = prodRepo.GetProdottoById(id);
            model.SliderModel = new ModelSlider();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            model.MenuModel.listaProdotti = scontoRepo.CheckOfferta(DateTime.Now);
            model.ListaTestiHome = repo.GetListaTestiHome();
            model.ScontiSpeciali = scontoRepo.GetScontoSpecialeAttivo();
            model.ProdottiPopup = new List<ProdottiPopup>();
            foreach( Prodotti p in prodRepo.GetPopupProdotti())
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

        public ActionResult Brand()
        {
            CombinedViewModel model = new CombinedViewModel();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            CategorieRepository catRepo = new CategorieRepository();
            ConfigurazioniRepository prodRepo = new ConfigurazioniRepository();


            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            model.ListaMarchi = prodRepo.GetTuttiMarchi();

            return View(model);
        }


        public ActionResult Laboratorio()
        {
            CombinedViewModel model = new CombinedViewModel();
                CategorieRepository catRepo = new CategorieRepository();
                model.MenuModel = new ModelMenu();
                model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
                foreach (var categoria in model.MenuModel.listaCategorie)
                {
                    categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            return View(model);
        }

        public ActionResult Appuntamento()
        {
            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            return View(model);
        }

        public ActionResult MisurazioneVista()
        {
            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            return View(model);
        }
        public ActionResult Carrello()
        {
            List<DettaglioProdottiOrdine> listaProdottiScontrino = Session["ListOrder"] as List<DettaglioProdottiOrdine>;

            if (listaProdottiScontrino == null || !listaProdottiScontrino.Any())
            {
                return View("CarrelloVuoto");
            }

            foreach (var prodotto in listaProdottiScontrino)
            {
                AnyeLabelEntities db = new AnyeLabelEntities();
                Prodotti prodottoDb = db.Prodotti.FirstOrDefault(p => p.id_prodotto == prodotto.id_prodotto);

                if (prodottoDb != null && prodottoDb.sconto.HasValue && prodottoDb.sconto > 0 && prodottoDb.sconto <= 100)
                {
                    prodotto.Sconto = prodottoDb.sconto;

                    double prezzoScontato = prodotto.prezzoUnitario - (prodotto.prezzoUnitario * (prodottoDb.sconto.Value / 100));
                    prodotto.prezzoScontato = (double)prezzoScontato;
                }
                else
                {
                    prodotto.Sconto = 0; 
                    prodotto.prezzoScontato = (double)prodotto.prezzoUnitario;
                }
            }

            return View(listaProdottiScontrino);
        }

        public ActionResult CartPartial()
        {
            // 1. Recupero il carrello dalla Sessione
            List<DAL.Model.DettaglioProdottiOrdine> listaProdottiScontrino = Session["ListOrder"] as List<DettaglioProdottiOrdine>;

            // 2. Se è null o vuoto, creo una lista vuota. 
            // La PartialView "_CartPartial" si occuperà di mostrare la grafica del carrello vuoto.
            if (listaProdottiScontrino == null || !listaProdottiScontrino.Any())
            {
                return PartialView("_CartPartial", new List<DAL.Model.DettaglioProdottiOrdine>());
            }

            // 3. Istanzio il Repository (Niente DbContext nel controller!)
            ProdottiRepository prodRepo = new ProdottiRepository();

            // 4. Ricalcolo i prezzi e gli sconti in tempo reale
            foreach (var prodotto in listaProdottiScontrino)
            {
                // Recupero il prodotto aggiornato dal DB usando il metodo del tuo Repository
                Prodotti prodottoDb = prodRepo.GetProdottoById(prodotto.id_prodotto);

                // Se il prodotto esiste e ha uno sconto valido in questo momento
                if (prodottoDb != null && prodottoDb.sconto.HasValue && prodottoDb.sconto > 0 && prodottoDb.sconto <= 100)
                {
                    prodotto.Sconto = prodottoDb.sconto;
                    double prezzoScontato = prodotto.prezzoUnitario - (prodotto.prezzoUnitario * (prodottoDb.sconto.Value / 100));
                    prodotto.prezzoScontato = (double)prezzoScontato;
                }
                else
                {
                    // Se lo sconto non c'è più o è scaduto, resetto a prezzo pieno
                    prodotto.Sconto = 0;
                    prodotto.prezzoScontato = (double)prodotto.prezzoUnitario;
                }
            }

            // 5. Restituisco la Partial View con i prezzi aggiornati
            return PartialView("_CartPartial", listaProdottiScontrino);
        }

        public ActionResult CarrelloVuoto()
        {
            return View();
        }
        
        public ActionResult PoliticheDiReso()
        {
            return View();
        }
        public ActionResult GuidaAlleTaglie()
        {
            return View();
        }
         public ActionResult PagamentoSicuro()
        {
            return View();
        }
        
        public ActionResult Consegne()
        {
            return View();
        }
        public ActionResult ThankYouPage()
        {
            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();

            model.MenuModel = new ModelMenu();
            model.ContattiModel = new ModelContatti();

            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteProduct(int idProdottiOrdine)
        {
            ProdottiRepository prodottiOrdineRepo = new ProdottiRepository();
            prodottiOrdineRepo.DeleteProduct(idProdottiOrdine);

            return RedirectToAction("Carrello");
        }
        [HttpPost]
        public ActionResult RemoveProductFromSession(int index)
        {
   
            List<DettaglioProdottiOrdine> carrello = Session["ListOrder"] as List<DettaglioProdottiOrdine>;

            if (carrello != null && index >= 0 && index < carrello.Count)
            {
                carrello.RemoveAt(index);        
                Session["ListOrder"] = carrello;
            }

            return Json(new { success = true });
        }
        public ActionResult AllProducts(int id = 0, string titolo = "All Products", int orderBy = 0)
        {
            CombinedViewModel model = new CombinedViewModel();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            ProdottiRepository prodottiRepo = new ProdottiRepository();

            model.MenuModel = ViewBag.MenuModel as ModelMenu;

            // Ottieni tutti i prodotti invece di filtrarli per categoria
            model.MenuModel.listaProdotti = (List<Prodotti>)prodottiRepo.GetAllProdotti();

            // Applica l'ordinamento
            switch (orderBy)
            {
                case 1:
                    model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderByDescending(p => p.PrezzoVendita).ToList();
                    break;
                case 2:
                    model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderBy(p => p.PrezzoVendita).ToList();
                    break;
                default:
                    // Ordinamento di default (puoi personalizzarlo)
                    model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderBy(p => p.id_prodotto).ToList();
                    break;
            }

            // Imposta il titolo appropriato
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith("en"))
            {
                model.MenuModel.nomeCategoriaEng = "All Products";
                model.MenuModel.nomeCategoria = "Tutti i Prodotti";
            }
            else
            {
                model.MenuModel.nomeCategoria = "Tutti i Prodotti";
                model.MenuModel.nomeCategoriaEng = "All Products";
            }

            return View(model);
        }


        public ActionResult Categoria(int id, string titolo, int orderBy = 0)
        {
            CombinedViewModel model = new CombinedViewModel();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            ProdottiRepository prodottiRepo = new ProdottiRepository();
            CategorieRepository catRepo = new CategorieRepository();

            // Inizializziamo il MenuModel
            model.MenuModel = new ModelMenu();
            // Recuperiamo TUTTE le categorie per generare i "bottoni" sotto al banner
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();

            // Recuperiamo la categoria corrente
            Categorie categoria = catRepo.getCategoriaById(Convert.ToString(id));

            if (categoria != null)
            {
                model.Categorie = categoria; // Passiamo l'oggetto categoria intero alla View
                model.MenuModel.nomeCategoria = categoria.Descrizione;
                model.MenuModel.idCategoria = id;

                // Recuperiamo i prodotti
                model.MenuModel.listaProdotti = (List<Prodotti>)prodottiRepo.GetProdottiByCat(id);

                switch (orderBy)
                {
                    case 1:
                        model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderByDescending(p => p.PrezzoVendita).ToList();
                        break;
                    case 2:
                        model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderBy(p => p.PrezzoVendita).ToList();
                        break;
                }

                model.MenuModel.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
                ViewBag.ListaSottocategorie = model.MenuModel.ListaSottocategorie;
            }
            else
            {
                return View("Error");
            }

            return View(model);
        }

        public ActionResult Collezione(int id, string titolo, int orderBy = 0)
        {
            CombinedViewModel model = new CombinedViewModel();
            CollezioniRepository collRepo = new CollezioniRepository();

            model.MenuModel = new ModelMenu();

            Categorie collezione = collRepo.GetCollezioneById(id);

            if (collezione != null)
            {
                model.Categorie = collezione;
                model.MenuModel.nomeCategoria = collezione.Descrizione;
                model.MenuModel.idCategoria = id;

                model.MenuModel.listaProdotti = collRepo.GetProdottiByCollezioneWeb(id);

                // 3. Ordinamento in memoria
                switch (orderBy)
                {
                    case 1:
                        model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderByDescending(p => p.PrezzoVendita).ToList();
                        break;
                    case 2:
                        model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderBy(p => p.PrezzoVendita).ToList();
                        break;
                }

                // 4. Carichiamo le altre collezioni per le "Pillole" sotto al banner
                model.MenuModel.listaCategorie = collRepo.GetCollezioniWeb();

                model.MenuModel.ListaSottocategorie = new List<Sottocategorie>();
                ViewBag.ListaSottocategorie = model.MenuModel.ListaSottocategorie;
            }
            else
            {
                return View("Error");
            }

            return View("Categoria", model);
        }

        public ActionResult Sottocategoria(int id, string titolo, int? categoriaId, int orderBy = 0)
        {
            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            ProdottiRepository prodRepo = new ProdottiRepository();

            model.MenuModel = new ModelMenu();
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();

            if (categoriaId.HasValue)
            {
                Categorie categoriaPadre = catRepo.getCategoriaById(categoriaId.Value.ToString());
                Sottocategorie sottocategoria = catRepo.getSottocategoriaById(id);

                if (categoriaPadre != null && sottocategoria != null)
                {
                    model.CategoriaPadre = categoriaPadre;
                    model.Sottocategorie = sottocategoria;

                    model.MenuModel.idSottocategoria = id;
                    model.MenuModel.nomeCategoria = !string.IsNullOrEmpty(sottocategoria.Nome) ? sottocategoria.Nome.Replace(" ", "-") : "";
                    model.MenuModel.nomeCategoriaEng = !string.IsNullOrEmpty(sottocategoria.Nome_eng) ? sottocategoria.Nome_eng.Replace(" ", "-") : "";

                    ViewBag.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoriaPadre.id_categorie);

                    Immagini_catProd imgSottocat = catRepo.GetImmagineSottocategoria(id);

                    // Gestione Immagine Sfondo Hero
                    if (imgSottocat != null && !string.IsNullOrEmpty(imgSottocat.path))
                    {
                        ViewBag.PathSfondoHero = imgSottocat.path;
                    }
                    else
                    {
                        ViewBag.PathSfondoHero = categoriaPadre.PathIcona;
                    }

                    // RECUPERO PRODOTTI
                    var prodottiSottocategoria = prodRepo.GetProdottiBySotCat(id) ?? new List<Prodotti>();

                    model.MenuModel.listaProdotti = prodottiSottocategoria
                                                    .Where(p => p.id_categoria == categoriaPadre.id_categorie)
                                                    .ToList();

                    // Ordinamento
                    switch (orderBy)
                    {
                        case 1:
                            model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderByDescending(p => p.PrezzoVendita).ToList();
                            break;
                        case 2:
                            model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderBy(p => p.PrezzoVendita).ToList();
                            break;
                    }

                    return View(model);
                }
            }

            return HttpNotFound();
        }
        public ActionResult Offerte()
        {
            CombinedViewModel model = new CombinedViewModel();
            ProdottiRepository prodRepo = new ProdottiRepository();
            CategorieRepository catRepo = new CategorieRepository();

            model.MenuModel = new ModelMenu();
            model.MenuModel.listaProdotti = prodRepo.GetProdottiInOfferta();
            model.MenuModel.listaProdotti = prodRepo.GetProdotti();

            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }

            return View(model);
        }

        public ActionResult ProdottiByBrand(int id, string nome, int orderBy = 0)
        {
            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            ProdottiRepository prodRepo = new ProdottiRepository();
            ConfigurazioniRepository confRepo = new ConfigurazioniRepository();

            model.MenuModel = new ModelMenu();

            // 1. Popoliamo la navbar con le categorie e sottocategorie
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }

            // 2. Recuperiamo le info del marchio usando il REPOSITORY
            Marchi marchioSelezionato = confRepo.GetMarchioById(id);

            if (marchioSelezionato != null)
            {
                // Passiamo i dati essenziali alla view per il banner
                ViewBag.NomeBrand = marchioSelezionato.nome_marchio;
                ViewBag.IdBrand = id;
                ViewBag.LogoBrand = marchioSelezionato.path_logo_marchio;
            }
            else
            {
                // Se il brand non esiste, restituiamo 404
                return HttpNotFound();
            }

            model.MenuModel.listaProdotti = prodRepo.GetProdottiByBrand(id) ?? new List<Prodotti>();

            // 4. Gestiamo l'ordinamento
            switch (orderBy)
            {
                case 1:
                    model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderByDescending(p => p.PrezzoVendita).ToList();
                    break;
                case 2:
                    model.MenuModel.listaProdotti = model.MenuModel.listaProdotti.OrderBy(p => p.PrezzoVendita).ToList();
                    break;
            }

            return View(model);
        }


        public ActionResult NuoviProdotti()
        {
            CombinedViewModel model = new CombinedViewModel();
            ProdottiRepository prodRepo = new ProdottiRepository();
            CategorieRepository catRepo = new CategorieRepository();

            model.MenuModel = new ModelMenu();
            model.MenuModel.listaProdotti = prodRepo.GetProdottiInOfferta();
            model.MenuModel.listaProdotti = prodRepo.GetProdotti();

            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }

            return View(model);
        }

        public ActionResult PiuVenduti()
        {
            CombinedViewModel model = new CombinedViewModel();
            ProdottiRepository prodRepo = new ProdottiRepository();
            CategorieRepository catRepo = new CategorieRepository();

            model.MenuModel = new ModelMenu();
            model.MenuModel.listaProdotti = prodRepo.GetProdottiInOfferta();
            model.MenuModel.listaProdotti = prodRepo.GetProdotti();
            model.MenuModel.listaProdottiPiuVenduti = prodRepo.GetProdottipiuVenduti();

            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }

            return View(model);
        }



        public ActionResult Prodotto(int id, string titolo)
        {
            CombinedViewModel model = new CombinedViewModel();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            ProdottiRepository prodottiRepo = new ProdottiRepository();
            CategorieRepository catRepo = new CategorieRepository();
            //Categorie categoria = catRepo.getCategoriaByNome(titolo);
            DettagliProdottiRepository prod_repo = new DettagliProdottiRepository();

            model.MenuModel = new ModelMenu(); 
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie(); 
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            model.MenuModel.listaProdotti = prodottiRepo.GetProdotti();
            model.ListSliderProdotto = prodottiRepo.GetListProdottiSlider();
            model.ContattiModel = new ModelContatti();
            model.SliderProdotto = prodottiRepo.GetProdottiSliderByIdProdotto(id);
            foreach (var cat in model.MenuModel.listaCategorie)
            {
                cat.ListaSottocategorie = catRepo.GetSottocategorieByCat(cat.id_categorie); 
            }

            model.Prodotto = prodottiRepo.GetProdottoById(id);
            model.listaProdottiTaglia = ProdottiRepository.GetListaProdottiTaglia();
            model.listaTaglie = prod_repo.GetUniqueTaglieByProdId(id);
            model.listaColori = prod_repo.GetUniqueColoriByProdId(id);
            model.listaSpessori = prod_repo.GetUniqueSpessoriByProdId(id);

            Categorie categoria = catRepo.getCategoriaById(Convert.ToString(model.Prodotto.id_categoria));
            model.Categorie = categoria;
            model.ListaNomiTaglie = model.listaProdottiTaglia
                .Where(pt => pt.id_prodotto == id)
                .Select(pt => prodottiRepo.GetNomeTagliaById((int)pt.id_taglia))
                .Distinct()
                .ToList();

            return View(model);
        }



        public ActionResult Contatti()
        {
            CombinedViewModel model = new CombinedViewModel();
            CategorieRepository catRepo = new CategorieRepository();
            ProdottiRepository prodRepo = new ProdottiRepository();
            model.MenuModel = new ModelMenu();
            model.ContattiModel = new ModelContatti();
            
            model.MenuModel.listaCategorie = (List<Categorie>)catRepo.GetCategorie();
            model.MenuModel.ListaSottocategorie = catRepo.getSottocategorie();
            model.MenuModel.listaProdotti = prodRepo.GetProdotti();
            foreach (var categoria in model.MenuModel.listaCategorie)
            {
                categoria.ListaSottocategorie = catRepo.GetSottocategorieByCat(categoria.id_categorie);
            }
            return View(model);
        }


        private AnyeLabelEntities db = new AnyeLabelEntities();

        public ActionResult getQuantitaMax(string id_taglia, string id_colore, string id_spessore, string id_prodotto)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            int quantita = 0;
            int listaColori = 0;
            int listaSpessori = 0;
            int listaTaglie = 0;

            int configurazioni = 1;
            ProdottiRepository prodRepo = new ProdottiRepository();

            var prodotto = prodRepo.GetProdottoById(Convert.ToInt32(id_prodotto));
            if(prodotto.id_gruppo_taglie != 0)
            {
                listaTaglie = 1;
            }
            if (prodotto.id_gruppo_spessori != 0)
            {
                listaSpessori = 1;
            }
            if (prodotto.id_gruppo_colori != 0)
            {
                listaColori = 1;
            }


            // Converti i valori, usando 0 se sono null o vuoti
            int tagliaId = string.IsNullOrEmpty(id_taglia) ? 0 : Convert.ToInt32(id_taglia);
            int coloreId = string.IsNullOrEmpty(id_colore) ? 0 : Convert.ToInt32(id_colore);
            int spessoreId = string.IsNullOrEmpty(id_spessore) ? 0 : Convert.ToInt32(id_spessore);
            int prodottoId = Convert.ToInt32(id_prodotto);
            double prezzo = 0.0;
            // Cerca la combinazione specifica
            ProdottoTaglia pt = repo.getProdottoTagliaByParams(tagliaId, coloreId, spessoreId, prodottoId);
            if (pt != null)
            {
                quantita = pt.quantita;
                prezzo = pt.prezzo;
            }
            if(id_taglia == "0" && id_colore == "0" && id_spessore == "0")
            {
                configurazioni = 0;
            }
            return Json(new { quantita = quantita, prezzo = prezzo , configurazioni = configurazioni, listaColori = listaColori, listaSpessori = listaSpessori, listaTaglie = listaTaglie}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InviaEmail(CombinedViewModel model)
        {
            if (ModelState.IsValid)
            {
                SendEmailContattiProdotto.SendMailContatti(model.ContattiModel.Email, model.ContattiModel.Nome, model.ContattiModel.Cognome, model.ContattiModel.Telefono, model.ContattiModel.Messaggio, model.Prodotto.Descrizione);

                TempData["Invio"] = "Email inviata correttamente!";
                return RedirectToAction("ThankYouPage");
            }

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult InviaEmailContatti(CombinedViewModel model)
        {
            if(ModelState.IsValid)
            {
                SendEmailContatti.SendMail(model.ContattiModel.Nome, model.ContattiModel.Cognome, model.ContattiModel.Email, model.ContattiModel.Telefono, model.ContattiModel.Messaggio);
                TempData["Invio"] = "Email inviata correttamente!";

                return RedirectToAction("ThankYouPage");
            }
            return View("Contatti", model);

        }


        public ActionResult VerificaSottocategorie(int categoriaId)
        {
            CategorieRepository catRepo = new CategorieRepository();
            var sottocategorie = catRepo.GetSottocategorie(categoriaId);
            bool haSottocategorie = sottocategorie.Any();

            var sottocategorieSelectList = sottocategorie.Select(sc => new SelectListItem { Value = sc.id_categorie.ToString(), Text = sc.Descrizione });

            return Json(new { haSottocategorie, sottocategorieSelectList }, JsonRequestBehavior.AllowGet);
        }



    }
}