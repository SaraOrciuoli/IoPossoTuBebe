using Admin.Pos.Classes;
using Admin.Pos.Utility;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Admin.Pos.Printers
{
    public class PosPrinter
    {
        string printer = "";
        string body = "";
        public PosPrinter(PrintConfig conf)
        {
            if (conf.pos.https)
            {
                printer = @"https://";
            }
            else
            {
                printer = @"http://";

            }
            printer += conf.pos.ip;
            CreateBodies body_req = new CreateBodies(conf);
            body = body_req.createBody();

        }


        public string GetBody()
        {
            return body;
        }
        public string GetStampante()
        {
            return printer+ "/cmd/wec";
        }
        public string Send()
        {
            var client = new RestClient(printer + "/cmd/wec");
            //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/plain");

            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public string ChiusuraGiornaliera()
        {
            var client = new RestClient(printer + "/cmd/wec");
            //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/plain");
            var body = @"clear" + "\n" +
            @"resprn" + "\n" +
            @";azzgio tipo=0		; Effettua solo azzeramento" + "\n" +
            @";azzgio tipo=1		; Effettua l'azzeramento giornaliero Esteso" + "\n" +
            @"azzgio tipo=2		; Effettua l'azzeramento giornaliero Breve" + "\n" +
            @";azzgio tipo=3		; Effettua l'azzeramento giornaliero Medio" + "\n" +
            @"wecfine" + "\n" +
            @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public string InviaAgenziaEntrate()
        {
            var client = new RestClient(printer + "/cmd/wec");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/plain");
            var body = @"clear" + "\n" +
            @"resprn" + "\n" +
            @"RTEJ INVIOAE" + "\n" +
            @"wecfine" + "\n" +
            @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}