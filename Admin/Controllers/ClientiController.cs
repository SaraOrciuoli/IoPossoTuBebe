using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Admin.Models.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Repository;
using DAL.Model;
using Newtonsoft.Json.Linq;
using DevExpress.XtraRichEdit.Import.Html;

namespace Admin.Controllers
{
    public class ClientiController : Controller
    {
        internal UtentiRepository repo = new UtentiRepository();
        public ActionResult IndexCliente()
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

        public ActionResult GetClienti(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(repo.GetClienti(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult EditCliente(string id) 
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditUtente model = new ModelEditUtente();
                if (!String.IsNullOrEmpty(id))
                {
                    model.Utente = repo.GetUtenteById(Convert.ToInt32(id));
                    if (String.IsNullOrEmpty(model.Utente.CodiceUnivoco))
                    {
                        model.is_privato = true;
                    }
                    else
                    {
                        model.is_azienda = true;
                    }
                }
                return View("EditCliente", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        public ActionResult SalvaCliente(string id, ModelEditUtente model)
        {
            Utenti utente = model.Utente;
            Ruoli cliente = repo.GetRuoloCliente();
            int id_utente = 0;
            if (!String.IsNullOrEmpty(id)) id_utente = Convert.ToInt32(id);
            Utenti u = repo.GetUtenteById(id_utente);
            if (u == null)
            {
                u = new Utenti();
                u.id_utente = id_utente;
            }
            u.id_Ruolo = cliente.id_ruolo;
            
            if (utente.Cellulare != null) u.Cellulare = utente.Cellulare;
            if (utente.Mail != null) u.Mail = utente.Mail;
            if (utente.p_iva != null) u.p_iva = utente.p_iva;
            if (utente.indirizzo != null) u.indirizzo = utente.indirizzo;
            if (utente.Cap != null) u.Cap = utente.Cap;
            if (utente.Citta != null) u.Citta = utente.Citta;
            if (utente.Iban != null) u.Iban = utente.Iban;
            if (utente.pec != null) u.pec = utente.pec;
            if (utente.SitoWeb != null) u.SitoWeb = utente.SitoWeb;
            if(utente.Codice_Fiscale != null) u.Codice_Fiscale = utente.Codice_Fiscale;
            if (utente.CodiceUnivoco != null) 
            {
                if (utente.Nome != null) { u.Nome = utente.Nome; u.NomeCompleto = utente.Nome; }
                u.CodiceUnivoco = utente.CodiceUnivoco; 
            } else
            {
                if (utente.Nome != null) u.Nome = utente.Nome;
                if (utente.Cognome != null) u.Cognome = utente.Cognome;
                u.NomeCompleto = u.Nome + " " + u.Cognome;
            }
            
            if (utente.Consenso != null) {
                u.Consenso = utente.Consenso;
                if(utente.Consenso == true)
                {
                    if(u.DataConsenso == null)
                    {
                        u.DataConsenso = DateTime.Now;
                    }
                } else
                {
                    u.DataConsenso = null;
                }
            }

            if (u.id_utente != 0) {
                repo.UpdateUtente(u);
            } else
            {
                repo.SalvaUtente(u);
            }
            return RedirectToAction("IndexCliente");
        }

        public ActionResult DeleteCliente(int key)
        {
            UtentiRepository.deleteUtenti(key);
            return RedirectToAction("IndexCliente");
        }
    }
}