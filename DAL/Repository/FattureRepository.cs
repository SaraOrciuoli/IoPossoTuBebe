using DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class FattureRepository
    {
        public static int CountFatture()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.DettaglioFatture.Count();
        }

        public IEnumerable<object> GetFatture()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.DettaglioFatture.ToList();
        }

        public void updateFatture(int key, string values)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            var cat = webEntities.Fatture.First(o => o.id_fatture == key); // Finding the item to be updated by key
            int id_fornitore = 0;
            dynamic data = JObject.Parse(values);
            if ( data.id_fornitore != null && int.TryParse(data.id_fornitore.ToString(), out id_fornitore))
            {
                Fornitori fornitore = this.getFornitoreById(id_fornitore);
                string ragioneSociale = fornitore.Descrizione_fornitore;
                string p_iva = fornitore.Partita_iva;
                data.PartitaIva = p_iva;
                data.RagioneSociale = ragioneSociale;
            }
            string new_values = data?.ToString();

            JsonConvert.PopulateObject(new_values, cat);
            webEntities.Entry(cat).State = System.Data.Entity.EntityState.Modified;




            webEntities.SaveChanges();
        }

        public void AddFatture(Fatture m)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Fatture.Add(m);
            webEntities.SaveChanges();
        }

        /// funzioni di supporto 
        /// 
        public string getTipoFattura(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            Tipo_Fatture tipo_fattura = db.Tipo_Fatture.Where(x => x.id_TipoFatture == key).FirstOrDefault();
            string result = "";
            if (tipo_fattura != null) result = tipo_fattura.Descrizione;
            return result;
        }

        public string getCategoriaFattura(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            CategorieFattura cat_fattura = db.CategorieFattura.Where(x => x.id == key).FirstOrDefault();
            string result = "";
            if (cat_fattura != null) result = cat_fattura.Descrizione;
            return result;
        }

        public Fornitori getFornitoreById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Fornitori.Where(x => x.id_fornitore == id).FirstOrDefault();
        }

    }
}
