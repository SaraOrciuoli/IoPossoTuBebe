using DAL.Model;
using DAL.Repository;
using Vetrina.Models.CustomModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Vetrina.Utility;
using Vetrina.Classi;
using static Vetrina.Classi.Configuration;
using Vetrina.Pos.Classes;
using Vetrina.Pos.Printers;
using DevExpress.DashboardCommon.Native;
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using System.Data.Entity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using Microsoft.Graph;
using DevExpress.XtraSpreadsheet.Model;
using DevExpress.XtraSpreadsheet.Commands;

namespace Vetrina.Controllers
{
    public class OrdiniController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Util.CheckCulture();
            base.Initialize(requestContext);
        }

        public ActionResult IndexTavoli()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelOrdini model = new ModelOrdini();
                ConfigurazioniRepository repository = new ConfigurazioniRepository();
               /* model.ListaTavoli = (List<Tavoli>)repository.GetListaTavoli();
                model.ListaGestioneTavolo = (List<DettaglioTavoli>)repository.GetListaPrenotazioniTavolo();*/

                if (TempData["Numero"] != null)
                {
                    ViewBag.Message = TempData["Numero"];
                }
                TempData["Numero"] = null;

                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult CheckDisponibilita(List<Prodotti> values_arr)
        {
            string code = "OK";
            string message = "";
            List<string> prod_not_available = new List<string>();

            if (values_arr != null && values_arr.Any())
            {
                for (int i = 0; i < values_arr.Count; i++)
                {
                    var item = values_arr[i];

                    // Chiamiamo il metodo passando SOLO id_prodotto e quantità
                    if (!OrdiniRepository.CheckQuantityAvailability(item.id_prodotto, item.Quantita))
                    {
                        code = "KO";
                        // Aggiungiamo l'indice 'i' alla lista. Questo corrisponde al data-id="..." nella View
                        prod_not_available.Add(i.ToString());
                        message = "Alcuni prodotti evidenziati non sono più disponibili nella quantità richiesta.";
                    }
                }
            }
            else
            {
                code = "KO";
                message = "Il carrello risulta vuoto o c'è stato un errore di lettura.";
            }

            return Json(new { code = code, message = message, prods = prod_not_available });
        }



        /*public ActionResult Tavoli(int id)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelOrdini model = new ModelOrdini();
                ConfigurazioniRepository repository = new ConfigurazioniRepository();
                model.Tavolo = repository.GetTavoloById(id.ToString());
                var tavoloPrenotato = repository.GetPrenotazioniByIdTavolo(id);
                if (tavoloPrenotato != null)
                {
                    if (tavoloPrenotato.id_stato == 2)
                    {
                        model.Ruolo = reqCookies["IdRuolo"];
                        //tavoloPrenotato.data_prenotazione.Value.ToString("yyyy/MM/dd HH:mm:ss");
                        model.GestioneTavolo = tavoloPrenotato;
                        //var ordineTavolo = repository.GetOrdineIdByIdGestione(tavoloPrenotato.id_gestione);
                        //model.ListaProdottiOrdine = (List<DettaglioProdottiOrdine>)repository.GetPrenotazioneTavoloByIdOrdine(ordineTavolo.id_ordine);
                        model.ListaProdottiOrdine = repository.GetOrdineTavoloByIdTavoloIdGestione(tavoloPrenotato.id_tavolo, tavoloPrenotato.id_gestione);
                        model.ListaTipoPagamento = (List<TipoPagamento>)repository.GetListaPagamenti();
                        double? tot = 0;
                        double perc = Convert.ToInt32(WebConfigurationManager.AppSettings["ValoreServizio"]);
                        foreach (var order in model.ListaProdottiOrdine)
                        {
                            if (order.id_prodotto != null)
                            {
                                tot += order.QuantitaTotale * order.prezzoUnitario;
                            }
                        }
                        double? costoServizio = (tot * perc) / 100;
                        if (model.GestioneTavolo.percentuale_servizio != costoServizio)
                        {
                            model.GestioneTavolo.percentuale_servizio = costoServizio;
                        }
                        double? totaleConServizio = tot + costoServizio;
                        if (model.GestioneTavolo.totale != totaleConServizio)
                        {
                            model.GestioneTavolo.totale = totaleConServizio;
                            repository.UpdatePrenotazioneTavolo(model.GestioneTavolo);
                        }
                    }
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult SalvaNumero(ModelOrdini model)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ConfigurazioniRepository repository = new ConfigurazioniRepository();
                string stato = "Occupato";
                var oldTavolo = repository.GetPrenotazioniByIdTavolo(model.Tavolo.id_tavolo);
                if (oldTavolo != null && oldTavolo.id_stato == 2 && oldTavolo.chiuso != true)
                {
                    model.GestioneTavolo.id_gestione = oldTavolo.id_gestione;
                }
                else if (oldTavolo != null && oldTavolo.id_stato == 1)
                {
                    model.IdStatoOrdine = repository.GetStatiTavoloByDescrizione(stato);
                    model.GestioneTavolo.id_gestione = oldTavolo.id_gestione;
                    model.GestioneTavolo.id_utente = Convert.ToInt32(reqCookies["IdUtente"]);
                    model.GestioneTavolo.id_tavolo = oldTavolo.id_tavolo;
                    model.GestioneTavolo.id_stato = Convert.ToInt32(model.IdStatoOrdine);
                    model.GestioneTavolo.stampa_preconto = oldTavolo.stampa_preconto;
                    model.GestioneTavolo.chiuso = oldTavolo.chiuso;
                    model.GestioneTavolo.data_prenotazione = DateTime.Now;
                    repository.UpdatePrenotazioneTavolo(model.GestioneTavolo);
                }
                else
                {
                    model.IdStatoOrdine = repository.GetStatiTavoloByDescrizione(stato);
                    model.GestioneTavolo.id_tavolo = model.Tavolo.id_tavolo;
                    model.GestioneTavolo.id_utente = Convert.ToInt32(reqCookies["IdUtente"]);
                    model.GestioneTavolo.id_stato = Convert.ToInt32(model.IdStatoOrdine);
                    model.GestioneTavolo.data_prenotazione = DateTime.Now;
                    model.GestioneTavolo.stampa_preconto = false;
                    model.GestioneTavolo.chiuso = false;
                    repository.AddPrenotazioneTavolo(model.GestioneTavolo);
                }*/

        /*TempData["Numero"] = "Numero persone selezionato";*//*
        Session["tavoloPrenotato"] = model.GestioneTavolo.id_gestione;
        Session["#Tavolo"] = model.Tavolo.id_tavolo;

        return RedirectToAction("Menu", "Shop");
    }
    else
    {
        return RedirectToAction("Login");
    }
}

public ActionResult ChiudiTavolo(string idGestione)
{
    HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
    if (reqCookies != null)
    {
        ChiusuraTavolo(idGestione);
        return RedirectToAction("IndexTavoli");
    }
    else
    {
        return RedirectToAction("Login");
    }
}

public void ChiusuraTavolo(string idGestione)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    ProdottiRepository prodottiRepository = new ProdottiRepository();
    var g = repository.GetGestioneById(idGestione);
    if (g.id_stato != 1)
    {
        int tavolo = g.id_tavolo;
        g.chiuso = true;
        repository.UpdatePrenotazioneTavolo(g);

        //controllo per eliminare eventuali prodotti speciali
        var to = repository.GetOrdineIdByIdGestione(g.id_gestione);
        if (to != null)
        {
            //var listaProdottiOrdine = repository.GetPrenotazioneTavoloByIdOrdine(to.id_ordine);
            var listaScontrino = repository.GetOrdineTavoloByIdTavoloIdGestione(Convert.ToInt32(tavolo), Convert.ToInt32(idGestione));
            foreach (var o in listaScontrino)
            {
                var prodotto = prodottiRepository.GetProdottoById(o.id_prodotto);

                if (prodotto.IsSpecial == true)
                {
                    prodotto.eliminato = true;
                    prodotto.Quantita = 0;
                    prodottiRepository.deleteProdotto(prodotto);
                }
            }
        }

        var tavoloPrenotato = repository.GetPrenotazioniByIdTavolo(g.id_tavolo);
        if (tavoloPrenotato.chiuso != false && tavoloPrenotato.id_stato != 1)
        {
            GestioneTavoli gt = new GestioneTavoli();
            gt.id_tavolo = tavolo;
            gt.stampa_preconto = false;
            gt.id_stato = 1;
            gt.chiuso = false;
            repository.AddPrenotazioneTavolo(gt);
        }
    }
}

public ActionResult ChiudiTavoloConto(string idGestione)
{
    ChiusuraTavolo(idGestione);

    return RedirectToAction("IndexTavoli");
}

public string StampaPreconto(string idGestione, string pagamento, string sconto)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    try
    {
        var g = repository.GetGestioneById(idGestione);
        g.stampa_preconto = true;
        g.preconto_stampato = true;
        g.metodo_pagamento = pagamento;
        g.fiscale = false;
        if (!string.IsNullOrEmpty(sconto))
        {
            g.sconto = Convert.ToDouble(sconto);
        }
        repository.UpdatePrenotazioneTavolo(g);

        if (!string.IsNullOrEmpty(sconto))
        {
            var o = OrdiniRepository.GetOrdineByIdGestione(Convert.ToInt32(idGestione));
            o.sconto = Convert.ToDouble(sconto);
            OrdiniRepository.updateOrdine(o);
        }

        return "OK";
    }
    catch (Exception ex)
    {
        return ex.Message;
    }

}*/

        public ActionResult Login()
        {
            if (TempData["message"] != null && !string.IsNullOrEmpty(TempData["message"].ToString()))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        public ActionResult Recupera()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            return RedirectToAction("Login");
        }

        public ActionResult LoginUtente(LoginModel l)
        {
            UtentiRepository rep = new UtentiRepository();
            Utenti u = new Utenti();
            u = rep.login(l.Email, l.Password);
            if (u != null)
            {
                HttpCookie userInfo = new HttpCookie("userRetailCassa");
                userInfo["IdUtente"] = u.id_utente.ToString();
                userInfo["IdRuolo"] = u.id_Ruolo.ToString();
                userInfo["NomeCompleto"] = u.NomeCompleto.ToString();
                userInfo["email"] = u.Mail.ToString();
                if (l.Rememberme)
                {
                    userInfo.Expires = DateTime.Now.AddYears(1);
                }
                else
                {
                    userInfo.Expires = DateTime.Now.AddHours(2);
                }

                Response.Cookies.Add(userInfo);

                return RedirectToAction("IndexTavoli");

            }
            else
            {
                ViewBag.Message = "Email o Password non riconosciute";
            }
            return View("Login");
        }


        public ActionResult RecuperaPassword(RecuperaModel r)
        {
            UtentiRepository rep = new UtentiRepository();
            Utenti u = new Utenti();
            u = rep.getUtenteRecupera(r.Email);
            if (u != null)
            {
                string passwordTemporanea = GenericUtil.RandomPassword(6);
                u.Password = passwordTemporanea;

                // Criptiamo a DB
                rep.UpdateUtente(u, true);

                string NomeCompleto = u.Nome + " " + u.Cognome;

                Vetrina.Utility.SendMailRecupera.RecuperaPasswordEmail(NomeCompleto, passwordTemporanea, r.Email);

                TempData["message"] = "Password recuperata correttamente a breve riceverai una mail con la nuova password.";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.Message = "Utente non riconosciuto, riprovare";
            }
            return View("Recupera");

        }
        public ActionResult DettaglioOrdine(int id)
        {

            List<ProdottiOrdine> prodottiOrdine = rep.GetProdottiOrdine(id);
            var utente = rep.GetUtenteById(id);

            CombinedViewModel model = new CombinedViewModel();
            model.ModelDettaglioOrdine= new ModelDettaglioOrdine
            {
                DettagliOrdine = prodottiOrdine,
                utente = utente
            };


            foreach (var dettaglio in model.ModelDettaglioOrdine.DettagliOrdine)
            {
                AnyeLabelEntities db = new AnyeLabelEntities();
                Prodotti prodottoDb = db.Prodotti.FirstOrDefault(p => p.id_prodotto == dettaglio.id_prodotto);
                dettaglio.Prodotto = rep.GetProdottoById(dettaglio.id_prodotto);
 
                
                if (dettaglio.Prodotto.in_offerta == true)
                {
                    double prezzoScontato = dettaglio.prezzoUnitario - (dettaglio.prezzoUnitario * (dettaglio.Prodotto.sconto.Value / 100));
                    dettaglio.prezzoScontato = prezzoScontato;
                }
                else
                {
                    dettaglio.Sconto = 0;
                    dettaglio.prezzoScontato = dettaglio.prezzoUnitario; 
                }
              
            }

            return View(model);
        }
        private string GetTagliaNomeById(int idTaglia)
        {
            using (var db = new AnyeLabelEntities())
            {
                var taglia = db.Taglie.FirstOrDefault(t => t.id_taglia == idTaglia);
                return taglia != null ? taglia.Descrizione_taglia : "N/A";
            }
        }

        // Rimuoviamo (int id) dai parametri. È più sicuro leggerlo dai cookie.
        public ActionResult Pagamento()
        {
            if (Request.Cookies["ForrestUser"] == null || string.IsNullOrEmpty(Request.Cookies["ForrestUser"]["IdUtente"]))
            {
                return RedirectToAction("Login", "Login", new { returnUrl = "/Ordini/Pagamento" });
            }

            int userId = Convert.ToInt32(Request.Cookies["ForrestUser"]["IdUtente"]);

            List<DettaglioProdottiOrdine> listaProdottiScontrino = Session["ListOrder"] as List<DettaglioProdottiOrdine>;

            if (listaProdottiScontrino == null || !listaProdottiScontrino.Any())
            {
                TempData["Message"] = "Il tuo carrello è vuoto. Aggiungi qualche prodotto per procedere!";
                return RedirectToAction("Index", "Home");
            }

            Utenti utente = rep.GetUtenteById(userId);
            if (utente == null)
            {
                return RedirectToAction("Error");
            }

            ProdottiRepository prodRepo = new ProdottiRepository();
            UtentiRepository repo = new UtentiRepository();
            CategorieRepository catRepo = new CategorieRepository();
            List<Comuni> listaComuniIsole = repo.GetListaComuniIsole();

            double speseSpedizioneCalcolate = 0;

            CombinedViewModel model = new CombinedViewModel();

            model.ModelPagamento = new ModelPagamento
            {
                Utente = utente,
                Prodotti = listaProdottiScontrino,
                SpeseSpedizioneTotali = speseSpedizioneCalcolate,
                ListaComuniIsole = listaComuniIsole
            };

            return View(model);
        }
        public ActionResult PayPalPagamentoRedirect()
        {
            string pagamento = "PayPal";
            return RedirectToAction("SalvaOrdine", new { pagamento });
        }

        UtentiRepository rep = new UtentiRepository();
        private readonly AnyeLabelEntities dbContext;
        public OrdiniController()
        {
            dbContext = new AnyeLabelEntities();
        }

        public ActionResult ApplicaCodiceSconto(string codiceSconto)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                CodiciSconto codiceScontoDaApplicare = db.CodiciSconto.FirstOrDefault(c => c.codice_sconto == codiceSconto);

                if (codiceScontoDaApplicare == null)
                {
                    return Json(new { success = false });
                }

                if ((bool)codiceScontoDaApplicare.utilizzato)
                {
                    return Json(new { success = false });
                }
                else
                {
                    if (Request.Cookies["ForrestUser"] == null)
                    {
                        return Json(new { success = false });
                    }
                    else
                    {
                        int IdUser = Convert.ToInt32(Request.Cookies["ForrestUser"]["IdUtente"]);
                        if (codiceScontoDaApplicare.id_utente != null && codiceScontoDaApplicare.id_utente != IdUser)
                        {
                            return Json(new { success = false });
                        }

                        if (db.Ordine.Where(x => x.id_codiceSconto == codiceScontoDaApplicare.id_codice_sconto && x.id_utente == IdUser).FirstOrDefault() == null)
                        {
                            Session["CodiceSconto"] = codiceScontoDaApplicare.id_codice_sconto;
                            Session["DiscountValue"] = codiceScontoDaApplicare.valore != 0 ? (double)codiceScontoDaApplicare.valore : 0;
                            Session["discountPercentage"]  = codiceScontoDaApplicare.percentuale != 0 ? (double)codiceScontoDaApplicare.percentuale * 100 : 0;

                            return Json(new { success = true, discountValue = (double)Session["DiscountValue"], discountPercentage = (double)Session["DiscountPercentage"], idCodSconto = codiceScontoDaApplicare.id_codice_sconto });
                        }
                        else
                        {
                            return Json(new { success = false });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        //[HttpPost]
        //public ActionResult SalvaOrdine(ModelPagamento model, string MetodoPagamento)
        //{
        //    double discountValue = Session["DiscountValue"] != null ? (double)Session["DiscountValue"] : 0;
        //    double discountPercentage = Session["DiscountPercentage"] != null ? (double)Session["DiscountPercentage"] : 0;

        //    AnyeLabelEntities db = new AnyeLabelEntities();
        //    List<DettaglioProdottiOrdine> lista_ProdOrd_corretta = new List<DettaglioProdottiOrdine>();
        //    double totale = 0.00;

        //    // 1. Preparazione lista prodotti
        //    foreach (DettaglioProdottiOrdine el in model.Prodotti)
        //    {
        //        Prodotti prod1 = db.Prodotti.Where(x => x.id_prodotto == el.id_prodotto).FirstOrDefault();

        //        lista_ProdOrd_corretta.Add(new DettaglioProdottiOrdine()
        //        {
        //            id_prodotto = el.id_prodotto,
        //            id_ordine = el.id_ordine,
        //            Quantita = el.Quantita,
        //            prezzoUnitario = (double)el.prezzoUnitario,
        //            Sconto = prod1.sconto,
        //            prezzoScontato = (prod1.in_offerta ?? false) ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (prod1.sconto / 100))) : (double)el.prezzoUnitario,
        //            InOfferta = prod1.in_offerta
        //        });

        //        totale += ((prod1.in_offerta ?? false) ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (prod1.sconto / 100))) : (double)el.prezzoUnitario) * el.Quantita;
        //    }

        //    string result = "";

        //    try
        //    {
        //        if (model == null || model.Prodotti == null || model.Prodotti.Count == 0 || string.IsNullOrEmpty(MetodoPagamento))
        //        {
        //            return RedirectToAction("Error");
        //        }

        //        int userId;
        //        if (Request.Cookies["ForrestUser"] != null && int.TryParse(Request.Cookies["ForrestUser"]["IdUtente"], out userId))
        //        {
        //            // Nota: assicurati di avere "rep" e "dbContext" istanziati a livello di classe
        //            Utenti utente = rep.GetUtenteById(userId);
        //            TipoPagamento tipoPagamento = dbContext.TipoPagamento.FirstOrDefault(tp => tp.descrizione_tipo_pagamento.ToLower() == MetodoPagamento.ToLower());

        //            if (utente == null || tipoPagamento == null)
        //            {
        //                return RedirectToAction("Error");
        //            }

        //            int idstatoOrdine = (tipoPagamento.id_tipo_pagamento == 6) ? 3 : 1;
        //            int idStatoPagamento = (idstatoOrdine == 3) ? 4 : 1;

        //            // 2. Creazione testata Ordine
        //            Ordine nuovoOrdine = new Ordine
        //            {
        //                DataOra = DateTime.Now,
        //                Ip = Request.UserHostAddress,
        //                NumeroPezzi = lista_ProdOrd_corretta.Sum(p => p.Quantita),
        //                Totale = (double)lista_ProdOrd_corretta.Sum(p => p.Quantita * (p.Sconto.HasValue && p.Sconto > 0 ? p.prezzoScontato : p.prezzoUnitario)),
        //                Indirizzo = utente.indirizzo,
        //                Cap = utente.Cap,
        //                Nominativo = $"{utente.Nome} {utente.Cognome}",
        //                NumTelefono = utente.Cellulare,
        //                id_utente = utente.id_utente,
        //                Tipo_Pagamento = tipoPagamento.descrizione_tipo_pagamento,
        //                Citta = utente.Citta,
        //                Nazione = utente.Nazione,
        //                TipoOrdine = "online",
        //                NumeroPersone = 1,
        //                Email = utente.Mail,
        //                id_tipoOrdine = 4,
        //                id_tipoPagamento = tipoPagamento.id_tipo_pagamento,
        //                id_stato = idstatoOrdine,
        //                id_stato_pagmento = idStatoPagamento,
        //                totale_spedizione = model.SpeseSpedizioneTotali
        //            };

        //            // 3. Gestione Codice Sconto
        //            string IdCodiceSconto = Session["CodiceSconto"] == null ? "" : Session["CodiceSconto"].ToString();
        //            if (!string.IsNullOrEmpty(IdCodiceSconto))
        //            {
        //                int id = Convert.ToInt32(IdCodiceSconto);
        //                CodiciSconto codiceScontoDaApplicare = db.CodiciSconto.Where(c => c.id_codice_sconto == id).FirstOrDefault();

        //                if (codiceScontoDaApplicare != null)
        //                {
        //                    if (codiceScontoDaApplicare.id_utente != null && codiceScontoDaApplicare.id_utente != utente.id_utente)
        //                    {
        //                        return RedirectToAction("Error");
        //                    }

        //                    nuovoOrdine.id_codiceSconto = codiceScontoDaApplicare.id_codice_sconto;
        //                    if (codiceScontoDaApplicare.id_tipoSconto != 1)
        //                    {
        //                        codiceScontoDaApplicare.utilizzato = true;
        //                        codiceScontoDaApplicare.data_utilizzo = DateTime.Now;
        //                        dbContext.Entry(codiceScontoDaApplicare).State = System.Data.Entity.EntityState.Modified;
        //                        dbContext.SaveChanges();
        //                    }
        //                }
        //            }

        //            dbContext.Ordine.Add(nuovoOrdine);
        //            dbContext.SaveChanges();

        //            Session.Remove("ListOrder");
        //            Session.Remove("CodiceSconto");

        //            // 4. Salvataggio Prodotti Ordine
        //            foreach (var prodotto in lista_ProdOrd_corretta)
        //            {
        //                ProdottiOrdine prodottoOrdine = new ProdottiOrdine
        //                {
        //                    id_prodotto = prodotto.id_prodotto,
        //                    id_ordine = nuovoOrdine.id_ordine,
        //                    Quantita = prodotto.Quantita,
        //                    prezzoUnitario = prodotto.prezzoUnitario
        //                };

        //                dbContext.ProdottiOrdine.Add(prodottoOrdine);
        //                dbContext.SaveChanges();

        //                OrdiniRepository.UpdateQuantitaProdottoAfterOrder(prodottoOrdine.id_prodotto, prodottoOrdine.Quantita);
        //            }

        //            // 5. Invio Email
        //            if (Thread.CurrentThread.CurrentCulture.Name == "en-US" || Thread.CurrentThread.CurrentCulture.Name == "en")
        //            {
        //                result = SendEmailInglese.SendMailInglese(utente.Nome, utente.Cognome, utente.Mail, totale, utente.indirizzo, utente.Citta, utente.Cap, tipoPagamento.descrizione_tipo_pagamento, lista_ProdOrd_corretta, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);
        //            }
        //            else
        //            {
        //                result = SendEmail.SendMail(utente.Nome, utente.Cognome, utente.Mail, totale, utente.indirizzo, utente.Citta, utente.Cap, tipoPagamento.descrizione_tipo_pagamento, lista_ProdOrd_corretta, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);
        //            }

        //            SendEmail.SendMailAdmin(utente.Nome, utente.Cognome, utente.Mail, totale, utente.indirizzo, utente.Citta, utente.Cap, tipoPagamento.descrizione_tipo_pagamento, lista_ProdOrd_corretta, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);

        //            if (result != "OK")
        //            {
        //                return RedirectToAction("Error");
        //            }

        //            // ==========================================
        //            // 6. REDIRECT FINALE IN BASE AL METODO
        //            // ==========================================

        //            // KLARNA
        //            if (tipoPagamento.descrizione_tipo_pagamento.ToLower() == "klarna")
        //            {
        //                try
        //                {
        //                    double totaleFinale = (double)nuovoOrdine.Totale + (nuovoOrdine.totale_spedizione ?? 0);

        //                    // Chiamiamo il metodo dal nostro Repository
        //                    DAL.Repository.PagamentiRepository pagRepo = new DAL.Repository.PagamentiRepository();
        //                    string redirectUrl = pagRepo.GeneraLinkPagamentoKlarna(nuovoOrdine.id_ordine, totaleFinale, utente.Mail);

        //                    // Reindirizziamo il cliente sul sito di Klarna
        //                    return Redirect(redirectUrl);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("Errore Klarna: " + ex.Message);
        //                    return RedirectToAction("KlarnaError");
        //                }
        //            }
        //            // PAYPAL
        //            else if (tipoPagamento.id_tipo_pagamento == 4)
        //            {
        //                return RedirectToAction("OrdineConfermato");
        //            }
        //            // BONIFICO E ALTRI
        //            else
        //            {
        //                return RedirectToAction("OrdineConfermatoBonifico");
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Error");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Errore durante il salvataggio dell'ordine: " + ex.Message);
        //        return RedirectToAction("Error");
        //    }
        //}

        //[HttpPost]
        //public ActionResult SalvaOrdine(ModelPagamento model, string MetodoPagamento)
        //{
        //    if (model == null || model.Prodotti == null || model.Prodotti.Count == 0 || string.IsNullOrEmpty(MetodoPagamento))
        //    {
        //        return RedirectToAction("Error");
        //    }

        //    try
        //    {
        //        int userId;
        //        if (Request.Cookies["ForrestUser"] == null || !int.TryParse(Request.Cookies["ForrestUser"]["IdUtente"], out userId))
        //        {
        //            return RedirectToAction("Login", "Login"); 
        //        }

        //        string idCodiceSconto = Session["CodiceSconto"]?.ToString() ?? "";
        //        string ipAddress = Request.UserHostAddress;


        //        OrdiniRepository ordRepo = new OrdiniRepository();
        //        Ordine nuovoOrdine = ordRepo.CreaNuovoOrdine(userId, MetodoPagamento, model.Prodotti, model.SpeseSpedizioneTotali, idCodiceSconto, ipAddress);

        //        Session.Remove("ListOrder");
        //        Session.Remove("CodiceSconto");

        //        // 2. RECUPERO DATI E INVIO EMAIL
        //        UtentiRepository utenteRepo = new UtentiRepository();
        //        Utenti utente = utenteRepo.GetUtenteById(userId);

        //        double discountValue = Session["DiscountValue"] != null ? (double)Session["DiscountValue"] : 0;
        //        double discountPercentage = Session["DiscountPercentage"] != null ? (double)Session["DiscountPercentage"] : 0;

        //        string result = "";
        //        if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.StartsWith("en"))
        //        {
        //            result = SendEmailInglese.SendMailInglese(utente.Nome, utente.Cognome, utente.Mail, nuovoOrdine.Totale.Value, utente.indirizzo, utente.Citta, utente.Cap, nuovoOrdine.Tipo_Pagamento, model.Prodotti, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);
        //        }
        //        else
        //        {
        //            result = SendEmail.SendMail(utente.Nome, utente.Cognome, utente.Mail, nuovoOrdine.Totale.Value, utente.indirizzo, utente.Citta, utente.Cap, nuovoOrdine.Tipo_Pagamento, model.Prodotti, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);
        //        }

        //        SendEmail.SendMailAdmin(utente.Nome, utente.Cognome, utente.Mail, nuovoOrdine.Totale.Value, utente.indirizzo, utente.Citta, utente.Cap, nuovoOrdine.Tipo_Pagamento, model.Prodotti, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);

        //        if (result != "OK")
        //        {
        //            return RedirectToAction("Error");
        //        }

        //        if (nuovoOrdine.Tipo_Pagamento.ToLower() == "klarna")
        //        {
        //            try
        //            {
        //                double totaleFinale = (double)nuovoOrdine.Totale + (nuovoOrdine.totale_spedizione ?? 0);
        //                DAL.Repository.PagamentiRepository pagRepo = new DAL.Repository.PagamentiRepository();
        //                string redirectUrl = pagRepo.GeneraLinkPagamentoKlarna(nuovoOrdine.id_ordine, totaleFinale, utente.Mail);

        //                return Redirect(redirectUrl);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Errore Klarna: " + ex.Message);
        //                return RedirectToAction("KlarnaError");
        //            }
        //        }
        //        else if (nuovoOrdine.id_tipoPagamento == 4) // PayPal
        //        {
        //            return RedirectToAction("OrdineConfermato");
        //        }
        //        else
        //        {
        //            return RedirectToAction("OrdineConfermatoBonifico");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Errore durante il salvataggio dell'ordine: " + ex.Message);
        //        return RedirectToAction("Error");
        //    }
        //}
        [HttpPost]
        public ActionResult SalvaOrdine(ModelPagamento model, string MetodoPagamento)
        {
            if (model == null || model.Prodotti == null || model.Prodotti.Count == 0 || string.IsNullOrEmpty(MetodoPagamento))
            {
                return RedirectToAction("Error");
            }

            try
            {
                int userId;
                if (Request.Cookies["ForrestUser"] == null || !int.TryParse(Request.Cookies["ForrestUser"]["IdUtente"], out userId))
                {
                    return RedirectToAction("Login", "Login");
                }

                // ==========================================
                // SE KLARNA: RITARDIAMO IL SALVATAGGIO
                // ==========================================
                if (MetodoPagamento.ToLower() == "klarna")
                {
                    // Salviamo il modello in sessione così non perdiamo le spese di spedizione
                    Session["KlarnaModelPending"] = model;

                    // Ricalcoliamo il totale esatto per Klarna (come facevi nel tuo codice)
                    double discountValue = Session["DiscountValue"] != null ? (double)Session["DiscountValue"] : 0;
                    double discountPercentage = Session["DiscountPercentage"] != null ? (double)Session["DiscountPercentage"] : 0;

                    AnyeLabelEntities dbKlarna = new AnyeLabelEntities();
                    double totaleNetto = 0.00;

                    foreach (var el in model.Prodotti)
                    {
                        Prodotti pDb = dbKlarna.Prodotti.Where(x => x.id_prodotto == el.id_prodotto).FirstOrDefault();
                        if (pDb != null)
                        {
                            bool inOfferta = pDb.in_offerta ?? false;
                            double prezzoReale = inOfferta ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (pDb.sconto / 100))) : (double)el.prezzoUnitario;
                            totaleNetto += prezzoReale * el.Quantita;
                        }
                    }

                    double scontoEuro = discountValue > 0 ? discountValue : (totaleNetto * discountPercentage / 100);
                    double totaleFinalePerKlarna = totaleNetto - scontoEuro + model.SpeseSpedizioneTotali;

                    int tempOrderId = (int)(DateTime.UtcNow - new DateTime(2023, 1, 1)).TotalSeconds;
                    Utenti utente = rep.GetUtenteById(userId);

                    DAL.Repository.PagamentiRepository pagRepo = new DAL.Repository.PagamentiRepository();
                    string redirectUrl = pagRepo.GeneraLinkPagamentoKlarna(tempOrderId, totaleFinalePerKlarna, utente.Mail);

                    // Fine. Reindirizziamo il cliente, l'ordine non è ancora salvato.
                    return Redirect(redirectUrl);
                }
                else if (MetodoPagamento.ToLower() == "stripe" || MetodoPagamento.ToLower() == "carta di credito")
                {
                    Session["StripeModelPending"] = model;

                    double discountValue = Session["DiscountValue"] != null ? (double)Session["DiscountValue"] : 0;
                    double discountPercentage = Session["DiscountPercentage"] != null ? (double)Session["DiscountPercentage"] : 0;

                    AnyeLabelEntities dbStripe = new AnyeLabelEntities();
                    double totaleNetto = 0.00;

                    foreach (var el in model.Prodotti)
                    {
                        Prodotti pDb = dbStripe.Prodotti.Where(x => x.id_prodotto == el.id_prodotto).FirstOrDefault();
                        if (pDb != null)
                        {
                            bool inOfferta = pDb.in_offerta ?? false;
                            double prezzoReale = inOfferta ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (pDb.sconto / 100))) : (double)el.prezzoUnitario;
                            totaleNetto += prezzoReale * el.Quantita;
                        }
                    }

                    double scontoEuro = discountValue > 0 ? discountValue : (totaleNetto * discountPercentage / 100);
                    double totaleFinalePerStripe = totaleNetto - scontoEuro + model.SpeseSpedizioneTotali;

                    int tempOrderId = (int)(DateTime.UtcNow - new DateTime(2023, 1, 1)).TotalSeconds;
                    Utenti utente = rep.GetUtenteById(userId);

                    DAL.Repository.PagamentiRepository pagRepo = new DAL.Repository.PagamentiRepository();

                    // Chiamiamo il NUOVO metodo che abbiamo creato!
                    string redirectUrl = pagRepo.GeneraLinkPagamentoStripe(tempOrderId, totaleFinalePerStripe, utente.Mail);

                    // Reindirizziamo il cliente alla pagina di checkout sicura di Stripe
                    return Redirect(redirectUrl);
                }


                return ProcessaSalvataggioFinale(model, MetodoPagamento, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante il salvataggio dell'ordine: " + ex.Message);
                return RedirectToAction("Error");
            }
        }
        public ActionResult OrdineConfermato()
        {

            return View();
        }
        public ActionResult OrdineConfermatoBonifico()
        {

            return View();
        }

        public ActionResult KlarnaSuccess(string authorization_token)
        {
            try
            {
                int userId;
                if (Request.Cookies["ForrestUser"] == null || !int.TryParse(Request.Cookies["ForrestUser"]["IdUtente"], out userId))
                {
                    return RedirectToAction("Login", "Login");
                }

                // Riprendiamo il modello che avevamo lasciato in sospeso prima del redirect
                ModelPagamento pendingModel = Session["KlarnaModelPending"] as ModelPagamento;

                if (pendingModel == null)
                {
                    // Se non c'è, significa che ha ricaricato la pagina (ordine già salvato)
                    return RedirectToAction("OrdineConfermato");
                }

                // Puliamo subito la sessione
                Session.Remove("KlarnaModelPending");

                // ORA POSSIAMO SALVARE L'ORDINE (poiché ha pagato)
                return ProcessaSalvataggioFinale(pendingModel, "klarna", userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore ritorno Klarna: " + ex.Message);
                return RedirectToAction("Error");
            }
        }

        public ActionResult StripeSuccess(string session_id)
        {
            try
            {
                int userId;
                if (Request.Cookies["ForrestUser"] == null || !int.TryParse(Request.Cookies["ForrestUser"]["IdUtente"], out userId))
                {
                    return RedirectToAction("Login", "Login");
                }

                ModelPagamento pendingModel = Session["StripeModelPending"] as ModelPagamento;

                if (pendingModel == null)
                {
                    return RedirectToAction("OrdineConfermato");
                }

                Session.Remove("StripeModelPending");

                // NOTA: Sostituisci "Stripe" con la stringa esatta che hai nel DB nella tabella TipoPagamento 
                // (es. se nel DB hai scritto "Carta di Credito", scrivi "Carta di Credito" qui sotto)
                return ProcessaSalvataggioFinale(pendingModel, "Stripe", userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore ritorno Stripe: " + ex.Message);
                return RedirectToAction("Error");
            }
        }
        public ActionResult StripeCancel()
        {
            if (Session["StripeModelPending"] != null)
            {
                Session.Remove("StripeModelPending");
            }

            TempData["ErrorPagamento"] = "Hai annullato il pagamento. L'ordine non è stato completato e nessun importo ti è stato addebitato.";

            return RedirectToAction("Pagamento", "Ordini");
        }


        private ActionResult ProcessaSalvataggioFinale(ModelPagamento model, string MetodoPagamento, int userId)
        {
            double discountValue = Session["DiscountValue"] != null ? (double)Session["DiscountValue"] : 0;
            double discountPercentage = Session["DiscountPercentage"] != null ? (double)Session["DiscountPercentage"] : 0;

            AnyeLabelEntities db = new AnyeLabelEntities();
            List<DettaglioProdottiOrdine> lista_ProdOrd_corretta = new List<DettaglioProdottiOrdine>();
            double totale = 0.00;

            foreach (DettaglioProdottiOrdine el in model.Prodotti)
            {
                Prodotti prod1 = db.Prodotti.Where(x => x.id_prodotto == el.id_prodotto).FirstOrDefault();

                lista_ProdOrd_corretta.Add(new DettaglioProdottiOrdine()
                {
                    id_prodotto = el.id_prodotto,
                    id_ordine = el.id_ordine,
                    Quantita = el.Quantita,
                    prezzoUnitario = (double)el.prezzoUnitario,
                    Sconto = prod1.sconto,
                    prezzoScontato = (prod1.in_offerta ?? false) ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (prod1.sconto / 100))) : (double)el.prezzoUnitario,
                    InOfferta = prod1.in_offerta,
                    id_taglia = el.id_taglia
                });

                totale += ((prod1.in_offerta ?? false) ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (prod1.sconto / 100))) : (double)el.prezzoUnitario) * el.Quantita;
            }

            string result = "";
            string idCodiceSconto = Session["CodiceSconto"]?.ToString() ?? "";
            string ipAddress = Request.UserHostAddress;

            Utenti utente = rep.GetUtenteById(userId);
            TipoPagamento tipoPagamento = db.TipoPagamento.FirstOrDefault(tp => tp.descrizione_tipo_pagamento.ToLower() == MetodoPagamento.ToLower());

            if (utente == null || tipoPagamento == null)
                return RedirectToAction("Error");

            int idstatoOrdine = (tipoPagamento.id_tipo_pagamento == 6) ? 3 : 1;
            int idStatoPagamento = (idstatoOrdine == 3) ? 4 : 1;

            Ordine nuovoOrdine = new Ordine
            {
                DataOra = DateTime.Now,
                Ip = ipAddress,
                NumeroPezzi = lista_ProdOrd_corretta.Sum(p => p.Quantita),
                Totale = (double)lista_ProdOrd_corretta.Sum(p => p.Quantita * (p.Sconto.HasValue && p.Sconto > 0 ? p.prezzoScontato : p.prezzoUnitario)),
                Indirizzo = utente.indirizzo,
                Cap = utente.Cap,
                Nominativo = $"{utente.Nome} {utente.Cognome}",
                NumTelefono = utente.Cellulare,
                id_utente = utente.id_utente,
                Tipo_Pagamento = tipoPagamento.descrizione_tipo_pagamento,
                Citta = utente.Citta,
                Nazione = utente.Nazione,
                TipoOrdine = "online",
                NumeroPersone = 1,
                Email = utente.Mail,
                id_tipoOrdine = 4,
                id_tipoPagamento = tipoPagamento.id_tipo_pagamento,
                id_stato = idstatoOrdine,
                id_stato_pagmento = idStatoPagamento,
                totale_spedizione = model.SpeseSpedizioneTotali
            };

            if (!string.IsNullOrEmpty(idCodiceSconto))
            {
                int id = Convert.ToInt32(idCodiceSconto);
                CodiciSconto codiceScontoDaApplicare = db.CodiciSconto.Where(c => c.id_codice_sconto == id).FirstOrDefault();

                if (codiceScontoDaApplicare != null)
                {
                    if (codiceScontoDaApplicare.id_utente != null && codiceScontoDaApplicare.id_utente != utente.id_utente)
                        return RedirectToAction("Error");

                    nuovoOrdine.id_codiceSconto = codiceScontoDaApplicare.id_codice_sconto;
                    if (codiceScontoDaApplicare.id_tipoSconto != 1)
                    {
                        codiceScontoDaApplicare.utilizzato = true;
                        codiceScontoDaApplicare.data_utilizzo = DateTime.Now;
                        db.Entry(codiceScontoDaApplicare).State = System.Data.Entity.EntityState.Modified;
                    }
                }
            }

            db.Ordine.Add(nuovoOrdine);
            db.SaveChanges();

            Session.Remove("ListOrder");
            Session.Remove("CodiceSconto");

            foreach (var prodotto in lista_ProdOrd_corretta)
            {
                ProdottiOrdine prodottoOrdine = new ProdottiOrdine
                {
                    id_prodotto = prodotto.id_prodotto,
                    id_ordine = nuovoOrdine.id_ordine,
                    Quantita = prodotto.Quantita,
                    prezzoUnitario = prodotto.prezzoUnitario,
                    id_taglia = prodotto.id_taglia
                };

                db.ProdottiOrdine.Add(prodottoOrdine);
                db.SaveChanges();

                if (prodotto.id_taglia != null && prodotto.id_taglia > 0)
                {
                    // Troviamo il record esatto nella tabella ProdottoTaglia
                    var varianteDb = db.ProdottoTaglia.FirstOrDefault(pt =>
                        pt.id_prodotto == prodotto.id_prodotto &&
                        pt.id_taglia == prodotto.id_taglia &&
                        pt.eliminato == false);

                    if (varianteDb != null)
                    {

                        varianteDb.quantita -= prodotto.Quantita;

                        if (varianteDb.quantita < 0) varianteDb.quantita = 0;

                        db.Entry(varianteDb).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        //Il trigger nel db aggiornerà le quantità generali del prodotto automaticamente
                    }
                }
                else
                {
                    OrdiniRepository.UpdateQuantitaProdottoAfterOrder(prodottoOrdine.id_prodotto, prodottoOrdine.Quantita);
                }
            }

            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.StartsWith("en"))
            {
                result = SendEmailInglese.SendMailInglese(utente.Nome, utente.Cognome, utente.Mail, nuovoOrdine.Totale.Value, utente.indirizzo, utente.Citta, utente.Cap, tipoPagamento.descrizione_tipo_pagamento, lista_ProdOrd_corretta, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione);
            }
            else
            {
                result = SendEmail.SendMail(utente.Nome, utente.Cognome, utente.Mail, nuovoOrdine.Totale.Value, utente.indirizzo, utente.Citta, utente.Cap, tipoPagamento.descrizione_tipo_pagamento, lista_ProdOrd_corretta, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione, (double)nuovoOrdine.totale_spedizione);
            }

            SendEmail.SendMailAdmin(utente.Nome, utente.Cognome, utente.Mail, nuovoOrdine.Totale.Value, utente.indirizzo, utente.Citta, utente.Cap, tipoPagamento.descrizione_tipo_pagamento, lista_ProdOrd_corretta, nuovoOrdine.id_ordine, discountValue, discountPercentage, utente.Nazione, (double)nuovoOrdine.totale_spedizione);

            if (result != "OK") return RedirectToAction("Error");

            if (nuovoOrdine.id_tipoPagamento == 4 || nuovoOrdine.Tipo_Pagamento.ToLower() == "klarna" || nuovoOrdine.Tipo_Pagamento.ToLower() == "stripe")
                return RedirectToAction("OrdineConfermato");
            else
                return RedirectToAction("OrdineConfermatoBonifico");
        }


        public ActionResult KlarnaCancel(int id_ordine)
        {
            // Il cliente ha chiuso la finestra di Klarna prima di pagare
            return View();
        }

        public ActionResult KlarnaError(int id_ordine)
        {
            return View();
        }

    }
}