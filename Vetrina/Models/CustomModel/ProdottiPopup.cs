using DAL.Model;
using DAL.Repository;
using DevExpress.XtraRichEdit.Commands;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class ProdottiPopup
    {
        public Prodotti Prodotto { get; set; }
        public Categorie Categorie { get; set; }
        public Sottocategorie Sottocategorie { get; set; }

    }
}