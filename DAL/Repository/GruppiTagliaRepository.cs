using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace DAL.Repository
{
    public class GruppiTagliaRepository
    {
        public string getGruppiTagliaTaglieDescByIDGruppo(int id_gruppo_taglia)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            string result = " ";
            foreach(GruppoTaglia_Taglie el in db.GruppoTaglia_Taglie.Where(x => x.id_gruppoTaglia == id_gruppo_taglia).ToList())
            {
                result += db.Taglie.Where(t => t.id_taglia == el.id_taglia).FirstOrDefault().Descrizione_taglia + ", ";
            }
            return result;
        }

        public List<DettaglioGruppoTaglie> getGruppiTaglia()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<DettaglioGruppoTaglie> result = new List<DettaglioGruppoTaglie>();
            foreach (DettaglioGruppoTaglie el in db.DettaglioGruppoTaglie.ToList())
            {
                el.taglie_ids_int = new List<int>();
                if (!string.IsNullOrEmpty(el.taglie_ids))
                {
                    foreach (string id in el.taglie_ids.Split(','))
                    {
                        if (!String.IsNullOrEmpty(id)) el.taglie_ids_int.Add(Convert.ToInt32(id));
                    }
                    result.Add(el);
                }

            }
            return result;
        }

        public List<DettaglioGruppoColori> getGruppiColori()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<DettaglioGruppoColori> result = new List<DettaglioGruppoColori>();
            foreach (DettaglioGruppoColori el in db.DettaglioGruppoColori.ToList())
            {
                el.colori_ids_int = new List<int>();
                if (!string.IsNullOrEmpty(el.colori_ids))
                {
                    foreach (string id in el.colori_ids.Split(','))
                    {
                        if (!String.IsNullOrEmpty(id)) el.colori_ids_int.Add(Convert.ToInt32(id));
                    }
                    result.Add(el);
                }

            }
            return result;
        }




        public List<Taglie> getTaglieByIdGruppo(int id_gruppo_taglia) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Taglie> result = new List<Taglie>();
            foreach(GruppoTaglia_Taglie el in db.GruppoTaglia_Taglie.Where(x => x.id_gruppoTaglia == id_gruppo_taglia).ToList())
            {
                result.Add(db.Taglie.Where(t => t.id_taglia == el.id_taglia).FirstOrDefault());
            }
            return result;
        }

        
        public List<Colori> getColoriByIdGruppo(int id_gruppo_colori)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Colori> result = new List<Colori>();
            foreach (GruppoColori_Colori el in db.GruppoColori_Colori.Where(x => x.id_gruppoColori == id_gruppo_colori).ToList())
            {
                result.Add(db.Colori.Where(t => t.id_Colori == el.id_colore).FirstOrDefault());
            }
            return result;
        }
        public List<Spessori> getSpessoriByIdGruppo(int id_gruppo_spessori)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<Spessori> result = new List<Spessori>();
            foreach (GruppoSpessori_Spessore el in db.GruppoSpessori_Spessore.Where(x => x.id_gruppoSpessori == id_gruppo_spessori).ToList())
            {
                result.Add(db.Spessori.Where(t => t.id_spessore == el.id_spessore).FirstOrDefault());
            }
            return result;
        }

        public int AddGruppoTaglia(GruppoTaglia gt)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            int id_gruppo_taglia = 0;
            try
            {
                db.GruppoTaglia.Add(gt);
                db.SaveChanges();
                id_gruppo_taglia = gt.id_gruppoTaglia;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return id_gruppo_taglia;
        }

        public int addGruppoColore(GruppoColori gt)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            int id_gruppo_colore = 0;
            try
            {
                db.GruppoColori.Add(gt);
                db.SaveChanges();
                id_gruppo_colore = gt.id_gruppoColore;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return id_gruppo_colore;
        }
        
        public void AddGruppoTagliaTaglie(GruppoTaglia_Taglie gtt) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.GruppoTaglia_Taglie.Add(gtt);
                db.SaveChanges();
            } catch (Exception ex) { string msg = ex.Message; }
        }

        public void addGruppoColori_Colore(GruppoColori_Colori gtt)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.GruppoColori_Colori.Add(gtt);
                db.SaveChanges();
            }
            catch (Exception ex) { string msg = ex.Message; }
        }

        public List<int> getTaglieIdsByGruppoID(int id_gruppo_taglia)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<int> ids = new List<int>();
            foreach(GruppoTaglia_Taglie el in db.GruppoTaglia_Taglie.Where(x => x.id_gruppoTaglia == id_gruppo_taglia).ToList())
            {
                ids.Add(el.id_taglia);
            }
            return ids;
        }
        public GruppoTaglia getGruppoTagliaById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GruppoTaglia.Where(x => x.id_gruppoTaglia == id).FirstOrDefault();
        }

        public GruppoColori getGruppoColoriById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GruppoColori.Where(x => x.id_gruppoColore == id).FirstOrDefault();
        }
        public void deleteGruppoTagliaTaglieByIdGruppo(int id) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            foreach(GruppoTaglia_Taglie el in db.GruppoTaglia_Taglie.Where(x => x.id_gruppoTaglia == id).ToList())
            {
                db.Entry(el).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void deleteGruppoColori_ColoreByIdGruppo(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            foreach (GruppoColori_Colori el in db.GruppoColori_Colori.Where(x => x.id_gruppoColori == id).ToList())
            {
                db.Entry(el).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void deleteGruppoTagliaTaglieByIdTaglia(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            foreach (GruppoTaglia_Taglie el in db.GruppoTaglia_Taglie.Where(x => x.id_taglia == id).ToList())
            {
                db.Entry(el).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void saveGruppoTagliaTaglie(int id_gruppo, int id_taglia)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            GruppoTaglia_Taglie gtt = new GruppoTaglia_Taglie();
            gtt.id_taglia = id_taglia;
            gtt.id_gruppoTaglia = id_gruppo;
            db.GruppoTaglia_Taglie.Add(gtt);
            db.SaveChanges();
        }
        public void saveGruppoColori_Colore(int id_gruppo, int id_taglia)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            GruppoColori_Colori gtt = new GruppoColori_Colori();
            gtt.id_colore = id_taglia;
            gtt.id_gruppoColori = id_gruppo;
            db.GruppoColori_Colori.Add(gtt);
            db.SaveChanges();
        }
        public void deleteGruppoTaglia(int id_gruppo) 
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            this.deleteGruppoTagliaTaglieByIdGruppo(id_gruppo);
            db.Entry(db.GruppoTaglia.Where(x=> x.id_gruppoTaglia == id_gruppo).FirstOrDefault()).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
        }

        public void deleteGruppoColore(int id_gruppo)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            this.deleteGruppoColori_ColoreByIdGruppo(id_gruppo);
            db.Entry(db.GruppoColori.Where(x => x.id_gruppoColore == id_gruppo).FirstOrDefault()).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
        }


        public List<GruppoTaglia> getGruppoTagliaPerProdotti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GruppoTaglia.ToList();
        }

        public List<GruppoColori> getGruppoColoriPerProdotti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GruppoColori.ToList();
        }
        public List<GruppoSpessori> getGruppoSpessoriPerProdotti()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GruppoSpessori.ToList();
        }

        //Spessori

        public List<DettaglioGruppoSpessori> getGruppiSpessori()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<DettaglioGruppoSpessori> result = new List<DettaglioGruppoSpessori>();
            foreach (DettaglioGruppoSpessori el in db.DettaglioGruppoSpessori.ToList())
            {
                el.spessori_ids_int = new List<int>();
                if (!string.IsNullOrEmpty(el.spessori_ids))
                {
                    foreach (string id in el.spessori_ids.Split(','))
                    {
                        if (!String.IsNullOrEmpty(id)) el.spessori_ids_int.Add(Convert.ToInt32(id));
                    }
                    result.Add(el);
                }

            }
            return result;
        }


        public int adddGruppoSpessore(GruppoSpessori gt)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            int id_gruppoSpessori = 0;
            try
            {
                db.GruppoSpessori.Add(gt);
                db.SaveChanges();
                id_gruppoSpessori = gt.id_gruppoSpessori;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return id_gruppoSpessori;
        }

        public void addGruppoSpessori_Spessore(GruppoSpessori_Spessore gtt)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.GruppoSpessori_Spessore.Add(gtt);
                db.SaveChanges();
            }
            catch (Exception ex) { string msg = ex.Message; }
        }
        public void deleteGruppoSpessori_SpessoreByIdGruppo(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            foreach (GruppoSpessori_Spessore el in db.GruppoSpessori_Spessore.Where(x => x.id_gruppoSpessori == id).ToList())
            {
                db.Entry(el).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void deleteGruppiSpessore(int id_gruppo)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            this.deleteGruppoSpessori_SpessoreByIdGruppo(id_gruppo);
            db.Entry(db.GruppoSpessori.Where(x => x.id_gruppoSpessori == id_gruppo).FirstOrDefault()).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
        }

        public GruppoSpessori getGruppoSpessoriById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.GruppoSpessori.Where(x => x.id_gruppoSpessori == id).FirstOrDefault();
        }
        public void saveGruppoSpessori_Spessore(int id_gruppo, int id_spessore)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            GruppoSpessori_Spessore gtt = new GruppoSpessori_Spessore();
            gtt.id_spessore = id_spessore;
            gtt.id_gruppoSpessori = id_gruppo;
            db.GruppoSpessori_Spessore.Add(gtt);
            db.SaveChanges();
        }
    }
}
