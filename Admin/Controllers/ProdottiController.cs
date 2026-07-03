using Admin.Models.CustomModel;
using Admin.Models.CustomModels;
using Admin.Utility;
using DAL.Model;
using DAL.Repository;
using DevExpress.Xpo;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraSpreadsheet.Model;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZXing;
using static Admin.Models.CustomModels.ModelEditProdotto;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;




namespace Admin.Controllers
{
    public class ProdottiController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditProdotto model = new ModelEditProdotto();

                if (Session["SessioneLingua"] == null)
                {
                    Session["SessioneLingua"] = "Ita";
                }
                if (Session["SessioneLingua"].ToString() == "Ita")
                {
                    model.Ita = true;
                    model.Eng = false;
                }
                else
                {
                    model.Ita = false;
                    model.Eng = true;
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult BarcodeProdotti(int id)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                ModelEditProdotto model = new ModelEditProdotto();
                DettagliProdottiRepository repo = new DettagliProdottiRepository();
                TagRepository repo_tag = new TagRepository();

                model.Prodotto = repo.GetProdottiById(id);
                if (model.Prodotto != null)
                {
                    model.DettaglioProdotti = repo.GetDettaglioProdottiById(id);
                    model.dettaglioProdottiTaglie = repo.getListTaglieByProdId(id);
                    Session["TaglieProd"] = model.dettaglioProdottiTaglie;
                }

                Session["Product"] = id != null ? id.ToString() : "";
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }





