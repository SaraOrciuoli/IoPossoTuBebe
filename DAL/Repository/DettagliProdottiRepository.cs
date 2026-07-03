using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Repository
{
    public class DettagliProdottiRepository
    {
        internal AnyeLabelEntities db = new AnyeLabelEntities();

        public List<DettaglioProdotti> GetDettaglioProdotti()
        {
            return db.DettaglioProdotti.OrderBy(x => x.Descrizione).ToList();
        }
        public DettaglioProdotti GetDettaglioProdottiById(int id)
        {
            return db.DettaglioProdotti.Where(x => x.id_prodotto == id).FirstOrDefault();
        }
        public Prodotti GetProdottiById(int id)
        {
            return db.Prodotti.AsNoTracking().FirstOrDefault(p => p.id_prodotto == id);
        }

        public List<Immagini_catProd> getImgProductByIdProd(int id)
        {
            return db.Immagini_catProd.Where(x => x.id_prod == id).ToList();
        }

        public void updateImgProduct(int key, string values)
        {
            var img = db.Immagini_catProd.Where(c => c.id_immagini == key).FirstOrDefault(); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, img);
            try
            {
                db.Entry(img).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public void addImgProduct(Immagini_catProd img_CatProd)
        {
            try
            {
                db.Immagini_catProd.Add(img_CatProd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void addAssociazioneProdottoTaglia(ProdottoTaglia associazione)
        {
            try
            {
                db.ProdottoTaglia.Add(associazione);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }


        public List<Tipi_Immagini> GetTipoImgProd()
        {
            return db.Tipi_Immagini.Where(t => t.Prodotto == true).ToList();
        }

        public void SvuotaCollezioniProdotto(int id_prodotto)
        {
            using (var db = new AnyeLabelEntities()) 
            {
                var associazioni = db.Associazione_Prodotto_Collezione.Where(x => x.id_prodotto == id_prodotto).ToList();
                if (associazioni.Any())
                {
                    db.Associazione_Prodotto_Collezione.RemoveRange(associazioni);
                    db.SaveChanges();
                }
            }
        }

        public void SaveProdCollezioni(int id_prodotto, List<int> collezioni_id)
        {
            if (collezioni_id != null && collezioni_id.Any())
            {
                using (var db = new AnyeLabelEntities()) 
                {
                    foreach (var id_collezione in collezioni_id)
                    {
                        Associazione_Prodotto_Collezione associazione = new Associazione_Prodotto_Collezione
                        {
                            id_prodotto = id_prodotto,
                            id_collezione = id_collezione
                        };
                        db.Associazione_Prodotto_Collezione.Add(associazione);
               
                    }
                    db.SaveChanges();
                }
            }
        }

        public void deleteProdotto(Prodotti p)
        {
            try
            {
                p.eliminato = true;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }
        public void deleteImgProduct(Immagini_catProd img)
        {
            try
            {
                db.Entry(img).State = System.Data.Entity.EntityState.Deleted;
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

        public int AddProdotto(Prodotti m)
        {
            int id_prod = 0;
            try
            {
                db.Prodotti.Add(m);
                db.SaveChanges();
                id_prod = m.id_prodotto;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return id_prod;
        }



        public void UpdateProdotto(Prodotti m)
        {
            try
            {
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }
        public void updateProdottoTaglia(ProdottoTaglia m)
        {
            try
            {
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public void SvuotaTagProdotto(int id_prod)
        {
            foreach (ProdottiTag pt in db.ProdottiTag.Where(x => x.id_prodotti == id_prod).ToList())
            {
                db.Entry(pt).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();
        }

        public void SaveProdottoTaglia(ProdottoTaglia pt)
        {
            db.ProdottoTaglia.Add(pt);
            db.SaveChanges();
        }
        public Immagini_catProd getImgCatProdById(int id)
        {
            return db.Immagini_catProd.Where(img => img.id_immagini == id).FirstOrDefault();
        }

        public ProdottoTaglia getProdottoTagliaByParams(int id_taglia, int id_colore, int id_spessore, int id_prod)
        {
            return db.ProdottoTaglia
                .Where(pt => pt.id_prodotto == id_prod
                          && pt.id_taglia == id_taglia
                          && pt.id_colore == id_colore
                          && pt.id_spessore == id_spessore
                          && pt.eliminato == false)
                .FirstOrDefault();
        }
        public Taglie getTagliaById(int id)
        {
            return db.Taglie.Where(x => x.id_taglia == id).FirstOrDefault();
        }

        public int GetMaxQtaValue(int tagliaId, int prodottoId)
        {
            var prodottoTaglia = db.ProdottoTaglia.FirstOrDefault(pt => pt.id_prodotto == prodottoId && pt.id_taglia == tagliaId && !pt.eliminato);
            return prodottoTaglia != null ? prodottoTaglia.quantita : 0;
        }
        public List<DettaglioProdottiTaglie> getListTaglieByProdId(int id)
        {
            return db.DettaglioProdottiTaglie.Where(x => x.id_prodotto == id && x.id_taglia != null).ToList();
        }
        public List<DettaglioProdottiTaglie> GetUniqueTaglieByProdId(int id)
        {
            return db.DettaglioProdottiTaglie
                     .Where(x => x.id_prodotto == id && x.id_taglia != null && x.eliminato == false && x.quantita > 0) 
                     .ToList();
        }


        public List<DettaglioProdottiTaglie> GetUniqueColoriByProdId(int id)
        {
            return db.DettaglioProdottiTaglie
                     .Where(x => x.id_prodotto == id && x.id_colore != null)
                     .GroupBy(x => x.id_colore) 
                     .Select(g => g.FirstOrDefault())  
                     .ToList();
        }
        public List<DettaglioProdottiTaglie> GetUniqueSpessoriByProdId(int id)
        {
            return db.DettaglioProdottiTaglie
                     .Where(x => x.id_prodotto == id && x.id_spessore != null)
                     .GroupBy(x => x.id_spessore)
                     .Select(g => g.FirstOrDefault())
                     .ToList();
        }

        public ProdottoTaglia getProdottoTagliaByIds(int id_prod, int id_taglia)
        {
            return db.ProdottoTaglia.Where(x => x.id_prodotto == id_prod && x.id_taglia == id_taglia && x.eliminato == false).FirstOrDefault();
        }
        public ProdottoTaglia getProdottoTagliaById(int key)
        {
            return db.ProdottoTaglia.Where(x => x.id_prodottoTaglia == key).FirstOrDefault();
        }
        public ProdottoTaglia getProdottoTagliaBySpecificId(int id_prodotto_taglia)
        {
            return db.ProdottoTaglia.Where(x => x.id_prodottoTaglia == id_prodotto_taglia && x.eliminato == false).FirstOrDefault();
        }

        public void deleteTagliaProdotto(ProdottoTaglia pt)
        {
            try
            {
                db.Entry(pt).State = System.Data.Entity.EntityState.Deleted;
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

        public void AggiornaTagliaProdotto(ProdottoTaglia pt)
        {
            db.Entry(pt).State = EntityState.Modified;
            db.SaveChanges();
        }

        public List<ProdottoTaglia> getProdottoTagliaByIdProdotto(int id)
        {
            return db.ProdottoTaglia.Where(x => x.id_prodotto == id && x.eliminato == false).ToList();
        }
        public void updateProdottoTaglia(int key, string values)
        {
            var pt = db.ProdottoTaglia.Where(c => c.id_prodottoTaglia == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, pt);
            try
            {
                db.Entry(pt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public string GetMaxProdId()
        {
            int maxId = db.Prodotti.Max(x => x.id_prodotto) + 1;
            return maxId.ToString();
        }
        public Prodotti getProdottoByBarCode(string barcode)
        {
            return db.Prodotti.Where(x => x.BarCode == barcode).FirstOrDefault();
        }

        public ProdottoTaglia getProdottoTagliaByBarcode(string barcode)
        {
            return db.ProdottoTaglia.Where(x => x.BarCode == barcode).FirstOrDefault();
        }

        public List<Taglie> GetTaglie()
        {
            return db.Taglie.ToList();
        }
        public void SvuotaTaglieProdotto(int id_prod)
        {
            foreach (ProdottoTaglia pt in db.ProdottoTaglia.Where(x => x.id_prodotto == id_prod).ToList())
            {
                db.Entry(pt).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();
        }

        public void updateImgCatProd(Immagini_catProd el)
        {
            db.Entry(el).State = EntityState.Modified; 
            db.SaveChanges();
        }
        public List<Slider> getSliderByProd(int prod) {
            return db.Slider.Where(x => x.id_prodotto == prod).ToList();
        }

        public void CheckTaglieProdotto(Prodotti p)
        {
            foreach(GruppoTaglia_Taglie el in db.GruppoTaglia_Taglie.Where(x => x.id_gruppoTaglia == p.id_gruppo_taglie).ToList())
            {
                if(db.ProdottoTaglia.Where(x => x.id_prodotto == p.id_prodotto && x.id_taglia == el.id_taglia).FirstOrDefault() == null)
                {
                    db.ProdottoTaglia.Add(new ProdottoTaglia()
                    {
                        id_taglia = el.id_taglia,
                        id_prodotto = p.id_prodotto,
                        quantita = 0,
                        BarCode = "",
                    });
                    db.SaveChanges();
                }
            }
            
        }
        public List<DettaglioProdottiTaglie> getConfigurazioniProdottiByIdProdotto(int idProdotto)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();

            var result = db.DettaglioProdottiTaglie.Where(x => x.id_prodotto == idProdotto).ToList() ;

            return (List<DettaglioProdottiTaglie>)result;
        }

        public void GeneraCombinazioniProdotto(int idProdotto, int? idGruppoTaglie, int? idGruppoColori, int? idGruppoSpessori, double prezzoVendita)
        {
            using (var db = new AnyeLabelEntities())
            {
                // Recupera gli elementi dei gruppi selezionati
                var taglie = new List<int>();
                var colori = new List<int>();
                var spessori = new List<int>();

                // Recupera le taglie del gruppo
                if (idGruppoTaglie.HasValue)
                {
                    taglie = db.GruppoTaglia_Taglie
                              .Where(gt => gt.id_gruppoTaglia == idGruppoTaglie.Value)
                              .Select(gt => gt.id_taglia)
                              .ToList();
                }

                // Recupera i colori del gruppo
                if (idGruppoColori.HasValue)
                {
                    colori = db.GruppoColori_Colori
                              .Where(gc => gc.id_gruppoColori == idGruppoColori.Value)
                              .Select(gc => gc.id_colore ?? 0)
                              .ToList();
                }

                // Recupera gli spessori del gruppo
                if (idGruppoSpessori.HasValue)
                {
                    spessori = db.GruppoSpessori_Spessore
                                .Where(gs => gs.id_gruppoSpessori == idGruppoSpessori.Value)
                                .Select(gs => gs.id_spessore ?? 0)
                                .ToList();
                }

                // Se non ci sono gruppi selezionati, esci
                if (!taglie.Any() && !colori.Any() && !spessori.Any())
                    return;

                // Genera tutte le combinazioni possibili
                var combinazioni = new List<ProdottoTaglia>();

                // Caso 1: Solo taglie (compatibilità con sistema esistente)
                if (taglie.Any() && !colori.Any() && !spessori.Any())
                {
                    foreach (var taglia in taglie)
                    {
                        combinazioni.Add(new ProdottoTaglia
                        {
                            id_prodotto = idProdotto,
                            id_taglia = taglia,
                            id_colore = 0,
                            id_spessore = 0,
                            quantita = 9999,
                            BarCode = GeneraBarcodeCombinazione(idProdotto, taglia, null, null),
                            prezzo = prezzoVendita 
                        });
                    }
                }
                // Caso 2: Combinazioni complete
                else
                {
                    // Crea liste default se alcuni gruppi non sono selezionati
                    var taglieDaUsare = taglie.Any() ? taglie : new List<int> { 0 };
                    var coloriDaUsare = colori.Any() ? colori : new List<int> { 0 };
                    var spessoriDaUsare = spessori.Any() ? spessori : new List<int> { 0 };

                    foreach (var taglia in taglieDaUsare)
                    {
                        foreach (var colore in coloriDaUsare)
                        {
                            foreach (var spessore in spessoriDaUsare)
                            {
                                // Salta la combinazione 0,0,0 (nessun gruppo selezionato)
                                if (taglia == 0 && colore == 0 && spessore == 0)
                                    continue;

                                combinazioni.Add(new ProdottoTaglia
                                {
                                    id_prodotto = idProdotto,
                                    id_taglia = taglia != 0 ? taglia : 0,
                                    id_colore = colore != 0 ? colore : 0,
                                    id_spessore = spessore != 0 ? spessore : 0,
                                    quantita = 9999,
                                    BarCode = GeneraBarcodeCombinazione(idProdotto, taglia, colore, spessore),
                                    prezzo = prezzoVendita
                                });
                            }
                        }
                    }
                }

                // Elimina combinazioni esistenti per questo prodotto
                var esistenti = db.ProdottoTaglia.Where(pt => pt.id_prodotto == idProdotto).ToList();
                db.ProdottoTaglia.RemoveRange(esistenti);

                // Aggiungi le nuove combinazioni
                db.ProdottoTaglia.AddRange(combinazioni);
                db.SaveChanges();
            }
        }

        private string GeneraBarcodeCombinazione(int idProdotto, int? taglia, int? colore, int? spessore)
        {
            // Implementa la tua logica per generare barcode univoci
            var baseBarcode = idProdotto.ToString("D6");
            var suffisso = "";

            if (taglia.HasValue && taglia.Value > 0)
                suffisso += $"T{taglia.Value:D3}";

            if (colore.HasValue && colore.Value > 0)
                suffisso += $"C{colore.Value:D3}";

            if (spessore.HasValue && spessore.Value > 0)
                suffisso += $"S{spessore.Value:D3}";

            return baseBarcode + suffisso;
        }

        public void AggiornaPrezziPerCriteri(int idProdotto, List<int> dimensioniIds, List<int> coloriIds, List<int> spessoriIds, decimal incremento, string tipoOperazione)
        {
            using (var db = new AnyeLabelEntities())
            {
                var query = db.ProdottoTaglia.Where(pt => pt.id_prodotto == idProdotto);

                // Filtra per dimensioni (considera anche il caso valore 0 = "nessuna dimensione")
                if (dimensioniIds != null && dimensioniIds.Any())
                {
                    if (dimensioniIds.Contains(0))
                    {
                        // Se è selezionato 0, include anche i record con id_taglia null o 0
                        query = query.Where(pt => !pt.id_taglia.HasValue || pt.id_taglia == 0 || dimensioniIds.Contains(pt.id_taglia.Value));
                    }
                    else
                    {
                        query = query.Where(pt => pt.id_taglia.HasValue && dimensioniIds.Contains(pt.id_taglia.Value));
                    }
                }

                // Filtra per colori
                if (coloriIds != null && coloriIds.Any())
                {
                    if (coloriIds.Contains(0))
                    {
                        query = query.Where(pt => !pt.id_colore.HasValue || pt.id_colore == 0 || coloriIds.Contains(pt.id_colore.Value));
                    }
                    else
                    {
                        query = query.Where(pt => pt.id_colore.HasValue && coloriIds.Contains(pt.id_colore.Value));
                    }
                }

                // Filtra per spessori
                if (spessoriIds != null && spessoriIds.Any())
                {
                    if (spessoriIds.Contains(0))
                    {
                        query = query.Where(pt => !pt.id_spessore.HasValue || pt.id_spessore == 0 || spessoriIds.Contains(pt.id_spessore.Value));
                    }
                    else
                    {
                        query = query.Where(pt => pt.id_spessore.HasValue && spessoriIds.Contains(pt.id_spessore.Value));
                    }
                }

                var combinazioniDaAggiornare = query.ToList();

                foreach (var combinazione in combinazioniDaAggiornare)
                {
                    if (tipoOperazione == "aggiungi")
                    {
                        combinazione.prezzo = combinazione.prezzo + (double)incremento;
                    }   
                    else if (tipoOperazione == "sostituisci")
                    {
                        combinazione.prezzo = (double)incremento;
                    }
                }

                db.SaveChanges();

                Console.WriteLine($"Aggiornate {combinazioniDaAggiornare.Count} combinazioni per il prodotto {idProdotto}");
            }
        }


        public void DuplicaProdotto(int idProdottoSorgente, int numeroCopie)
        {
            using (var db = new AnyeLabelEntities())
            {
                var prodottoBase = db.Prodotti.FirstOrDefault(p => p.id_prodotto == idProdottoSorgente);
                if (prodottoBase == null)
                    throw new Exception("Prodotto sorgente non trovato.");

                for (int i = 1; i <= numeroCopie; i++)
                {
                    Prodotti nuovoProdotto = new Prodotti
                    {
                        Descrizione = prodottoBase.Descrizione + " - copia " + i,
                        Ingredienti = prodottoBase.Ingredienti,
                        Descrizione_eng = prodottoBase.Descrizione_eng,
                        Ingredienti_eng = prodottoBase.Ingredienti_eng,
                        id_categoria = prodottoBase.id_categoria,
                        id_sottocategoria = prodottoBase.id_sottocategoria,
                        id_Marca = prodottoBase.id_Marca,
                        PrezzoAcquisto = prodottoBase.PrezzoAcquisto,
                        PrezzoVendita = prodottoBase.PrezzoVendita,
                        iva = prodottoBase.iva,
                        Quantita = prodottoBase.Quantita,
                        MostraWeb = prodottoBase.MostraWeb,
                        prezzo_visibile = prodottoBase.prezzo_visibile,
                        in_offerta = prodottoBase.in_offerta,
                        sconto = prodottoBase.sconto,

                        pathFoto = null,
                        eliminato = false
                    };

                    db.Prodotti.Add(nuovoProdotto);
                    db.SaveChanges();


                    //nuovoProdotto.BarCode = GetNewBarcode(nuovoProdotto.id_prodotto.ToString());
                    db.Entry(nuovoProdotto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}
