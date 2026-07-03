using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelListaUtenti
    {
        public List<Lista_Utenti> lista { get; set; }
        public List<Utenti> ListaUtenti { get; set; }
        public Utenti Utenti { get; set; }
        public int Count { get; set; }
    }
}