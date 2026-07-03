using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class ModelShop
    {
        public string Nominativo { get; set; }
        public string Tavolo { get; set; }

        public string Tipo { get; set; }
        public int id_Tipo { get; set; }
        public Ordine Order { get; set; }

        public string idfila { get; set; }
        public int Numero_Ombrellone { get; set; }


        public List<Fascia_Oraria> ListaFascieOrarie { get; set; }
    }
}