using DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class EventiRepository
    {
        public static int CountEventi()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Eventi.Count();
        }

        public IEnumerable<object> GetEventi()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Eventi.ToList();
        }

        public static Eventi GetEventiById(int id)
        {
            AnyeLabelEntities barEntities = new AnyeLabelEntities();
            return barEntities.Eventi.Where(x => x.id_evento == id).FirstOrDefault();
        }

        public void UpdateEventi(Eventi e)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(e).State = System.Data.Entity.EntityState.Modified;
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

        public void AddEvento(Eventi e)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(e).State = System.Data.Entity.EntityState.Added;
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

        public static void DeleteEvento(Eventi e)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(e).State = System.Data.Entity.EntityState.Deleted;
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
    }
}
