using DAL.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Repository
{
    public class ProdottiRepository
    {
        internal AnyeLabelEntities db = new AnyeLabelEntities();
        public static int CountProdotti()
        {
            AnyeLabelEntities barEntities = new AnyeLabelEntities();
            return barEntities.DettaglioProdotti.Where(x => x.eliminato != true).Count();
        }

        public Immagini_catProd GetProdottiSliderByIdProdotto(int idProd)
        {
            return db.Immagini_catProd.Where(x => x.id_prod == idProd).FirstOrDefault();
        }
        public List<Prodotti> GetProdotti()
        {
            return db.Prodotti.Where(x => x.eliminato != true).OrderBy(x => x.Descrizione).ToList();
        }


        public List<View_ProdPiuVenduti> GetProdottipiuVenduti()
        {
            return db.View_ProdPiuVenduti.Where(x => x.eliminato != true).OrderBy(x => x.Descrizione).ToList();
        }
        public List<Immagini_catProd> GetListProdottiSlider()
        {
            return db.Immagini_catProd.ToList();
        }
        public List<Prodotti> GetServizi()
        {
            return db.Prodotti.Where(x => x.eliminato == false && x.is_servizio == true).ToList();
        }

        public List<Prodotti> GetProdottiByBrand(int idMarca)
        {

           return db.Prodotti.Where(p => p.id_Marca == idMarca && p.eliminato == false && p.MostraWeb == true).ToList();
            
        }
        public List<Prodotti> GetPopupProdotti()
        {
            return db.Prodotti.Where(x => x.Popup == true).ToList();
        }

        public Prodotti GetServizioById(int id)
        {
            return db.Prodotti.Where(x => x.eliminato == false & x.is_servizio == true && x.id_prodotto == id).FirstOrDefault();
        }

        public Prodotti GetLastInsertedProductID()
        {
            return db.Prodotti.Where(x => x.eliminato == false)
                              .OrderByDescending(x => x.id_prodotto)
                              .FirstOrDefault();
        }
        

        public static string GetServizioByBarCode(string barCode)
        {
            AnyeLabelEntities barEntities = new AnyeLabelEntities();
            return barEntities.Prodotti.Where(x => x.BarCode == barCode && x.is_servizio == true && x.eliminato == false).Select(x => x.id_prodotto).FirstOrDefault().ToString();
        }
        public void UpdateServizio(Prodotti p)
        {    
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

          public void AddServizio(Prodotti p)
          {
              db.Entry(p).State = System.Data.Entity.EntityState.Added;
              db.SaveChanges();
          }
        
                public void DeleteServizio(Prodotti p)
                {
                    db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

        public List<Prodotti> GetProdottiByCat(int idCat)
        {
            return db.Prodotti.Where(x => x.id_categoria == idCat && x.eliminato != true && x.Quantita > 0).OrderBy(x => x.Descrizione).ToList();
        }
        public List<Prodotti> GetProdottiBySotCat(int idSotCat)
        {
            return db.Prodotti.Where(x => x.id_sottocategoria == idSotCat && x.eliminato != true && x.Quantita > 0).OrderBy(x => x.Descrizione).ToList();
        }

        public Prodotti GetProdottoById(int? id)
        {
            return db.Prodotti.Where(x => x.id_prodotto == id).FirstOrDefault();
        }
        public List <Prodotti> GetAllProdotti()
        {
            {
                return db.Prodotti.Where(p => p.eliminato != true).ToList();
            }
        }
        public void SalvaProdotto(Prodotti prodotto)
        {
            try
            {
                db.Entry(prodotto).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                string exception2 = dbEx.Message;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        public void UpdateProdotto(Prodotti prodotto)
        {
            try
            {
                db.Entry(prodotto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                string exception2 = dbEx.Message;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        public void deleteProdotto(Prodotti p)
        {
            try
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        public string GetImageCategoria(int idCat)
        {
            return db.Categorie.Where(x => x.id_categorie == idCat).Select(x => x.PathIcona).FirstOrDefault();
        }

        public static string GetProdottoByBarCode(string barcode)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();

            return webEntities.Prodotti.Where(x => x.BarCode == barcode && (x.eliminato != null ? x.eliminato : true) == false).Select(x => x.id_prodotto).FirstOrDefault().ToString();
        }

        public List<Prodotti> GetProdottiInOfferta()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Prodotti.Where(x => x.in_offerta == true).ToList();
        }

        public static Prodotti GetProdottoByBarCode2(string barcode)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Prodotti.Where(x => x.BarCode == barcode).FirstOrDefault();
        }
        public static List<Prodotti> getProdotti()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Prodotti.Where(x => x.eliminato == false && (x.is_servizio == false || x.is_servizio == null)).ToList();
        }

        public static List<DettaglioProdottiOrdine> Getprodottiordine()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.DettaglioProdottiOrdine.Where(x => x.id_prodotto > 0).ToList();
        }

        public static List<ProdottoTaglia> GetListaProdottiTaglia()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.ProdottoTaglia.Where(x => x.eliminato == false).ToList();
        }
        public static ProdottoTaglia GetProdottoTagliaById(int id)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.ProdottoTaglia.Where(x => x.id_prodottoTaglia == id).FirstOrDefault();
        }

        public void DeleteProduct(int idProdottiOrdine)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var product = db.DettaglioProdottiOrdine.FirstOrDefault(p => p.id_prodotto == idProdottiOrdine);
            if (product != null)
            {
                db.DettaglioProdottiOrdine.Remove(product);
                db.SaveChanges();
            }
        }

        public static List<string> GetNomiTaglie()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return (from pt in webEntities.ProdottoTaglia
                    join t in webEntities.Taglie on pt.id_taglia equals t.id_taglia
                    where pt.eliminato == false
                    select t.Descrizione_taglia).Distinct().ToList();
        }

        public static IEnumerable<object> GetMovimentiMagazzino()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.View_DettaglioMovimentiMagazzino.ToList();
        }

        public void AddMovimentoMagazzino(MovimentiMagazzino mov)
        {
            try
            {
                db.Entry(mov).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                string exception2 = dbEx.Message;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        public static List<ProdottiOrdine> GetprodottiordineByIdordine(int id_ordine)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.ProdottiOrdine.Where(x => x.id_ordine == id_ordine).ToList();
        }

        public string GetNomeTagliaById(int idTaglia)
        {
            return db.Taglie.Where(t => t.id_taglia == idTaglia).Select(t => t.Descrizione_taglia).FirstOrDefault();
        }

        //public int GetQuantitaMassimaDalDatabase()
        //{
        //    AnyeLabelEntities webEntities = new AnyeLabelEntities();
        //    int quantitaMassima = webEntities.Prodotti.Max(p => p.Quantita);
        //    return quantitaMassima;
        //}


        /*     public List<DettaglioProdotti> cercaProdottoByLotto(string lotto)
             {
                 return db.DettaglioProdotti.Where(p => p.LottoProdotto.Contains(lotto)).ToList();
             }

             public int CountProdottoByLotto(string lotto)
             {
                 return db.DettaglioProdotti.Where(p => p.LottoProdotto.Contains(lotto)).Count();
             }

             public int CountProdottoByLottoAlimento(string lottoAli)
             {
                 return db.View_DettaglioAlimentiProdotto.Where(p => p.LottoAlimento.Contains(lottoAli)).Count();
             }

             public List<View_DettaglioAlimentiProdotto> cercaProdottoByLottoAlimento(string lottoAli)
             {
                 return db.View_DettaglioAlimentiProdotto.Where(p => p.LottoAlimento.Contains(lottoAli)).ToList();
             }*/
        public static void deleteConfigurazioneProdotto(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            ProdottoTaglia u = db.ProdottoTaglia.Where(x => x.id_prodottoTaglia == key).FirstOrDefault();
            if (u != null)
            {
                try
                {
                    db.Entry(u).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    string exception2 = dbEx.Message;
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
            }
        }
        public List<DettaglioProdottiTaglie> getProdottiTaglieByIdProd(int idProd)
        {
            return db.DettaglioProdottiTaglie.Where(x => x.id_prodotto == idProd).ToList();
        }

        /*        public static List<string> getProdottoByBarCodeTaglie(string barcode)
                {
                    AnyeLabelEntities db = new AnyeLabelEntities();
                    var resulto =
           (from li in db.DettaglioProdottiTaglie 
           where li.BarCode == barcode
           select new { li.id_prodotto, li.Descrizione_taglia } ).ToList();

                    return resulto;
                    *//* db.DettaglioProdottiTaglie.Where(x => x.BarCode == barcode).Select(x => x.id_prodotto).FirstOrDefault().ToString();*//*
                    //return db.DettaglioProdottiTaglie.Where(x => x.BarCode == barcode).Select(x=> new { x.BarCode, x.Descrizione_taglia }).ToList();

                }*/
        public List<DettaglioProdottiTaglie> getProdottoByBarCodeTaglie(string barcode)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            var test = webEntities.DettaglioProdottiTaglie.Where(x => x.BarCode == barcode).ToList();
            return webEntities.DettaglioProdottiTaglie.Where(x => x.BarCode == barcode).ToList();
        }

    }
}
