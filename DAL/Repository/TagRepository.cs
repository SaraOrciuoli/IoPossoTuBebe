using DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class TagRepository
    {
        internal AnyeLabelEntities db = new AnyeLabelEntities();

        public List<Tag> GetTags()
        {
            return this.db.Tag.ToList();
        }
        public Tag GetTagById(int id_tag)
        {
            return this.db.Tag.Where(x => x.id_tag == id_tag).FirstOrDefault();
        }

        public void SaveTag(Tag tag)
        {
            try
            {
                db.Entry(tag).State = System.Data.Entity.EntityState.Added;
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

        public void UpdateTag(int key, string values)
        {
            try
            {
                Tag u = db.Tag.First(o => o.id_tag == key); 
                JsonConvert.PopulateObject(values, u);
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

        public void deleteTag(Tag tag)
        {
            try
            {
                db.Entry(tag).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        public void deleteTagProdotti(int id_prodotto)
        {
            foreach(ProdottiTag el in db.ProdottiTag.Where(x=> x.id_prodotti == id_prodotto).ToList())
            {
                try
                {
                    db.Entry(el).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
            }
        }
        public void saveTagProdotti(ProdottiTag prodTag)
        {
            db.Entry(prodTag).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
        }

        public ProdottiTag getProdottiTagByParam(int id_prodotto, int id_tag)
        {
            return db.ProdottiTag.Where(x => x.id_prodotti == id_prodotto && x.id_tag == id_tag).FirstOrDefault();
        }

        public List<int> getTagsIdByProduct(int id_prodotto)
        {
            List<int> tags_id = new List<int>();
            foreach (ProdottiTag el in db.ProdottiTag.Where(x => x.id_prodotti == id_prodotto).ToList())
            {
                if (!tags_id.Contains(el.id_tag)) tags_id.Add(el.id_tag);
            }
            return tags_id;
        }
    }
}