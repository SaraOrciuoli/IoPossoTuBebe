using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class RecuperaModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}