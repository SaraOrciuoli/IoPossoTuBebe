using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WhatsAppService
{
    // ---------------- CONFIGURAZIONE ----------------
    // Sostituisci con i tuoi dati VERI che hai salvato prima
    private const string PhoneId = "1027948587061001";
    private const string AccessToken = "EAANe8RjeRycBQjz99rFKERAB3Tp2DH6TFxkDocsZBD5Kuu0anwSz94FHSY1CSwhA4w3kVBYlwJNVTUlL1ONaTiQtPVzJYBa7xP5b9Nhnt5Oz69dVZBe5qDDLD5pici1H8CugExUzfzA0EyP8HA6hR8W4Ms1tlvHAEwfaIbJScPj3bA8DG73nRQ4HLgZATknmwZCi0PZBZAT8GUpfjmQZAZBbIrnMUkLFNR3sIxq3FgAis6FZC3dl26ZBgtErrcIf3ZCh1FmhGdhOJB5KZCzY7H3xAk8ZD";
    // ------------------------------------------------

    private const string ApiUrl = "https://graph.facebook.com/v22.0/";

    public async Task<bool> InviaPromemoriaAsync(string numeroDestinatario, string nomeCliente, string dataApp, string oraApp)
    {
        string numeroPulito = PulisciNumero(numeroDestinatario);

        // 2. Configura TLS 1.2 (Necessario per .NET Framework vecchi come 4.5/4.6/4.7)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(ApiUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

            // 3. Costruzione del JSON (Payload)
            // Deve corrispondere ESATTAMENTE al template creato al Passo 1
            var payload = new
            {
                messaging_product = "whatsapp",
                to = numeroPulito,
                type = "template",
                template = new
                {
                    name = "hello_world", // Nome del template creato su Meta
                    language = new { code = "en_US" },
                    //components = new[]
                    //{
                    //    new
                    //    {
                    //        type = "body",
                    //        parameters = new[]
                    //        {
                    //            new { type = "text", text = nomeCliente }, // {{1}}
                    //            new { type = "text", text = dataApp },     // {{2}}
                    //            new { type = "text", text = oraApp }       // {{3}}
                    //        }
                    //    }
                    //}
                }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            try
            {
                // 4. Invio della richiesta
                var response = await client.PostAsync($"{PhoneId}/messages", jsonContent);

                // Leggi la risposta per debug
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true; // Inviato con successo
                }
                else
                {
                    // Logga l'errore nel tuo sistema
                    System.Diagnostics.Debug.WriteLine($"Errore WhatsApp: {responseString}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eccezione WhatsApp: {ex.Message}");
                return false;
            }
        }
    }

    private string PulisciNumero(string numero)
    {
        // Rimuove spazi, trattini, parentesi
        string n = System.Text.RegularExpressions.Regex.Replace(numero, "[^0-9]", "");

        // Se inizia con 3... aggiungi 39 (Italia)
        if (n.Length == 10 && (n.StartsWith("3")))
        {
            return "39" + n;
        }
        // Se inizia con 0039... togli 00
        if (n.StartsWith("0039"))
        {
            return n.Substring(2);
        }

        return n; 
    }
}