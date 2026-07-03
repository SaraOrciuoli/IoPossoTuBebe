using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModel
{
    public class Lato
    {
        public int id { get; set; }
        public string Descrizione { get; set; }
    }


    public class ModelShop
    {
        public string Nominativo { get; set; }
        public string Tavolo { get; set; }

        public string Tipo { get; set; }
        public int id_Tipo { get; set; }
        public Ordine Order { get; set; }

        public List<Fascia_Oraria> ListaFascieOrarie { get; set; }
    }
}