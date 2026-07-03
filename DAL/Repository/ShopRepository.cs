using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ShopRepository
    {
        public static string GetFasciaOrariaById(string fasciaOraria)
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            int idfascia = Convert.ToInt32(fasciaOraria);
            return entities.Fascia_Oraria.Where(x => x.id_fasciaorario == idfascia).Select(x => x.Descrizione_fascia).FirstOrDefault();
        }

        public List<Fascia_Oraria> GetFascieOrarie()
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            return entities.Fascia_Oraria.ToList();
        }

     /*   public List<Fila> GetFile()
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            List<Fila> lista = new List<Fila>();
            //Fila f = new Fila();
            //f.id_fila = -1;
            //f.Descrizione = "Selezione Fila";
            //lista.Add(f);
            lista.AddRange(entities.Fila.ToList());
            return lista;
        }*/

        public string GetNomeVenditaById(int id)
        {
            AnyeLabelEntities entities = new AnyeLabelEntities();
            return entities.Tipi_Vendita.Where(x => x.id_TipoVendita == id).Select(x => x.Descrizione_tipoVendita).FirstOrDefault();
        }

        
    }
}
