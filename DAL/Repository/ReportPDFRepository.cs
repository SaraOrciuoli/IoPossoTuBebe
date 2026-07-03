using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ReportPDFRepository
    {
        public double GetTotalExpensesByCategory(DateTime da, DateTime a, int id_categoria)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Fatture.Where(x => x.id_categoria == id_categoria && x.Id_tipo == 2 && (x.Data >= da && x.Data <= a)).Sum(x => x.Valore);
        }

        public List<object> GetAllInvoiceByCategory(DateTime da, DateTime a, int id_categoria)
        {
            AnyeLabelEntities db = new AnyeLabelEntities();
            return db.Fatture.Where(x => x.id_categoria == id_categoria && x.Id_tipo == 2 && (x.Data >= da && x.Data <= a)).GroupBy(x => x.RagioneSociale).Select(g => new { RagioneSociale = g.Key, Totale = g.Sum(x => x.Valore) }).ToList<object>();
        }
    }
}
