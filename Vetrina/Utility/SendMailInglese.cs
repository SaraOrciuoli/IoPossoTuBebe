using AegisImplicitMail;
using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Vetrina.Models.CustomModel;

namespace Vetrina.Utility
{
    public class SendEmailInglese
    {

        public static string SendMailInglese(string nome, string cognome, string email, double totale, string indirizzo, string città, string cap, string pagamento, List<DettaglioProdottiOrdine> prodotti, int ordine, double? discountValue, double? discountPercentage, string nazione)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"]);
                mail.To.Add(email);
                mail.Subject = $"Forrest - Order Sent";
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
                    productList += $"<tr>" +
                        $"<td>Nome: {NomePrd}</td>" +
                        $"<td>{prodotto.Quantita}</td>" +
                        $"<td>{tagliaNome}</td>" +
                        $"<td>&euro; {((prodotto.Quantita * prodotto.prezzoScontato).ToString("0.00"))}</td>" +
                        $"</tr>";
                }

                productList += "</table>";

                double shippingCost = 0;
                bool isIsola = false;



                string discountInfo = "";
                if (nazione == "Italia")
                {
                    ModelPagamento model = new ModelPagamento();
                    UtentiRepository repo = new UtentiRepository();
                    model.ListaComuniIsole = repo.GetListaComuniIsole();

                    isIsola = model.ListaComuniIsole != null && model.ListaComuniIsole.Any(isola => isola.Descrizione_Comune.Equals(città, StringComparison.OrdinalIgnoreCase));

                    if (totale >= 300)
                    {
                        shippingCost = 0;  // Spedizione gratuita
                    }
                    else
                    {
                        if (isIsola)
                        {
                            shippingCost = 13;  // Tariffa per le isole
                        }
                        else
                        {
                            shippingCost = 10;  // Tariffa per il resto d'Italia
                        }
                    }
                }
                else
                {
                    shippingCost = 40;
                }

                if (discountValue.HasValue && discountValue.Value > 0)
                {
                    totale -= discountValue.Value;
                    discountInfo = $"<p>Discount value: &euro; {discountValue.Value}</p>";
                }
                else if (discountPercentage.HasValue && discountPercentage.Value > 0)
                {
                    totale -= (totale / 100) * discountPercentage.Value;
                    discountInfo = $"<p>Discount percentage: {discountPercentage.Value}%</p>";
                }
                else
                {
                    totale -= 0;
                    discountInfo = $"<p>Discount percentage: you have not applied discounts</p>";
                }
                double DaPagare = totale + shippingCost;
                productList += $"<tr>" +
                      $"<td colspan='3'><strong>Shipping cost</strong></td>" +
                      $"<td>&euro; {shippingCost.ToString("0.00")}</td>" +
                      $"</tr>";

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
                        <p>Dear {nome} {cognome}, your order has been sent!</p>
                        <p>Below are the payment details:</p>                

                        <table>
                            <tr>
                                <th>
                                    <p class='font-size'>Bank Name</p>
                                </th>
                       
                                 <th>
                                     <p class='font-size'>Owner</p>
                                </th>

                                 <th>
                                     <p class='font-size'>Causal</p>
                                </th>
                            </tr>

                             <tr>
                                <td>
                                    <p class='font-size'>BPER Banca</p>
                                </td>
                       
                                <td>
                                    <p class='font-size'>Forrest Di D'ambra Alessio</p>
                                </td>
                       
                                 <td>
                                    <p class='font-size'>""Forrest - Ordine Numero {ordine}""</p>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>                               
                                 <th>
                                     <p class='font-size'>IBAN</p>
                                </th>

                                <th>
                                     <p class='font-size'>Total to be paid</p>
                                </th>
                            </tr>

                             <tr>
                                 <td>
                                    <p class='font-size'>IT44N0538739920000042968788</p>
                                </td>

                                <td>
                                    <p class='font-size'>&euro; {DaPagare.ToString("0.00")}</p>
                                </td>
                            </tr>
                        </table>
                        {discountInfo}

                        <p>Shipping specifications:</p>                
                         <table>
                    
                            <tr>
                                <th>Address</th>
                                <td>{indirizzo}</td>
                            </tr>
                            <tr>
                                <th>City</th>
                                <td>{città}</td>
                            </tr>
    
                            <tr>
                                <th>CAP</th>
                                <td>{cap}</td>
                            </tr>
   
                        </table>
                        <p>Details of your order:</p>
                        {productList}
                        <h1>THANK YOU FOR CHOOSING US</h1>
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