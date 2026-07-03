using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class RecuperaModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}