        [HttpPost]
        public ActionResult SalvaImmaginiGalleria(RichiestaSalvataggioImmagini dati)
        {
            try
            {
                // Sostituisci "AnyeLabelEntities" con il nome esatto del tuo DB Context se diverso
                using (var db = new DAL.Model.AnyeLabelEntities())
                {
                    var prodotto = db.Prodotti.FirstOrDefault(p => p.id_prodotto == dati.idProdotto);
                    if (prodotto == null) return Json(new { success = false, message = "Prodotto non trovato." });

                    foreach (var img in dati.immagini)
                    {

                        if (img.IdTipoImmagine == 2)
                        {
                            prodotto.pathFoto = img.PathFoto;
                            db.Entry(prodotto).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            var nuovaImmagine = new Immagini_catProd
                            {
                                id_prod = dati.idProdotto,
                                path = img.PathFoto,
                                id_tipo_immagine = img.IdTipoImmagine,
                                Descrizione = dati.descrizione,
                                Alt = dati.altTesto,
                                Link = dati.link
                            };
                            db.Immagini_catProd.Add(nuovaImmagine);
                        }
                    }

                    db.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult StampaSingoloBarcodePdf(int id,string barcode, string descrizioneTaglia, string nomeProdotto)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            var prodotto = repo.GetProdottiById(id);
            string barcodeUrl = Url.Action("GenerateBarcode", "Prodotti", new { code = barcode }, Request.Url.Scheme);

            Utility.PdfUtility pdf = new Utility.PdfUtility();
            return pdf.PdfSingoloBarcode(prodotto.Descrizione, descrizioneTaglia, barcode, barcodeUrl);
        }

        public ActionResult StampaBarcodes(int id)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            var prodotto = repo.GetProdottiById(id);
            var taglie = repo.getListTaglieByProdId(id);

            // Genera HTML per il PDF
            StringBuilder htmlString = new StringBuilder();
            htmlString.Append("<html><head><style>");
            htmlString.Append("body {");
            htmlString.Append("margin-top: 30px;");
            htmlString.Append("display: flex;");
            htmlString.Append("justify-content: center;");
            htmlString.Append("align-items: center;");
            htmlString.Append("text-align: center;");
            htmlString.Append("}");
            htmlString.Append("table {");
            htmlString.Append("width: 100%;");
            htmlString.Append("height: 100%;");
            htmlString.Append("border-collapse: collapse;");
            htmlString.Append("}");
            htmlString.Append("td {");
            htmlString.Append("vertical-align: middle;");
            htmlString.Append("text-align: center;");
            htmlString.Append("}");
            htmlString.Append("h3 {");
            htmlString.Append("font-size: 44px;"); // Dimensione del font per il nome del prodotto
            htmlString.Append("margin: 0;");
            htmlString.Append("}");
            htmlString.Append("img {");
            htmlString.Append("width: 80%;"); // L'immagine occuperà l'intera larghezza della cella
            htmlString.Append("height: auto;"); // Mantiene le proporzioni
            htmlString.Append("max-height: 100%;"); // Limita l'altezza massima dell'immagine
            htmlString.Append("}");
            htmlString.Append("</style></head><body>");

            htmlString.Append("<table>");
            htmlString.Append("<tr>");
            htmlString.Append("<td>");

            htmlString.Append("<br><br><br><br><br>");

            htmlString.Append($"<h3>{HttpUtility.HtmlEncode(prodotto.Descrizione)}</h3>");

            if(prodotto.in_offerta == true && prodotto.sconto != null && prodotto.fine_offerta > DateTime.Now)
            {
                htmlString.Append($"<h3>&euro; {HttpUtility.HtmlEncode(prodotto.PrezzoVendita - (prodotto.PrezzoVendita * prodotto.sconto / 100))}</h3>");

            }
            else
            {
                htmlString.Append($"<h3>&euro; {HttpUtility.HtmlEncode(prodotto.PrezzoVendita)}</h3>");

            }
            htmlString.Append("<br><br>");

            string barcodeUrl = Url.Action("GenerateBarcode", "Prodotti", new { code = prodotto.BarCode }, Request.Url.Scheme);
            htmlString.Append($"<img src='{HttpUtility.HtmlEncode(barcodeUrl)}' alt='Barcode' />");

            htmlString.Append("<br><br><br>");

            htmlString.Append("</td>");
            htmlString.Append("</tr>");
            htmlString.Append("</table>");

            htmlString.Append("</body></html>");

            // Crea il PDF
            Utility.PdfUtility pdf = new Utility.PdfUtility();
            return pdf.PdfComplete(prodotto.Descrizione, htmlString.ToString());
        }


        public ActionResult GenerateBarcode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 100,
                    Width = 300
                }
            };

            using (var bitmap = barcodeWriter.Write(code))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }
        }


        public ActionResult GetProdotti(DataSourceLoadOptions loadOptions)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            var result = DataSourceLoader.Load(repository.GetDettaglioProdotti(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult Edit(string id)
        {
            ModelEditProdotto model = new ModelEditProdotto();
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            ConfigurazioniRepository repo_conf = new ConfigurazioniRepository();
            TagRepository repo_tag = new TagRepository();
            CategorieRepository repo_cat = new CategorieRepository();
            CollezioniRepository repo_coll = new CollezioniRepository(); 
            Session["ImgsProd"] = null;

            model.DettaglioProdotti = new DettaglioProdotti();
            model.Prodotto = new Prodotti();
            model.ListaSottocategorie = new List<Sottocategorie>();

         
            if (!String.IsNullOrEmpty(id))
            {
                int idProd = Convert.ToInt32(id);
                model.Prodotto = repo.GetProdottiById(idProd);

                if (model.Prodotto != null)
                {
                    model.DettaglioProdotti = repo.GetDettaglioProdottiById(idProd);
                    model.id_taglie = new List<int>();
                    model.tags_id = repo_tag.getTagsIdByProduct(idProd);
                    model.ListaSottocategorie = repo_cat.getSottocategorieByIdCat(model.Prodotto.id_categoria);

                    model.collezioni_id = repo_coll.GetCollezioniIdByProdotto(idProd);
                }
            }
            Session["Product"] = id ?? "";
            return View(model);
        }

        public ActionResult GetImgProductByIdProd(string idProd, DataSourceLoadOptions loadOptions)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            List<Immagini_catProd> prod_imgs = new List<Immagini_catProd>();
            if (!String.IsNullOrEmpty(idProd)) prod_imgs = repository.getImgProductByIdProd(Convert.ToInt32(idProd));

            List<Immagini_catProd> ListaImmaginiSession = Session["ImgsProd"] as List<Immagini_catProd>;
            LoadResult result = new LoadResult();

            if (prod_imgs.Count() > 0)
            {
                result = DataSourceLoader.Load(prod_imgs, loadOptions);
            }
            else
            {
                if (ListaImmaginiSession != null && ListaImmaginiSession.Count() > 0) result = DataSourceLoader.Load(ListaImmaginiSession, loadOptions);
            }
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult UpdateImgProduct(int key, string values)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();

            repository.updateImgProduct(key, values);
            return Content("OK");
        }

        public ActionResult AddImgProduct(int id_prod, string values)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(values);
            List<Immagini_catProd> ListaImmaginiSession = new List<Immagini_catProd>();
            Immagini_catProd img_CatProd = repository.getImgCatProdById(Convert.ToInt32(data.id_immagini));
            if(img_CatProd == null)
            {
                img_CatProd = new Immagini_catProd();
            }
            if (data.id_immagini != null) img_CatProd.id_immagini = data.id_immagini;
            if (data.Descrizione != null) img_CatProd.Descrizione = data.Descrizione;
            if (data.id_tipo_immagine != null) img_CatProd.id_tipo_immagine = data.id_tipo_immagine;
            if (data.path != null) img_CatProd.path = data.path;
            if (data.Alt != null) img_CatProd.Alt = data.Alt;
            if (data.Link != null) img_CatProd.Link = data.Link;


            if (id_prod != 0)
            {
                //salvataggio su DB delle immagini prodotto

                Prodotti prod = repository.GetProdottiById(id_prod);
                
                if (prod != null)
                {
                    img_CatProd.id_prod = prod.id_prodotto;
                    if(img_CatProd.id_immagini == 0)
                    {
                        repository.addImgProduct(img_CatProd);
                    } else
                    {
                        repository.updateImgCatProd(img_CatProd);
                    }
                }
            }
            else
            {
                // salvataggio in sessione delle immagini categoria
                ListaImmaginiSession = Session["ImgsProd"] == null ? new List<Immagini_catProd>() : Session["ImgsProd"] as List<Immagini_catProd>;
                Immagini_catProd el = ListaImmaginiSession.Where(x => x.path == img_CatProd.path).FirstOrDefault();
                if(el != null)
                {
                    ListaImmaginiSession.Remove(el);
                }
                ListaImmaginiSession.Add(img_CatProd);
                Session["ImgsProd"] = ListaImmaginiSession;
            }
            return Content(data.path.ToString());
        }

        public ActionResult GetTipoImgProd(DataSourceLoadOptions loadOptions)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            var result = DataSourceLoader.Load(repository.GetTipoImgProd(), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public ActionResult DeleteProdotto(int key)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                DettagliProdottiRepository repository = new DettagliProdottiRepository();
                SliderRepository sl_repo = new SliderRepository();

                Prodotti prod = repository.GetProdottiById(key);
                foreach (Immagini_catProd img in repository.getImgProductByIdProd(key))
                {
                    repository.deleteImgProduct(img);
                }
                foreach (ProdottoTaglia pt in repository.getProdottoTagliaByIdProdotto(key))
                {
                    repository.deleteTagliaProdotto(pt);
                }
                foreach(Slider sl in repository.getSliderByProd(key))
                {
                    sl_repo.deleteSlider(sl.id_immagine);
                }
                repository.SvuotaTagProdotto(key);
                repository.deleteProdotto(prod);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        //public ActionResult SalvaProdotto(ModelEditProdotto model)
        //{
        //    DettagliProdottiRepository repository = new DettagliProdottiRepository();
        //    TagRepository repoTag = new TagRepository();
        //    List<int> taglie_id = model.id_taglie;
        //    List<int> tags_id = model.tags_id;
        //    model.Prodotto.id_gruppo_spessori = model.id_gruppo_spessori;
        //    model.Prodotto.id_gruppo_taglie = model.id_gruppo_taglie;
        //    model.Prodotto.id_gruppo_colori = model.id_gruppo_colori;
        //    if (String.IsNullOrEmpty(model.Prodotto.BarCode)) model.Prodotto.BarCode = this.GetNewBarcode(model.Prodotto.id_prodotto.ToString());
        //    if (model.Prodotto.id_prodotto != 0)
        //    {
        //        repository.UpdateProdotto(model.Prodotto);
        //        repository.SvuotaTagProdotto(model.Prodotto.id_prodotto);
        //    }
        //    else
        //    {
        //        int id_prod = repository.AddProdotto(model.Prodotto);
        //        this.SaveSessionProdImgs(id_prod);
        //        //this.SaveSessionTaglieProd(id_prod);
        //    }
        //    repository.GeneraCombinazioniProdotto(
        //                model.Prodotto.id_prodotto,
        //                model.Prodotto.id_gruppo_taglie,
        //                model.Prodotto.id_gruppo_colori,
        //                model.Prodotto.id_gruppo_spessori,
        //                (double)model.Prodotto.PrezzoVendita
        //    );

        //    this.SaveProdTag(model.Prodotto.id_prodotto, tags_id);
        //    Session["TaglieProd"] = null;
        //    return RedirectToAction("Index");
        //    //return Content("ok", "json");
        //}
        [HttpGet]
        public ActionResult GetProdottiPerSelectBox()
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            // Recupera una lista leggera per la tendina del popup
            var prodotti = repo.GetDettaglioProdotti() 
                               .Where(p => p.eliminato == false)
                               .Select(p => new {
                                   id_prodotto = p.id_prodotto,
                                   Descrizione = p.Descrizione
                               })
                               .OrderBy(p => p.Descrizione)
                               .ToList();

            return Json(prodotti, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DuplicaProdottiMassivo(int idProdottoSorgente, int numeroCopie)
        {
            try
            {
                if (numeroCopie < 1 || idProdottoSorgente <= 0)
                    return Json(new { success = false, message = "Dati non validi." });

                DettagliProdottiRepository repo = new DettagliProdottiRepository();

                repo.DuplicaProdotto(idProdottoSorgente, numeroCopie);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult SalvaProdotto(ModelEditProdotto model, string azioneSuccessiva)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            TagRepository repoTag = new TagRepository();

            List<int> taglie_id = model.id_taglie;
            List<int> tags_id = model.tags_id;
            List<int> collezioni_id = model.collezioni_id;
            model.Prodotto.id_categoria = 1;

            if (String.IsNullOrEmpty(model.Prodotto.BarCode))
                model.Prodotto.BarCode = this.GetNewBarcode(model.Prodotto.id_prodotto.ToString());

            int prodottoId;

            if (model.Prodotto.id_prodotto != 0)
            {
                var prodottoPrecedente = repository.GetProdottiById(model.Prodotto.id_prodotto);

                var prezzoPrecedente = prodottoPrecedente.PrezzoVendita;

                repository.UpdateProdotto(model.Prodotto);
                //repository.SvuotaTagProdotto(model.Prodotto.id_prodotto);
                repository.SvuotaCollezioniProdotto(model.Prodotto.id_prodotto); // NUOVO: Puliamo le vecchie associazioni

                prodottoId = model.Prodotto.id_prodotto;

            }
            else
            {
                prodottoId = repository.AddProdotto(model.Prodotto);
                this.SaveSessionProdImgs(prodottoId);

            }

            // Salviamo Tag e Collezioni per il nuovo prodotto (o quello aggiornato)
            this.SaveProdTag(prodottoId, tags_id);
            repository.SaveProdCollezioni(prodottoId, collezioni_id); 

            Session["TaglieProd"] = null;

            if (azioneSuccessiva == "chiudi")
            {
                return RedirectToAction("Index", "Prodotti"); 
            }

            return RedirectToAction("Edit", "Prodotti", new { id = prodottoId });
        }



        [HttpGet]
        public object GetAllProdottiForLookup(DataSourceLoadOptions loadOptions)
        {
            using (var db = new AnyeLabelEntities())
            {
                var prod = db.Prodotti.Select(p => new { p.id_prodotto, p.Descrizione }).ToList();
                return DataSourceLoader.Load(prod, loadOptions);
            }
        }

        [HttpPost]
        public JsonResult AggiornaPrezziPerCriteri(int idProdotto, string dimensioniIds, string coloriIds, string spessoriIds, decimal incremento, string tipoOperazione)
        {
            try
            {
                var repo = new DettagliProdottiRepository();

                // Deserializza gli ID selezionati
                var dimensioniIdsList = JsonConvert.DeserializeObject<List<int>>(dimensioniIds ?? "[]");
                var coloriIdsList = JsonConvert.DeserializeObject<List<int>>(coloriIds ?? "[]");
                var spessoriIdsList = JsonConvert.DeserializeObject<List<int>>(spessoriIds ?? "[]");

                // Aggiorna i prezzi basandosi sui criteri
                repo.AggiornaPrezziPerCriteri(idProdotto, dimensioniIdsList, coloriIdsList, spessoriIdsList, incremento, tipoOperazione);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        public void SaveSessionTaglieProd(int id_prod)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            List<DettaglioProdottiTaglie> taglie_prod = (List<DettaglioProdottiTaglie>)Session["TaglieProd"];

            foreach (DettaglioProdottiTaglie el in taglie_prod)
            {
                ProdottoTaglia pt = repo.getProdottoTagliaByIds(el.id_prodotto, (int)el.id_taglia);
                if (pt == null)
                {
                    pt = new ProdottoTaglia();
                }
                pt.id_taglia = (int)el.id_taglia;
                pt.id_prodotto = id_prod;
                pt.quantita = el.quantita;
                pt.BarCode = el.BarCode;
                if (el.prezzo != 0.00) pt.prezzo = el.prezzo;
                repo.SaveProdottoTaglia(pt);
            }
            Session["TaglieProd"] = null;
        }

        public void SaveSessionProdImgs(int id_prod)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            List<Immagini_catProd> immagini_prod = (List<Immagini_catProd>)Session["ImgsProd"];
            if (immagini_prod == null) immagini_prod = new List<Immagini_catProd>();
            foreach (Immagini_catProd img in immagini_prod)
            {
                img.id_prod = id_prod;
                repo.addImgProduct(img);
            }
            Session["ImgsProd"] = null;
        }

        private void SaveProdTag(int id_prod, List<int> tags)
        {
            TagRepository repo = new TagRepository();
            if(tags != null && tags.Count() > 0)
            {
                 foreach (int tag in tags)
                 {
                     ProdottiTag pt = repo.getProdottiTagByParam(id_prod, tag);
                     if (pt == null)
                     {
                         pt = new ProdottiTag();
                         pt.id_prodotti = id_prod;
                         pt.id_tag = tag;
                         repo.saveTagProdotti(pt);
                     }
                 }
            }
           
        }

        [HttpPost]
        public string GetProdottoByBarCode(string barcode)
        {
            string retvalue = string.Empty;
            if (!String.IsNullOrEmpty(barcode))
            {
                retvalue = ProdottiRepository.GetProdottoByBarCode(barcode);
            }
            return retvalue;
        }

        [HttpPost]
        public ActionResult DeleteImgProduct(string id)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            int id_img;
            List<Immagini_catProd> ImmaginiSession = new List<Immagini_catProd>();
            if (reqCookies != null)
            {
                DettagliProdottiRepository repository = new DettagliProdottiRepository();

                if (!String.IsNullOrEmpty(id))
                {
                    if (int.TryParse(id, out id_img))
                    {
                        repository.deleteImgProduct(repository.getImgCatProdById(id_img));
                    }
                    else
                    {
                        ImmaginiSession = Session["ImgsProd"] as List<Immagini_catProd>;
                        foreach (Immagini_catProd img in ImmaginiSession)
                        {
                            if (img.path.Contains(id))
                            {
                                ImmaginiSession.Remove(img);
                                break;
                            }
                        }
                        Session["ImgsProd"] = ImmaginiSession;
                    }

                }
                return Content("OK");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult getProdottoTagliaByProdId(string idProd, DataSourceLoadOptions loadOptions)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            List<DettaglioProdottiTaglie> prod_taglie = new List<DettaglioProdottiTaglie>();

            if (!String.IsNullOrEmpty(idProd)) prod_taglie = repository.getListTaglieByProdId(Convert.ToInt32(idProd));

            List<DettaglioProdottiTaglie> ListaTaglieSession = Session["TaglieProd"] as List<DettaglioProdottiTaglie>;
            LoadResult result = new LoadResult();

            if (prod_taglie.Count() > 0)
            {
                result = DataSourceLoader.Load(prod_taglie, loadOptions);
            }
            else
            {
                if (ListaTaglieSession != null && ListaTaglieSession.Count() > 0) result = DataSourceLoader.Load(ListaTaglieSession, loadOptions);
            }
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public ActionResult DeleteConfigurazioneProdotto(int key)
        {
            UtentiRepository.deleteUtenti(key);
            ProdottiRepository.deleteConfigurazioneProdotto(key);
            return RedirectToAction("Index");
        }




        [HttpPost]
        public ActionResult DeleteProdTaglia(string id_prod)
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            int prod;
            if (reqCookies != null)
            {
                DettagliProdottiRepository repository = new DettagliProdottiRepository();

                if (!String.IsNullOrEmpty(id_prod))
                {
                    if (int.TryParse(id_prod, out prod))
                    {
                        repository.SvuotaTaglieProdotto(prod);
                    }
                } else
                {
                    Session.Remove("TaglieProd");
                }
                return RedirectToAction("Edit");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddTaglieProduct(string values, int gruppo_taglie, int id_prodotto)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            dynamic data = JArray.Parse(values);
            List<DettaglioProdottiTaglie> ListaTaglieSession;
            int quantita_prod = 0;
            if (id_prodotto != 0)
            {
                //salvataggio su DB delle taglie prodotto

                Prodotti prod = repository.GetProdottiById(Convert.ToInt32(id_prodotto));
                if (prod != null)
                {
                    repository.SvuotaTaglieProdotto(prod.id_prodotto);
                    foreach (dynamic el in data)
                    {
                        ProdottoTaglia pt = new ProdottoTaglia();
                        pt.id_prodotto = prod.id_prodotto;
                        pt.id_taglia = el.id_taglia;
                        pt.quantita = el.quantita;
                        pt.BarCode = el.barcode;
                        quantita_prod += (int)el.quantita;
                        repository.SaveProdottoTaglia(pt);
                    }
                    prod.id_gruppo_taglie = gruppo_taglie;
                    prod.Quantita = quantita_prod;
                    repository.UpdateProdotto(prod);
                }
            }
            else
            {
                ListaTaglieSession = new List<DettaglioProdottiTaglie>();
                foreach (dynamic el in data)
                {
                    DettaglioProdottiTaglie dpt = new DettaglioProdottiTaglie();
                    if (el.id_taglia != null)
                    {
                        dpt.id_taglia = el.id_taglia;
                        int id_taglia = el.id_taglia;
                        Taglie t = repository.getTagliaById(id_taglia);
                        dpt.Descrizione_taglia = t.Descrizione_taglia;
                        dpt.quantita = el.quantita;
                        //dpt.prezzo = el.prezzo == null ? 0.00 : el.prezzo;
                        quantita_prod += (int)el.quantita;
                        dpt.BarCode = el.barcode;
                        ListaTaglieSession.Add(dpt);
                    }
                }
                
                // salvataggio in sessione delle immagini categoria
                Session["TaglieProd"] = ListaTaglieSession;
            }
            return Json(quantita_prod);
        }
   
        [HttpPost]
        public ActionResult InsertProdottoTaglia(string values)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository(); 
            ProdottoTaglia associazione = new ProdottoTaglia();
            JsonConvert.PopulateObject(values, associazione);

            associazione.eliminato = false;

            repository.addAssociazioneProdottoTaglia(associazione); 

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [HttpPost]
        public ActionResult UpdateProdottoTaglia(int key, string values)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            ProdottoTaglia associazioneEsistente = repository.getProdottoTagliaById(key);

            if (associazioneEsistente != null)
            {
                JsonConvert.PopulateObject(values, associazioneEsistente);

                repository.updateProdottoTaglia(associazioneEsistente);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost] 
        public ActionResult DeleteProdottoTaglia(int key)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();

            // 1. Recupera il record e lo elimina (hard o soft delete a seconda della tua logica)
            // Esempio Soft Delete:
            ProdottoTaglia associazioneEsistente = repository.getProdottoTagliaById(key);
            if (associazioneEsistente != null)
            {
                associazioneEsistente.eliminato = true;
                repository.updateProdottoTaglia(associazioneEsistente);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        public ActionResult getQuantitySum(string id_prod)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            List<DettaglioProdottiTaglie> ListaTaglieSession;
            int tot_quantity = 0;
            if (id_prod != null && !String.IsNullOrEmpty(id_prod) && Convert.ToInt32(id_prod) != 0)
            {
                int id_prodotto = Convert.ToInt32(id_prod);
                foreach (ProdottoTaglia el in repository.getProdottoTagliaByIdProdotto(id_prodotto))
                {
                    tot_quantity += el.quantita;
                }
            } else
            {
                ListaTaglieSession = Session["TaglieProd"] == null ? new List<DettaglioProdottiTaglie>() : Session["TaglieProd"] as List<DettaglioProdottiTaglie>;
                if (ListaTaglieSession.Count > 0)
                {
                    foreach (DettaglioProdottiTaglie el in ListaTaglieSession)
                    {
                        tot_quantity += el.quantita;
                    }
                }
            }
            return Content(tot_quantity.ToString());
        }
        //[HttpPost]
        //public ActionResult UpdateProdottoTaglia(int key, string values)
        //{
        //    DettagliProdottiRepository repository = new DettagliProdottiRepository();

        //    repository.updateProdottoTaglia(key, values);
        //    return RedirectToAction("Edit");
        //}

        [HttpPost]
        public ActionResult getSottocategorieUpdated(int id_categoria)
        {
            CategorieRepository repo = new CategorieRepository();
            List<Sottocategorie> result = repo.getSottocategorieByIdCat(id_categoria);
            return Json(result);
        }

        [HttpPost]
        public ActionResult getTipoImmagineDimension(string tipo_immagine)
        {
            TipiImmaginiRepository repo = new TipiImmaginiRepository();
            return Json(repo.getTipiImmaginiById(Convert.ToInt32(tipo_immagine)));
        }


        public string UploadImg()
        {
            string retvalue = string.Empty;
            var myFile = Request.Files["myFile2"];
            var targetLocation = Server.MapPath("/Content/ImgProdotti/");
            var path = string.Empty;
            string new_fileName = string.Empty;
            try
            {
                string img_name = myFile.FileName.Replace(" ", "_").Split('.')[0];
                string img_format = myFile.ContentType.Split('/')[1];

                new_fileName = img_name + "_" + (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString().Replace(",", "_") + "."+img_format;
                path = Path.Combine(targetLocation, new_fileName);
                myFile.SaveAs(path);
            }
            catch
            {
                Response.StatusCode = 400;
            }

            retvalue = Path.Combine("/Content/ImgProdotti/", new_fileName);

            return retvalue;
        }

        public string GetNewBarcode(string id)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            string barcode = this.ProdBarcodeGenerator(id);
            while (repo.getProdottoByBarCode(barcode) != null) {
                barcode = this.ProdBarcodeGenerator(id);
            }
            return barcode;
        }

        public string GetTaglieBarcode(string id, string taglia, int quantita, int id_gruppo_taglie)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            ConfigurazioniRepository configurazioniRepository = new ConfigurazioniRepository();
            string barcode = this.TaglieBarcodeGenerator(id, taglia);
            while(repo.getProdottoTagliaByBarcode(barcode) != null)
            {
                barcode = this.TaglieBarcodeGenerator(id, taglia);
            }
            if(String.IsNullOrEmpty(id) || id == "0")
            {
                List<DettaglioProdottiTaglie> ListaTaglieSession = Session["TaglieProd"] as List<DettaglioProdottiTaglie> ?? new List<DettaglioProdottiTaglie> ();

                DettaglioProdottiTaglie el = new DettaglioProdottiTaglie();

         
                el.id_taglia = Convert.ToInt32(taglia);
                Taglie t = repo.getTagliaById(Convert.ToInt32(taglia));
                el.Descrizione_taglia = t.Descrizione_taglia;
                el.BarCode = barcode;
                el.quantita = quantita;

                DettaglioProdottiTaglie dettprodt = ListaTaglieSession.Where(x => x.id_taglia == Convert.ToInt32(taglia)).FirstOrDefault();

                if(dettprodt != null)
                {
                    ListaTaglieSession.Remove(dettprodt);
                }

                ListaTaglieSession.Add(el);
                Session["TaglieProd"] = ListaTaglieSession;
            }
            else
            {
                int qta = 0;
                ProdottoTaglia tagliaprod = repo.getProdottoTagliaByIds(Convert.ToInt32(id),Convert.ToInt32(taglia));
                if(tagliaprod == null)
                {
                    qta = quantita;
                    tagliaprod = new ProdottoTaglia();

                }
                tagliaprod.id_taglia = Convert.ToInt32(taglia);
                tagliaprod.id_prodotto = Convert.ToInt32(id);
                qta = quantita - tagliaprod.quantita;
                tagliaprod.quantita = quantita;
                tagliaprod.BarCode = barcode;
                if(tagliaprod.id_prodottoTaglia == 0)
                {
                    repo.SaveProdottoTaglia(tagliaprod);
                }
                else
                {
                    repo.AggiornaTagliaProdotto(tagliaprod);
                }

                Prodotti p = repo.GetProdottiById(Convert.ToInt32(id));
                p.Quantita += Math.Abs(qta);
                p.id_gruppo_taglie = id_gruppo_taglie;
                repo.UpdateProdotto(p);

            }
           
            return barcode;
        }
        internal string ProdBarcodeGenerator(string id)
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            string barcode;
            string prefix = "800";
            Random rnd = new Random();
            int myRandomNo = rnd.Next(100000, 999999);
            string middle = myRandomNo.ToString();
            string suffix;
            if (!String.IsNullOrEmpty(id))
            {
                suffix = id;
            }
            else
            {
                suffix = repo.GetMaxProdId();
            }
            barcode = prefix + middle + suffix;
            while (barcode.Length < 13)
            {
                int randomNum = rnd.Next(0, 9);
                barcode = barcode + randomNum.ToString();
            }


            return barcode;
        }
        internal string TaglieBarcodeGenerator(string id, string taglie)
        {
            string barcode = "800" + id + taglie;
            while (barcode.Length < 13)
            {
                Random random = new Random();
                int randomNum = random.Next(0, 9);
                barcode = barcode + randomNum.ToString();
            }
            return barcode;
        }

        public ActionResult GetTaglie(DataSourceLoadOptions loadOptions)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();

            List<Taglie> taglie = repository.GetTaglie();
            var result = DataSourceLoader.Load(taglie, loadOptions);

            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        internal void addTagliaUnica(int id_prodotto)
        {
            DettagliProdottiRepository repo_det = new DettagliProdottiRepository();
            ProdottiRepository repo_p = new ProdottiRepository();
            Prodotti p = repo_p.GetProdottoById(id_prodotto);
            ProdottoTaglia pt = new ProdottoTaglia();
            pt.id_prodotto = id_prodotto;
            pt.id_taglia = 1010;
            pt.quantita = p.Quantita;
            repo_det.SaveProdottoTaglia(pt);
        }
        public ActionResult getTipoImmagineByIDImmagine(int id_immagini)
        {
            TipiImmaginiRepository repo = new TipiImmaginiRepository();
            return Json(repo.getTipiImmaginiById(id_immagini));
        }

        //--------------------------
        [HttpPost]
        public ActionResult GetConfig()
        {
            var config = new
            {
                marketID = ConfigurationManager.AppSettings["marketID"],
                client_id = ConfigurationManager.AppSettings["client_id"],
                client_secret = ConfigurationManager.AppSettings["client_secret"],
                refresh_token = ConfigurationManager.AppSettings["refresh_token"]
            };

            return Json(config);
        }

        [HttpPost]
        public int GetLastInsertedProductID()
        {
            ProdottiRepository repo = new ProdottiRepository();
            Prodotti p = repo.GetLastInsertedProductID();
            return p.id_prodotto;
        }
        public void ClearSession()
        {
            Session.Remove("Product");
            Session.Remove("TaglieProd");
        }

        public ActionResult getGruppoTagliaProdotti(int id_gruppo, string id_prodotto)
        {
            GruppiTagliaRepository repo_gt = new GruppiTagliaRepository();
            DettagliProdottiRepository p_repo = new DettagliProdottiRepository();
            if(!String.IsNullOrEmpty(id_prodotto) && Convert.ToInt32(id_prodotto) != 0)
            {
                p_repo.CheckTaglieProdotto(p_repo.GetProdottiById(Convert.ToInt32(id_prodotto)));
            } else
            {
                List<DettaglioProdottiTaglie> ListaTaglieSession = new List<DettaglioProdottiTaglie>();
                foreach (Taglie el in repo_gt.getTaglieByIdGruppo(id_gruppo))
                {
                    ListaTaglieSession.Add(new DettaglioProdottiTaglie()
                    {
                        id_prodottoTaglia = 0,
                        id_prodotto = 0,
                        id_taglia = el.id_taglia,
                        Descrizione_taglia = el.Descrizione_taglia,
                        eliminato = false,
                        quantita = 0,
                        prezzo = 0,
                        BarCode = "",
                    });
                }
            }
            return Json(repo_gt.getTaglieByIdGruppo(id_gruppo));
        }

        public ActionResult GetConfigurazioniProdottiByIdProdotto(DataSourceLoadOptions loadOptions, int idProdotto)
        {
            DettagliProdottiRepository repository = new DettagliProdottiRepository();
            var result = DataSourceLoader.Load(repository.getConfigurazioniProdottiByIdProdotto(idProdotto), loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        [HttpPost]
        public JsonResult GetValoriDisponibiliPerProdotto(int idProdotto)
        {
            var result = new
            {
                dimensioni = new List<object>(),
                colori = new List<object>(),
                spessori = new List<object>()
            };

            try
            {
                var repo = new DettagliProdottiRepository();

                // Recupera i valori distinti dalle combinazioni esistenti del prodotto
                var combinazioni = repo.getConfigurazioniProdottiByIdProdotto(idProdotto);

                // Creazione di nuove liste per evitare l'assegnazione diretta a proprietà di sola lettura
                var dimensioniList = combinazioni
                    .Where(c => c.id_taglia.HasValue)
                    .Select(c => new { id = c.id_taglia.Value, descrizione = c.Descrizione_taglia })
                    .Distinct()
                    .ToList<object>();

                var coloriList = combinazioni
                    .Where(c => c.id_colore.HasValue)
                    .Select(c => new { id = c.id_colore.Value, descrizione = c.Descrizione_colore })
                    .Distinct()
                    .ToList<object>();

                var spessoriList = combinazioni
                    .Where(c => c.id_spessore.HasValue)
                    .Select(c => new { id = c.id_spessore.Value, descrizione = c.descrizione_spessore })
                    .Distinct()
                    .ToList<object>();

                // Assegnazione delle liste create alle proprietà dell'oggetto anonimo
                result = new
                {
                    dimensioni = dimensioniList,
                    colori = coloriList,
                    spessori = spessoriList
                };
            }
            catch (Exception ex)
            {
                // Log dell'errore
            }

            return Json(result);
        }


    }
}
