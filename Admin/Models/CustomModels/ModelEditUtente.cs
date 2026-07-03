using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelEditUtente
    {
        public Utenti Utente { get; set; }

        public List<Ruoli> listaRuoli { get; set; }

        public List<Utenti> ListaUtenti { get; set; }
        public bool is_azienda { get; set; }    
        public bool is_privato { get; set; }
        public bool consentito { get; set; }
    }
}