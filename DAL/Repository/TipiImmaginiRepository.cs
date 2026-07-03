using DAL.Model;
using DAL.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class TipiImmaginiRepository
    {
        public List<Tipi_Immagini> getTipiImmagini()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Tipi_Immagini.ToList();
        }

        public void deleteTipiImmagini(Tipi_Immagini tipi)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Entry(tipi).State = System.Data.Entity.EntityState.Deleted;
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
        public Tipi_Immagini getTipiImmaginiById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Tipi_Immagini.Where(tp => tp.id_tipo_immagine == id).FirstOrDefault();
        }

        public void AddTipiImmagini(Tipi_Immagini tipi)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Entry(tipi).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            } catch(Exception e)
            {

            }
        }

        public void UpdateTipiImmagini(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            Tipi_Immagini tp = db.Tipi_Immagini.Where(t => t.id_tipo_immagine == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, tp);
            try
            {
                db.Entry(tp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
        public List<Tipi_Immagini> getTipiImmaginiProdotti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Tipi_Immagini.Where(x => x.Prodotto == true).ToList();
        }
    }
}
