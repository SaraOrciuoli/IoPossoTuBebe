using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Vetrina.Controllers
{
    public class PaypalController : Controller
    {
        private readonly string clientId = "AS5tDwz6JU4nr0F_eBhVOb6OmrWlfFZZZwzJPCw39MJ5pozOxpKwOV6RklxEpe5A5B2umJ0WG395z68z";
        private readonly string secret = "EKHrQvXWeHQa78PbQAgTM7ecsyTalkMJHsaTUPYv8uxOUtkO73U5CPzzjVfmLWl7jtHcJyy2V-xQxwJF";

        private async Task<string> GetAccessToken()
        {
            using (var client = new HttpClient())
            {
                var authToken = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());

                return json["access_token"].ToString();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder()
        {
            var accessToken = await GetAccessToken();

            // Qui puoi recuperare dati dal tuo DB o sessione per il prezzo, sconti, etc.
            decimal totalPrice = 100.00m; // esempio
            decimal shippingCost = 10.00m;
            decimal discount = 5.00m;
            decimal finalAmount = totalPrice + shippingCost - discount;

            var orderPayload = new JObject
            {
                ["intent"] = "CAPTURE",
                ["purchase_units"] = new JArray
                {
                    new JObject
                    {
                        ["reference_id"] = "default",
                        ["amount"] = new JObject
                        {
                            ["currency_code"] = "EUR",
                            ["value"] = finalAmount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                            ["breakdown"] = new JObject
                            {
                                ["item_total"] = new JObject { ["currency_code"] = "EUR", ["value"] = totalPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) },
                                ["shipping"] = new JObject { ["currency_code"] = "EUR", ["value"] = shippingCost.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) },
                                ["discount"] = new JObject { ["currency_code"] = "EUR", ["value"] = discount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) }
                            }
                        }
                    }
                }
            };


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders",
                    new StringContent(orderPayload.ToString(), System.Text.Encoding.UTF8, "application/json"));

                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);
                string orderId = json["id"]?.ToString();

                return Json(new { id = orderId });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CaptureOrder(string orderID)
        {
            var accessToken = await GetAccessToken();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.PostAsync($"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderID}/capture", null);
                var responseBody = await response.Content.ReadAsStringAsync();

                return Content(responseBody, "application/json");
            }
        }
    }
}
