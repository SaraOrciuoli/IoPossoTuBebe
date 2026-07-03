using DAL.Model;
using DAL.Repository;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Admin.Models.CustomModels;
using Admin.Pos.Classes;
using Admin.Pos.Printers;
using Admin.Pos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Admin.Models.CustomModel;
using Admin.Classi;
using static Admin.Classi.Configuration;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;
using System.Xml.Linq;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Linq.Expressions;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.DataProcessing;
using System.Net;
using Admin.Utility;
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;

namespace Admin.Controllers
{
    public class OrdiniController : Controller
    {
        internal OrdiniRepository repo = new OrdiniRepository();

        public ActionResult IndexOrdini() {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                return View();
            } else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult OrdiniDaEvadere()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult GetOrdini(DataSourceLoadOptions loadOptions) 
        {
            var result = DataSourceLoader.Load(repo.GetOrdini(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        public ActionResult GetOrdiniDaEvadere(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(repo.GetOrdiniDaEvadere(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }


        public ActionResult GetOrdiniWeb(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(OrdiniRepository.GetOrdiniWeb(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult GetOrdiniOnline(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(OrdiniRepository.GetOrdiniOnline(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        [HttpPost]
        public ActionResult DeleteOrdini(int key)
        {
            foreach (ProdottiOrdine el in repo.getPOByOrderId(key))
            {
                repo.DeleteProdottiOrdine(el);
            }
            repo.DeleteOrdine(repo.GetOrdineById(key));
            return RedirectToAction("IndexOrdini");
        }

        public ActionResult Edit(string id)
        {
            ModelEditOrdine model = new ModelEditOrdine();
            OrdiniRepository repo = new OrdiniRepository();
            UtentiRepository rep = new UtentiRepository();

            Session["ImgsProd"] = null;

            model.DettagliOrdine = new DettagliOrdine();
            model.ordine = new Ordine();
            if (!String.IsNullOrEmpty(id))
            {
                model.ordine = repo.GetOrdineById(Convert.ToInt32(id));
                model.StatiOrdine = repo.GetStatiOrdine();
                model.StatiPagamento = repo.GetStatiPagamento();
                model.ListaComuniIsole = rep.GetListaComuniIsole();

                var prodottiOrdine = rep.GetProdottiOrdine(Convert.ToInt32(id)) ?? new List<ProdottiOrdine>();
                model.DettagliProdottiOrdine = prodottiOrdine.Select(po => new DettaglioProdottiOrdine
                {
                    id_ProdottiOrdine = po.id_ProdottiOrdine,
                    id_prodotto = po.id_prodotto,
                    id_taglia = po.id_taglia ?? 0 ,
                    Quantita = po.Quantita,
                    prezzoUnitario = po.prezzoUnitario,
                    Prodotto = rep.GetProdottoById(po.id_prodotto),
                }).ToList();

                var statoCorrente = model.StatiOrdine.FirstOrDefault(s => s.id_stato == model.ordine.id_stato);
                var statoCorrentePag = model.StatiPagamento.FirstOrDefault(s => s.id_stato_pagamento == model.ordine.id_stato_pagmento);
                if (statoCorrente != null)
                {
                    model.ordine.NomeStato = statoCorrente.Descrizione;
                }
                
                    if(model.ordine.id_stato_pagmento != null)
                    {
                        model.ordine.NomeStatoPagamento = statoCorrentePag.Descrizione_stato_pagamento;

                    }
                

                foreach (var dettaglio in model.DettagliProdottiOrdine)
                {
                    if (dettaglio.Prodotto != null)
                    {
                        dettaglio.Taglia = GetTagliaNomeById(dettaglio.id_taglia);
                        dettaglio.descrizione_spessore = GetSpessoreById(dettaglio.id_spessore ?? 0);
                        dettaglio.Descrizione_colore = GetColoreById(dettaglio.id_colore ?? 0);



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
                }
            }

            Session["Order"] = id ?? "";
            return View(model);
        }

        // Metodo per ottenere il nome della taglia per ID (esempio)
        private string GetTagliaNomeById(int idTaglia)
        {
            using (var db = new AnyeLabelEntities())
            {
                var taglia = db.Taglie.FirstOrDefault(t => t.id_taglia == idTaglia);
                return taglia != null ? taglia.Descrizione_taglia : "N/A";  
            }
        }
        private string GetSpessoreById(int idSpessore)
        {
            using (var db = new AnyeLabelEntities())
            {
                var taglia = db.Spessori.FirstOrDefault(t => t.id_spessore == idSpessore);
                return taglia != null ? taglia.descrizione_spessore : "N/A";
            }
        }
        private string GetColoreById(int idColore)
        {
            using (var db = new AnyeLabelEntities())
            {
                var taglia = db.Colori.FirstOrDefault(t => t.id_Colori == idColore);
                return taglia != null ? taglia.Descrizione_colore : "N/A";
            }
        }
        

        //[HttpPost]
        //public JsonResult CambiaStato(int id, int nuovoStato)
        //{
        //    OrdiniRepository repo = new OrdiniRepository();
        //    UtentiRepository utenti = new UtentiRepository();

        //    var ordine = repo.GetOrdineById(id);

        //    if (ordine != null)
        //    {
        //        ordine.id_stato = nuovoStato;
        //        repo.UpdateOrdine(ordine);


        //        var statoCorrente = repo.GetStatiOrdine().FirstOrDefault(s => s.id_stato == nuovoStato);
        //        string nomeStato = statoCorrente != null ? statoCorrente.Descrizione : "Stato sconosciuto";

        //        var utente = utenti.GetUtenteById(Convert.ToInt32(ordine.id_utente));

        //        var tipoPagamento = ordine.Tipo_Pagamento;

        //        var prodotti = repo.GetProdottiOrdineById(id);

        //        string risultatoEmail = SendMailStato.EmailStatoOrdine(
        //            stato: nomeStato,
        //            email: ordine.Email,
        //            ordine: ordine.id_ordine.ToString(),
        //            totale: Convert.ToDecimal(ordine.Totale),
        //            indirizzo: ordine.Indirizzo,
        //            citta: ordine.Citta,
        //            cap: ordine.Cap,
        //            pagamento: tipoPagamento,
        //            prodotti: prodotti,
        //            statoOrdine: nomeStato 
        //        );

        //        if (risultatoEmail == "OK")
        //        {
        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Errore nell'invio dell'email." });
        //        }
        //    }

        //    return Json(new { success = false, message = "Ordine non trovato." });
        //}

        [HttpPost]
        public JsonResult CambiaStatoPagamento(int id, int nuovoStatoPagamento)
        {
            OrdiniRepository repo = new OrdiniRepository();
            UtentiRepository utenti = new UtentiRepository();

            var ordine = repo.GetOrdineById(id);

            if (ordine != null)
            {
                ordine.id_stato_pagmento = nuovoStatoPagamento;
                repo.UpdateOrdine(ordine);

                var statoCorrente = repo.GetStatiPagamento().FirstOrDefault(s => s.id_stato_pagamento == nuovoStatoPagamento);
                string nomeStatoPagamento = statoCorrente != null ? statoCorrente.Descrizione_stato_pagamento : "Stato sconosciuto";

                var utente = utenti.GetUtenteById(Convert.ToInt32(ordine.id_utente));

                var tipoPagamento = ordine.Tipo_Pagamento;

                var prodotti = repo.GetProdottiOrdineById(id);

                return Json(new { success = true });

            }

            return Json(new { success = false, message = "Ordine non trovato." });
        }
    }
}