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
using System.Data;
using System.Globalization;

namespace Admin.Controllers
{
    public class ReportMensileController : Controller
    {
        public ActionResult IndexReportMensile()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                StoricoFatturatoRepository storico = new StoricoFatturatoRepository();
                ReportMensile model = new ReportMensile();
                DateTime now = DateTime.Now;
                DateTime yesterday = now.AddDays(-1);
                model.DataSceltaDa = yesterday;
                model.DataSceltaA = now;
                model.year = now.Year;
                model.totale = storico.getTotale(yesterday, now).ToString("N2") + " €";
                model.spese = storico.getSpese(yesterday, now).ToString("N2") + " €";
                model.conto_economico = storico.getContoEconomico(yesterday, now).ToString("N2") + " €";
                model.color = "bg-green";
                model.storico_graph = this.GetStoricoGraphModel(model.DataSceltaDa.ToString(), model.DataSceltaA.ToString());
                model.profitti_graph = this.GetProfittiGraph(model);

                return View("IndexReportMensile", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }


        public ActionResult GetAnni(DataSourceLoadOptions loadOptions)
        {
            StoricoFatturatoRepository repository = new StoricoFatturatoRepository();
            List<YearSelect> years = new List<YearSelect>();
            foreach (int anno in repository.getAnniStorico())
            {
                YearSelect year = new YearSelect();
                year.label = anno.ToString();
                year.value = anno;
                years.Add(year);
            }
            var result = DataSourceLoader.Load(years, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }



        public ActionResult MostraDati(ReportMensile m)
        {
            StoricoFatturatoRepository repo = new StoricoFatturatoRepository();
            ReportMensile reportMensile = new ReportMensile();

            reportMensile.spese = repo.getSpese(m.DataSceltaDa, m.DataSceltaA).ToString("N2") + " €";
            reportMensile.DataSceltaDa = m.DataSceltaDa;
            reportMensile.DataSceltaA = m.DataSceltaA;
            reportMensile.totale = repo.getTotale(reportMensile.DataSceltaDa, reportMensile.DataSceltaA).ToString("N2") + " €";
            reportMensile.conto_economico = repo.getContoEconomico(reportMensile.DataSceltaDa, reportMensile.DataSceltaA).ToString("N2") + " €";
            reportMensile.color = "bg-green";

            reportMensile.storico_graph = this.GetStoricoGraphModel(m.DataSceltaDa.ToString(), m.DataSceltaA.ToString());

            return PartialView("PartialView_ReportMensile", reportMensile);
        }

        public ActionResult getStoricoGraphs(ReportMensile m)
        {
            ReportMensile reportMensile = new ReportMensile();
            if (m.year < 1000) m.year = DateTime.Now.Year;
            reportMensile.profitti_graph = this.GetProfittiGraph(m);

            return PartialView("Report_Graphs", reportMensile);
        }


        private string GetStoricoGraphModel(string da, string a)
        {
            DateTime DataSceltaDa = DateTime.Parse(da);
            DateTime DataSceltaA = DateTime.Parse(a);

            // getting instances of all operational classes
            StoricoGraphModel model = new StoricoGraphModel();
            CultureInfo culture = new CultureInfo("it-IT");
            Dataset totale = new Dataset();
            Dataset spese = new Dataset();
            Dataset conto_economico = new Dataset();
            StoricoFatturatoRepository repo = new StoricoFatturatoRepository();

            //instances of classes' lists
            model.labels = new List<string>();
            model.datasets = new List<Dataset>();
            totale.data = new List<double>();
            spese.data = new List<double>();
            conto_economico.data = new List<double>();

            //invariable data
            totale.label = "Totale";
            totale.backgroundColor = "#2196f3";
            totale.stack = "Stack 0";

            spese.label = "Spese";
            spese.backgroundColor = "#f44336";
            spese.stack = "Stack 1";

            conto_economico.label = "Perdite/Profitti";
            conto_economico.backgroundColor = "#4caf50";
            conto_economico.stack = "Stack 1";

            //all date related data
            while (DataSceltaDa <= DataSceltaA)
            {
                string month_name = DataSceltaDa.ToString("MMMM", culture);
                model.labels.Add(month_name);

                if (DataSceltaDa.Month == DataSceltaA.Month)
                {
                    totale.data.Add((double)repo.getTotale(DataSceltaDa, DataSceltaA));
                    spese.data.Add((double)repo.getSpese(DataSceltaDa, DataSceltaA));
                    conto_economico.data.Add((double)repo.getContoEconomico(DataSceltaDa, DataSceltaA));
                    DataSceltaDa = DataSceltaDa.AddMonths(1);
                }
                else
                {
                    DateTime new_data_a = DataSceltaDa.AddDays((DateTime.DaysInMonth(DataSceltaA.Year, DataSceltaDa.Month) - DataSceltaDa.Day) + 1);
                    totale.data.Add((double)repo.getTotale(DataSceltaDa, new_data_a));
                    spese.data.Add((double)repo.getSpese(DataSceltaDa, new_data_a));
                    conto_economico.data.Add((double)repo.getContoEconomico(DataSceltaDa, new_data_a));
                    DataSceltaDa = new_data_a;
                }
            }

            model.datasets.Add(totale);
            model.datasets.Add(spese);
            model.datasets.Add(conto_economico);

            return JsonConvert.SerializeObject(model);
        }

        private string GetProfittiGraph(ReportMensile m)
        {
            LineDatasets conto_economico = new LineDatasets();
            StoricoFatturatoRepository repo = new StoricoFatturatoRepository();
            List<double?> conto_data = new List<double?>();
            List<LineDatasets> model_dt = new List<LineDatasets>();
            ProfittiGraph model = new ProfittiGraph();
            List<string> mesi = new List<string> {
                "Gennaio",
                "Febbraio",
                "Marzo",
                "Aprile",
                "Maggio",
                "Giugno",
                "Luglio",
                "Agosto",
                "Settembre",
                "Ottobre",
                "Novembre",
                "Dicembre"
                        };
            model.labels = mesi;

            foreach (KeyValuePair<int, double> entry in repo.getStoricoPerYear(m.year))
            {
                if (m.year == DateTime.Now.Year && entry.Value == 0.00)
                {
                    conto_data.Add(null);
                }
                else
                {
                    conto_data.Add(entry.Value);
                }
            }

            conto_economico.label = "Perdite/Profitti";
            conto_economico.borderColor = "green";
            conto_economico.fill = true;
            conto_economico.tension = 0.2;
            conto_economico.data = conto_data;

            model_dt.Add(conto_economico);

            model.datasets = model_dt;
            return JsonConvert.SerializeObject(model);
        }
    }
        
}