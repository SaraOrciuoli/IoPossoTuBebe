using DAL.Model;
using DAL.Repository;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Admin.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class FattureController : Controller
    {
        // GET: Fatture
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelFatture model = new ModelFatture();
                model.Count = FattureRepository.CountFatture();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult GetFatture(DataSourceLoadOptions loadOptions)
        {
            FattureRepository repository = new FattureRepository();
            var result = DataSourceLoader.Load(repository.GetFatture(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult UpdateFattura(int key, string values)
        {
            FattureRepository repository = new FattureRepository();
            repository.updateFatture(key, values);
            return RedirectToAction("Index");
        }

        public ActionResult AddFattura(string values)
        {
            FattureRepository repository = new FattureRepository();
            Fatture m = new Fatture();
            dynamic data = JObject.Parse(values);
            int id_fornitore;

            m.Id_tipo = data.Id_tipo;
            m.numeroFattura = data.numeroFattura;
            m.Valore = data.Valore;
            m.Data = data.Data;
            m.id_categoria = data.id_categoria;
            m.id_fornitore = data.id_fornitore;
            if (int.TryParse(data.id_fornitore.ToString(), out id_fornitore))
            {
                Fornitori fornitore = repository.getFornitoreById(id_fornitore);
                m.PartitaIva = fornitore.Descrizione_fornitore;
                m.RagioneSociale = fornitore.Partita_iva;
            }
            repository.AddFatture(m);
            return RedirectToAction("Index");
        }
    }
}