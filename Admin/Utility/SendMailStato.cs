using AegisImplicitMail;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Admin.Utility
{
    public class SendMailStato
    {
        public static string EmailStatoOrdine(
            string stato,
            string email,
            string ordine,
            decimal totale,
            string indirizzo,
            string citta,
            string cap,
            string pagamento,
            List<ProdottiOrdine> prodotti,
    string statoOrdine)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();

                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"]);
                mail.To.Add(email);
                mail.Subject = $"Forrest - Stato Ordine Aggiornato: {ordine} ({stato})";

                var prodottiHtml = "<table style='width: 100%; border-collapse: collapse;'><tr><th>Prodotto</th><th>Quantità</th><th>Prezzo Unitario</th><th>Totale</th></tr>";
                foreach (var prodotto in prodotti)
                {
                    var nomeProdotto = GetNomeProdottoById(prodotto.id_prodotto);
                    var totaleProdotto = prodotto.Quantita * prodotto.prezzoUnitario;
                    prodottiHtml += $"<tr><td>{nomeProdotto}</td><td>{prodotto.Quantita}</td><td>{prodotto.prezzoUnitario:C}</td><td>{totaleProdotto:C}</td></tr>";
                }
                prodottiHtml += $"<tr><td colspan='3' style='text-align: right;'>Totale</td><td>{totale:C}</td></tr></table>";

                string emailBody = $@"
        <html>
        <head>
            <style>
            body {{
                font-family: 'Arial', sans-serif;
                color: #000;
                padding: 0;
                margin: 0;
            }}
            .container {{
                max-width: 700px;
                margin: 0 auto;
                padding: 20px;
            }}
            .center {{
                text-align: center;
            }}
            .bg-color {{
                background-color: #333333; 
                padding: 10px;
            }}
            h1, h2 {{
                text-align:center;
            }}
            p {{
                text-align:center;
                margin-bottom: 15px;
                font-size: 18px;
            }}
            table {{
                width: 100%;
                border-collapse: collapse;
            }}
            th, td {{
                padding: 8px;
                text-align: left;
                border-bottom: 1px solid #ddd;
            }}
            th {{
                background-color: #829fbf;
                color:white;
            }}
            .font-size {{
                font-size:13px !important;
                margin:0 !important;
            }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='center bg-color'>
                    <img src='https://www.forrest.it/images/ecommerce/home/Logo.png' style='margin: 0 auto; display: block;'/>
                </div>
                <p>Stato aggiornato per l'ordine {ordine}</p>

                <p>Stato dell'ordine: {statoOrdine}</p>

                {prodottiHtml}

                 <table>
                            
                            <tr>
                                <th>Indirizzo</th>
                                <td>{indirizzo}</td>
                            </tr>
                            <tr>
                                <th>Città</th>
                                <td>{citta}</td>
                            </tr>
            
                            <tr>
                                <th>CAP</th>
                                <td>{cap}</td>
                            </tr>
           
                        </table>
                        <table>
                            <tr>
                               
                                 <th>
                                     <p class='font-size'>Metodo di pagamento</p>
                                </th>

                                
                            </tr>

                             <tr>
                               
                                <td>
                                    <p class='font-size'>{pagamento}</p>
                                </td>
                               
                            </tr>
                        </table>        
                    </div>
        </body>
        </html>";

                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                SmtpServer.Host = ConfigurationManager.AppSettings["SmtpServer"];
                SmtpServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["Port"]);
                SmtpServer.AuthenticationMode = AuthenticationType.Base64;
                SmtpServer.SslType = SslMode.Ssl;
                SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["Password"]);
                SmtpServer.EnableImplicitSsl = true;

                SmtpServer.Send(mail);

                return "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'invio dell'email: {ex.Message}");
                return "KO";
            }
        }


        private static string GetNomeProdottoById(int idProdotto)
        {
            try
            {
                using (var db = new AnyeLabelEntities())
                {
                    var prodotto = db.Prodotti
                        .Where(p => p.id_prodotto == idProdotto)
                        .FirstOrDefault();

                    return prodotto != null ? prodotto.Descrizione : "Nome prodotto non trovato";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero del nome del prodotto: {ex.Message}");
                return "Nome prodotto non disponibile";
            }
        }
    }
}
