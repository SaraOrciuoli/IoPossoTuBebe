using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AppuntamentiRepository
    {
        public IEnumerable<object> getAppuntamenti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GestioneAppuntamenti.ToList();
        }

        public void updateAppuntamenti(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var appuntamento = db.GestioneAppuntamenti.First(o => o.id_appuntamento == key);
            JsonConvert.PopulateObject(values, appuntamento);
            db.Entry(appuntamento).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void addAppuntamento(GestioneAppuntamenti m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.GestioneAppuntamenti.Add(m);
            db.SaveChanges();
        }

        public List<GestioneAppuntamenti> getAppuntamentiPerDomani()
        {
            using (var db = new AnyeLabelEntities())
            {
                DateTime domani = DateTime.Today.AddDays(1);

                var lista = db.GestioneAppuntamenti
                    .Where(a => System.Data.Entity.DbFunctions.TruncateTime(a.data_appuntamento) == domani)
                    .ToList();

                return lista;
            }
        }

    }
}
