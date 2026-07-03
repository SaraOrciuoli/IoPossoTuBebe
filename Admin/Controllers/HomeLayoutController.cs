using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Admin.Security;
using System.Web.Mvc;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using DevExpress.DataAccess.Native.ExpressionEditor;
using System.IO;
using DevExpress.CodeParser;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using System.Drawing;
using Admin.Models.CustomModels;
using DevExpress.DataAccess.Native;

namespace Admin.Controllers
{
    public class HomeLayoutController : Controller
    {
        internal SliderRepository repo  = new SliderRepository();
        internal CategorieRepository cat_repo = new CategorieRepository();
        internal ProdottiRepository prd_repo = new ProdottiRepository();
        internal class SliderImgs
        {
            public string id { get; set; }
            public string path { get; set; }
        }


        internal string MoveFile(string path, int position, int slider, int index)
        {
            string[] names = path.Split('/');
            string filename = names[names.Count() - 1];
            string[] name_parts = filename.Split('.');
            string foldername = names[names.Count() - 2];
            string extension = name_parts[name_parts.Count() - 1];

            string new_filename = "_s" + slider.ToString() + "_p" + position.ToString()+"-"+ index.ToString() + "." + extension; 
            string new_path = path;
            
            if (foldername == "Slider") {
                string new_location = Server.MapPath("/Content/SliderActive/");
                string old_location = Server.MapPath("/Content/Slider/");
                string old_path = Path.Combine(old_location, filename);
                new_path = Path.Combine(new_location, new_filename);
                if (!System.IO.File.Exists(new_path))
                {
                    System.IO.File.Move(old_path, new_path);
                } else 
                {
                    System.IO.File.Delete(new_path);
                    System.IO.File.Move(old_path, new_path);
                }
                new_path = Path.Combine("/Content/SliderActive/", new_filename);
                System.IO.File.Delete(old_path);
            }

            return new_path;
        }

        public ActionResult MainSlider()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                Admin.Models.CustomModels.SliderModel model = new Models.CustomModels.SliderModel();
                model.Prodotti = repo.GetProdottiSlider(1005);
                model.Categorie = repo.GetCategorieSlider(1005);
                model.ProdottiSide = repo.GetProdottiSlider(2005);
                model.CategorieSide = repo.GetCategorieSlider(2005);
                model.main_s_p1_prod_ids = new List<int>();
                model.main_s_p1_cat_ids = new List<int>();
                model.main_s_p1_paths = new List<string>();
                model.main_s_p1 = repo.getSliderByPosition(1, 1);
                model.s_p2 = (repo.getSliderByPosition(1, 2) == null || repo.getSliderByPosition(1, 2).Count() == 0) ? new Slider() : repo.getSliderByPosition(1, 2)[0];
                model.s_p3 = (repo.getSliderByPosition(1, 3) == null || repo.getSliderByPosition(1, 3).Count() == 0) ? new Slider() : repo.getSliderByPosition(1, 3)[0];
                model.s_p4 = (repo.getSliderByPosition(1, 4) == null || repo.getSliderByPosition(1, 4).Count() == 0) ? new Slider() : repo.getSliderByPosition(1, 4)[0];
                model.s_p1 = new Slider();
                model.promotion_msg_ita = repo.getPromotionalMessage("ita");
                model.promotion_msg_eng = repo.getPromotionalMessage("eng");
                if(model.s_p2.id_prodotto == null)  model.s_p2.id_prodotto = 0;
                if(model.s_p3.id_prodotto == null)  model.s_p3 .id_prodotto = 0;
                if(model.s_p4.id_prodotto == null)  model.s_p4 .id_prodotto = 0;

                if(model.s_p2.id_categoria == null) model.s_p2.id_categoria = 0;
                if(model.s_p3.id_categoria == null) model.s_p3.id_categoria = 0;
                if(model.s_p4.id_categoria == null) model.s_p4.id_categoria = 0;

                foreach(Slider el in model.main_s_p1)
                {
                    if (el.type == "cat") model.main_s_p1_cat_ids.Add((int)el.id_categoria);
                    if (el.type == "prod") model.main_s_p1_prod_ids.Add((int)el.id_prodotto);
                }

