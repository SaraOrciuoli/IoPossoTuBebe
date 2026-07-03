using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Admin.Models.CustomModels
{
    public class ModelTestiHome
    {
        public List<TestiHome> ListaTestiHome { get; set; }
        public TestiHome TestiHome { get; set; }
    }
}