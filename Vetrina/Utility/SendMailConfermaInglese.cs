using AegisImplicitMail;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace Vetrina.Utility
{
    public class SendMailConfermaInglese
    {
        public static string EmailDiConfermaInglese(string nome, string cognome, string email, string indirizzo, string città, string cap, string pagamento, List<DettaglioProdottiOrdine> prodotti, string nazione)
        {
            try
            {
                double totale = prodotti.Sum(p => p.Quantita * (p.prezzoScontato > 0 ? p.prezzoScontato : p.prezzoUnitario));
                double shippingCost = 0;
                if (nazione == "Italia")
                {
                    shippingCost = 10;
                }
                else
                {
                    shippingCost = 40;
                }

                totale += shippingCost;
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"]);
                mail.To.Add(email);
                mail.Subject = $"Forrest - Ordine Effettuato";
                string productList = "<table><tr>" +
                    "<th>Prodotto</th>" +
                    "<th>Quantità</th>" +
                    "<th>Taglia</th>" +
                    "<th>Prezzo Totale</th>" +
                    "</tr>";

                foreach (var prodotto in prodotti)
                {
                    string tagliaNome = GetTagliaNomeById(prodotto.id_taglia);
                    string NomePrd = GetProdNomeById(prodotto.id_prodotto);
                    double prezzoEffettivo = prodotto.prezzoScontato > 0 ? prodotto.prezzoScontato : prodotto.prezzoUnitario;

                    productList += $"<tr>" +
                        $"<td>{NomePrd}</td>" +
                        $"<td>{prodotto.Quantita}</td>" +
                        $"<td>{tagliaNome}</td>" +
                        $"<td>&euro; {prodotto.Quantita * prezzoEffettivo}</td>" +
                        $"</tr>";
                }

                productList += "</table>";

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
                    h1 {{
                        text-align:center;
                    }}
                    h2 {{
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
                        <p>Gentile {nome} {cognome}, grazie per aver acquistato!</p>
                        <p>Specifiche spedizione:</p>                
                         <table>
                            
                            <tr>
                                <th>Indirizzo</th>
                                <td>{indirizzo}</td>
                            </tr>
                            <tr>
                                <th>Città</th>
                                <td>{città}</td>
                            </tr>
            
                            <tr>
                                <th>CAP</th>
                                <td>{cap}</td>
                            </tr>
           
                        </table>
                        <table>
                            <tr>
                                <th>
                                    <p class='font-size'>Data di acquisto</p>
                                </th>
                               
                                 <th>
                                     <p class='font-size'>Metodo di pagamento</p>
                                </th>

                                 <th>
                                     <p class='font-size'>Totale</p>
                                </th>
                            </tr>

                             <tr>
                                <td>
                                    <p class='font-size'>{DateTime.Now}</p>
                                </td>
                               
                                <td>
                                    <p class='font-size'>{pagamento}</p>
                                </td>
                               
                                 <td>
                                    <p class='font-size'>&euro; {totale}</p>
                                </td>
                            </tr>
                        </table>
                        <p>Dettagli del tuo ordine:</p>
                        {productList}
                        <h1>GRAZIE PER AVERCI SCELTO</h1>
                    </div>
                </body>
                </html>
                ";

                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                SmtpServer.Host = "out.postassl.it";
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
                Console.WriteLine(ex.Message);

                return "KO";
            }
        }

        private static string GetTagliaNomeById(int idTaglia)
        {

            AnyeLabelEntities db = new AnyeLabelEntities();
            var taglia = db.Taglie.FirstOrDefault(t => t.id_taglia == idTaglia);
            return taglia != null ? taglia.Descrizione_taglia : "Taglia non trovata";

        }
        private static string GetProdNomeById(int idProd)
        {

            AnyeLabelEntities db = new AnyeLabelEntities();
            var prodotti = db.Prodotti.FirstOrDefault(t => t.id_prodotto == idProd);
            return prodotti != null ? prodotti.Descrizione : "nome prod non trovato";

        }
    }
}
