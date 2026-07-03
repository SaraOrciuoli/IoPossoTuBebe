using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModel
{
    public class RichiestaSalvataggioImmagini
    {
        public int idProdotto { get; set; }
        public List<ImmagineCaricataDto> immagini { get; set; }
        public string descrizione { get; set; }
        public string altTesto { get; set; }
        public string link { get; set; }
    }
}