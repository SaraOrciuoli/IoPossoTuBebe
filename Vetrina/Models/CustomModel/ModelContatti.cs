using DAL.Model;
using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vetrina.Models.CustomModel
{
    public class ModelContatti
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        [Email]
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Messaggio { get; set; }
        
    }
}