using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Admin.Models.CustomModels;
using Admin.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Repository;
using DAL.Model;
using Newtonsoft.Json.Linq;

namespace Admin.Controllers
{
    public class UtentiController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null && reqCookies["IdRuolo"] == "1")
            {
                ModelListaUtenti modelLista = new ModelListaUtenti();
                modelLista.Count = UtentiRepository.CountUtenti();
                return View(modelLista);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult GetUtenti(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(UtentiRepository.GetUtenti(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

      /*  public ActionResult GetClienti(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(UtentiRepository.GetClienti(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }*/
        public ActionResult GetUtentiById(int id)
        {
            UtentiRepository repository = new UtentiRepository();
            ModelListaUtenti model = new ModelListaUtenti();
            model.Utenti = repository.GetUtenteById(id);
            var Cliente = Newtonsoft.Json.JsonConvert.SerializeObject(model.Utenti);
            return Json(Cliente, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult DeleteUtenti(int key)
        {
            UtentiRepository.deleteUtenti(key);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateUtenti(int key, string values)
        {
            UtentiRepository repo = new UtentiRepository();
            Utenti u = repo.GetUtenteById(key);

            // Popoliamo l'oggetto con i nuovi valori (se la password è stata cambiata, qui è ancora in chiaro)
            Newtonsoft.Json.JsonConvert.PopulateObject(values, u);

            // Controlliamo se nel JSON inviato dalla griglia è presente il campo Password
            // Sostituisci "Password" con il nome esatto della proprietà nel tuo modello, se diverso (es. "password", "Pwd")
            bool passwordModificata = Newtonsoft.Json.Linq.JObject.Parse(values).ContainsKey("Password");

            // Passiamo l'oggetto e il flag al repository
            repo.UpdateUtente(u, passwordModificata);

            // DevExtreme si aspetta un OK per chiudere il popup di caricamento
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        public ActionResult AddUtenti(string values)
        {
            UtentiRepository repo = new UtentiRepository();
            dynamic data = JObject.Parse(values);

            Utenti u = new Utenti();
            if(data.Nome != null) u.Nome = data.Nome;
            if(data.Cognome != null) u.Cognome = data.Cognome;
            if(data.Cellulare != null) u.Cellulare = data.Cellulare;
            if(data.Mail != null) u.Mail = data.Mail;
            if(data.id_Ruolo != null) u.id_Ruolo = data.id_Ruolo;
            if (data.Password != null) u.Password = data.Password;

            if (data.Consenso != false)
            {
                u.Consenso = data.Consenso;
                u.DataConsenso = DateTime.Now;
            }
            string passwordGenerata = u.Password;
            u.Password = BCrypt.Net.BCrypt.HashPassword(passwordGenerata);
            repo.SalvaUtente(u);

            return RedirectToAction("Index");
        }

        public ActionResult GetRuoliUtente(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.getRuoliUtente(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
    }
}