                return View(model);
            } else
            {
                return RedirectToAction("Login", "Home");
            }
                
        }


        public ActionResult SecondSlider()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                SliderModel model = new SliderModel();
                model.Prodotti = repo.GetProdottiSlider(2006);
                model.Categorie = repo.GetCategorieSlider(2006);
                model.Sottocategorie = repo.GetSottocategorieSlider(2006);
                model.s_p1 = (repo.getSliderByPosition(2, 1) == null || repo.getSliderByPosition(2, 1).Count() == 0) ? new Slider() : repo.getSliderByPosition(2, 1)[0];
                model.s_p2 = (repo.getSliderByPosition(2, 2) == null || repo.getSliderByPosition(2, 2).Count() == 0) ? new Slider() : repo.getSliderByPosition(2, 2)[0];
                model.s_p3 = (repo.getSliderByPosition(2, 3) == null || repo.getSliderByPosition(2, 3).Count() == 0) ? new Slider() : repo.getSliderByPosition(2, 3)[0];
                model.s_p4 = (repo.getSliderByPosition(2, 4) == null || repo.getSliderByPosition(2, 4).Count() == 0) ? new Slider() : repo.getSliderByPosition(2, 4)[0];

                if(model.s_p1.id_prodotto == null) model.s_p1.id_prodotto = 0;
                if(model.s_p2.id_prodotto == null) model.s_p2.id_prodotto = 0;
                if(model.s_p3.id_prodotto == null) model.s_p3.id_prodotto = 0;
                if(model.s_p4.id_prodotto == null) model.s_p4.id_prodotto = 0;

                if(model.s_p1.id_categoria == null) model.s_p1.id_categoria = 0;
                if(model.s_p2.id_categoria == null) model.s_p2.id_categoria = 0;
                if(model.s_p3.id_categoria == null) model.s_p3.id_categoria = 0;
                if(model.s_p4.id_categoria == null) model.s_p4.id_categoria = 0;

                if (model.s_p1.id_sottocategoria == null) model.s_p1.id_sottocategoria = 0;
                if (model.s_p2.id_sottocategoria == null) model.s_p2.id_sottocategoria = 0;
                if (model.s_p3.id_sottocategoria == null) model.s_p3.id_sottocategoria = 0;
                if (model.s_p4.id_sottocategoria == null) model.s_p4.id_sottocategoria = 0;

                return View(model);
            }
             
            {
                return RedirectToAction("Login", "Home");
            }

        }
        [HttpPost]
        public string GetImmagineByProdId(string id, string position)
        {
            string result = "";
            if(!String.IsNullOrEmpty(id))
            {
                result = repo.GetSliderImgPathFromProdId(Convert.ToInt32(id), Convert.ToInt32(position));
            }

            return result;
        }

        [HttpPost]
        public string GetImmagineByCatId(string id, string position)
        {
            string result = "";
            if (!String.IsNullOrEmpty(id))
            {
                result = repo.GetSliderImgPathFromCatId(Convert.ToInt32(id), Convert.ToInt32(position));
            }

            return result;
        }
        
        [HttpPost]
        public string GetImmagineBySottCatId(string id, string position)
        {
            string result = "";
            if (!String.IsNullOrEmpty(id))
            {
                result = repo.GetSliderImgPathFromSottCatId(Convert.ToInt32(id), Convert.ToInt32(position));
            }

            return result;
        }

        [HttpPost]
        public string UploadImg()
        {
            string retvalue = string.Empty;
            var myFile = Request.Files["myFile"];
            var targetLocation = Server.MapPath("/Content/Slider/");
            var path = string.Empty;
            try
            {
                path = Path.Combine(targetLocation, myFile.FileName.Replace(" ", "_"));
                myFile.SaveAs(path);
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return e.Message;
            }

            retvalue = Path.Combine("/Content/Slider/", myFile.FileName.Replace(" ", "_"));
            
            return retvalue;
        }

        [HttpPost]
        public ActionResult GetImmaginiByProdId(List<int> ids, string position)
        {
            List<SliderImgs> result = new List<SliderImgs>();
            if(ids != null) 
            { 
                foreach (int id in ids)
                {
                    SliderImgs sliderImgs = new SliderImgs();
                    sliderImgs.path = repo.GetSliderImgPathFromProdId(id, Convert.ToInt32(position));
                    sliderImgs.id = id.ToString();
                    result.Add(sliderImgs);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetImmaginiByCatId(List<int> ids, string position)
        {
            List<SliderImgs> result = new List<SliderImgs>();
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    SliderImgs sliderImgs = new SliderImgs();
                    string el = id.ToString();
                    sliderImgs.id = el;
                    sliderImgs.path = repo.GetSliderImgPathFromCatId(id, Convert.ToInt32(position));
                    result.Add(sliderImgs);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult SalvaMainSlider(string values)
        {
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(values);
            if(data["s1_p1"] != null ) repo.ResetSliderConfiguration(1, 1);
            if(data["s1_p2"].img != "") repo.ResetSliderConfiguration(1, 2);
            if(data["s1_p3"].img != "") repo.ResetSliderConfiguration(1, 3);
            if(data["s1_p4"].img != "") repo.ResetSliderConfiguration(1, 4);
            if(data["s1_text"] != "" || data["s1_text"] != null) repo.ResetSliderConfiguration(1, 10);

            foreach (dynamic item in data["s1_p1"])
            {
                Slider p1 = new Slider();
                p1.posizione = 1;
                p1.slider_num = 1;
                p1.path_img_slider = MoveFile((string)item.img, 1,1, Convert.ToInt32((string)item.index));
                p1.type = item.source;
                if (item.source == "cat")
                {
                    p1.id_categoria = item.id;
                    p1.Link = "/Ecommerce/Categoria/" + item.id + "/" + cat_repo.getCategoriaById((string)item.id).Descrizione;
                }
                if (item.source == "prod") 
                { 
                    p1.id_prodotto = item.id;
                    p1.Link = "/Ecommerce/Prodotto/"+item.id+"/"+ prd_repo.GetProdottoById(Convert.ToInt32(item.id)).Descrizione;
                }
                try
                {
                    repo.AddSlider(p1);
                } catch(Exception ex)
                {
                    string msg = ex.Message;
                    string mess = msg;
                }
                
            }
            Slider p2 = new Slider();
            Slider p3 = new Slider();
            Slider p4 = new Slider();
            Slider text = new Slider();
            p2.slider_num = 1;
            p3.slider_num = 1;
            p4.slider_num = 1;
            p2.posizione = 2;
            p3.posizione = 3;
            p4.posizione = 4;
            p2.path_img_slider = MoveFile((string)data["s1_p2"].img, 2, 1, 2);
            p3.path_img_slider = MoveFile((string)data["s1_p3"].img, 3, 1, 3);
            p4.path_img_slider = MoveFile((string)data["s1_p4"].img, 4, 1, 4);
            p2.type = data["s1_p2"].source;
            p3.type = data["s1_p3"].source;
            p4.type = data["s1_p4"].source;
            p2.Descrizione = data["s1_p2"].desc;
            p3.Descrizione = data["s1_p3"].desc;
            p4.Descrizione = data["s1_p4"].desc;
            p2.Descrizione_eng = data["s1_p2"].desc_eng;
            p3.Descrizione_eng = data["s1_p3"].desc_eng;
            p4.Descrizione_eng = data["s1_p4"].desc_eng;

            if (data["s1_p2"].source == "cat") 
            {
                p2.id_categoria = data["s1_p2"].id;
                p2.Link = "/Ecommerce/Categoria/" + data["s1_p2"].id + "/" + cat_repo.getCategoriaById(data["s1_p2"].id.ToString()).Descrizione;
            }
            if(data["s1_p3"].source == "cat") 
            {
                p3.id_categoria = data["s1_p3"].id;
                p3.Link = "/Ecommerce/Categoria/" + data["s1_p3"].id + "/" + cat_repo.getCategoriaById(data["s1_p3"].id.ToString()).Descrizione;
            }
            if (data["s1_p4"].source == "cat") 
            { 
                p4.id_categoria = data["s1_p4"].id;
                p4.Link = "/Ecommerce/Categoria/" + data["s1_p4"].id + "/" + cat_repo.getCategoriaById(data["s1_p4"].id.ToString()).Descrizione;
            }

            if (data["s1_p2"].source == "prod")
            {
                p2.id_prodotto = data["s1_p2"].id;
                p2.Link = "/Ecommerce/Prodotto/"+ data["s1_p2"].id + "/"+ prd_repo.GetProdottoById(Convert.ToInt32(data["s1_p2"].id)).Descrizione;
            }
            if (data["s1_p3"].source == "prod")
            {
                p3.id_prodotto = data["s1_p3"].id;
                p3.Link = "/Ecommerce/Prodotto/"+ data["s1_p3"].id + "/"+ prd_repo.GetProdottoById(Convert.ToInt32(data["s1_p3"].id)).Descrizione;
            }
            if (data["s1_p4"].source == "prod")
            {
                p4.id_prodotto = data["s1_p4"].id;
                p4.Link = "/Ecommerce/Prodotto/" + data["s1_p4"].id + "/" + prd_repo.GetProdottoById(Convert.ToInt32(data["s1_p4"].id)).Descrizione;
            }
            
            if(data["s1_text"] != "" || data["s1_text"] != null)
            {
                text.slider_num = 1;
                text.posizione = 10;
                text.Descrizione = data["s1_text"];
                text.Descrizione_eng = data["s1_text_eng"];
                text.type = "msg";
            }
            try
            {
                repo.AddSlider(p2);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string mess = msg;
            }
            try
            {
                repo.AddSlider(p3);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string mess = msg;
            }
            try
            {
                repo.AddSlider(p4);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string mess = msg;
            }
            try
            {
                repo.AddSlider(text);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string mess = msg;
            }
            

            return Content("OK");
        }

        public ActionResult SalvaSecondSlider(string values)
        {
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(values);
            if (data["s2_p1"].img != "") repo.ResetSliderConfiguration(2, 1);
            if (data["s2_p2"].img != "") repo.ResetSliderConfiguration(2, 2);
            if (data["s2_p3"].img != "") repo.ResetSliderConfiguration(2, 3);
            if (data["s2_p4"].img != "") repo.ResetSliderConfiguration(2, 4);
            Slider p1 = new Slider();
            Slider p2 = new Slider();
            Slider p3 = new Slider();
            Slider p4 = new Slider();

            p1.posizione = 1;
            p2.posizione = 2;
            p3.posizione = 3;
            p4.posizione = 4;
            p1.slider_num = 2;
            p2.slider_num = 2;
            p3.slider_num = 2;
            p4.slider_num = 2;
            string new_path_1 = MoveFile((string)data["s2_p1"].img, 1, 2, 1);
            string new_path_2 = MoveFile((string)data["s2_p2"].img, 2, 2, 2);
            string new_path_3 = MoveFile((string)data["s2_p3"].img, 3, 2, 3);
            string new_path_4 = MoveFile((string)data["s2_p4"].img, 4, 2, 4);
            p1.path_img_slider = new_path_1;
            p2.path_img_slider = new_path_2;
            p3.path_img_slider = new_path_3;
            p4.path_img_slider = new_path_4;
            p1.type = data["s2_p1"].source;
            p2.type = data["s2_p2"].source;
            p3.type = data["s2_p3"].source;
            p4.type = data["s2_p4"].source;
            p1.Descrizione = data["s2_p1"].desc;
            p2.Descrizione = data["s2_p2"].desc;
            p3.Descrizione = data["s2_p3"].desc;
            p4.Descrizione = data["s2_p4"].desc;
            p1.Descrizione_eng = data["s2_p1"].desc_eng;
            p2.Descrizione_eng = data["s2_p2"].desc_eng;
            p3.Descrizione_eng = data["s2_p3"].desc_eng;
            p4.Descrizione_eng = data["s2_p4"].desc_eng;

            if (data["s2_p1"].source == "cat")
            {
                p1.id_categoria = data["s2_p1"].id;
                p1.Link = "/Ecommerce/Categoria/" + data["s2_p1"].id + "/" + cat_repo.getCategoriaById(data["s2_p1"].id.ToString()).Descrizione;
            }
            if (data["s2_p2"].source == "cat")
            {
                p2.id_categoria = data["s2_p2"].id;
                p2.Link = "/Ecommerce/Categoria/" + data["s2_p2"].id + "/" + cat_repo.getCategoriaById(data["s2_p2"].id.ToString()).Descrizione;
            }
            if (data["s2_p3"].source == "cat")
            {
                p3.id_categoria = data["s2_p3"].id;
                p3.Link = "/Ecommerce/Categoria/" + data["s2_p3"].id + "/" + cat_repo.getCategoriaById(data["s2_p3"].id.ToString()).Descrizione;
            }
            if (data["s2_p4"].source == "cat")
            {
                p4.id_categoria = data["s2_p4"].id;
                p4.Link = "/Ecommerce/Categoria/" + data["s2_p4"].id + "/" + cat_repo.getCategoriaById(data["s2_p4"].id.ToString()).Descrizione;
            }

            if (data["s2_p1"].source == "prod")
            {
                p1.id_prodotto = data["s2_p1"].id;
                p1.Link = "/Ecommerce/Prodotto/" + data["s2_p1"].id + "/" + prd_repo.GetProdottoById(Convert.ToInt32(data["s2_p1"].id)).Descrizione;
            }
            if (data["s2_p2"].source == "prod")
            {
                p2.id_prodotto = data["s2_p2"].id;
                p2.Link = "/Ecommerce/Prodotto/" + data["s2_p2"].id + "/" + prd_repo.GetProdottoById(Convert.ToInt32(data["s2_p2"].id)).Descrizione;
            }
            if (data["s2_p3"].source == "prod")
            {
                p3.id_prodotto = data["s2_p3"].id;
                p3.Link = "/Ecommerce/Prodotto/" + data["s2_p3"].id + "/" + prd_repo.GetProdottoById(Convert.ToInt32(data["s2_p3"].id)).Descrizione;
            }
            if (data["s2_p4"].source == "prod")
            {
                p4.id_prodotto = data["s2_p4"].id;
                p4.Link = "/Ecommerce/Prodotto/" + data["s2_p4"].id + "/" + prd_repo.GetProdottoById(Convert.ToInt32(data["s2_p4"].id)).Descrizione;
            }
            if (data["s2_p1"].source == "sottcat")
            {
                p1.id_sottocategoria = data["s2_p1"].id;
                p1.Link = "/Ecommerce/Sottocategoria/" + data["s2_p1"].id + "/" + cat_repo.getSottocategoriaById(Convert.ToInt32(data["s2_p1"].id)).Nome + "?categoriaId=" + cat_repo.GetAssociazioniCatSubcat(Convert.ToInt32(data["s2_p1"].id));
            }
            if (data["s2_p2"].source == "sottcat")
            {
                p2.id_sottocategoria = data["s2_p2"].id;
                p2.Link = "/Ecommerce/Sottocategoria/" + data["s2_p2"].id + "/" + cat_repo.getSottocategoriaById(Convert.ToInt32(data["s2_p2"].id)).Nome + "?categoriaId=" + cat_repo.GetAssociazioniCatSubcat(Convert.ToInt32(data["s2_p2"].id));
            }
            if (data["s2_p3"].source == "sottcat")
            {
                p3.id_sottocategoria = data["s2_p3"].id;
                p3.Link = "/Ecommerce/Sottocategoria/" + data["s2_p3"].id + "/" + cat_repo.getSottocategoriaById(Convert.ToInt32(data["s2_p3"].id)).Nome + "?categoriaId=" + cat_repo.GetAssociazioniCatSubcat(Convert.ToInt32(data["s2_p3"].id));
            }
            if (data["s2_p4"].source == "sottcat")
            {
                p4.id_sottocategoria = data["s2_p4"].id;
                p4.Link = "/Ecommerce/Sottocategoria/" + data["s2_p4"].id + "/" + cat_repo.getSottocategoriaById(Convert.ToInt32(data["s2_p4"].id)).Nome + "?categoriaId=" + cat_repo.GetAssociazioniCatSubcat(Convert.ToInt32(data["s2_p4"].id));
            }
            repo.AddSlider(p1);
            repo.AddSlider(p2);
            repo.AddSlider(p3);
            repo.AddSlider(p4);

            return Content("OK");
        }
    }
}