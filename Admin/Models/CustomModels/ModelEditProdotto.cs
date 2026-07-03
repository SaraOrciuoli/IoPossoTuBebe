using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelEditProdotto
    {
        public Prodotti Prodotto { get; set; }

        public List<Marchi> listaMarche { get 
            {
                DAL.Repository.ConfigurazioniRepository repo = new DAL.Repository.ConfigurazioniRepository ();
                return (List<Marchi>)repo.GetMarche();
            } 
        }
        public List<Categorie> listaCategorie { 
            get
            {
                DAL.Repository.CategorieRepository repo = new DAL.Repository.CategorieRepository();
                return (List<Categorie>)repo.GetCategorie();
            } 
        }
        public List<Taglie> listaTaglie
        {
            get
            {
                DAL.Repository.ConfigurazioniRepository repo = new DAL.Repository.ConfigurazioniRepository();
                return (List<Taglie>)repo.getTaglie();
            }
            
        }
        public List<Tag> listaTags
        {
            get
            {
                DAL.Repository.TagRepository repo = new DAL.Repository.TagRepository();
                return (List<Tag>)repo.GetTags();
            }
        }
        public List<GruppoTaglia> GruppiTaglia
        {
            get
            {
                DAL.Repository.GruppiTagliaRepository repo = new DAL.Repository.GruppiTagliaRepository();
                return repo.getGruppoTagliaPerProdotti();
            }
        }
        public List<GruppoColori> GruppiColori
        {
            get
            {
                DAL.Repository.GruppiTagliaRepository repo = new DAL.Repository.GruppiTagliaRepository();
                return repo.getGruppoColoriPerProdotti();
            }
        }
        public List<GruppoSpessori> GruppiSpessori
        {
            get
            {
                DAL.Repository.GruppiTagliaRepository repo = new DAL.Repository.GruppiTagliaRepository();
                return repo.getGruppoSpessoriPerProdotti();
            }
        }

        public List<Tipi_Immagini> listaTipi_Immagini
        {
            get
            {
                DAL.Repository.TipiImmaginiRepository repo = new DAL.Repository.TipiImmaginiRepository();
                return repo.getTipiImmaginiProdotti();
            }
        }
        public List<Sottocategorie> ListaSottocategorie { get; set; }
        public List<View_DettaglioAlimentiProdotto> listaAlimentiProdotti { get; set; }
        public List<DettaglioProdotti> ListaProdotti { get; set; }
        public List<Prodotti> ListaProduct { get; set; }
        public DettaglioProdotti DettaglioProdotti { get; set; }
        public List<DettaglioProdottiTaglie> dettaglioProdottiTaglie { get; set; }
        public List<int> id_taglie { get; set; }

        public List<int> tags_id { get; set; }
        public int Count { get; set; }
        public bool Ita { get; set; }
        public bool Eng {  get; set; }
        public string DescrizioneCambiata { get; set; }
        public List<int> IVAs
        {
            get
            {
                List<int> ivas = new List<int>() { 4, 10, 22 };
                return ivas;
            }
        }

        public bool offerta_visible { get; set; }

        public ModelEditProdotto()
        {
            ListaProduct = new List<Prodotti>(); 
            DettaglioProdotti = new DettaglioProdotti();
            dettaglioProdottiTaglie = new List<DettaglioProdottiTaglie>();

        }

        public int id_gruppo_taglie { get; set; }
        public int id_gruppo_spessori { get; set; }
        public int id_gruppo_colori { get; set; }


        public List<Categorie> listaCollezioni
        {
            get
            {
                DAL.Repository.CollezioniRepository repo = new DAL.Repository.CollezioniRepository();
                // Filtriamo prendendo solo le categorie che sono anche collezioni
                return repo.getCollezioniAdmin();
            }
        }

        public List<int> collezioni_id { get; set; }
    }
}