using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelListaOrdini
    {
        public List<DettagliOrdine> lista { get; set; }

        public int Count { get; set; }
    }
}