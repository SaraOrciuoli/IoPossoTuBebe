using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UtentiRepository
    {
        internal AnyeLabelEntities db = new AnyeLabelEntities();
        public Utenti login(string email, string passwordInChiaro)
        {

            var utente = db.Utenti.FirstOrDefault(u => u.Mail == email);

            if (utente != null)
            {
                bool passwordCorretta = BCrypt.Net.BCrypt.Verify(passwordInChiaro, utente.Password);

                if (passwordCorretta)
                {
                    return utente;
                }
            }

            return null;
        }

        public void UpdateUtente(Utenti u, bool cifraPassword = false)
        {
            try
            {
                if (cifraPassword && !string.IsNullOrEmpty(u.Password))
                {
                    u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);
                }

                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
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

        public List<Comuni> GetListaComuniIsole()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Comuni.Where(x => x.Isola == true).ToList();
        }
        public static IEnumerable<object> GetUtenti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Lista_Utenti.Where(x=> x.id_Ruolo == 1).ToList();
        }

        public List<Ordine> GetOrdiniUtente(int id)
        {
            return db.Ordine.Where(o => o.id_utente == id).OrderByDescending(x => x.DataOra).ToList();
        }

        public Prodotti GetProdottoById(int id)
        {
            return db.Prodotti.FirstOrDefault(x => x.id_prodotto == id);
        }

        public IEnumerable<Ordine> GetOrdiniByUtente(string id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            int iduser = int.Parse(id); 
            return db.Ordine.Where(m => m.id_utente == iduser).ToList();
        }


        public string GetDescrizioneStato(int? idStato)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var stato = db.Stato_Ordine.FirstOrDefault(s => s.id_stato == idStato);
            return stato != null ? stato.Descrizione : string.Empty;
        }
         public string GetDescrizioneStatoPagamento(int? idStatoPag)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var statoPag = db.Stato_Pagamento.FirstOrDefault(s => s.id_stato_pagamento == idStatoPag);
            return statoPag != null ? statoPag.Descrizione_stato_pagamento : string.Empty;
        }

        public List<ProdottiOrdine> GetProdottiOrdine(int idOrdine)
        {
            return db.ProdottiOrdine.Where(x => x.id_ordine == idOrdine).ToList();
        }
        public bool CheckEmailEsistente(string email)
        {
            return db.Utenti.Where(x => x.Mail == email).Any();
        }

        public static int CountUtenti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Lista_Utenti.Count();
        }

        public bool LoginWs(string username, string password)
        {
            Utenti u = db.Utenti.Where(z => z.Mail == username && z.Password == password).FirstOrDefault();
            return u != null;
        }

        public Utenti getUtenteRecupera(string email)
        {
            int count = db.Utenti.Where(x => x.Mail == email).Count();
            if (count > 1 || count == 0)
            {
                return null;
            }
            else
            {
                return db.Utenti.Where(x => x.Mail == email).FirstOrDefault();
            }

        }

        public Utenti GetUtenteById(int idUser)
        {
            return db.Utenti.Where(x => x.id_utente == idUser).FirstOrDefault();
        }

        public void SalvaUtente(Utenti utente)
        {
            db.Utenti.Add(utente);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string messaggio = ex.Message;
            }
        }

        public void SalvaCliente(Utenti utente)
        {
            var id = utente.id_utente;
            db.Utenti.Add(utente);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string messaggio = ex.Message;
            }
        }
        public static void deleteUtenti(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            Utenti u = db.Utenti.Where(x => x.id_utente == key).FirstOrDefault();
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

        public List<Utenti> GetClienti()
        {
            return db.Utenti.Where(u => u.id_Ruolo == 2).ToList();
        }
        public List<Utenti> GetClientiNomeCompleto()
        {
            List<Utenti> result = new List<Utenti>();
            foreach(Utenti u in db.Utenti.Where(u => u.id_Ruolo == 4).ToList())
            {
                if(u.NomeCompleto == null) u.NomeCompleto = u.Nome + " " + u.Cognome;
                result.Add(u);
            }
            return result;
        }

        public Ruoli GetRuoloCliente()
        {
            return db.Ruoli.Where(x => x.Descrizione == "Cliente").FirstOrDefault();
        }

        public List<Utenti> searchClient(string name, string ragSoc, string pIva)
        { if(name.Trim() == "")
            {
                return new List<Utenti>();
            }
            return db.Utenti.Where(u => u.Nome.Contains(name) || u.NomeCompleto.Contains(ragSoc) || u.p_iva.Contains(pIva) && u.id_Ruolo == 4).ToList();
        }

   /*     public static List<Lista_Utenti> getClienti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Lista_Utenti.Where()
        }*/
    }
}
