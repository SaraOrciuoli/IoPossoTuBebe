using DAL.Model;
using DAL.Repository;
using System;
using System.Web;
using Admin.Security;
using System.Web.Mvc;
using DevExtreme.AspNet.Mvc;
using Admin.Models.CustomModels;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace Admin.Controllers
{
    
    public class ServiziController : Controller
    {
        internal ProdottiRepository repo = new ProdottiRepository();
        public ActionResult Index()
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

        public ActionResult GetServizi(DataSourceLoadOptions loadOptions) 
        {
            var result = DataSourceLoader.Load(repo.GetServizi(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public string GetServizioByBarCode(string barcode)
        {
            string retvalue = string.Empty;
            if (!String.IsNullOrEmpty(barcode))
            {
                retvalue = ProdottiRepository.GetServizioByBarCode(barcode);
            }
            return retvalue;
        }

        [HttpPost]
        public ActionResult UpdateServizio(int key, string values)
        {
            Prodotti p = repo.GetServizioById(key);
            JsonConvert.PopulateObject(values, p);
            repo.UpdateServizio(p);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddServizio(string values)
        {
            Prodotti p = new Prodotti();
            dynamic data = JObject.Parse(values);
            if(data.Descrizione != null) p.Descrizione = data.Descrizione;
            if(data.PrezzoAcquisto != null) p.PrezzoAcquisto = data.PrezzoAcquisto;
            if(data.PrezzoVendita != null) p.PrezzoVendita = data.PrezzoVendita;
            if(data.iva != null) p.iva = data.iva;
            if(data.BarCode != null) p.BarCode = data.BarCode;
            if(data.Descrizione_eng != null) p.Descrizione_eng = data.Descrizione_eng;
            if(data.Ingredienti != null) p.Ingredienti = data.Ingredienti;
            if (data.MostraWeb != null) p.MostraWeb = data.MostraWeb;
            p.is_servizio = true;
            repo.AddServizio(p);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteServizio(int key)
        {
            Prodotti p = repo.GetServizioById(key);
            repo.DeleteServizio(p);
            return RedirectToAction("Index");
        }

        public ActionResult GetIVA(DataSourceLoadOptions loadOptions)
        {
            List<int> IVA = new List<int>() { 4, 10, 22 };
            var result = DataSourceLoader.Load(IVA, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
    }
}