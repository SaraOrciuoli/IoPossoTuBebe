using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Vetrina.Models.CustomModel;
using Vetrina.Utility;
using static Vetrina.Classi.Configuration;

namespace Vetrina.Controllers
{
    public class ShopController : Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Util.CheckCulture();
            base.Initialize(requestContext);
        }

        public ActionResult Index(string Ombrellone, string Fila, string Lato)
        {
            TimeSpan OpenService = TimeSpan.Parse(WebConfigurationManager.AppSettings["OpenService"]);
            TimeSpan CloseService = TimeSpan.Parse(WebConfigurationManager.AppSettings["CloseService"]);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (OpenService < now && now < CloseService)
            {
                ShopRepository repository = new ShopRepository();
                ModelShop model = new ModelShop();
                model.Order = new Ordine();
                model.id_Tipo = (int)EnumTipoVendita.TipoVendita.Ombrelloni;
                model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Ombrelloni;
                /*model.Order.Tipo_Vendita = repository.GetNomeVenditaById((int)EnumTipoVendita.TipoVendita.Ombrelloni);*/
                model.idfila = Fila;
                model.Order.id_fila = Convert.ToInt32(Fila);
                if (!String.IsNullOrEmpty(Ombrellone)) model.Order.Numero = Convert.ToInt32(Ombrellone);
                return View(model);
            }
            else
            {
                return View("IndexClose");
            }
        }

        public ActionResult IndexRitira()
        {
            TimeSpan OpenService = TimeSpan.Parse(WebConfigurationManager.AppSettings["OpenService"]);
            TimeSpan CloseService = TimeSpan.Parse(WebConfigurationManager.AppSettings["CloseService"]);
            TimeSpan now = DateTime.Now.TimeOfDay;
            Session["TipoVendita"] = (int)EnumTipoVendita.TipoVendita.Ritiro;
            if (OpenService < now && now < CloseService)
            {
                ShopRepository repository = new ShopRepository();
                ModelShop model = new ModelShop();
                model.Order = new Ordine();
                model.id_Tipo = (int)EnumTipoVendita.TipoVendita.Ritiro;
                model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Ritiro;
                model.Order.Tipo_Vendita = repository.GetNomeVenditaById((int)EnumTipoVendita.TipoVendita.Ritiro);
                model.ListaFascieOrarie = repository.GetFascieOrarie();
                return View(model);
            }
            else
            {
                return View("IndexClose");
            }
        }

        public ActionResult IndexOrdinaCasa()
        {
            TimeSpan OpenService = TimeSpan.Parse(WebConfigurationManager.AppSettings["OpenService"]);
            TimeSpan CloseService = TimeSpan.Parse(WebConfigurationManager.AppSettings["CloseService"]);
            TimeSpan now = DateTime.Now.TimeOfDay;
            Session["TipoVendita"] = (int)EnumTipoVendita.TipoVendita.Cosegna;
            if (OpenService < now && now < CloseService)
            {
                ShopRepository repository = new ShopRepository();
                ModelShop model = new ModelShop();
                model.Order = new Ordine();
                model.id_Tipo = (int)EnumTipoVendita.TipoVendita.Cosegna;
                model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Cosegna;
                model.Order.Tipo_Vendita = repository.GetNomeVenditaById((int)EnumTipoVendita.TipoVendita.Cosegna);
                model.Order.Tipo_Vendita = repository.GetNomeVenditaById(4);
                model.ListaFascieOrarie = repository.GetFascieOrarie();
                return View(model);
            }
            else
            {
                return View("IndexClose");
            }
        }

        public ActionResult IndexTavoli(string Tavolo)
        {
            TimeSpan OpenService = TimeSpan.Parse(WebConfigurationManager.AppSettings["OpenService"]);
            TimeSpan CloseService = TimeSpan.Parse(WebConfigurationManager.AppSettings["CloseService"]);
            TimeSpan now = DateTime.Now.TimeOfDay;
            ShopRepository repository = new ShopRepository();
            Session["TipoVendita"] = (int)EnumTipoVendita.TipoVendita.Tavolo;
            if (OpenService < now && now < CloseService)
            {
                if (!String.IsNullOrEmpty(Tavolo))
                {
                    ModelShop model = new ModelShop();
                    model.Order = new Ordine();
                    model.Order.DataOra = DateTime.Now;
                    model.Order.Tavolo = Convert.ToInt32(Tavolo);
                    model.id_Tipo = (int)EnumTipoVendita.TipoVendita.Tavolo;
                    model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Tavolo;
                    model.Order.Tipo_Vendita = repository.GetNomeVenditaById((int)EnumTipoVendita.TipoVendita.Tavolo);
                    Session["Order"] = model.Order;
                    return RedirectToAction("Menu");
                }
                else
                {
                    ModelShop model = new ModelShop();
                    model.Order = new Ordine();
                    model.Tavolo = Tavolo;
                    model.Order.Tavolo = Convert.ToInt32(Tavolo);
                    model.id_Tipo = (int)EnumTipoVendita.TipoVendita.Tavolo;
                    model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Tavolo;
                    model.Order.Tipo_Vendita = repository.GetNomeVenditaById((int)EnumTipoVendita.TipoVendita.Tavolo);
                    return View(model);
                }
            }
            else
            {
                return View("IndexClose");
            }
        }

        public ActionResult Salva(ModelShop model)
        {

            Session["Order"] = model.Order;
            return RedirectToAction("Menu");
        }

        public ActionResult SalvaTavolo(ModelShop model)
        {
            model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Tavolo;
            Session["Order"] = model.Order;
            return RedirectToAction("Menu");
        }

        public ActionResult SalvaRitiro(ModelShop model)
        {
            model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Ritiro;
            if (!String.IsNullOrEmpty(model.Order.FasciaOraria)) model.Order.FasciaOraria = ShopRepository.GetFasciaOrariaById(model.Order.FasciaOraria);
            Session["Order"] = model.Order;
            return RedirectToAction("Menu");
        }

        public ActionResult SalvaOrdineCasa(ModelShop model)
        {
            model.Order.id_tipoVendita = (int)EnumTipoVendita.TipoVendita.Cosegna;
            if (!String.IsNullOrEmpty(model.Order.FasciaOraria)) model.Order.FasciaOraria = ShopRepository.GetFasciaOrariaById(model.Order.FasciaOraria);
            Session["Order"] = model.Order;
            return RedirectToAction("Menu");
        }
        [HttpPost]
        public ActionResult AddProdotto(string idProd, int quantity, string prezzoAggiornato, int? idTaglia)
        {
            try
            {
                int id = Convert.ToInt32(idProd);
                ProdottiRepository prodottiRepository = new ProdottiRepository();
                DettagliProdottiRepository dettagliProdottiRepository = new DettagliProdottiRepository();
                Prodotti prodotto = prodottiRepository.GetProdottoById(id);

                if (prodotto != null)
                {
                    // === INIZIO MODIFICA: CALCOLO QUANTITA GIA' NEL CARRELLO ===
                    int quantitaGiaNelCarrello = 0;
                    if (Session["ListOrder"] != null)
                    {
                        var listaTemp = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                        var itemEsistente = listaTemp.FirstOrDefault(x => x.id_prodotto == id && x.id_taglia == idTaglia);
                        if (itemEsistente != null)
                        {
                            quantitaGiaNelCarrello = itemEsistente.Quantita;
                        }
                    }
                    int quantitaRichiestaTotale = quantity + quantitaGiaNelCarrello;
                    // === FINE MODIFICA ===

                    if (idTaglia.HasValue && idTaglia.Value > 0)
                    {
                        var variantiProdotto = dettagliProdottiRepository.getProdottoTagliaByIdProdotto(id);
                        var varianteSelezionata = variantiProdotto.FirstOrDefault(v => v.id_taglia == idTaglia.Value);

                        if (varianteSelezionata == null)
                        {
                            return Json(new { success = false, message = "La taglia selezionata non è valida." });
                        }

                        // MODIFICA: Usa quantitaRichiestaTotale invece di quantity per il controllo
                        if (varianteSelezionata.quantita < quantitaRichiestaTotale)
                        {
                            int rimasti = varianteSelezionata.quantita - quantitaGiaNelCarrello;
                            if (rimasti <= 0)
                            {
                                return Json(new { success = false, message = "Hai già raggiunto la quantità massima disponibile per questa taglia." });
                            }
                            else
                            {
                                return Json(new { success = false, message = $"Siamo spiacenti, quantità non sufficiente. Puoi aggiungere massimo altri {rimasti} pezzi." });
                            }
                        }
                    }
                    else
                    {
                        var varianti = dettagliProdottiRepository.getProdottoTagliaByIdProdotto(id);
                        if (varianti != null && varianti.Count > 0)
                        {
                            return Json(new { success = false, message = "Per favore, seleziona una taglia prima di aggiungere al carrello." });
                        }
                    }

                    DettaglioProdottiOrdine item = new DettaglioProdottiOrdine();
                    item.id_prodotto = prodotto.id_prodotto;
                    item.Quantita = quantity;
                    item.prezzoUnitario = (double)prodotto.PrezzoVendita;
                    item.NomeProdotto = prodotto.Descrizione;
                    item.iva = prodotto.iva;
                    item.pathFoto = prodotto.pathFoto;

                    // Gestione sicura dell'id_taglia
                    if (idTaglia.HasValue && idTaglia != 0)
                    {
                        item.id_taglia = idTaglia.Value;
                        item.Taglia = Convert.ToString(dettagliProdottiRepository.getTagliaById(idTaglia.Value).Descrizione_taglia);
                    }

                    item.listaProdottiTaglia = dettagliProdottiRepository.getProdottoTagliaByIdProdotto(id);
                    item.InOfferta = prodotto.in_offerta;

                    List<DettaglioProdottiOrdine> listaProdottiScontrino;
                    if (Session["ListOrder"] == null)
                    {
                        listaProdottiScontrino = new List<DettaglioProdottiOrdine>();
                        Session["ListOrder"] = listaProdottiScontrino;
                    }
                    else
                    {
                        listaProdottiScontrino = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                    }

                    DettaglioProdottiOrdine existingItem = listaProdottiScontrino.FirstOrDefault(x => x.id_prodotto == id && x.id_taglia == item.id_taglia);
                    if (existingItem != null)
                    {
                        existingItem.Quantita += quantity;
                    }
                    else
                    {
                        listaProdottiScontrino.Add(item);
                    }

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Prodotto non trovato." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Si è verificato un errore durante l'aggiunta al carrello." });
            }
        }

        public ActionResult GetMaxQtaValue(int tagliaId, int prodottoId)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            int maxValue = repo.GetMaxQtaValue(tagliaId, prodottoId);
            return Json(maxValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCartItemCount()
        {
            try
            {
                List<DettaglioProdottiOrdine> listaProdottiScontrino = Session["ListOrder"] as List<DettaglioProdottiOrdine>;

                if (listaProdottiScontrino != null)
                {
                    var count = listaProdottiScontrino.Sum(item => item.Quantita);
                    return Content(count.ToString());
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("Errore durante il recupero del conteggio dei prodotti nel carrello.");
            }
        }


        private List<DettaglioProdottiOrdine> GetDettagliProdottiOrdine()
        {
            return new List<DettaglioProdottiOrdine>();
        }

        [HttpGet]
        public string GetProdottoOrdine(string idProd)
        {
            string numeroProd = "0";
            int Prod = Convert.ToInt32(idProd);
            List<DettaglioProdottiOrdine> ListaProdScontrino = new List<DettaglioProdottiOrdine>();
            if (Session["ListOrder"] != null)
            {
                ListaProdScontrino = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                numeroProd = ListaProdScontrino.Where(x => x.id_prodotto == Prod).Select(x => x.Quantita).FirstOrDefault().ToString();
            }
            return numeroProd;

        }
        // GET: Shop
        /* public ActionResult Index(string Ombrellone,string Fila,string Lato)
         {
             ShopRepository repository = new ShopRepository();
             ModelShop model = new ModelShop();
             model.Order = new Ordine();
             return View(model);
         }

         public ActionResult Asporto()
         {
             ModelShop model = new ModelShop();
             model.Order = new Ordine();
             model.Order.TipoOrdine = "Asporto";
             return View(model);
         }
         public ActionResult Servizio()
         {
             ModelShop model = new ModelShop();
             model.Order = new Ordine();
             model.Order.TipoOrdine = "Servizio";
             return View(model);
         }

         public ActionResult Consegna()
         {
             ModelShop model = new ModelShop();
             model.Order = new Ordine();
             model.Order.TipoOrdine = "Consegna";
             return View(model);
         }*/

        /*        public ActionResult Cameriere()
                {
                    ModelShop model = new ModelShop();
                    model.Order = new Ordine();
                    if (Session["#Tavolo"] != null)
                    {
                        model.Order.TipoOrdine = "Ordine Cameriere";
                        model.Order.Tavolo = (int)Session["#Tavolo"];
                    }
                    return RedirectToAction("Salva", model);
                }*//*


        public ActionResult Menu()
        {
            ModelMenu model = new ModelMenu();
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            model.listaCategorie = (List<DAL.Model.Categorie>)repository.GetCategorie();
            model.Categorie = CaricaCategorie(model.listaCategorie);
            return View(model);
        }

        public ActionResult MenuCamerieri()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelMenu model = new ModelMenu();
                ConfigurazioniRepository repository = new ConfigurazioniRepository();
                ModelShop ms = new ModelShop();
                ms.Order = new Ordine();
                if (Session["#Tavolo"] != null)
                {
                    ms.Order.TipoOrdine = "Ordine Cameriere";
                    ms.Order.Tavolo = (int)Session["#Tavolo"];
                    Session["Order"] = ms.Order;
                    ViewBag.IdTavolo = Session["#Tavolo"];
                }
                model.Utente = reqCookies["IdUtente"];
                Session["operatore"] = reqCookies["IdUtente"];
                model.listaCategorie = (List<Categorie>)repository.GetCategorie(*//*false*//*);
                model.Categorie = CaricaCategorie(model.listaCategorie);
                *//* model.Ordine.TipoOrdine = Session["Order"];*//*
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Ordini");
            }
        }

        private string CaricaCategorie(List<Categorie> listaCategorie)
        {
            string retvalue = "[";
            List<string> listCatBloccate =WebConfigurationManager.AppSettings["HideCategory"].ToString().Split(',').ToList();
            foreach (Categorie item in listaCategorie)
            {
                if(!listCatBloccate.Contains(item.id_categorie.ToString()))
                {
                    retvalue = retvalue + "{" + String.Format("id_categorie:'{0}',Descrizione:'{1}',PathIcona:'{2}'", item.id_categorie, item.Descrizione, item.PathIcona) + "},";
                }
            }
            retvalue = retvalue.Substring(0, retvalue.Length - 1) + "]";
            return retvalue;
        }



        public PartialViewResult prodoctView(string id)
        {
            string retvalue = "[";
            int idCat = Convert.ToInt32(id);
            ProdottiRepository prodottiRepository = new ProdottiRepository();
            List<Prodotti> listaProdotti = prodottiRepository.GetProdottiByCat(idCat);
            List<DettaglioProdottiOrdine> ProdottiOrdine = new List<DettaglioProdottiOrdine>();
            if (Session["ListOrder"] != null)
            {
                ProdottiOrdine = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
            }

            retvalue = CalcolaListaProdotti(listaProdotti.Where(x =>x.Quantita >0).ToList(), ProdottiOrdine);

            return PartialView("PartialView_prodotti", retvalue);

        }

        public string CalcolaListaProdotti(List<Prodotti> listaProdotti, List<DettaglioProdottiOrdine> ProdottiOrdine)
        {
            string retvalue = "[";
            foreach (Prodotti item in listaProdotti)
            {
                string Quantita = "0";
                DettaglioProdottiOrdine detOrdine = new DettaglioProdottiOrdine();
                if (ProdottiOrdine != null && ProdottiOrdine.Count > 0) detOrdine = ProdottiOrdine.Where(x => x.id_prodotto == item.id_prodotto).FirstOrDefault();
                if (detOrdine != null && detOrdine.Quantita > 0) Quantita = detOrdine.Quantita.ToString();
                retvalue = retvalue + "{" + String.Format("id_Prod:'{0}',Descrizione:'{1}',Prezzo:'{2}',Quantita:'{3}',PathFoto:'{4}',Ingredienti:'{5}'", item.id_prodotto, item.Descrizione, item.PrezzoVendita.ToString(), Quantita, item.pathFoto, item.Ingredienti) + "},";
            }
            if (retvalue != "[")
            {
                retvalue = retvalue.Substring(0, retvalue.Length - 1) + "]";
            }
            else
            {
                retvalue = retvalue + "]";
            }
            return retvalue;
        }

        public string mostraListaProdotti(List<DettaglioProdottiOrdine> ProdottiOrdine)
        {
            string retvalue = "[";
            foreach (DettaglioProdottiOrdine item in ProdottiOrdine)
            {
                string Quantita = "0";
                DettaglioProdottiOrdine detOrdine = new DettaglioProdottiOrdine();
                if (ProdottiOrdine != null && ProdottiOrdine.Count > 0) detOrdine = ProdottiOrdine.Where(x => x.id_prodotto == item.id_prodotto).FirstOrDefault();
                if (detOrdine != null && detOrdine.Quantita > 0) Quantita = detOrdine.Quantita.ToString();
                string ordineUscita = item.OrdineUscita != null || item.OrdineUscita != 0 ? item.OrdineUscita.ToString() : " - ";
                string stringaCottura = !string.IsNullOrEmpty(item.LivelloCottura) ? ", Cottura: '{8}'" : "";
                string cottura = !string.IsNullOrEmpty(item.LivelloCottura) ? item.LivelloCottura : "";
                retvalue = retvalue + "{" + String.Format("id_Prod:'{0}',Descrizione:'{1}',Prezzo:'{2}',Quantita:'{3}',PathFoto:'{4}',Ingredienti:'{5}',Calculate:'{6}', OrdineUscita: '{7}'" + stringaCottura, item.id_prodotto, item.NomeProdotto, string.Format("{0:#.00}", item.prezzoUnitario), Quantita, item.pathFoto, item.Ingredienti, string.Format("{0:#.00}", item.Calculate), ordineUscita, cottura) +  "},";
            }
            if (retvalue != "[")
            {
                retvalue = retvalue.Substring(0, retvalue.Length - 1) + "]";
            }
            else
            {
                retvalue = retvalue + "]";
            }
            return retvalue;
        }

        

        

        public JsonResult DetailCat(string idCat)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            Session["livelliCottura"] = null;
            Session["categoria"] = idCat;
            return Json(repository.GetCategoriaById(Convert.ToInt32(idCat)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string AddProdotto(string idProd)
        {
            string idCat;
            int id = Convert.ToInt32(idProd);
            ProdottiRepository prodottiRepository = new ProdottiRepository();
            Prodotti Prod = prodottiRepository.GetProdottoById(id);
            idCat = Prod.id_categoria.ToString();
            List<DettaglioProdottiOrdine> ListaProdScontrino = new List<DettaglioProdottiOrdine>();
            if (Session["ListOrder"] == null)
            {
                DettaglioProdottiOrdine item = new DettaglioProdottiOrdine();
                item.id_prodotto = Prod.id_prodotto;
                item.Quantita = 1;
                item.prezzoUnitario = (double)Prod.PrezzoVendita;
                item.NomeProdotto = Prod.Descrizione;
                item.iva = Prod.iva;
                item.pathFoto = Prod.pathFoto;
                ListaProdScontrino.Add(item);
                Session["ListOrder"] = ListaProdScontrino;
            }
            else
            {
                ListaProdScontrino = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                DettaglioProdottiOrdine itemp = ListaProdScontrino.Where(x => x.id_prodotto == id).FirstOrDefault();
                if (ListaProdScontrino != null && itemp != null)
                {
                    ListaProdScontrino.Remove(itemp);
                    itemp.Quantita = itemp.Quantita + 1;
                    ListaProdScontrino.Add(itemp);
                }
                else
                {
                    DettaglioProdottiOrdine item = new DettaglioProdottiOrdine();
                    item.id_prodotto = Prod.id_prodotto;
                    item.Quantita = 1;
                    item.prezzoUnitario = (double)Prod.PrezzoVendita;
                    item.NomeProdotto = Prod.Descrizione;
                    item.iva = Prod.iva;
                    item.pathFoto = Prod.pathFoto;
                    ListaProdScontrino.Add(item);
                }
                Session["ListOrder"] = ListaProdScontrino;
            }

            return idCat;

        }

        [HttpGet]
        public string GetProdottoOrdine(string idProd)
        {
            string numeroProd = "0";
            int Prod = Convert.ToInt32(idProd);
            List<DettaglioProdottiOrdine> ListaProdScontrino = new List<DettaglioProdottiOrdine>();
            if (Session["ListOrder"] != null)
            {
                ListaProdScontrino = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                numeroProd = ListaProdScontrino.Where(x => x.id_prodotto == Prod).Select(x => x.Quantita).FirstOrDefault().ToString();
            }
            return numeroProd;

        }

        [HttpPost]
        public string ModifyProdotto(string idProd, string nota, string quantita, string portata, string cottura)
        {
            string idCat;
            int id = Convert.ToInt32(idProd);
            ProdottiRepository prodottiRepository = new ProdottiRepository();
            Prodotti Prod = prodottiRepository.GetProdottoById(id);
            idCat = Prod.id_categoria.ToString();
            List<DettaglioProdottiOrdine> ListaProdScontrino = new List<DettaglioProdottiOrdine>();
            if (Session["ListOrder"] == null)
            {
                DettaglioProdottiOrdine item = new DettaglioProdottiOrdine();
                item.id_prodotto = Prod.id_prodotto;
                item.Quantita = Convert.ToInt32(quantita);
                item.prezzoUnitario = (double)Prod.PrezzoVendita;
                item.NomeProdotto = Prod.Descrizione;
                if (!string.IsNullOrEmpty(nota))
                {
                    item.Note = nota;
                }
                if (!string.IsNullOrEmpty(portata) && Convert.ToInt32(quantita) != 0)
                {
                    item.OrdineUscita = Convert.ToInt32(portata);
                }
                if (!string.IsNullOrEmpty(cottura))
                {
                    item.LivelloCottura = cottura;
                }
                item.iva = Prod.iva;
                item.pathFoto = Prod.pathFoto;
                ListaProdScontrino.Add(item);
                Session["ListOrder"] = ListaProdScontrino;
            }
            else
            {
                ListaProdScontrino = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                DettaglioProdottiOrdine itemp = ListaProdScontrino.Where(x => x.id_prodotto == id).FirstOrDefault();
                if (ListaProdScontrino != null && itemp != null)
                {
                    ListaProdScontrino.Remove(itemp);
                    itemp.Quantita = Convert.ToInt32(quantita);
                    if (!string.IsNullOrEmpty(nota))
                    {
                        itemp.Note = nota;
                    }
                    if (!string.IsNullOrEmpty(portata) && Convert.ToInt32(quantita) != 0)
                    {
                        itemp.OrdineUscita = Convert.ToInt32(portata);
                    }
                    if (!string.IsNullOrEmpty(cottura))
                    {
                        itemp.LivelloCottura = cottura;
                    }
                    ListaProdScontrino.Add(itemp);
                }
                else
                {
                    DettaglioProdottiOrdine item = new DettaglioProdottiOrdine();
                    item.id_prodotto = Prod.id_prodotto;
                    item.Quantita = Convert.ToInt32(quantita);
                    item.prezzoUnitario = (double)Prod.PrezzoVendita;
                    item.NomeProdotto = Prod.Descrizione;
                    item.iva = Prod.iva;
                    if (!string.IsNullOrEmpty(nota))
                    {
                        item.Note = nota;
                    }
                    if (!string.IsNullOrEmpty(portata) && Convert.ToInt32(quantita) != 0)
                    {
                        item.OrdineUscita = Convert.ToInt32(portata);
                    }
                    if (!string.IsNullOrEmpty(cottura))
                    {
                        item.LivelloCottura = cottura;
                    }
                    item.pathFoto = Prod.pathFoto;
                    ListaProdScontrino.Add(item);
                }
                Session["ListOrder"] = ListaProdScontrino;
            }

            return idCat;

        }

        [HttpPost]
        public string AddProdottoSpecial(string descrizione, string prezzo)
        {
            string id = Session["categoria"].ToString();
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            ProdottiRepository prodottiRepository = new ProdottiRepository();
            Prodotti p = new Prodotti();
            var cat = repository.GetCategoriaById(Convert.ToInt32(id));
            var tavolo = repository.GetTavoloById(Session["#Tavolo"].ToString());
            if (!string.IsNullOrEmpty(descrizione) && !string.IsNullOrEmpty(prezzo))
            {
                p.Descrizione = *//*tavolo.numero + " - " + *//*descrizione;
                p.id_categoria = Convert.ToInt32(id);
                p.pathFoto = cat.PathIcona;
                p.PrezzoVendita = Convert.ToDouble(prezzo);
                p.iva = 10;
                p.Quantita = 10;
                p.Ingredienti = descrizione;
                p.eliminato = false;
                p.IsSpecial = true;
                prodottiRepository.SalvaProdotto(p);

                return id;
            }

            return "errore";
            
        }

        [HttpPost]
        public string RemoveProdotto(string idProd)
        {
            string idCat;
            int id = Convert.ToInt32(idProd);
            ProdottiRepository prodottiRepository = new ProdottiRepository();
            Prodotti Prod = prodottiRepository.GetProdottoById(id);
            idCat = Prod.id_categoria.ToString();
            List<DettaglioProdottiOrdine> ListaProdScontrino = new List<DettaglioProdottiOrdine>();
            if (Session["ListOrder"] != null)
            {
                ListaProdScontrino = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                foreach(DettaglioProdottiOrdine item in ListaProdScontrino)
                {
                    if ((item.id_prodotto.ToString() == idProd) && item.Quantita > 0) item.Quantita = item.Quantita - 1;
                }
                Session["ListOrder"] = ListaProdScontrino;
            }
            return idCat;
        }

        [HttpPost]
        public ActionResult ProcediOrdine(ModelShop model)
        {
            ModelCompleta modelCompleta = new ModelCompleta();
            if (Session["ListOrder"] != null)
            {
                modelCompleta.listaProdotti = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
            }
            if (Session["Order"] != null)
            {
                modelCompleta.Ordine = ((Ordine)Session["Order"]);
            }
            if (Session["ListOrder"] != null)
            {
                modelCompleta.Prodotti = mostraListaProdotti(modelCompleta.listaProdotti);
                modelCompleta.totale = string.Format("{0:#.00}", modelCompleta.listaProdotti.Sum(x => x.Quantita * x.prezzoUnitario));
                modelCompleta.quantita = modelCompleta.listaProdotti.Sum(x => x.Quantita).ToString();
                string mod = WebConfigurationManager.AppSettings["ModalitaServizio"];
                //if (modelCompleta.Ordine.TipoOrdine == "Asporto") mod = "1";
                //if (modelCompleta.Ordine.TipoOrdine == "Consegna") mod = "2";
                //if (modelCompleta.Ordine.TipoOrdine == "Ordine Cameriere") mod = "3";
                modelCompleta.MostraBancomat = WebConfigurationManager.AppSettings["MetodiPagamento"];
                modelCompleta.MostraServizio = false;
                modelCompleta.MostraPulsante = true;
                modelCompleta.minimo = WebConfigurationManager.AppSettings["ImportoMinimo"];
                switch (mod)
                {
                    case "1":
                        //Modalità Costo senza servizio
                        modelCompleta.TotaleConServizio = (Convert.ToDouble(modelCompleta.totale)).ToString("N2");
                        modelCompleta.MostraServizio = false;
                        break;

                    case "2":
                        //Modalità Costo Fisso valore servizio
                        modelCompleta.CostoServizio = WebConfigurationManager.AppSettings["DeliveryService"];
                        modelCompleta.TotaleConServizio = (Convert.ToDouble(modelCompleta.totale) + Convert.ToDouble(modelCompleta.CostoServizio)).ToString("N2");
                        modelCompleta.MostraServizio = true;
                        break;

                    case "3":
                        //Modalità % valore servizio
                        int perc = Convert.ToInt32(WebConfigurationManager.AppSettings["ValoreServizio"]);
                        modelCompleta.CostoServizio = ((Convert.ToDouble(modelCompleta.totale) * Convert.ToDouble(perc)) / 100).ToString("N2");
                        modelCompleta.TotaleConServizio = (Convert.ToDouble(modelCompleta.totale) + Convert.ToDouble(modelCompleta.CostoServizio)).ToString("N2");
                        modelCompleta.MostraServizio = true;
                        break;
                }
            }
            if (Convert.ToDouble(modelCompleta.totale) < Convert.ToDouble(WebConfigurationManager.AppSettings["ImportoMinimo"])) modelCompleta.MostraPulsante = false;
            return View("Completa", modelCompleta);
        }

        [HttpPost]
        public ActionResult SalvaOrdine(ModelCompleta model, string buttonType)
        {
            if(buttonType == "modifica")
            {
                return RedirectToAction("Menu");
            }
            else
            {
                ConfigurazioniRepository configurazioniRepository = new ConfigurazioniRepository();
                int gestioneId = 0;
                if (Session["tavoloPrenotato"] != null)
                {
                    gestioneId = Convert.ToInt32(Session["tavoloPrenotato"]);
                }
                if (Session["ListOrder"] != null)
                {
                    model.listaProdotti = (List<DettaglioProdottiOrdine>)Session["ListOrder"];
                }
                if (Session["Order"] != null)
                {
                    model.Ordine = ((Ordine)Session["Order"]);
                }
                model.Ordine.DataOra = DateTime.Now;
                model.Ordine.Ip = Request.UserHostAddress;
                model.Ordine.NumeroPezzi = model.listaProdotti.Sum(x => x.Quantita);
                if(WebConfigurationManager.AppSettings["ModalitaServizio"] != "1")
                {
                    model.Ordine.Totale = Convert.ToDouble(model.TotaleConServizio);
                }
                else
                {
                    model.Ordine.Totale = Convert.ToDouble(model.totale);
                }
                //Da controllare se va spittato il servizio per gli scontrini fiscali se un giorno si dovesse integrare la cosa
                if (!string.IsNullOrEmpty(model.MetodoPagamento))
                {
                    model.Ordine.Tipo_Pagamento = model.MetodoPagamento;
                }
                else
                {
                    model.Ordine.Tipo_Pagamento = "Non specificato";
                }
                model.Ordine.id_stato = 1;
                if (!string.IsNullOrEmpty(model.Resto))
                {
                    model.Ordine.Note = model.Resto;
                }
                
                model.Ordine = OrdiniRepository.salvaOrdine(model.Ordine);
                double perc = Convert.ToInt32(WebConfigurationManager.AppSettings["ValoreServizio"]);
                double? costoServizio = (Convert.ToDouble(model.totale) * perc) / 100;
                if (Session["tavoloPrenotato"] != null)
                {

                    TavoliOrdini to = new TavoliOrdini();
                    to.id_gestione = Convert.ToInt32(Session["tavoloPrenotato"]);
                    gestioneId = to.id_gestione;
                    to.id_ordine = model.Ordine.id_ordine;
                    configurazioniRepository.AddTavoloOrdine(to);

                }
                else
                {
                    GestioneTavoli gt = new GestioneTavoli();
                    if (Session["#Tavolo"] != null)
                    {
                        gt.id_tavolo = Convert.ToInt32(Session["#Tavolo"]);
                    }
                    else
                    {
                        gt.id_tavolo = -1;
                    }
                    gt.id_utente = -99;
                    gt.id_stato = 1;
                    gt.data_prenotazione = DateTime.Now;
                    gt.stampa_fiscale = false;
                    gt.metodo_pagamento = "Contanti";//Debug solo per non far partire il pos model.MetodoPagamento;
                    gt.totale = Convert.ToDouble(model.totale) + costoServizio;
                    gt.fiscale = true;
                    gt.chiuso = false;
                    gt.percentuale_servizio = costoServizio;
                    configurazioniRepository.AddPrenotazioneTavolo(gt);

                    TavoliOrdini to = new TavoliOrdini();
                    to.id_gestione = gt.id_gestione;
                    to.id_ordine = model.Ordine.id_ordine;
                    configurazioniRepository.AddTavoloOrdine(to);
                }
                foreach (DettaglioProdottiOrdine item in model.listaProdotti)
                {
                    if (item.Quantita != 0)
                    {
                        ProdottiOrdine prodOrdine = new ProdottiOrdine();
                        prodOrdine.id_ordine = model.Ordine.id_ordine;
                        prodOrdine.id_prodotto = item.id_prodotto;
                        prodOrdine.prezzoUnitario = item.prezzoUnitario;
                        prodOrdine.Quantita = item.Quantita;
                        prodOrdine.iva = (int)item.iva;
                        prodOrdine.Note = item.Note;
                        prodOrdine.OrdineUscita = item.OrdineUscita;
                        if (!string.IsNullOrEmpty(item.LivelloCottura))
                        {
                            prodOrdine.LivelloCottura = item.LivelloCottura;
                        }
                        if (gestioneId != 0)
                        {
                            prodOrdine.id_gestione = gestioneId;
                        }
                        OrdiniRepository.SalvaProdottoOrdine(prodOrdine);
                        OrdiniRepository.RiduciQuantita(item.id_prodotto, item.Quantita);
                    }
                }
                Session["ListOrder"] = null;
                Session["Order"] = null;
                ModelOrdineCompletato modelresult = new ModelOrdineCompletato();
                modelresult.MessaggioSaluto = "Un nostro corriere verrà a consegnare il tuo ordine direttamente a casa tua.";
                //switch (model.Ordine.TipoOrdine)
                //{
                //    case "Consegna":
                //        modelresult.MessaggioSaluto = "Un nostro corriere verrà a consegnare il tuo ordine direttamente a casa tua.";
                //        break;
                //    case "Asporto":
                //        modelresult.MessaggioSaluto = "Aspetta il tuo turno, a breve verrai chiamato da un nostro addetto.";
                //        break;
                //    case "Servizio":
                //        modelresult.MessaggioSaluto = "Siediti al tuo tavolo, a breve riceverai la tua ordinazione.";
                //        break;
                //    case "Ordine Cameriere":
                //        modelresult.MessaggioSaluto = "Ordine effettuato con successo.";
                //        break;
                //}
                switch (model.Ordine.id_tipoVendita)
                {
                    case 3:
                        modelresult.ActionRedirect = "IndexRitira";
                        break;

                    case 4:
                        modelresult.ActionRedirect = "IndexOrdinaCasa";
                        break;

                    case 5:
                        modelresult.ActionRedirect = "IndexTavoli";
                        break;

                    default:
                        modelresult.ActionRedirect = "IndexTavoli";
                        break;
                }
                if (Session["operatore"] != null)
                {
                    TempData["Numero"] = modelresult.MessaggioSaluto;

                    ProdottiRepository prodottiRepository = new ProdottiRepository();
                    var tavOrd = configurazioniRepository.GetOrdineIdByIdGestione(gestioneId);
                    var listaProdottiOrdine = configurazioniRepository.GetPrenotazioneTavoloByIdOrdine(tavOrd.id_ordine);
                    foreach (var o in listaProdottiOrdine as List<DettaglioProdottiOrdine>)
                    {
                        var prodotto = prodottiRepository.GetProdottoById(o.id_prodotto);

                        if (prodotto.IsSpecial == true)
                        {
                            prodotto.eliminato = true;
                            prodotto.Quantita = 0;
                            prodottiRepository.deleteProdotto(prodotto);
                        }
                    }

                    return RedirectToAction("IndexTavoli", "Ordini");
                }
                else
                {
                    return View("OrdineCompletato", modelresult);
                }
                
            }
            
        }
*/


    }
}