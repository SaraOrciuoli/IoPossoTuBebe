using DAL.Model;
using DAL.Repository;
using DevExpress.XtraRichEdit.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vetrina.Models.CustomModel
{
    public class CombinedViewModel
    {
        public ModelSlider SliderModel { get; set; }
        public ModelMenu MenuModel { get; set; }
        public ModelUser UserModel { get; set; }
        public ModelContatti ContattiModel { get; set; }
        public Prodotti Prodotto { get; set; }
        public Categorie CategoriaPadre { get; set; }
        public Categorie Categorie { get; set; }
        public Sottocategorie Sottocategorie { get; set; }
        public ProdottoTaglia ProdottoTaglia { get; set; }
        public ScontiSpeciali ScontiSpeciali { get; set; }
        public List<ProdottoTaglia> listaProdottiTaglia { get; set; }
        public List<string> ListaNomiTaglie { get; set; }
        public double? TotaleCarrello { get; internal set; }
        public string TagliaSelezionata { get; set; }
        public string NomeCategoria { get; set; }
        public List<DettaglioProdottiTaglie> listaTaglie { get; set; }
        public List<DettaglioProdottiTaglie> listaColori { get; set; }
        public List<DettaglioProdottiTaglie> listaSpessori { get; set; }

       
        public List<TestiHome> ListaTestiHome { get; set; }
        public Immagini_catProd SliderProdotto { get; set; }
        public List<Immagini_catProd> ListSliderProdotto { get; set; }

        public List<ProdottiPopup> ProdottiPopup { get; set; }
        
        public List<Marchi> ListaMarchi { get; set; }

        public ModelUser ModelUser { get; set; }

        public ModelPagamento ModelPagamento { get; set; }
        public RecuperaModel RecuperaModel { get; set; }

        public ModelDettaglioOrdine ModelDettaglioOrdine { get; set; }

        public List <Prodotti> bestSellers { get; set; }





    }
}