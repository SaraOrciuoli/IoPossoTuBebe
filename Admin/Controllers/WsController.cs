/*using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Admin.Security;

namespace Admin.Controllers
{
    [BasicAuthentication]
    public class WsController : ApiController
    {

      *//*  public IHttpActionResult GetStampanti()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            List<DettaglioStampanti> list = repository.GetListaStampanti();
            return Ok(list);
        }*/

        /*public IHttpActionResult GetCatStampante(int idstampante)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            List<DettaglioAssociazioneStampantiCategorie> list = (List<DettaglioAssociazioneStampantiCategorie>)repository.GetListaCategorieStampanti(idstampante);
            return Ok(list);
        }*//*

        public IHttpActionResult GetOrdiniDaStampare()
        {
            OrdiniRepository repository = new OrdiniRepository();
            List<DettagliOrdine> list = repository.GetOrdiniStampare();
            return Ok(list);
        }


        [HttpPost]
        public void UpdateStatoOrdine(int order)
        {
            OrdiniRepository repository = new OrdiniRepository();
            repository.UpdateStatoOrdine(order);
        }

        [HttpPost]
        public void UpdatePreOrder(string idgestione,string idTavolo)
        {
            OrdiniRepository repository = new OrdiniRepository();
            repository.UpdatePreOrder (Convert.ToInt32(idgestione), Convert.ToInt32(idTavolo));
        }

        [HttpPost]
        public void UpdateCheck(string idgestione, string idTavolo)
        {
            OrdiniRepository repository = new OrdiniRepository();
            repository.UpdateCheck(Convert.ToInt32(idgestione), Convert.ToInt32(idTavolo));
        }

        public IHttpActionResult GetProdottiOrdine(string order)
        {
            OrdiniRepository repository = new OrdiniRepository();
            List<DettaglioProdottiOrdine> list = repository.GetprodottiOrdine(order);
            return Ok(list);
        }

        public IHttpActionResult GetPreContoDaStampare()
        {
            OrdiniRepository repository = new OrdiniRepository();
            List<DettaglioGestioneTavoli> list = repository.GetPreContoDaStampare();
            return Ok(list);
        }

        public IHttpActionResult GetCheckDaStampare()
        {
            OrdiniRepository repository = new OrdiniRepository();
            List<DettaglioGestioneTavoli> list = repository.GetCheckDaStampare();
            return Ok(list);
        }

        public IHttpActionResult GetCheckDaStampareNonFiscale()
        {
            OrdiniRepository repository = new OrdiniRepository();
            List<DettaglioGestioneTavoli> list = repository.GetCheckDaStampareNonFiscale();
            return Ok(list);
        }

       *//* public IHttpActionResult GetDettaglioOrdineTavoloStampare(int idTavolo, int idGestione)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            List<TabellaOrdiniTavolo> list = repository.GetOrdineTavoloByIdTavoloIdGestione(idTavolo, idGestione);
            return Ok(list);
        }

        [HttpPost]
        public void UpdateChiusura()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.UpdateChiusura();
        }

        [HttpPost]
        public void UpdateRegalo()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.Updateregalo();
        }


        public IHttpActionResult GetChiusura()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            bool chiusura = repository.GetChiusura();
            return Ok(chiusura);
        }

        [HttpPost]
        public void UpdateLettura()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.UpdateLettura();
        }

        public IHttpActionResult GetLettura()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            bool chiusura = repository.GetLettura();
            return Ok(chiusura);
        }

       *//* public IHttpActionResult GetRegalo()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            bool chiusura = repository.GetRegalo();
            return Ok(chiusura);
        }*//*

        [HttpPost]
        public void UpdateAnnullo(string id_annullo)
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            repository.UpdateAnnullo(Convert.ToInt32(id_annullo));
        }

        public IHttpActionResult GetAnnullaScontrino()
        {
            ConfigurazioniRepository repository = new ConfigurazioniRepository();
            return Ok(repository.GetAnnullaScontrino());
        }*//*

    }
}*/