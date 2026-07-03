using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class ModelUser
    {
        public Utenti Utente { get; set; }
        public List<Ordine> Ordini { get; set; }
        public string idUtente { get; set; }
        public List<Comuni> ListaComuniIsole { get; set; }
        public List<ProdottiOrdine> ProdottiOrdine { get; set; }
       
    }
}