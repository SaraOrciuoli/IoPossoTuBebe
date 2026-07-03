using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelTipiImmagini
    {
        public Tipi_Immagini TipiImmagini { get; set; }
        public List<Tipi_Immagini> all_tipi_immagini { get; set; }

    }
}