using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class MarcheModel
    {
        public int id_marca { get; set; }
        public string Descrizione { get; set; }
        public int Count { get; set; }
    }
}