using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;

namespace Admin.Models.CustomModels
{
    public class ReportMensile
    {
        public DateTime DataSceltaDa { get; set; }
        public DateTime DataSceltaA { get; set; }
        public string totale { get; set; }
        public string spese { get; set; }
        public string conto_economico { get; set; }
        public string color { get; set; }
        public string storico_graph { get; set; }

        public int year { get; set; }

        public string profitti_graph { get; set; }
    }

    public class StoricoGraphModel
    {
        public List<string> labels { get; set; }

        public List<Dataset> datasets { get; set; }
    }

    public class Dataset
    {
        public string label { get; set; }
        public List<double> data { get; set; }

        public string backgroundColor { get; set; }
        public string stack { get; set; }

    }

    public class LineDatasets
    {
        public string label { get; set; }
        public List<double?> data { get; set; }
        public bool fill { get; set; }
        public double tension { get; set; }
        public string borderColor { get; set; }
    }

    public class YearSelect
    {
        public string label { get; set; }
        public int value { get; set; }
    }

    public class ProfittiGraph
    {
        public List<string> labels { get; set; }
        public List<LineDatasets> datasets { get; set; }
    }

}