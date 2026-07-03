using DAL.Model;
using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class ModelEventi
    {
        public List<Eventi> Lista { get; set; }

        public int Count { get; set; }
    }
}
