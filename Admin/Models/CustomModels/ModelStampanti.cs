using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ModelStampanti
    {
        public List<Stampanti> ListaStampanti { get; set; }
        public Stampanti Stampante { get; set; }
        public List<DettaglioStampanti> ListaDettaglioStampanti { get; set; }
        public DettaglioStampanti DettaglioStampante { get; set; }
        public List<TipoStampante> ListaTipiStampanti { get; set; }
        public TipoStampante TipoStampante { get; set; }
        public List<AssociazioneStampantiCategorie> ListaAssociazioni { get; set; }
        public AssociazioneStampantiCategorie Associazione { get; set; }
        public List<DettaglioAssociazioneStampantiCategorie> ListaDettaglioAssociazioni { get; set; }
        public DettaglioAssociazioneStampantiCategorie DettaglioAssociazione { get; set; }
        public List<Categorie> ListaCategorie { get; set; }
        public string Messaggio { get; set; }
        public int? IdStampante { get; set; }
        public int Count { get; set; }
    }
}