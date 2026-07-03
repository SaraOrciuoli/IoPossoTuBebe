using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Repository
{
    public class CodiciScontoRepository
    {

        public List<CodiciSconto> getCodiciScontoByTipo(int id_tipoSconto)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.CodiciSconto.Where(x => x.id_tipoSconto == id_tipoSconto).ToList();
        }
        public CodiciSconto getCodiciScontoById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.CodiciSconto.Where(x => x.id_codice_sconto == id).FirstOrDefault();
        }
        public ScontiSpeciali GetScontiSpecialiByID(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.ScontiSpeciali.Where(x => x.id_scontoSpeciale == id).FirstOrDefault();
        }
        public List<ScontiSpeciali> GetScontiSpeciali()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.ScontiSpeciali.ToList();
        }

        public List<int> getScontiSpecialiCatID(int id) {

            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.ScontiSpec_Cat.Where(x => x.id_scontoSpeciale == id).Select(x => x.id_categorie).ToList();
        }

        public void UpdateCodiciSconto(CodiciSconto cs)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cs).State = System.Data.Entity.EntityState.Modified;
                webEntities.SaveChanges();
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
        public void UpdateScontoSpeciale(ScontiSpeciali cs)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cs).State = System.Data.Entity.EntityState.Modified;
                webEntities.SaveChanges();
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

        public void AddCodiciSconto(CodiciSconto cs)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.CodiciSconto.Add(cs);
                webEntities.SaveChanges();
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
         public int AddScontiSpeciali(ScontiSpeciali cs)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.ScontiSpeciali.Add(cs);
                webEntities.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                string exception2 = dbEx.Message;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
            return cs.id_scontoSpeciale;
        }

        public void saveScontiSpec_Cat(ScontiSpec_Cat sc)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.ScontiSpec_Cat.Add(sc);
            webEntities.SaveChanges();

        }
        public void deleteScontiSpec_Cat(int id_scontoSpeciale)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            foreach (ScontiSpec_Cat ds in webEntities.ScontiSpec_Cat.Where(x => x.id_scontoSpeciale == id_scontoSpeciale).ToList())
            {
                webEntities.Entry(ds).State = System.Data.Entity.EntityState.Deleted;
                webEntities.SaveChanges();
            }
        }
        public void DeleteCodiciSconto(CodiciSconto cs)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cs).State = System.Data.Entity.EntityState.Deleted;
                webEntities.SaveChanges();
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
         public void DeleteScontiSpeciali(ScontiSpeciali cs)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cs).State = System.Data.Entity.EntityState.Deleted;
                webEntities.SaveChanges();
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

        public CodiciSconto getCodiciScontoByCodice(string codice)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.CodiciSconto.Where(x => x.codice_sconto == codice).FirstOrDefault();
        }
    }
}
