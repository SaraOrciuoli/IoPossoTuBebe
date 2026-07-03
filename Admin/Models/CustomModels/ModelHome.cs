using DAL.Model;
using DAL.Repository;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelHome
    {
        public List<DettagliOrdine> UltimiOrdini { get; set; }
        public List<DettagliOrdine> OrdiniDaEvadere { get; set; }
        public List<DettagliOrdine> OrdiniOggi { get; set; }
        public List<DettagliOrdine> OrdiniEvasi { get; set; }
        public List<DettagliOrdine> OrdiniInAttesaDiPagamento { get; set; }
        public List<string> ChartLabels { get; set; }
        public List<int> ChartData { get; set; }

        // Costruttore che popola i dati
        public ModelHome()
        {
            PopulateDashboardData();
        }

        private void PopulateDashboardData()
        {
            var repo = new OrdiniRepository();
            var utentiRepo = new UtentiRepository();
            var oggi = DateTime.Today;

            // Ottieni tutti gli ordini
            var tuttiOrdini = repo.GetOrdini()?.ToList() ?? new List<DettagliOrdine>();

            // Ultimi 10 ordini
            UltimiOrdini = tuttiOrdini
                .OrderByDescending(o => o.DataOra)
                .Take(10)
                .ToList();

            // Ordini da evadere (stati in elaborazione/pending)
            OrdiniDaEvadere = tuttiOrdini
                .Where(o => o.id_stato == 1)
                .OrderBy(o => o.DataOra)
                .ToList();

            // Ordini di oggi
            OrdiniOggi = tuttiOrdini
                .Where(o => o.DataOra.Date == oggi)
                .ToList();

            // Ordini evasi oggi (stato completato/delivered oggi)
            OrdiniEvasi = tuttiOrdini
                .Where(o => (o.id_stato == 2))
                .ToList();

            var dataLimite = oggi.AddDays(-2);
            OrdiniInAttesaDiPagamento = tuttiOrdini
                .Where(o => o.id_stato_pagmento == 4)
                .ToList();

            PopulateChartData(tuttiOrdini);

        }

        private void PopulateChartData(List<DettagliOrdine> ordini)
        {
            ChartLabels = new List<string>();
            ChartData = new List<int>();

            for (int i = 30; i >= 0; i--)
            {
                var data = DateTime.Today.AddDays(-i);
                var count = ordini.Count(o => o.DataOra.Date == data.Date);

                ChartLabels.Add(data.ToString("dd/MM"));
                ChartData.Add(count);
            }
        }
    }
}