using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class StoricoFatturatoRepository
    {
        public StoricoFatturato GetStoricoByMeseAnno(int month, int year)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.StoricoFatturato.Where(x => x.mese == month && x.anno == year).FirstOrDefault();
        }

        public void AddStorico(StoricoFatturato m)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.StoricoFatturato.Add(m);
            webEntities.SaveChanges();
        }

        public void UpdateStorico(StoricoFatturato m)
        {
            AnyeLabelEntities webEntities = new AnyeLabelEntities();
            webEntities.Entry(m).State = System.Data.Entity.EntityState.Modified;
            webEntities.SaveChanges();
        }

        //funzioni per calcolo andamento
        public double getTotale(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            int id_fatture_ingresso = db.Tipo_Fatture.Where(x => x.Descrizione == "ingresso" || x.Descrizione == "entrata").FirstOrDefault().id_TipoFatture;

            List<Ordine> ordini = db.Ordine.Where(x => x.id_stato == 4 && x.DataOra.CompareTo(dataSceltaDa) >= 0 && x.DataOra.CompareTo(dataSceltaA) <= 0).ToList();
            double sum_ordini = 0.00;

            foreach (Ordine ordine in ordini)
            {
                sum_ordini += (double)(ordine.Totale != null ? ordine.Totale : 0.00);
            }

            List<Fatture> fatture = db.Fatture.Where(x => x.Id_tipo == id_fatture_ingresso && x.Data.CompareTo(dataSceltaDa) >= 0 && x.Data.CompareTo(dataSceltaA) <= 0).ToList();

            double sum_fatture = 0.00;
            foreach (Fatture fattura in fatture)
            {
                sum_fatture += (double)(fattura.Valore);
            }

            return Math.Round((sum_fatture + sum_ordini), 3);
        }

        public double getSpese(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();

            int id_fatture_uscita = db.Tipo_Fatture.Where(x => x.Descrizione == "uscita").FirstOrDefault().id_TipoFatture;
            List<Fatture> fatture = db.Fatture.Where(x => x.Id_tipo == id_fatture_uscita && x.Data.CompareTo(dataSceltaDa) >= 0 && x.Data.CompareTo(dataSceltaA) <= 0).ToList();

            double sum_fatture = 0.00;
            foreach (Fatture fattura in fatture)
            {
                sum_fatture += (double)(fattura.Valore);
            }

            return Math.Round(sum_fatture, 3);
        }

        public double getContoEconomico(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            return getTotale(dataSceltaDa, dataSceltaA) - getSpese(dataSceltaDa, dataSceltaA);
        }

        //prendere lo storico per i grafici
        public Dictionary<int, double> getStoricoPerYear(int year = 0)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            DateTime now = DateTime.Now;
            if (year == 0) year = now.Year;
            Dictionary<int, double> result = new Dictionary<int, double>() {
                {1, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 1).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 1).FirstOrDefault().contoEconomico : 0.00) },
                {2, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 2).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 2).FirstOrDefault().contoEconomico : 0.00)},
                {3, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 3).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 3).FirstOrDefault().contoEconomico : 0.00)},
                {4, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 4).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 4).FirstOrDefault().contoEconomico : 0.00)},
                {5, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 5).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 5).FirstOrDefault().contoEconomico : 0.00)},
                {6, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 6).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 6).FirstOrDefault().contoEconomico : 0.00)},
                {7, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 7).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 7).FirstOrDefault().contoEconomico : 0.00)},
                {8, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 8).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 8).FirstOrDefault().contoEconomico : 0.00)},
                {9, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 9).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 9).FirstOrDefault().contoEconomico : 0.00)},
                {10, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 10).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 10).FirstOrDefault().contoEconomico : 0.00)},
                {11, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 11).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 11).FirstOrDefault().contoEconomico : 0.00)},
                {12, (double)(db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 12).FirstOrDefault() != null ? db.StoricoFatturato.Where(x=>x.anno == year && x.mese == 12).FirstOrDefault().contoEconomico : 0.00)},
            };
            return result;
        }

        //prendere gli anni di storico registrati
        public List<int> getAnniStorico()
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            List<int> anni = db.StoricoFatturato.Select(x => x.anno).Distinct().ToList();
            return anni;
        }

    }
}
