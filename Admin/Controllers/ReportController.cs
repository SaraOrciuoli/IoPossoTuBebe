using Admin.Models.CustomModels;
using DAL.Model;
using DAL.Repository;
using DevExpress.Office.Utils;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                Modelreport model = new Modelreport();
                model.DataSceltaDa = DateTime.Now;
                model.DataSceltaA = DateTime.Now;
                double carte = ReportRepository.GetValoreCartaByDay(model.DataSceltaDa,model.DataSceltaA);
                double Contanti = ReportRepository.GetValoreContantiByDay(model.DataSceltaDa, model.DataSceltaA);
                double servizi = ReportRepository.GetValoreServiziByDay(model.DataSceltaDa, model.DataSceltaA);
                double prodotti = ReportRepository.GetValoreProdottiByDay(model.DataSceltaDa, model.DataSceltaA);
                model.prodottiPiuVenduti = GetProdottiPiuVenduti(model.DataSceltaDa, model.DataSceltaA);
                model.valoreCarte = carte.ToString("N2") + " €";
                model.valoreContanti = Contanti.ToString("N2") + " €";
                model.valoreIncassato = (carte + Contanti).ToString("N2") + " €";
                model.valoreProdotti = prodotti.ToString("N2") + " €";
                model.valoreServizi = servizi.ToString("N2") + " €";
                model.prodottiEsaurimento = GetprodottiInEsaurimento();
                model.prodottiVenduti = Getprodottivenduti();
                
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        private List<DettaglioReportProdotti> GetProdottiPiuVenduti(DateTime dataSceltaDa, DateTime dataSceltaA)
         {
             List<DettaglioReportProdotti> lista = new List<DettaglioReportProdotti>();
             List<DettaglioReportProdotti> listaPartial = ReportRepository.GetProdottiPiuVenduti(dataSceltaDa, dataSceltaA);
             foreach(int item in listaPartial.Select(x => x.id_prodotto).Distinct())
             {
                 DettaglioReportProdotti itemp = listaPartial.Where(x => x.id_prodotto == item).FirstOrDefault();
                 itemp.Quantita = listaPartial.Where(x => x.id_prodotto == item).Sum(x => x.Quantita);
                 lista.Add(itemp);

             }
             return lista;
         }

        private List<ProdottiReport> GetprodottiInEsaurimento()
        {
            List<ProdottiReport> retvalue = new List<ProdottiReport>();
            List<Prodotti> listaProdotti = ProdottiRepository.getProdotti();
            foreach(Prodotti item in listaProdotti.OrderBy(x => x.Quantita).Take(100).ToList())
            {
                ProdottiReport prod = new ProdottiReport();
                prod.idProdotto = item.id_prodotto;
                prod.quantita = item.Quantita;
                prod.descrizione = item.Descrizione;
                retvalue.Add(prod);                          
            }
            return retvalue;
        }



        private List<ProdottiReport> Getprodottivenduti()
        {
            DettagliProdottiRepository repo = new DettagliProdottiRepository();
            List<ProdottiReport> retvalue = new List<ProdottiReport>();
            List<DettaglioProdottiOrdine> listaProdotti = ProdottiRepository.Getprodottiordine();
            List<DettaglioProdottiOrdine> listaprodottiDistinti = listaProdotti.DistinctBy(x => new { x.id_prodotto, x.id_taglia }).ToList();
            foreach(DettaglioProdottiOrdine el in listaprodottiDistinti) 
            {
                if(el.NomeProdotto != "" && el.NomeProdotto != null)
                {
                    ProdottiReport prod = new ProdottiReport();
                    prod.idProdotto = el.id_prodotto;
                    prod.quantita = listaProdotti.Where(x => x.id_prodotto == el.id_prodotto && x.id_taglia == el.id_taglia).Sum(x => x.Quantita);
                    Taglie t = repo.getTagliaById(el.id_taglia);
                    prod.descrizione = el.NomeProdotto + (t != null ? (" - "+t.Descrizione_taglia) : "");
                    retvalue.Add(prod);
                }
                
            }
            return retvalue.Take(100).OrderByDescending(x =>x.quantita).ToList();
        }

        public ActionResult MostraDati(Modelreport data) 
         {
             Modelreport model = new Modelreport();
             model.DataSceltaDa = data.DataSceltaDa;
             model.DataSceltaA = data.DataSceltaA;
             double carte = ReportRepository.GetValoreCartaByDay(model.DataSceltaDa, model.DataSceltaA);
             double Contanti = ReportRepository.GetValoreContantiByDay(model.DataSceltaDa, model.DataSceltaA);
             double servizi = ReportRepository.GetValoreServiziByDay(model.DataSceltaDa, model.DataSceltaA);
             double prodotti = ReportRepository.GetValoreProdottiByDay(model.DataSceltaDa, model.DataSceltaA);
             model.prodottiPiuVenduti = GetProdottiPiuVenduti(model.DataSceltaDa, model.DataSceltaA);
             model.valoreCarte = carte.ToString("N2") + " €";
             model.valoreContanti = Contanti.ToString("N2") + " €";
             model.valoreIncassato = (carte + Contanti).ToString("N2") + " €";
             model.valoreProdotti = prodotti.ToString("N2") + " €";
             model.valoreServizi = servizi.ToString("N2") + " €";
             return PartialView("PartialView_Report",model);
         }
    }
}