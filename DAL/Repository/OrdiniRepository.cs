using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class OrdiniRepository
    {
       internal AnyeLabelEntities db = new AnyeLabelEntities();

        public List<DettagliOrdine> GetOrdini()
        {
            return db.DettagliOrdine.OrderByDescending(x => x.DataOra).ToList();
        }
        public List<DettagliOrdine> GetOrdiniDaEvadere()
        {
            return db.DettagliOrdine.Where(x=>x.id_stato == 1).OrderByDescending(x => x.DataOra).ToList();
        }

        public double CalcolaTotaleCarrelloNetto(List<DettaglioProdottiOrdine> carrelloCliente)
        {
            using (var db = new AnyeLabelEntities())
            {
                double totale = 0;
                foreach (var el in carrelloCliente)
                {
                    Prodotti prodottoDb = db.Prodotti.FirstOrDefault(x => x.id_prodotto == el.id_prodotto);
                    if (prodottoDb != null)
                    {
                        bool inOfferta = prodottoDb.in_offerta ?? false;
                        double prezzoScontato = inOfferta ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (prodottoDb.sconto / 100))) : (double)el.prezzoUnitario;

                        totale += prezzoScontato * el.Quantita;
                    }
                }
                return totale;
            }
        }
        public Ordine GetOrdineById(int id)
        {
            return db.Ordine.Where(x => x.id_ordine == id).FirstOrDefault();
        }
        public DettagliOrdine GetDettagliOrdineById(int ordineId)
        {
            return db.DettagliOrdine
                     .FirstOrDefault(d => d.id_ordine == ordineId); 
        }
        public List<Stato_Ordine> GetStatiOrdine() 
        {
            return db.Stato_Ordine.ToList();
        }public List<Stato_Pagamento> GetStatiPagamento() 
        {
            return db.Stato_Pagamento.ToList();
        }
        public List<TipoOrdine> GetTipiOrdine()
        {
            return db.TipoOrdine.ToList();
        }
        public List<TipoPagamento> GetTipiPagamento()
        {
            return db.TipoPagamento.ToList();
        }

        public List<DettaglioProdottiOrdine> GetProdottiOrdineByIdOrdine(int id)
        {
            return db.DettaglioProdottiOrdine.Where(x => x.id_ordine == id).ToList();
        }
        public List<Prodotti> GetProdottiForOrdine()
        {
            return db.Prodotti.Where(x => x.Quantita > 0).ToList();
        }

       
        public List<ProdottiOrdine> getPOByOrderId(int id)
        {
            return db.ProdottiOrdine.Where(x => x.id_ordine == id).ToList();
        }
        public ProdottiOrdine getProdottoOrdineByPOid(int id)
        {
            return db.ProdottiOrdine.Where(x => x.id_ProdottiOrdine == id).FirstOrDefault();
        }

        public void UpdateOrdine(Ordine ord)
        {
            try
            {
                db.Entry(ord).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void UpdateProdottiOrdine(int key, string values)
        {
            ProdottiOrdine p_o = db.ProdottiOrdine.Where(c => c.id_ProdottiOrdine == key).FirstOrDefault();
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(values);
            if(data.id_prodotto != null) p_o.id_prodotto = data.id_prodotto ;
            if(data.id_ordine != null) p_o.id_ordine = data.id_ordine ;
            if(data.Quantita != null) p_o.Quantita = data.Quantita ;
            if(data.prezzoUnitario != null) p_o.prezzoUnitario = data.prezzoUnitario ;
            if(data.Note != null) p_o.Note = data.Note ;
            if(data.iva != null) p_o.iva = data.iva ;
            if(data.Sconto != null) p_o.Sconto = data.Sconto ;
            if(data.id_taglia != null) p_o.id_taglia = data.id_taglia;

            try
            {
                db.Entry(p_o).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public static List<DettaglioProdottiOrdine> GetListaProdottiOrdine(int idOrd)
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            return entities.DettaglioProdottiOrdine.Where(x => x.id_ordine == idOrd).ToList();
        }

        public void AddProdottiOrdine(ProdottiOrdine p_o)
        {
            try
            {
                db.ProdottiOrdine.Add(p_o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public int AddOrdine(Ordine ord)
        {
            int id_ordine = 0;
            try
            {
                db.Ordine.Add(ord);
                db.SaveChanges();
                id_ordine = ord.id_ordine;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return id_ordine;
        }
        

        public void DeleteProdottiOrdine(ProdottiOrdine p_o)
        {
            try
            {
                db.Entry(p_o).State = System.Data.Entity.EntityState.Deleted;
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

        public void DeleteOrdine(Ordine ord)
        {
            try
            {
                db.Entry(ord).State = System.Data.Entity.EntityState.Deleted;
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
        public Ordine CreaNuovoOrdine(int userId, string metodoPagamento, List<DettaglioProdottiOrdine> carrelloCliente, double speseSpedizione, string idCodiceScontoStr, string ipAddress)
        {
            using (var db = new AnyeLabelEntities())
            {
                Utenti utente = db.Utenti.FirstOrDefault(u => u.id_utente == userId);
                TipoPagamento tipoPag = db.TipoPagamento.FirstOrDefault(tp => tp.descrizione_tipo_pagamento.ToLower() == metodoPagamento.ToLower());

                if (utente == null || tipoPag == null)
                    throw new Exception("Utente o metodo di pagamento non validi.");

                int idStatoOrdine = (tipoPag.id_tipo_pagamento == 6) ? 3 : 1;
                int idStatoPagamento = (idStatoOrdine == 3) ? 4 : 1;

                // 1. Ricalcolo sicuro dei prezzi dal DB
                List<DettaglioProdottiOrdine> righeOrdine = new List<DettaglioProdottiOrdine>();
                double totaleOrdine = 0;

                foreach (var el in carrelloCliente)
                {
                    Prodotti prodottoDb = db.Prodotti.FirstOrDefault(x => x.id_prodotto == el.id_prodotto);
                    if (prodottoDb == null) continue;

                    bool inOfferta = prodottoDb.in_offerta ?? false;
                    double prezzoScontato = inOfferta ? (double)(el.prezzoUnitario - (el.prezzoUnitario * (prodottoDb.sconto / 100))) : (double)el.prezzoUnitario;

                    righeOrdine.Add(new DettaglioProdottiOrdine()
                    {
                        id_prodotto = el.id_prodotto,
                        Quantita = el.Quantita,
                        prezzoUnitario = (double)el.prezzoUnitario,
                        Sconto = prodottoDb.sconto,
                        prezzoScontato = prezzoScontato,
                        InOfferta = inOfferta
                    });

                    totaleOrdine += prezzoScontato * el.Quantita;
                }

                // 2. Creazione Testata
                Ordine nuovoOrdine = new Ordine
                {
                    DataOra = DateTime.Now,
                    Ip = ipAddress,
                    NumeroPezzi = righeOrdine.Sum(p => p.Quantita),
                    Totale = totaleOrdine,
                    Indirizzo = utente.indirizzo,
                    Cap = utente.Cap,
                    Nominativo = $"{utente.Nome} {utente.Cognome}",
                    NumTelefono = utente.Cellulare,
                    id_utente = utente.id_utente,
                    Tipo_Pagamento = tipoPag.descrizione_tipo_pagamento,
                    Citta = utente.Citta,
                    Nazione = utente.Nazione,
                    TipoOrdine = "online",
                    NumeroPersone = 1,
                    Email = utente.Mail,
                    id_tipoOrdine = 4,
                    id_tipoPagamento = tipoPag.id_tipo_pagamento,
                    id_stato = idStatoOrdine,
                    id_stato_pagmento = idStatoPagamento,
                    totale_spedizione = speseSpedizione
                };

                // 3. Gestione Codice Sconto
                if (!string.IsNullOrEmpty(idCodiceScontoStr))
                {
                    int idSconto = Convert.ToInt32(idCodiceScontoStr);
                    CodiciSconto codiceDb = db.CodiciSconto.FirstOrDefault(c => c.id_codice_sconto == idSconto);

                    if (codiceDb != null && (codiceDb.id_utente == null || codiceDb.id_utente == utente.id_utente))
                    {
                        nuovoOrdine.id_codiceSconto = codiceDb.id_codice_sconto;
                        if (codiceDb.id_tipoSconto != 1)
                        {
                            codiceDb.utilizzato = true;
                            codiceDb.data_utilizzo = DateTime.Now;
                            db.Entry(codiceDb).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }

                // 4. Salvataggio Testata
                db.Ordine.Add(nuovoOrdine);
                db.SaveChanges();

                // 5. Salvataggio Dettagli e Aggiornamento Magazzino
                foreach (var riga in righeOrdine)
                {
                    ProdottiOrdine rigaDb = new ProdottiOrdine
                    {
                        id_prodotto = riga.id_prodotto,
                        id_ordine = nuovoOrdine.id_ordine,
                        Quantita = riga.Quantita,
                        prezzoUnitario = riga.prezzoUnitario
                    };

                    db.ProdottiOrdine.Add(rigaDb);


                    UpdateQuantitaProdottoAfterOrder(rigaDb.id_prodotto, rigaDb.Quantita);
                }
                db.SaveChanges();

                return nuovoOrdine;
            }
        }
        public static Ordine salvaOrdine(Ordine ord)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.Ordine.Add(ord);
            db.SaveChanges();
            return ord;
        }

        public static void SalvaProdottoOrdine(ProdottiOrdine prodOrdine)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.ProdottiOrdine.Add(prodOrdine);
            db.SaveChanges();
        }
        public static void UpdateQuantitaProdottoAfterOrder(int id_prodotto, int quantita)
        {
            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                // 1. Cerchiamo solo il Prodotto principale
                Prodotti p = db.Prodotti.Where(x => x.id_prodotto == id_prodotto).FirstOrDefault();

                if (p != null)
                {
                    // 2. Scaliamo la quantità
                    int nuovaQuantita = (p.Quantita) - quantita;

                    // Per sicurezza, evitiamo che il magazzino vada sotto zero
                    p.Quantita = nuovaQuantita < 0 ? 0 : nuovaQuantita;

                    try
                    {
                        // 3. Salviamo la modifica
                        db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        // Qui potresti loggare l'errore se necessario
                    }
                }
            }
        }

        //public static void AggiornaQtaCassa(int idProd, int)
        public List<ProdottiOrdine> GetProdottiOrdineById(int idOrdine)
        {
            try
            {
                var dettagliProdotto = db.ProdottiOrdine
                    .Where(p => p.id_ordine == idOrdine)
                    .ToList();

                return dettagliProdotto;
            }
            catch (Exception ex)
            {
                // Log dell'eccezione
                Console.WriteLine($"Errore durante il recupero dei dettagli del prodotto: {ex.Message}");
                return new List<ProdottiOrdine>();
            }
        }
        public static bool CheckQuantityAvailability(int id_prodotto, int quantitaRichiesta)
        {
            // Usa il nome corretto del tuo DbContext
            using (var db = new AnyeLabelEntities())
            {
                // Cerchiamo il prodotto nel database
                var prodottoDb = db.Prodotti.FirstOrDefault(p => p.id_prodotto == id_prodotto);

                // Se il prodotto esiste E la quantità in magazzino è sufficiente, restituiamo true
                if (prodottoDb != null && prodottoDb.Quantita >= quantitaRichiesta)
                {
                    return true;
                }

                // Altrimenti (non esiste o non ce ne sono abbastanza), restituiamo false
                return false;
            }
        }

        public static IEnumerable<object> GetOrdiniWeb()
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            return entities.DettagliOrdine.Where(x => x.id_tipoVendita != 1).OrderByDescending(x => x.id_ordine).ToList();
        }
        public static IEnumerable<object> GetOrdiniOnline()
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            return entities.DettagliOrdine.Where(x => x.id_tipoVendita != 1 && x.id_tipoVendita != 2).OrderByDescending(x => x.id_ordine).ToList();
        }

       
    }
}
