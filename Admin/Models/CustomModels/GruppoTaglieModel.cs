using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DAL.Model;
using System.Web;
using DevExpress.Web.Internal;

namespace Admin.Models.CustomModels
{
    public class GruppoTaglieModel
    {
        public List<Taglie> taglie { get; set; }
        public Taglie taglia { get; set; }
        public List<Colori> colori { get; set; }
        public List<Spessori> spessori { get; set; }
        public List<DettaglioGruppoTaglie> gruppi_taglie { get; set; }
        public List<DettaglioGruppoColori> gruppi_colori { get; set; }
        public List<DettaglioGruppoSpessori> gruppi_spessori { get; set; }
        public DettaglioGruppoTaglie gruppo_taglie { get; set; }
        public List<int> taglie_ids { get; set; }
    }

}