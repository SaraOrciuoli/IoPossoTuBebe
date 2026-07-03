//using DAL.Model;
//using DAL.Repository;
//using Newtonsoft.Json;
//using Org.BouncyCastle.Asn1.X509;
//using RestSharp;
//using RestSharp.Authenticators;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.UI.WebControls;

//namespace ServizioStampaAdmin.Model
//{
//    class LocalProxy
//    {
//        internal List<DettagliOrdine> GetOrdiniDaStampare()
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                List<DettagliOrdine> data = repository.GetOrdiniStampare();
//                return data;
//            }
//            catch(Exception ex)
//            {
//                return null;
//            }
            
//        }

//        internal void UpdateStatoOrdine(int id_ordine)
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                repository.UpdateStatoOrdine(id_ordine);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        internal List<DettaglioProdottiOrdine> GetprodottiOrdine(string id)
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                List<DettaglioProdottiOrdine> list = repository.GetprodottiOrdine(id);
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public List<TabellaOrdiniTavolo> GetDettaglioOrdineTavoloStampare(string idGestione, string idTavolo)
//        {
//            try
//            {
//                ConfigurazioniRepository repository = new ConfigurazioniRepository();
//                List<TabellaOrdiniTavolo> list = repository.GetOrdineTavoloByIdTavoloIdGestione(Convert.ToInt32(idTavolo), Convert.ToInt32(idGestione));
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public List<DettaglioStampanti> GetStampanti()
//        {
//            try
//            {
//                ConfigurazioniRepository repository = new ConfigurazioniRepository();
//                List<DettaglioStampanti> list = repository.GetListaStampanti();
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public List<DettaglioAssociazioneStampantiCategorie> GetCatStampanti(string idstampante)
//        {
//            try
//            {
//                ConfigurazioniRepository repository = new ConfigurazioniRepository();
//                List<DettaglioAssociazioneStampantiCategorie> list = (List<DettaglioAssociazioneStampantiCategorie>)repository.GetListaCategorieStampanti(Convert.ToInt32(idstampante));
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public void UpdatePreOrder(int id_gestione, int id_tavolo)
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                repository.UpdatePreOrder(id_gestione, id_tavolo);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        internal List<DettaglioGestioneTavoli> GetPreOrderDaStampare()
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                List<DettaglioGestioneTavoli> list = repository.GetPreContoDaStampare();
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        internal List<DettaglioGestioneTavoli> GetCheckDaStampare()
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                List<DettaglioGestioneTavoli> list = repository.GetCheckDaStampare();
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public void UpdateCheck(int id_gestione, int id_tavolo)
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                repository.UpdateCheck(id_gestione, id_tavolo);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        internal static void SaveCode(Codici cod)
//        {
//            try
//            {
//                ConfigurazioniRepository repository = new ConfigurazioniRepository();
//                repository.SaveCod(cod);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        internal bool GetChiusura()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            return repository.GetChiusura();
//        }

//        internal bool GetLettura()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            return repository.GetLettura();
//        }

//        internal bool GetRegalo()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            return repository.GetRegalo();
//        }


//        internal void UpdateChiusura()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            repository.UpdateChiusura();
//        }

//        internal void UpdateLettura()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            repository.UpdateLettura();
//        }

//        internal void UpdateRegalo()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            repository.Updateregalo();
//        }

//        internal AnnulloScontrino GetAnnullaScontrino()
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            return repository.GetAnnullaScontrino();
//        }

//        internal void UpdateAnnullaScontrino(int? id_annullo)
//        {
//            ConfigurazioniRepository repository = new ConfigurazioniRepository();
//            repository.UpdateAnnullo(id_annullo);
//        }

//        internal List<DettaglioGestioneTavoli> GetCheckDaStampareNonFiscale()
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                List<DettaglioGestioneTavoli> list = repository.GetCheckDaStampareNonFiscale();
//                return list;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        internal void UpdateCheckNonFiscale(int id_gestione, int id_tavolo)
//        {
//            try
//            {
//                OrdiniRepository repository = new OrdiniRepository();
//                repository.UpdateCheckNonFiscale(id_gestione, id_tavolo);
//            }
//            catch (Exception ex)
//            {

//            }
//        }
//    }
//}
