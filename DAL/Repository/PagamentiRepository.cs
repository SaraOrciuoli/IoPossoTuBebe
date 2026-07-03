using Newtonsoft.Json;
using RestSharp;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Repository
{
    public class PagamentiRepository
    {
        private readonly string klarnaApiUrl = "https://api.playground.klarna.com/hpp/v1/sessions";
        private readonly string klarnaUsername = "87e316e6-82ce-40cb-bc0d-b622485bad29";
        private readonly string klarnaPassword = "klarna_test_api_NDh6LVZiSVR1UCpFS1UhST92MlJCdXJWLWp4dChTWFYsODdlMzE2ZTYtODJjZS00MGNiLWJjMGQtYjYyMjQ4NWJhZDI5LDEsM1plaTB6YVhzdTFiMEpDZk5qNzgrN0MxWVk3U0ZCZkVpRlA4YWNNYXFBUT0";

        private readonly string stripeKey = "sk_test_51THk7xRvZnwkKyEA1cF4Ho23pIpBK0iDLakH7bMbBq1I56C9KJe3K4rlz4WxVNaFZDcgQra2FkBC6b3AoWjuKChB00ejN2vaz4";
        public string GeneraLinkPagamentoKlarna(int idOrdine, double totale, string emailCliente)
        {
            // Klarna lavora sempre in centesimi (es. 150.50€ diventano 15050)
            int totaleCentesimi = (int)(Math.Round(totale, 2) * 100);


            string baseUrl = "https://localhost:44321";

            // Generiamo l'autenticazione Base64 (UID:Password)
            string authInfo = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{klarnaUsername}:{klarnaPassword}"));

            // ====================================================================
            // STEP 1: CREAZIONE DELLA SESSIONE DI PAGAMENTO
            // ====================================================================
            var clientPayments = new RestClient("https://api.playground.klarna.com/payments/v1/sessions");
            var requestPayments = new RestRequest(Method.POST);
            requestPayments.AddHeader("Authorization", $"Basic {authInfo}");
            requestPayments.AddHeader("Content-Type", "application/json");

            var payloadPayments = new
            {
                purchase_country = "IT",
                purchase_currency = "EUR",
                locale = "it-IT",
                order_amount = totaleCentesimi,
                order_tax_amount = 0,
                order_lines = new[]
                {
            new
            {
                type = "physical",
                reference = "ORD-" + idOrdine,
                name = "Ordine Bettanini #" + idOrdine,
                quantity = 1,
                unit_price = totaleCentesimi,
                tax_rate = 0,
                total_amount = totaleCentesimi,
                total_discount_amount = 0,
                total_tax_amount = 0
            }
        }
            };

            requestPayments.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(payloadPayments), ParameterType.RequestBody);
            IRestResponse responsePayments = clientPayments.Execute(requestPayments);

            if (!responsePayments.IsSuccessful)
            {
                throw new Exception("Errore Step 1 (Payment Session): " + responsePayments.Content);
            }

            // Estraiamo l'ID della sessione creata
            dynamic resultPayments = Newtonsoft.Json.JsonConvert.DeserializeObject(responsePayments.Content);
            string sessionId = resultPayments.session_id;


            var clientHpp = new RestClient("https://api.playground.klarna.com/hpp/v1/sessions");
            var requestHpp = new RestRequest(Method.POST);
            requestHpp.AddHeader("Authorization", $"Basic {authInfo}");
            requestHpp.AddHeader("Content-Type", "application/json");

            var payloadHpp = new
            {
                payment_session_url = $"https://api.playground.klarna.com/payments/v1/sessions/{sessionId}",
                merchant_urls = new
                {
                    success = $"{baseUrl}/Ordini/KlarnaSuccess?id_ordine={idOrdine}&authorization_token={{{{authorization_token}}}}",
                    cancel = $"{baseUrl}/Ordini/KlarnaCancel?id_ordine={idOrdine}",
                    back = $"{baseUrl}/Ordini/KlarnaCancel?id_ordine={idOrdine}",
                    failure = $"{baseUrl}/Ordini/KlarnaError?id_ordine={idOrdine}",
                    error = $"{baseUrl}/Ordini/KlarnaError?id_ordine={idOrdine}"
                }
            };

            requestHpp.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(payloadHpp), ParameterType.RequestBody);
            IRestResponse responseHpp = clientHpp.Execute(requestHpp);

            if (!responseHpp.IsSuccessful)
            {
                throw new Exception("Errore Step 2 (HPP): " + responseHpp.Content);
            }

            // Decodifichiamo la risposta finale e restituiamo l'URL!
            dynamic resultHpp = Newtonsoft.Json.JsonConvert.DeserializeObject(responseHpp.Content);
            return resultHpp.redirect_url;
        }


        public string GeneraLinkPagamentoStripe(int idOrdine, double totale, string emailCliente)
        {

            StripeConfiguration.ApiKey = stripeKey;

            string baseUrl = "https://localhost:44321"; 

            long totaleCentesimi = (long)(Math.Round(totale, 2) * 100);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                    "klarna"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = totaleCentesimi,
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Ordine Bettanini #" + idOrdine,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                CustomerEmail = emailCliente,
                SuccessUrl = $"{baseUrl}/Ordini/StripeSuccess?id_ordine={idOrdine}&session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{baseUrl}/Ordini/StripeCancel?id_ordine={idOrdine}",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Url;
        }
    }
}