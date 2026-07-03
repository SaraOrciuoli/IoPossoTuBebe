using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ScontoSpecialeRepository
    {
        public List<Prodotti> CheckOfferta(DateTime oggi) {

            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Prodotti> result = db.Prodotti.Where(x => x.MostraWeb == true).ToList();

            ScontiSpeciali special = db.ScontiSpeciali.Where(x => x.Data_Inizio.Value.CompareTo(oggi) <= 0 && x.Data_Fine.Value.CompareTo(oggi) >= 0 && x.Attivo == true).FirstOrDefault();

            foreach(Prodotti prod in result.Where(x => x.in_offerta == true).ToList())
            {
                if(prod.fine_offerta < DateTime.Now)
                {
                    prod.in_offerta = false;

                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                
            }

            if (special != null && db.ScontiSpec_Cat.Where(x => x.id_scontoSpeciale == special.id_scontoSpeciale).Select(x => x.id_categorie).ToList().Count > 0)
            {
                List<int> Cat_specialId = db.ScontiSpec_Cat.Where(x => x.id_scontoSpeciale == special.id_scontoSpeciale).Select(x => x.id_categorie).ToList();

                foreach(Prodotti prod in result.Where(x => Cat_specialId.Contains(x.id_categoria) && x.MostraWeb == true).ToList())
                {
                    if(prod.is_special_off == null || prod.is_special_off == false)
                    {
                        prod.sconto_3 = prod.sconto;
                        prod.scontoSpeciale = prod.sconto;

                    }
                    prod.is_special_off = true;
                    prod.in_offerta = true;
                    prod.sconto = special.Valore * 100;
                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
            }
            else
            {
                foreach (Prodotti prod in result.Where(x => x.is_special_off && x.MostraWeb == true).ToList())
                {
                    prod.sconto = prod.sconto_3;
                    prod.scontoSpeciale = null;
                    prod.is_special_off = false;
                    if (prod.fine_offerta != null && prod.fine_offerta.Value.CompareTo(DateTime.Now) >= 0)
                    {
                        prod.in_offerta = true;
                    } else
                    {
                        prod.in_offerta = false;
                    }
                    
                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                   
                }
            }

            return result;


        }

        public ScontiSpeciali GetScontoSpecialeAttivo()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            
                return db.ScontiSpeciali
                         .Where(x => x.Data_Inizio.Value.CompareTo(DateTime.Now) <= 0
                                     && x.Data_Fine.Value.CompareTo(DateTime.Now) >= 0
                                     && x.Attivo == true)
                         .FirstOrDefault();
            
        }
    }
}
