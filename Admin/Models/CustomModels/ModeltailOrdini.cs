using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelDetailOrdini
    {
        public List<DettaglioProdottiOrdine> lista { get; set; }

        public DettagliOrdine Order { get; set; }
    }
}