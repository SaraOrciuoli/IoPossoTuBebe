using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelEditEventi
    {
        public List<Eventi> Lista { get; set; }

        public Eventi Evento { get; set; }

        public int Count { get; set; }
    }
}
