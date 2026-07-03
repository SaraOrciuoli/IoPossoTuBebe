using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CollezioniRepository
    {
        public List<Categorie> getCollezioniAdmin()
        
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Categorie.Where(x => x.eliminato != true && x.is_collezione == true).ToList();
        }

        public static int CountCollezioni()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Categorie.Where(x => x.eliminato != true && x.is_collezione == true).Count();
        }
        public Categorie getCategoriaById(string id)
        {
            int idcat = Convert.ToInt32(id);
            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                return db.Categorie.FirstOrDefault(p => p.id_categorie == idcat && p.eliminato == false);
            }
        }

        public List <View_Collezioni_Prodotti> getProdottiPerCollezione(int id_collezione)
        {

            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                return db.View_Collezioni_Prodotti.Where(p => p.id_collezione == id_collezione).ToList();
            }
        }


        public List<int> GetCollezioniIdByProdotto(int id_prodotto)
        {
            using (var db = new AnyeLabelEntities())
            {
                return db.Associazione_Prodotto_Collezione
                         .Where(x => x.id_prodotto == id_prodotto && x.id_collezione != null)
                         .Select(x => x.id_collezione.Value)
                         .ToList();
            }
        }
        public void AggiungiProdottoACollezione(Associazione_Prodotto_Collezione associazione)
        {
            using (var db = new AnyeLabelEntities())
            {
                // Evitiamo di inserire due volte lo stesso prodotto nella stessa collezione
                bool esiste = db.Associazione_Prodotto_Collezione.Any(a => a.id_collezione == associazione.id_collezione && a.id_prodotto == associazione.id_prodotto);

                if (!esiste)
                {
                    db.Associazione_Prodotto_Collezione.Add(associazione);
                    db.SaveChanges(); // <-- Se ti dà l'errore IDENTITY_INSERT qui, ricorda di mettere "Identity" nel file .edmx come detto prima!
                }
            }
        }

        public void RimuoviProdottoDaCollezione(int key)
        {
            using (var db = new AnyeLabelEntities())
            {
                var associazione = db.Associazione_Prodotto_Collezione.Find(key);
                if (associazione != null)
                {
                    db.Associazione_Prodotto_Collezione.Remove(associazione);
                    db.SaveChanges();
                }
            }
        }

        // Recupera la singola collezione verificando che sia attiva
        public Categorie GetCollezioneById(int id)
        {
            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                return db.Categorie.FirstOrDefault(c => c.id_categorie == id && c.is_collezione == true && c.eliminato != true);
            }
        }

        // Recupera tutti i prodotti associati a una specifica collezione e visibili sul web
        public List<Prodotti> GetProdottiByCollezioneWeb(int id_collezione)
        {
            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                var idProdottiNellaCollezione = db.Associazione_Prodotto_Collezione
                                                  .Where(pc => pc.id_collezione == id_collezione)
                                                  .Select(pc => pc.id_prodotto)
                                                  .ToList();

                return db.Prodotti
                         .Where(p => idProdottiNellaCollezione.Contains(p.id_prodotto)
                                  && p.MostraWeb == true
                                  && p.eliminato != true && p.Quantita > 0)
                         .ToList();
            }
        }

        public List<Categorie> GetCollezioniWeb()
        {
            using (AnyeLabelEntities db = new AnyeLabelEntities())
            {
                return db.Categorie
                         .Where(x => x.eliminato != true && x.MostraWeb == true && x.is_collezione == true)
                         .ToList();
            }
        }



    }
}
