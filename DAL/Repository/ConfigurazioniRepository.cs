//using Admin.Models.CustomModels;
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
    public class ConfigurazioniRepository
    {

        public void addColore(Colori c)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Colori.Add(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void addSpessore(Spessori c)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Spessori.Add(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        
        public void addTaglia(Taglie t)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Taglie.Add(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public void deleteColore(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var colore = db.Colori.First(m => m.id_Colori == key);

            try
            {
                db.Entry(colore).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void deleteSpessore(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var colore = db.Spessori.First(m => m.id_spessore == key);

            try
            {
                db.Entry(colore).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        

        public void deleteTaglia(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            GruppiTagliaRepository repo = new GruppiTagliaRepository();
            var taglia = db.Taglie.First(t => t.id_taglia == key);
            repo.deleteGruppoTagliaTaglieByIdTaglia(key);
            try
            {
                db.Entry(taglia).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public IEnumerable<object> getColori()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Colori.ToList();
        }
        
        public IEnumerable<object> getSpessori()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Spessori.ToList();
        }
        public IEnumerable<object> getTaglie()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Taglie.ToList();
        }

        public List<TestiHome> GetListaTestiHome()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.TestiHome.ToList();
        }
        public TestiHome GetTestoHomeByID(int ID)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.TestiHome.Where(x => x.id_testo == ID).FirstOrDefault();
        }
        
        public void updateColore(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var colore = db.Colori.First(c => c.id_Colori == key);
            JsonConvert.PopulateObject(values, colore);
            try
            {
                db.Entry(colore).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public void updateSpessore(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var colore = db.Spessori.First(c => c.id_spessore == key);
            JsonConvert.PopulateObject(values, colore);
            try
            {
                db.Entry(colore).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        

        public void updateTaglia(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var taglia = db.Taglie.First(t => t.id_taglia == key);
            JsonConvert.PopulateObject(values, taglia);
            try
            {
                db.Entry(taglia).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }


        public List<Ruoli> GetRuoli()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Ruoli.ToList();
        }

        public Slider GetImgSliderById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Slider.Where(x => x.id_immagine == id).FirstOrDefault();
        }

        public List<string> GetLinkImmagini()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Slider.Select(x => x.Link).ToList();
        }

        public List<Slider> GetImmaginiSlider()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Slider.Where(x => x.eliminato != true && x.posizione == 1 && x.slider_num == 1).ToList();
        }

        public static int CountMarche()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Marche.Count();
        }

        public IEnumerable<object> GetMarche()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Marchi.ToList();
        }

        public IEnumerable<Toolbar> getTestiToolbar()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Toolbar.ToList();
        }
        public void updateTestoToolbar(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var marca = db.Toolbar.First(o => o.id_testo_toolbar == key);
            JsonConvert.PopulateObject(values, marca);
            db.Entry(marca).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void addTestoToolbar(Toolbar m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.Toolbar.Add(m);
            db.SaveChanges();
        }
        public void deleteTestoToolbar(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var marca = db.Toolbar.First(t => t.id_testo_toolbar == key);

            try
            {
                db.Entry(marca).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        public Marchi GetMarchioById(int idMarca)
        {
            using (var context = new AnyeLabelEntities()) 
            {
                return context.Marchi.FirstOrDefault(m => m.id_marchio == idMarca);
            }
        }
        public void updateMarca(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var marca = db.Marchi.First(o => o.id_marchio == key); 
            JsonConvert.PopulateObject(values, marca);
            db.Entry(marca).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void addMarca(Marchi m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.Marchi.Add(m);
            db.SaveChanges();
        }

        public void deleteMarca(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var marca = db.Marchi.First(t => t.id_marchio == key);

            try
            {
                db.Entry(marca).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public List<Marchi> GetTuttiMarchi()
        {
            using (var context = new AnyeLabelEntities()) 
            {
                return context.Marchi.OrderBy(m => m.nome_marchio).ToList();
            }
        }
        public static int CountTipo_Fatture()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Tipo_Fatture.Count();
        }

        public IEnumerable<object> GetTipiFatture()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Tipo_Fatture.ToList();
        }

        public void updateTipoFatture(int key, string values)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            var cat = webEntities.Tipo_Fatture.First(o => o.id_TipoFatture == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, cat);
            webEntities.Entry(cat).State = System.Data.Entity.EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void AddTipoFatture(Tipo_Fatture m)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Tipo_Fatture.Add(m);
            webEntities.SaveChanges();
        }



        public static int CountStampanti()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Stampanti.Where(x => x.eliminato != true).Count();
        }

        public static int CountStampantiCategorie(string id)
        {
            int _id = Convert.ToInt32(id);
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.AssociazioneStampantiCategorie.Where(x => x.stampante == _id && x.eliminato != true).Count();
        }

        public static int CountTipiStampanti()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.TipoStampante.Where(x => x.eliminato != true).Count();
        }

        public Stampanti GetStampantiById(string id)
        {
            int _id = Convert.ToInt32(id);
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Stampanti.Where(x => x.id_stampante == _id).FirstOrDefault();
        }

        public DettaglioStampanti GetDettaglioStampantiById(string id)
        {
            int _id = Convert.ToInt32(id);
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.DettaglioStampanti.Where(x => x.id_stampante == _id).FirstOrDefault();
        }

        public TipoStampante GetTipoStampantiById(string id)
        {
            int _id = Convert.ToInt32(id);
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.TipoStampante.Where(x => x.id_tipo_stampante == _id).FirstOrDefault();
        }

        public AssociazioneStampantiCategorie GetAssociazioneById(string id)
        {
            int _id = Convert.ToInt32(id);
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.AssociazioneStampantiCategorie.Where(x => x.id_stampante_categoria == _id).FirstOrDefault();
        }

        public AssociazioneStampantiCategorie GetAssociazioneByIdStampante(int id)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.AssociazioneStampantiCategorie.Where(x => x.stampante == id).FirstOrDefault();
        }

        public List<AssociazioneStampantiCategorie> GetListaAssociazioniByIdStampante(int id)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.AssociazioneStampantiCategorie.Where(x => x.stampante == id).ToList();
        }

        public List<DettaglioStampanti> GetListaStampanti()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.DettaglioStampanti.Where(x => x.eliminato != true).ToList();
        }

        public List<Stampanti> GetStampanti()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Stampanti.Where(x => x.eliminato != true).ToList();
        }

        public void updateStampante(int key, string values)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            var s = webEntities.Stampanti.First(o => o.id_stampante == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, s);
            webEntities.Entry(s).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void addStampante(Stampanti s)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Stampanti.Add(s);
            webEntities.SaveChanges();
        }

        public void deleteStampante(Stampanti s)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Entry(s).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void deleteTipoStampante(TipoStampante t)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Entry(t).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public List<TipoStampante> GetListaTipiStampanti()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.TipoStampante.Where(x => x.eliminato != true).ToList();
        }

        public IEnumerable<object> GetListaCategorieStampanti(int id)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.DettaglioAssociazioneStampantiCategorie.Where(x => x.stampante == id && x.eliminato != true).ToList();
        }

        public void addCategoriaStampante(AssociazioneStampantiCategorie asc)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.AssociazioneStampantiCategorie.Add(asc);
            webEntities.SaveChanges();
        }

        public void updateCategoriaStampante(AssociazioneStampantiCategorie asc)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Entry(asc).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void deleteCategoriaStampante(AssociazioneStampantiCategorie asc)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Entry(asc).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void addTipoStampante(TipoStampante ts)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.TipoStampante.Add(ts);
            webEntities.SaveChanges();
        }

        public void updateTipoStampante(int key, string values)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            var ts = webEntities.TipoStampante.First(o => o.id_tipo_stampante == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, ts);
            webEntities.Entry(ts).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void SaveCod(Codici cod)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cod).State = EntityState.Added;
                webEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        public static void DeleteCod(Codici cod)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cod).State = EntityState.Modified;
                webEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        public List<Codici> GetCode()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Codici.Where(x => x.Utilizzato != true).ToList();
        }

        public static double GetValueByCode(string code)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Codici.Where(x => x.Codice == code && x.Utilizzato != true).Select(x => x.Valore).FirstOrDefault();
        }

        public static Codici GetCodeByCode(string code)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Codici.Where(x => x.Codice == code && x.Utilizzato != true).FirstOrDefault();
        }

        public bool GetChiusura()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura.Day == DateTime.Now.Day && x.dataChiusura.Month == DateTime.Now.Month && x.dataChiusura.Year == DateTime.Now.Year && x.chiudicassa == true).Select(x => x.chiudicassa).FirstOrDefault();
        }

        public bool GetLettura()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura.Day == DateTime.Now.Day && x.dataChiusura.Month == DateTime.Now.Month && x.dataChiusura.Year == DateTime.Now.Year && x.LetturaCassa == true).Select(x => x.LetturaCassa).FirstOrDefault();
        }

        public void UpdateChiusura()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            ConfigurazioneCassa ts = webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura.Day == DateTime.Now.Day && x.dataChiusura.Month == DateTime.Now.Month && x.dataChiusura.Year == DateTime.Now.Year && x.chiudicassa == true).FirstOrDefault();
            ts.chiudicassa = false;
            webEntities.Entry(ts).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public static ConfigurazioneCassa CreateGetChiusura()
        {
            DateTime today = DateTime.Now.Date;
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            ConfigurazioneCassa cc = new ConfigurazioneCassa();
            cc.chiudicassa = false;
            cc.LetturaCassa = false;
            cc.ScontrinoRegalo = false;
            cc.dataChiusura = today;
            webEntities.Entry(cc).State = EntityState.Added;
            webEntities.SaveChanges();
            return webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura == today).FirstOrDefault();
        }

        public static ConfigurazioneCassa CreateGetLetturaCassa()
        {
            DateTime today = DateTime.Now.Date;
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            ConfigurazioneCassa cc = new ConfigurazioneCassa();
            cc.chiudicassa = false;
            cc.LetturaCassa = true;
            cc.ScontrinoRegalo = false;
            cc.dataChiusura = today;
            webEntities.Entry(cc).State = EntityState.Added;
            webEntities.SaveChanges();
            return webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura == today).FirstOrDefault();
        }

        public static ConfigurazioneCassa CreateGetScontrinoRegalo()
        {
            DateTime today = DateTime.Now.Date;
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            ConfigurazioneCassa cc = new ConfigurazioneCassa();
            cc.chiudicassa = false;
            cc.LetturaCassa = false;
            cc.ScontrinoRegalo = true;
            cc.dataChiusura = today;
            webEntities.Entry(cc).State = EntityState.Added;
            webEntities.SaveChanges();
            return webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura == today).FirstOrDefault();
        }

        public static void ChiudiCassa(ConfigurazioneCassa cc)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            try
            {
                webEntities.Entry(cc).State = EntityState.Modified;
                webEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        public static ConfigurazioneCassa GetChiusuraByData()
        {
            DateTime today = DateTime.Now.Date;
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.ConfigurazioneCassa.Where(x => x.dataChiusura == today).FirstOrDefault();
        }

        public static IEnumerable<object> GetTipiMovimentoMagazzino()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.TipoMovimentoMagazzino.ToList();
        }

        public static IEnumerable<object> GetFornitori()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Fornitori.ToList();
        }
        public static void addFornitore(Fornitori m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.Fornitori.Add(m);
            db.SaveChanges();
        }

        public static void updateFornitore(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var u = db.Fornitori.First(o => o.id_fornitore == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, u);
            db.Entry(u).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void deleteFornitore(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var fornitore = db.Fornitori.First(f => f.id_fornitore == key);

            try
            {
                db.Entry(fornitore).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public List<DettaglioModelli> getModelli()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.DettaglioModelli.ToList();
        }

        public void addModello(Modello m)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            try
            {
                db.Modello.Add(m);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public void updateModello(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var mod = db.Modello.First(m => m.id_modello == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, mod);
            db.Entry(mod).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void deleteModello(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var modello = db.Modello.First(m => m.id_modello == key);

            try
            {
                db.Entry(modello).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        
        public void updateStampa(int id_tavolo, int id_gestione)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            GestioneTavoli gt = new GestioneTavoli();
            gt = webEntities.GestioneTavoli.Where(x => x.id_gestione == id_gestione && x.id_tavolo == id_tavolo).FirstOrDefault();
            gt.stampa_fiscale = true;
            webEntities.Entry(gt).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public static void InsertAnnulloScontrino(string codA, string codS, string codG, string DataScontrino)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            AnnulloScontrino ann = new AnnulloScontrino();
            ann.codiceScontrino = codS;
            ann.codiceAzzeramento = codA;
            if (!string.IsNullOrEmpty(codG)) ann.gestionale = codG;
            ann.dataInserimento = DateTime.Now;
            ann.DaAnnullare = true;
            ann.DataScontrino = DateTime.Parse(DataScontrino);
            webEntities.AnnulloScontrino.Add(ann);
            webEntities.SaveChanges();
        }

        public AnnulloScontrino GetAnnullaScontrino()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.AnnulloScontrino.Where(x => x.DaAnnullare == true).FirstOrDefault();
        }

        public void UpdateAnnullo(int? id_annullo)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            AnnulloScontrino gt = new AnnulloScontrino();
            gt = webEntities.AnnulloScontrino.Where(x => x.id_annullo == id_annullo).FirstOrDefault();
            gt.DaAnnullare = false;
            webEntities.Entry(gt).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public static void AnnullaOrdine(int codiceGestionale, int idTavolo)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            GestioneTavoli gt = webEntities.GestioneTavoli.Where(x => x.id_gestione == codiceGestionale && x.id_tavolo == idTavolo).FirstOrDefault();
            gt.Annullato = true;
            webEntities.Entry(gt).State = EntityState.Modified;
            webEntities.SaveChanges();

        }

        public void UpdateQuantitaProdotti(int id_prodotto, int quantita)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            Prodotti p = new Prodotti();
            p = webEntities.Prodotti.Where(x => x.id_prodotto == id_prodotto).FirstOrDefault();
            p.Quantita = p.Quantita + quantita;
            webEntities.Entry(p).State = EntityState.Modified;
            webEntities.SaveChanges();
        }

        public IEnumerable<object> GetCategorieFatture()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.CategorieFattura.ToList();
        }

        public IEnumerable<object> GetFornitoriFatture()
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            return webEntities.Fornitori.ToList();
        }

        public void updateCategorieFatture(int key, string values)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            var cat = webEntities.CategorieFattura.First(o => o.id == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, cat);
            webEntities.Entry(cat).State = System.Data.Entity.EntityState.Modified;
            webEntities.SaveChanges();
        }

        public void AddCategorieFatture(CategorieFattura m)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.CategorieFattura.Add(m);
            webEntities.SaveChanges();
        }

        public Categorie GetCategoriaById(int id)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Categorie.Where(x => x.id_categorie == id).FirstOrDefault();
        }

        public List<Ruoli> getRuoliUtente()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            Ruoli ruolo_cliente = db.Ruoli.Where(x => x.Descrizione == "Cliente").FirstOrDefault();
            return db.Ruoli.Where(x => x.id_ruolo != ruolo_cliente.id_ruolo).ToList();
        }


        public IEnumerable<object> GetTipiSconto()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.TipiSconto.ToList();
        }
         public IEnumerable<object> GetTestiHome()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.TestiHome.ToList();
        }

        public void updateTipoSconto(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var tipo_sconto = db.TipiSconto.First(o => o.id_tipoSconto == key); // Finding the item to be updated by key
            JsonConvert.PopulateObject(values, tipo_sconto);
            db.Entry(tipo_sconto).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void updateTestoHome(int key, string values)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var testo_home = db.TestiHome.First(o => o.id_testo == key); 
            JsonConvert.PopulateObject(values, testo_home);
            db.Entry(testo_home).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void addTipoSconto(TipiSconto ts)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            db.TipiSconto.Add(ts);
            db.SaveChanges();
        }

        public void deleteTipoSconto(int key)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            var ts = db.TipiSconto.First(t => t.id_tipoSconto == key);

            try
            {
                db.Entry(ts).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
    }
}
