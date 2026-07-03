using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ReportRepository
    {
        internal static AnyeLabelEntities db = new AnyeLabelEntities();
        public static double GetValoreCartaByDay(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            
            if(ReportRepository.db.Ordine.Where(x => x.Totale > 0 && x.id_stato == 4 && x.Tipo_Pagamento != "Contanti" && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => x.Totale).HasValue)
            {
                return ReportRepository.db.Ordine.Where(x => x.Totale > 0 && x.id_stato == 4 && x.Tipo_Pagamento != "Contanti" && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => x.Totale).Value;
            } else 
            {
                return 0.00;
            }
        }

        public static double GetValoreContantiByDay(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            if (ReportRepository.db.Ordine.Where(x => x.Totale > 0 && x.id_stato == 4 && x.Tipo_Pagamento == "Contanti" && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => x.Totale).HasValue)
            {
                return ReportRepository.db.Ordine.Where(x => x.Totale > 0 && x.id_stato == 4 && x.Tipo_Pagamento == "Contanti" && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => x.Totale).Value;
            }
            else
            {
                return 0.00;
            }
        }

        public static double GetValoreProdottiByDay(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            if (ReportRepository.db.DettaglioReport.Where(x =>  x.id_stato == 4 && x.is_servizio == false && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => (x.Totale - x.sconto)).HasValue)
            {
                return ReportRepository.db.DettaglioReport.Where(x =>  x.id_stato == 4 && x.is_servizio == false && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => (x.Totale - x.sconto)).Value;
            }
            else
            {
                return 0.00;
            }
        }
        
        public static double GetValoreServiziByDay(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            if(ReportRepository.db.DettaglioReport.Where(x =>  x.id_stato == 4 && x.is_servizio == true && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => (x.Totale - x.sconto)).HasValue)
            {
                return ReportRepository.db.DettaglioReport.Where(x =>  x.id_stato == 4 && x.is_servizio == true && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).Sum(x => (x.Totale - x.sconto)).Value;
            }
            else
            {
                return 0.00;
            }
        }

        public static List<DettaglioReportProdotti> GetProdottiPiuVenduti(DateTime dataSceltaDa, DateTime dataSceltaA)
        {
            return ReportRepository.db.DettaglioReportProdotti.Where(x => x.id_stato == 4 && x.is_servizio == false && x.DataOra.Year >= dataSceltaDa.Year && x.DataOra.Month >= dataSceltaDa.Month && x.DataOra.Day >= dataSceltaDa.Day && x.DataOra.Year <= dataSceltaA.Year && x.DataOra.Month <= dataSceltaA.Month && x.DataOra.Day <= dataSceltaA.Day).ToList();
        }

    }
}