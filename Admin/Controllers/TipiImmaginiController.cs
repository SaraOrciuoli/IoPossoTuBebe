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
    public class TipiImmaginiController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                TipiImmaginiRepository repo = new TipiImmaginiRepository();
                ModelTipiImmagini model = new ModelTipiImmagini();
                model.all_tipi_immagini = repo.getTipiImmagini();
                return View("IndexTipiImmagini", model);
            }
            else
            {
                return RedirectToAction("Home/Login");
            }
        }

        public ActionResult GetTipiImmagini(DataSourceLoadOptions loadOptions)
        {
            TipiImmaginiRepository repo = new TipiImmaginiRepository();
            var result = DataSourceLoader.Load(repo.getTipiImmagini(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        
        public ActionResult DeleteTipiImmagini(string key) 
        {
            TipiImmaginiRepository repo = new TipiImmaginiRepository();
            if(!String.IsNullOrEmpty(key))
            {
                repo.deleteTipiImmagini(repo.getTipiImmaginiById(Convert.ToInt32(key)));
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult AddTipiImmagini(string values) {
            TipiImmaginiRepository repo = new TipiImmaginiRepository();
            Tipi_Immagini tp = new Tipi_Immagini();
            dynamic data = JObject.Parse(values);

            if(data.Descrizione_tipo_Immagine != null) tp.Descrizione_tipo_Immagine = data.Descrizione_tipo_Immagine;
            if(data.Prodotto != null) tp.Prodotto = data.Prodotto;
            if(data.Categoria != null) tp.Categoria = data.Categoria;
            if(data.Width != null) tp.Width = data.Width;
            if(data.Height != null) tp.Height = data.Height;                   
            repo.AddTipiImmagini(tp);
            return RedirectToAction("Index");
        }
        
        public ActionResult UpdateTipiImmagini(int key, string values)
        {
            TipiImmaginiRepository repo = new TipiImmaginiRepository();
            repo.UpdateTipiImmagini(key, values);
            return RedirectToAction("Index");
        }
    }
}