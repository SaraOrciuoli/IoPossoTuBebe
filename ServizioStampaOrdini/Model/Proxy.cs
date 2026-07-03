using DAL.Model;
using DAL.Repository;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServizioStampaAdmin.Model
{
    class Proxy
    {
        internal List<DettagliOrdine> GetOrdiniDaStampare()
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetOrdiniDaStampare");
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettagliOrdine> data = JsonConvert.DeserializeObject<List<DettagliOrdine>>(result);
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        internal void UpdateStatoOrdine(int id_ordine)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdateStatoOrdine");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("?order=" + id_ordine);
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal List<DettaglioProdottiOrdine> GetprodottiOrdine(string id)
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetProdottiOrdine?order=" + id);
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettaglioProdottiOrdine> data = JsonConvert.DeserializeObject<List<DettaglioProdottiOrdine>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TabellaOrdiniTavolo> GetDettaglioOrdineTavoloStampare(string idGestione, string idTavolo)
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetDettaglioOrdineTavoloStampare?idTavolo=" + idTavolo + "&idGestione=" + idGestione);
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<TabellaOrdiniTavolo> data = JsonConvert.DeserializeObject<List<TabellaOrdiniTavolo>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DettaglioStampanti> GetStampanti()
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetStampanti");
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettaglioStampanti> data = JsonConvert.DeserializeObject<List<DettaglioStampanti>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DettaglioAssociazioneStampantiCategorie> GetCatStampanti(string idstampante)
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetCatStampante?idstampante=" + idstampante);
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettaglioAssociazioneStampantiCategorie> data = JsonConvert.DeserializeObject<List<DettaglioAssociazioneStampantiCategorie>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdatePreOrder(int id_gestione, int id_tavolo)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdatePreOrder");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("?idgestione=" + id_gestione + "&idTavolo=" +id_tavolo);
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal List<DettaglioGestioneTavoli> GetPreOrderDaStampare()
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetPreContoDaStampare");
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettaglioGestioneTavoli> data = JsonConvert.DeserializeObject<List<DettaglioGestioneTavoli>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<DettaglioGestioneTavoli> GetCheckDaStampare()
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetCheckDaStampare");
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettaglioGestioneTavoli> data = JsonConvert.DeserializeObject<List<DettaglioGestioneTavoli>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdateCheck(int id_gestione, int id_tavolo)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdateCheck");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("?idgestione=" + id_gestione + "&idTavolo=" + id_tavolo);
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal bool GetChiusura()
        {
            var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetChiusura");
            string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            var response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string strStatus = ((HttpWebResponse)response).StatusDescription;
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            bool data = JsonConvert.DeserializeObject<bool>(result);
            return data;
        }

        internal bool GetLettura()
        {
            var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetLettura");
            string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            var response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string strStatus = ((HttpWebResponse)response).StatusDescription;
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            bool data = JsonConvert.DeserializeObject<bool>(result);
            return data;
        }

        internal bool GetRegalo()
        {
            var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetRegalo");
            string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            var response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string strStatus = ((HttpWebResponse)response).StatusDescription;
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            bool data = JsonConvert.DeserializeObject<bool>(result);
            return data;
        }


        internal void UpdateChiusura()
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdateChiusura");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("");
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal void UpdateLettura()
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdateLettura");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("");
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal void UpdateRegalo()
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdateRegalo");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("");
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal void UpdateAnnullaScontrino(int id_annullo)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["Pathws"] + "UpdateAnnullo");
                client.Authenticator = new HttpBasicAuthenticator("dariomdagostino@gmail.com", "131915");
                var request = new RestRequest("?id_annullo=" + id_annullo );
                var response = client.Post(request);
                var content = response.Content; // Raw content as string
            }
            catch (Exception ex)
            {

            }
        }

        internal AnnulloScontrino GetAnnullaScontrino()
        {
            var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetAnnullaScontrino");
            string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            var response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string strStatus = ((HttpWebResponse)response).StatusDescription;
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            AnnulloScontrino data = JsonConvert.DeserializeObject<AnnulloScontrino>(result);
            return data;
        }

        internal List<DettaglioGestioneTavoli> GetCheckDaStampareNonFiscale()
        {
            try
            {
                var request = WebRequest.Create(ConfigurationManager.AppSettings["Pathws"] + "GetCheckDaStampareNonFiscale");
                string authInfo = "dariomdagostino@gmail.com" + ":" + "131915";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string strStatus = ((HttpWebResponse)response).StatusDescription;
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                List<DettaglioGestioneTavoli> data = JsonConvert.DeserializeObject<List<DettaglioGestioneTavoli>>(result);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
