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
using DevExpress.XtraScheduler.iCalendar.Components;
using DevExpress.Web.Internal;

namespace Admin.Controllers
{
    public class CodiciScontoController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null && reqCookies["IdRuolo"] == "1")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult GetClientiNomeCompleto(DataSourceLoadOptions loadOptions)
        {
            UtentiRepository repo = new UtentiRepository();
            var result = DataSourceLoader.Load(repo.GetClientiNomeCompleto(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        public ActionResult getScontiTemporanei(DataSourceLoadOptions loadOptions) {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            var result = DataSourceLoader.Load(repo.getCodiciScontoByTipo(1), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        public ActionResult GetScontiSpeciali(DataSourceLoadOptions loadOptions)
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            List<ScontiSpecialiModel> listaSconti = new List<ScontiSpecialiModel>();

            foreach(var el in repo.GetScontiSpeciali())
            {
                listaSconti.Add(new ScontiSpecialiModel()
                {
                    id_scontoSpeciale = el.id_scontoSpeciale,
                    Data_Inizio = el.Data_Inizio,
                    Data_Fine = el.Data_Fine,
                    Valore = el.Valore,
                    Attivo = el.Attivo,
                    Sconto_speciale = el.Sconto_speciale,
                    categorie = repo.getScontiSpecialiCatID(el.id_scontoSpeciale),

                });
            }

            var result = DataSourceLoader.Load(listaSconti ,loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }



        public ActionResult getScontiRimborso(DataSourceLoadOptions loadOptions) { 
            CodiciScontoRepository repo = new CodiciScontoRepository();
            var result = DataSourceLoader.Load(repo.getCodiciScontoByTipo(2), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");

        }
        [HttpPost]
        public string checkCodiceSconto(string codice_sconto) {
            string result = "KO";
            CodiciScontoRepository repo = new CodiciScontoRepository();
            if(repo.getCodiciScontoByCodice(codice_sconto) == null)
            {
                result = "OK";
            }
            return result;
        }

        public ActionResult UpdateCodiceSconto(int key, string values)
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            UtentiRepository repo_u = new UtentiRepository();
            dynamic data = JObject.Parse(values);
            CodiciSconto cs = repo.getCodiciScontoById(key);

            if(data.codice_sconto != null) cs.codice_sconto = data.codice_sconto;
            if(data.Descrizione != null) cs.Descrizione = data.Descrizione;
            if(data.data_fine != null) cs.data_fine = data.data_fine;
            if(data.percentuale != null) cs.percentuale = data.percentuale;
            if(data.valore != null) cs.valore = data.valore;
            if(data.ScontoPrimoOrdine != null) cs.ScontoPrimoOrdine = data.ScontoPrimoOrdine;
            if(data.utilizzato != null && data.utilizzato == true)
            {
                cs.utilizzato = true;
                cs.data_utilizzo = DateTime.Now;
            } else
            {
                cs.utilizzato = false;
            }
            if (data.id_utente != null) {
                cs.id_utente = data.id_utente;
                Utenti ut = repo_u.GetUtenteById((int)cs.id_utente);
                cs.NomeUtente = ut.NomeCompleto != null ? ut.NomeCompleto : (ut.Nome + " " + ut.Cognome);
            }
            repo.UpdateCodiciSconto(cs);
            return RedirectToAction("Index");
        } 
        
        public ActionResult UpdateScontoSpeciale(int key, string values)
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();

            UtentiRepository repo_u = new UtentiRepository();
            dynamic data = JObject.Parse(values);
            ScontiSpeciali cs = repo.GetScontiSpecialiByID(key);

            if(data.Sconto_speciale != null) cs.Sconto_speciale = data.Sconto_speciale;
            if(data.Data_Inizio != null) cs.Data_Inizio = data.Data_Inizio;
            if(data.Data_Fine != null) cs.Data_Fine = data.Data_Fine;
            if(data.Valore != null) cs.Valore = data.Valore;
            if(data.Attivo != null) cs.Attivo = data.Attivo;

            if (data.categorie != null && data.categorie.Count > 0)
            {
                repo.deleteScontiSpec_Cat(key);

                foreach (int el in data.categorie)
                {
                    ScontiSpec_Cat scontiCat = new ScontiSpec_Cat()
                    {
                        id_categorie = el,
                        id_scontoSpeciale = key,
                    };
                    repo.saveScontiSpec_Cat(scontiCat);
                }

            }

            repo.UpdateScontoSpeciale(cs);
            return RedirectToAction("Index");
        }
        public ActionResult AddCodiceSconto(string values)
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            UtentiRepository repo_u = new UtentiRepository();
            dynamic data = JObject.Parse(values);
            CodiciSconto cs = new CodiciSconto();
            cs.id_tipoSconto = data.id_tipoSconto;

            if (data.codice_sconto != null) cs.codice_sconto = data.codice_sconto;
            if (data.Descrizione != null) cs.Descrizione = data.Descrizione;
            if (data.data_fine != null) cs.data_fine = data.data_fine;
            cs.percentuale = data.percentuale != null ? data.percentuale : 0.00;
            cs.valore = data.valore != null ? data.valore : 0.00;
            if (data.id_tipoSconto != null) cs.id_tipoSconto = data.id_tipoSconto;
            if (data.ScontoPrimoOrdine != null) cs.ScontoPrimoOrdine = data.ScontoPrimoOrdine;
            if (data.utilizzato != null && data.utilizzato == true)
            {
                cs.utilizzato = true;
                cs.data_utilizzo = DateTime.Now;
            } else
            {
                cs.utilizzato = false;
            }
            if (data.id_utente != null)
            {
                cs.id_utente = data.id_utente;
                Utenti ut = repo_u.GetUtenteById((int)cs.id_utente);
                cs.NomeUtente = ut.NomeCompleto != null ? ut.NomeCompleto : (ut.Nome + " " + ut.Cognome);
            }
            repo.AddCodiciSconto(cs);

            return RedirectToAction("Index");
        }

        public ActionResult AddScontoSpeciale(string values)
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            UtentiRepository repo_u = new UtentiRepository();
            dynamic data = JObject.Parse(values);
            ScontiSpeciali cs = new ScontiSpeciali();

            if (data.Sconto_speciale != null) cs.Sconto_speciale = data.Sconto_speciale;
            if (data.Data_Inizio != null) cs.Data_Inizio = data.Data_Inizio;
            if (data.Data_Fine != null) cs.Data_Fine = data.Data_Fine;
            if (data.Attivo != null) cs.Attivo = data.Attivo;
            if (data.Valore != null) cs.Valore = data.Valore;
           
            int id_sconto = repo.AddScontiSpeciali(cs);
            if (data.categorie != null && data.categorie.Count > 0)
            {
                foreach (int el in data.categorie)
                {
                    ScontiSpec_Cat scontiCat = new ScontiSpec_Cat()
                    {
                        id_categorie = el,
                        id_scontoSpeciale = id_sconto,
                    };
                    repo.saveScontiSpec_Cat(scontiCat);
                }

            }
            return RedirectToAction("Index");
        } 
        public ActionResult DeleteCodiceSconto(int key) 
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            CodiciSconto cs = repo.getCodiciScontoById(key);
            repo.DeleteCodiciSconto(cs);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteScontiSpeciali(int key) 
        {
            CodiciScontoRepository repo = new CodiciScontoRepository();
            repo.deleteScontiSpec_Cat(key);
            ScontiSpeciali cs = repo.GetScontiSpecialiByID(key);
            repo.DeleteScontiSpeciali(cs);
            return RedirectToAction("Index");
        }


    }
}