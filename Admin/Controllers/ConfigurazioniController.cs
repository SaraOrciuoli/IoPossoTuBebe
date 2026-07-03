using Admin.Models.CustomModels;
using DAL.Model;
using DAL.Repository;
using DevExpress.XtraReports;
using DevExpress.XtraRichEdit.Model;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace Admin.Controllers
{
    public class ConfigurazioniController : Controller
    {
        /*/////////////////////////Colori//////////////////////*/

        public ActionResult GetColori(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.getColori(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        public ActionResult GetSpessori(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.getSpessori(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        [HttpPost]
        public ActionResult AddColore(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            Colori c = new Colori();
            dynamic data = JObject.Parse(values);
            if (data.id_Colori != null) c.id_Colori = data.id_Colori;
            if (data.Descrizione_colore != null) c.Descrizione_colore = data.Descrizione_colore;
            repository.addColore(c);
            return RedirectToAction("IndexColori");
        }
        public ActionResult AddSpessore(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            Spessori c = new Spessori();
            dynamic data = JObject.Parse(values);
            if (data.id_spessore != null) c.id_spessore = data.id_spessore;
            if (data.descrizione_spessore != null) c.descrizione_spessore = data.descrizione_spessore;
            repository.addSpessore(c);
            return RedirectToAction("IndexSpessori");
        }

        public ActionResult UpdateColore(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();

            repository.updateColore(key, values);
            return RedirectToAction("IndexColori");
        }
        public ActionResult UpdateSpessore(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();

            repository.updateSpessore(key, values);
            return RedirectToAction("IndexSpessori");
        }

        public ActionResult DeleteColore(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteColore(key);
            return RedirectToAction("IndexColori");

        }
        public ActionResult DeleteSpessore(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteSpessore(key);
            return RedirectToAction("IndexSpessori");

        }


        /////////////////////////Marche////////////////////////

        public ActionResult IndexMarche()
          {
              HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
              if (reqCookies != null)
              {
                  MarcheModel model = new MarcheModel();
                  model.Count = ConfigurazioniRepository.CountMarche();
                  return View(model);
              }
              else
              {
                  return RedirectToAction("Login", "Home");
              }
          }
        public ActionResult IndexTestiHome()
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

        public ActionResult IndexToolbar()
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

        public ActionResult GetMarche(DataSourceLoadOptions loadOptions)
          {
              ConfigurazioniRepository repository = new ConfigurazioniRepository();
              ModelProdotti model = new ModelProdotti();
              
              var result = DataSourceLoader.Load(repository.GetMarche(), loadOptions);
              var resultJson = JsonConvert.SerializeObject(result);
              return Content(resultJson, "application/json");
          }

        public ActionResult GetTestiToolbar(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            ModelProdotti model = new ModelProdotti();

            var result = DataSourceLoader.Load(repository.getTestiToolbar(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateTestoToolbar(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.updateTestoToolbar(key, values);
            return RedirectToAction("IndexToolbar");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddTestoToolbar(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            DAL.Model.Toolbar m = new DAL.Model.Toolbar();
            dynamic data = JObject.Parse(values);
            if (data.id_testo_toolbar != null) m.id_testo_toolbar = data.id_testo_toolbar;
            if (data.testo_toolbar != null) m.testo_toolbar = data.testo_toolbar;


            repository.addTestoToolbar(m);
            return RedirectToAction("IndexToolbar");
        }

        public ActionResult DeleteTestoToolbar(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteTestoToolbar(key);
            return RedirectToAction("IndexToolbar");

        }
        public ActionResult UpdateMarca(int key, string values)
         {
             ConfigurazioniRepository repository = new ConfigurazioniRepository();
             repository.updateMarca(key, values);
             return RedirectToAction("IndexMarche");
         }

         public ActionResult AddMarchio(string values)
         {
             ConfigurazioniRepository repository = new ConfigurazioniRepository();
             Marchi m = new Marchi();
             dynamic data = JObject.Parse(values);
            if (data.id_marchio != null) m.id_marchio = data.id_marchio;
            if (data.nome_marchio != null) m.nome_marchio = data.nome_marchio;
            if (data.path_logo_marchio != null) m.path_logo_marchio = data.path_logo_marchio;

            repository.addMarca(m);
             return RedirectToAction("IndexMarchi");
         }

        public ActionResult DeleteMarca(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteMarca(key);
            return RedirectToAction("IndexMarchi");

        }


        //////////////////Categorie////////////////////////

        
       
        [HttpPost]
         public string UploadFile(string key)
         {
             var myFile = Request.Files["myFile"];
             var targetLocation = Server.MapPath("~/Content/Upload/");
             var uniqueFileName = string.Format("{0}_{1}{2}",
                 Path.GetFileNameWithoutExtension(myFile.FileName),
                 Request["key"],
                 Path.GetExtension(myFile.FileName));
             try
             {
                 var path = Path.Combine(targetLocation, uniqueFileName);
                 myFile.SaveAs(path);
                 Session["currentFilePath"] = Path.Combine("/Content/Upload/", uniqueFileName);
                 //if(key != "undefined")
                 //{
                 //    ConfigurazioniRepository repository = new ConfigurazioniRepository();
                 //    repository.UpdateCategoria(key, Session["currentFilePath"].ToString());
                 //}

             }
             catch
             {
                 Response.StatusCode = 400;
             }
             return Session["currentFilePath"].ToString();
         }

         [HttpPost]
         public string UploadImg()
         {
             string retvalue = string.Empty;
             var myFile = Request.Files["myFile"];
             var targetLocation = Server.MapPath("../Content/ImgCat/");
             var path = string.Empty;
             try
             
            {
                 path = Path.Combine(targetLocation, myFile.FileName.Replace(" ", "_"));
                 myFile.SaveAs(path);
             }
             catch
             {
                 Response.StatusCode = 400;
             }

             retvalue = Path.Combine("/Content/ImgCat/", myFile.FileName.Replace(" ", "_"));
             return retvalue;
         }
/*
        public string UploadFileCopertina(string key)
        {
            var myFile2 = Request.Files["myFile2"];
            var targetLocation = Server.MapPath("~/Content/Upload/");
            var uniqueFileName = string.Format("{0}_{1}{2}",
                Path.GetFileNameWithoutExtension(myFile2.FileName),
                Request["key"],
                Path.GetExtension(myFile2.FileName));
            try
            {
                var path = Path.Combine(targetLocation, uniqueFileName);
                myFile2.SaveAs(path);
                Session["currentFilePath"] = Path.Combine("/Content/Upload/", uniqueFileName);
                //if(key != "undefined")
                //{
                //    ConfigurazioniRepository repository = new ConfigurazioniRepository();
                //    repository.UpdateCategoria(key, Session["currentFilePath"].ToString());
                //}

            }
            catch
            {
                Response.StatusCode = 400;
            }
            return Session["currentFilePath"].ToString();
        }*/

        [HttpPost]
        public string UploadImgCopertina()
        {
            string retvalue = string.Empty;
            var myFile2 = Request.Files["myFile2"];
            var targetLocation = Server.MapPath("../Content/ImgCat/");
            var path = string.Empty;
            try

            {
                path = Path.Combine(targetLocation, myFile2.FileName.Replace(" ", "_"));
                myFile2.SaveAs(path);
            }
            catch
            {
                Response.StatusCode = 400;
            }

            retvalue = Path.Combine("/Content/ImgCat/", myFile2.FileName.Replace(" ", "_"));
            return retvalue;
        }
        



        /*/////////////////////////Modelli//////////////////////*/
        public ActionResult IndexModelli()
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

        public ActionResult GetModelli(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.getModelli(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult AddModello(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            Modello m = new Modello();
            dynamic data = JObject.Parse(values);
            if (data.id_modello != null) m.id_modello = data.id_modello;
            if (data.Descrizione_modello != null) m.Descrizione_modello = data.Descrizione_modello;
            if (data.id_marca != null) m.id_marca = data.id_marca;
            repository.addModello(m);
            return RedirectToAction("IndexModelli");
        }

        public ActionResult UpdateModello(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();

            repository.updateModello(key, values);
            return RedirectToAction("IndexModelli");
        }

        public ActionResult DeleteModello(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteModello(key);
            return RedirectToAction("IndexModelli");

        }
        public ActionResult IndexTaglie()
        {
            GruppiTagliaRepository repo_gt = new GruppiTagliaRepository();
            ConfigurazioniRepository repo_t = new ConfigurazioniRepository();
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                GruppoTaglieModel model = new GruppoTaglieModel();
                model.taglie = (List<Taglie>)repo_t.getTaglie();
                model.gruppi_taglie = repo_gt.getGruppiTaglia();
                return View("IndexTaglie", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //public ActionResult IndexColori()
        //{
        //    GruppiTagliaRepository repo_gt = new GruppiTagliaRepository();
        //    ConfigurazioniRepository repo_t = new ConfigurazioniRepository();
        //    HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
        //    if (reqCookies != null)
        //    {
        //        GruppoTaglieModel model = new GruppoTaglieModel();
        //        model.taglie = (List<Taglie>)repo_t.getTaglie();
        //        model.gruppi_taglie = repo_gt.getGruppiTaglia();
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}

        public ActionResult IndexColori()
        {
            GruppiTagliaRepository repo_gt = new GruppiTagliaRepository();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];

            if (reqCookies != null)
            {
                GruppoTaglieModel model = new GruppoTaglieModel();
                model.colori = (List<Colori>)repo.getColori(); // 
                model.gruppi_colori = repo_gt.getGruppiColori(); // Nuovo metodo
                return View("IndexColori", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult IndexSpessori()
        {
            GruppiTagliaRepository repo_gt = new GruppiTagliaRepository();
            ConfigurazioniRepository repo = new ConfigurazioniRepository();
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];

            if (reqCookies != null)
            {
                GruppoTaglieModel model = new GruppoTaglieModel();
                model.spessori = (List<Spessori>)repo.getSpessori(); // 
                model.gruppi_spessori = repo_gt.getGruppiSpessori(); // Nuovo metodo
                return View("IndexSpessori", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //////////////////Tipi Fatture/////////////////////////////////
        /*  public ActionResult IndexTipoFatture()
          {
              HttpCookie reqCookies = Request.Cookies["userInfoBar"];
              if (reqCookies != null)
              {
                  Tipo_Fatture model = new Tipo_Fatture();
                  model.Count = ConfigurazioniRepository.CountTipo_Fatture();
                  return View(model);
              }
              else
              {
                  return RedirectToAction("Login", "Home");
              }
          }

          public ActionResult GetTipiFatture(DataSourceLoadOptions loadOptions)
          {
              ConfigurazioniRepository repository = new ConfigurazioniRepository();
              var result = DataSourceLoader.Load(repository.GetTipiFatture(), loadOptions);
              var resultJson = JsonConvert.SerializeObject(result);
              return Content(resultJson, "application/json");
          }

          public ActionResult UpdateTipoFatture(int key, string values)
          {
              ConfigurazioniRepository repository = new ConfigurazioniRepository();
              repository.updateTipoFatture(key, values);
              return RedirectToAction("IndexTipoFatture");
          }

          public ActionResult AddTipoFatture(string values)
          {
              ConfigurazioniRepository repository = new ConfigurazioniRepository();
              Tipo_Fatture m = new Tipo_Fatture();
              dynamic data = JObject.Parse(values);
              m.Descrizione = data.Descrizione;
              repository.AddTipoFatture(m);
              return RedirectToAction("IndexFile");
          }*/


        ////////////////Stampanti////////////////// 

        /* public ActionResult IndexStampanti()
         {
             HttpCookie reqCookies = Request.Cookies["userInfoBar"];
             if (reqCookies != null)
             {
                 ConfigurazioniRepository repository = new ConfigurazioniRepository();
                 ModelStampanti model = new ModelStampanti();
                 model.Count = ConfigurazioniRepository.CountStampanti();
                 model.ListaTipiStampanti = repository.GetListaTipiStampanti();

                 return View(model);
             }
             else
             {
                 return RedirectToAction("Login", "Home");
             }
         }

         public ActionResult IndexTipiStampanti()
         {
             HttpCookie reqCookies = Request.Cookies["userInfoBar"];
             if (reqCookies != null)
             {
                 ModelStampanti model = new ModelStampanti();
                 model.Count = ConfigurazioniRepository.CountTipiStampanti();
                 return View(model);
             }
             else
             {
                 return RedirectToAction("Login", "Home");
             }
         }

         public ActionResult EditAssociazione(string id)
         {
             HttpCookie reqCookies = Request.Cookies["userInfoBar"];
             if (reqCookies != null)
             {
                 ConfigurazioniRepository repository = new ConfigurazioniRepository();
                 ModelStampanti model = new ModelStampanti();
                 var s = repository.GetDettaglioStampantiById(id);
                 model.Count = ConfigurazioniRepository.CountStampantiCategorie(id);*/
        //string message = null;
        /*if (TempData["messaggio"] != null)
        {
           TempData["messaggio"] = null;
        }*//*
        Session["idStampante"] = Convert.ToInt32(id);
        model.IdStampante = Convert.ToInt32(id);
        ViewBag.Stampante = s.nome;
        ViewBag.TipoStampante = s.tipo_stampante;
        //ViewBag.Message = message;
        return View(model);
    }
    else
    {
        return RedirectToAction("Login", "Home");
    }
}

public string GetDataStampa()
{
     return TempData["messaggio"].ToString();
}

public ActionResult GetStampanti(DataSourceLoadOptions loadOptions)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    var result = DataSourceLoader.Load(repository.GetListaStampanti(), loadOptions);
    var resultJson = JsonConvert.SerializeObject(result);
    return Content(resultJson, "application/json");
}

[HttpPost]
public ActionResult UpdateStampante(int key, string values)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    repository.updateStampante(key, values);
    return RedirectToAction("IndexStampanti");
}

public ActionResult AddStampante(string values)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    Stampanti s = new Stampanti();
    dynamic data = JObject.Parse(values);
    s.nome = data.nome;
    s.ip = data.ip;
    s.tipo = data.tipo;
    repository.addStampante(s);
    return RedirectToAction("IndexStampanti");
}

[HttpPost]
public ActionResult DeleteStampante(int key)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    var s = repository.GetStampantiById(key.ToString());
    s.eliminato = true;
    repository.deleteStampante(s);
    return RedirectToAction("IndexStampanti");
}

*//*        [HttpPost]
        public ActionResult DeleteTavolo(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var t = repository.GetTavoloById(key.ToString());
            t.eliminato = true;
            repository.deleteTavolo(t);
            return RedirectToAction("IndexTavoli");
        }*//*

public ActionResult GetTipiStampanti(DataSourceLoadOptions loadOptions)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    var result = DataSourceLoader.Load(repository.GetListaTipiStampanti(), loadOptions);
    var resultJson = JsonConvert.SerializeObject(result);
    return Content(resultJson, "application/json");
}

public ActionResult AddTipoStampante(string values)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    TipoStampante ts = new TipoStampante();
    dynamic data = JObject.Parse(values);
    ts.tipo_stampante = data.tipo_stampante;
    repository.addTipoStampante(ts);
    return RedirectToAction("IndexTipiStampanti");
}

[HttpPost]
public ActionResult UpdateTipoStampante(int key, string values)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    repository.updateTipoStampante(key, values);
    return RedirectToAction("IndexTipiStampanti");
}

[HttpPost]
public ActionResult DeleteTipoStampante(int key)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    var t = repository.GetTipoStampantiById(key.ToString());
    t.eliminato = true;
    repository.deleteTipoStampante(t);
    return RedirectToAction("IndexStampanti");
}

public ActionResult GetCategorieStampanti(DataSourceLoadOptions loadOptions, int idStampa)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    //int id = Convert.ToInt32(Session["idStampante"]);
    var result = DataSourceLoader.Load(repository.GetListaCategorieStampanti(idStampa), loadOptions);
    var resultJson = JsonConvert.SerializeObject(result);
    return Content(resultJson, "application/json");
}

public ActionResult AddCategoriaStampante(string values)
{
    ConfigurazioniRepository repository = new ConfigurazioniRepository();
    AssociazioneStampantiCategorie asc = new AssociazioneStampantiCategorie();
    dynamic data = JObject.Parse(values);
    asc.stampante = Convert.ToInt32(Session["idStampante"]);
    asc.categoria = data.categoria;
    var oldAsc = repository.GetListaAssociazioniByIdStampante(*//*(int)asc.stampante*//*);*/
        /*    string permit = "";
            foreach (var a in oldAsc)
            {
                if (asc.categoria == a.categoria && a.eliminato != true)
                {
                    permit = "NO";
                }
        }
        if (permit == "")
        {
            TempData["messaggio"] = "SI";
            repository.addCategoriaStampante(asc);
        }
        else
        {
            TempData["messaggio"] = permit;
        }
        return RedirectToAction("EditAssociazione", new { id = asc.stampante });
        }


        [HttpPost]
        public ActionResult DeleteCategoriaStampante(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var asc = repository.GetAssociazioneById(key.ToString());
            int id = (int)asc.stampante;
            asc.eliminato = true;
            repository.deleteCategoriaStampante(asc);
            TempData["messaggio"] = "DELETE";
            return RedirectToAction("EditAssociazione", new { id = id });
        }

        public ActionResult GetTipiMovimentoMagazzino(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(ConfigurazioniRepository.GetTipiMovimentoMagazzino(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }*/
        //////////////////////////Fornitori//////////////////////////////////
        public ActionResult IndexFornitori()
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

public ActionResult GetFornitori(DataSourceLoadOptions loadOptions)
{
    var result = DataSourceLoader.Load(ConfigurazioniRepository.GetFornitori(), loadOptions);
    var resultJson = JsonConvert.SerializeObject(result);
    return Content(resultJson, "application/json");
}


        [HttpPost]
        public ActionResult UpdateFornitore(int key, string values)
        {
            ConfigurazioniRepository.updateFornitore(key, values);
            return RedirectToAction("IndexFornitori", "Configurazioni");
        }

        public ActionResult AddFornitore(string values)
        {
            Fornitori m = new Fornitori();
            dynamic data = JObject.Parse(values);
            if (data.Descrizione_fornitore != null) m.Descrizione_fornitore = data.Descrizione_fornitore;
            if (data.Cap != null) m.Cap = data.Cap;
            if (data.Citta != null) m.Citta = data.Citta;
            if (data.Codice_univoco != null) m.Codice_univoco = data.Codice_univoco;
            if (data.Email != null) m.Email = data.Email;
            if (data.Iban != null) m.Iban = data.Iban;
            /*if (data.id_referente != null) m.id_referente = data.id_referente;*/
            if (data.indirizzo != null) m.indirizzo = data.indirizzo;
            if (data.num_Telefono != null) m.num_Telefono = data.num_Telefono;
            if (data.Partita_iva != null) m.Partita_iva = data.Partita_iva;
            if (data.pec != null) m.pec = data.pec;
            if (data.Sito_web != null) m.Sito_web = data.Sito_web;

            ConfigurazioniRepository.addFornitore(m);
            return RedirectToAction("IndexFornitori", "Configurazioni");
        }

        public ActionResult DeleteFornitore(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteFornitore(key);
            return RedirectToAction("IndexMarche");

        }
        /*public ActionResult CambioLingua(string lingua)
        {
            Session["SessioneLingua"] = lingua;
            return Json("OK");
        }*/


        /// //////////////////////Marchi ///////////////////////////////////
        public ActionResult IndexMarchi()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                return View("IndexMarchi");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }



        /*/////////////////////////Tag//////////////////////*/
        public ActionResult IndexTag()
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

        public ActionResult GetTags(DataSourceLoadOptions loadOptions) {
            TagRepository repo = new TagRepository();
            var resultJson = JsonConvert.SerializeObject(repo.GetTags());
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult UpdateTag(int key, string values) {
            TagRepository repo = new TagRepository();
            repo.UpdateTag(key, values);
            return RedirectToAction("IndexTag");
        }
        public ActionResult AddTag(string values) {
            TagRepository repo = new TagRepository();
            dynamic data = JObject.Parse(values);

            Tag tag = new Tag();
            if(data.descrizione_tag != null && data.descrizione_tag != "")
            {
                tag.descrizione_tag = data.descrizione_tag;
                repo.SaveTag(tag);
            }
            return RedirectToAction("IndexTag");
        }
        public ActionResult DeleteTag(int key) {
            TagRepository repo = new TagRepository();
            Tag tag = repo.GetTagById(key);
            if (tag != null) repo.deleteTag(tag);
            return RedirectToAction("IndexTag");
        }

        //////////////////Tipi Fatture/////////////////////////////////
        public ActionResult IndexTipoFatture()
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

        public ActionResult GetTipiFatture(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.GetTipiFatture(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult UpdateTipoFatture(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.updateTipoFatture(key, values);
            return RedirectToAction("IndexTipoFatture");
        }

        public ActionResult AddTipoFatture(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            Tipo_Fatture m = new Tipo_Fatture();
            dynamic data = JObject.Parse(values);
            m.Descrizione = data.Descrizione;
            repository.AddTipoFatture(m);
            return RedirectToAction("IndexTipoFatture");
        }
        //////////////////Categorie Fatture/////////////////////////////////
        public ActionResult IndexCategorieFatture()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                return View("IndexCategorieFatture");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult GetCategorieFatture(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.GetCategorieFatture(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult UpdateCategorieFatture(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.updateCategorieFatture(key, values);
            return RedirectToAction("IndexTipoFatture");
        }

        public ActionResult AddCategorieFatture(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            CategorieFattura m = new CategorieFattura();
            dynamic data = JObject.Parse(values);
            m.Descrizione = data.Descrizione;
            repository.AddCategorieFatture(m);
            return RedirectToAction("IndexCategorieFatture");
        }

        public ActionResult GetFornitoriFatture(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.GetFornitoriFatture(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }



        /////////////////////// Tipi CodiceSconto ////////////////
        public ActionResult IndexTipiSconto()
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

        public ActionResult GetTipiSconto(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.GetTipiSconto(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
         public ActionResult GetTestiHome(DataSourceLoadOptions loadOptions)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            var result = DataSourceLoader.Load(repository.GetTestiHome(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult AddTipoSconto(string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            TipiSconto ts = new TipiSconto();
            dynamic data = JObject.Parse(values);
            if (data.id_tipoSconto != null) ts.id_tipoSconto = data.id_tipoSconto;
            if (data.DescrizioneSconto != null) ts.DescrizioneSconto = data.DescrizioneSconto;
            repository.addTipoSconto(ts);
            return RedirectToAction("IndexTipiSconto");
        }

        public ActionResult UpdateTipoSconto(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();

            repository.updateTipoSconto(key, values);
            return RedirectToAction("IndexTipiSconto");
        }
        public ActionResult UpdateTestoHome(int key, string values)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();

            repository.updateTestoHome(key, values);
            return RedirectToAction("IndexTipiSconto");
        }

        public ActionResult DeleteTipoSconto(int key)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.deleteTipoSconto(key);
            return RedirectToAction("IndexTipiSconto");

        }


    }
}





