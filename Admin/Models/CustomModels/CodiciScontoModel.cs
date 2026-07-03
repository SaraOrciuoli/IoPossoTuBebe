using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class CodiciScontoModel
    {
        public List<Utenti> listaUtenti { get; set; }
        public List<CodiciSconto> scontiTemporanei { get; set; }
        public List<CodiciSconto> scontiRegalo { get; set; }
        public List<CodiciSconto> scontiRimborso { get; set; }
        public List<ScontiSpeciali> scontiSpeciali { get; set; }

       public List<int> cat_ids { get; set; }

    }

    public class ScontiSpecialiModel
    {
        public int id_scontoSpeciale { get; set; }
        public Nullable<System.DateTime> Data_Inizio { get; set; }
        public Nullable<System.DateTime> Data_Fine { get; set; }
        public Nullable<double> Valore { get; set; }
        public bool Attivo { get; set; }
        public string Sconto_speciale { get; set; }
        public List<int> categorie { get; set; }
    }
}