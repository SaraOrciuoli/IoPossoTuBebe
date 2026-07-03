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
    public class SendEmail
    {

        public static string SendMail(string nome, string cognome, string email, double totale, string indirizzo, string città, string cap, string pagamento, List<DettaglioProdottiOrdine> prodotti, int ordine, double? discountValue, double? discountPercentage, string nazione, double shippingCost)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["nomeCliente"]);
                mail.To.Add(email);
                mail.Subject = $"{ConfigurationManager.AppSettings["nomeCliente"]} - Conferma Ordine #{ordine}";

                // Costruzione Tabella Prodotti
                string productList = @"
            <table width='100%' cellpadding='12' cellspacing='0' style='border-collapse: collapse; margin-bottom: 20px;'>
                <thead>
                    <tr>
                        <th align='left' style='border-bottom: 2px solid #2c3e50; color: #2c3e50; font-size: 14px; text-transform: uppercase;'>Prodotto</th>
                        <th align='center' style='border-bottom: 2px solid #2c3e50; color: #2c3e50; font-size: 14px; text-transform: uppercase;'>Qtà</th>
                        <th align='right' style='border-bottom: 2px solid #2c3e50; color: #2c3e50; font-size: 14px; text-transform: uppercase;'>Prezzo</th>
                    </tr>
                </thead>
                <tbody>";

                double subTotaleProdotti = 0;
                DAL.Repository.DettagliProdottiRepository dettRep = new DAL.Repository.DettagliProdottiRepository();


                foreach (var prodotto in prodotti)
                {
                    string stringaTaglia = "";
                    if (prodotto.id_taglia != null && prodotto.id_taglia > 0)
                    {
                        var objTaglia = dettRep.getTagliaById(prodotto.id_taglia);
                        if (objTaglia != null)
                        {
                            stringaTaglia = $"<br/><small style='color: #777; font-size: 12px;'>Taglia: {objTaglia.Descrizione_taglia}</small>";
                        }
                    }

                    string NomePrd = GetProdNomeById(prodotto.id_prodotto);
                    double prezzoTotRiga = (prodotto.Quantita * prodotto.prezzoUnitario);
                    subTotaleProdotti += prezzoTotRiga;

                    productList += $@"
                <tr>
                <td style='border-bottom: 1px solid #eee; color: #333; font-size: 15px; padding: 15px 5px;'>
                    {NomePrd}
                    {stringaTaglia}
                </td>
                    <td align='center' style='border-bottom: 1px solid #eee; color: #333; font-size: 15px; padding: 15px 5px;'>{prodotto.Quantita}</td>
                    <td align='right' style='border-bottom: 1px solid #eee; color: #2c3e50; font-weight: bold; font-size: 15px; padding: 15px 5px;'>&euro; {prezzoTotRiga.ToString("0.00")}</td>
                </tr>";
                }
                productList += "</tbody></table>";

             
                string discountInfoRow = "";
                double valoreScontoDaSottrarre = 0;

                if (discountValue.HasValue && discountValue.Value > 0)
                {
                    valoreScontoDaSottrarre = discountValue.Value;
                    discountInfoRow = $@"
                <tr>
                    <td align='right' style='padding: 8px 0; color: #8B0000; font-size: 14px;'>Sconto Applicato:</td>
                    <td align='right' style='padding: 8px 0; color: #8B0000; font-weight: bold; font-size: 15px;'>- &euro; {valoreScontoDaSottrarre.ToString("0.00")}</td>
                </tr>";
                }
                else if (discountPercentage.HasValue && discountPercentage.Value > 0)
                {
                    valoreScontoDaSottrarre = (totale / 100) * discountPercentage.Value;
                    discountInfoRow = $@"
                <tr>
                    <td align='right' style='padding: 8px 0; color: #8B0000; font-size: 14px;'>Sconto ({discountPercentage.Value}%):</td>
                    <td align='right' style='padding: 8px 0; color: #8B0000; font-weight: bold; font-size: 15px;'>- &euro; {valoreScontoDaSottrarre.ToString("0.00")}</td>
                </tr>";
                }

                totale -= valoreScontoDaSottrarre;
                double DaPagare = totale + shippingCost;

                string bankInfo = "";
                if (pagamento.ToLower().Contains("bonifico"))
                {
                    bankInfo = $@"
            <div style='background-color: #f8f9fa; border: 1px solid #e0e0e0; border-radius: 8px; padding: 20px; margin-top: 30px;'>
                <h3 style='color: #8B0000; margin-top: 0; font-size: 16px; text-transform: uppercase;'>Coordinate per il Bonifico</h3>
                <p style='margin: 5px 0; font-size: 14px;'><strong style='color:#2c3e50;'>Banca:</strong> {ConfigurationManager.AppSettings["NomeBanca"]}</p>
                <p style='margin: 5px 0; font-size: 14px;'><strong style='color:#2c3e50;'>Intestatario:</strong> {ConfigurationManager.AppSettings["Intestatario"]}</p>
                <p style='margin: 5px 0; font-size: 14px;'><strong style='color:#2c3e50;'>IBAN:</strong> <span style='color: #8B0000; font-weight: bold;'>{ConfigurationManager.AppSettings["IBAN"]}</span></p>
                <p style='margin: 5px 0; font-size: 14px;'><strong style='color:#2c3e50;'>Causale:</strong> Ordine #{ordine} - {nome} {cognome}</p>
                <p style='margin-top: 15px; font-size: 13px; color: #666;'>L'ordine verrà elaborato non appena riceveremo l'accredito sul nostro conto.</p>
            </div>";
                }

                // Costruzione Body Email
                string emailBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8'>
        </head>
        <body style='font-family: Arial, sans-serif; background-color: #f1f2f6; margin: 0; padding: 40px 10px;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 5px 20px rgba(0,0,0,0.05);'>
                
                <div style='text-align: center; padding: 30px 20px; background-color: #ffffff; border-bottom: 3px solid #8B0000;'>
                    <img src='' alt='{ConfigurationManager.AppSettings["nomeCliente"]}' style='max-width: 200px; height: auto;' />
                </div>

                <div style='padding: 40px 30px;'>
                    <h1 style='color: #2c3e50; font-size: 22px; margin-top: 0; text-align: center;'>Grazie per il tuo ordine, {nome}!</h1>
                    <p style='color: #555; font-size: 15px; line-height: 1.6; text-align: center; margin-bottom: 30px;'>
                        Abbiamo ricevuto il tuo ordine <strong>#{ordine}</strong> e lo stiamo elaborando. Di seguito trovi il riepilogo dei tuoi acquisti.
                    </p>

                    {productList}

                    <table width='100%' cellpadding='0' cellspacing='0' style='margin-top: 20px;'>
                        <tr>
                            <td width='50%'></td>
                            <td width='50%'>
                                <table width='100%' cellpadding='0' cellspacing='0'>
                                    {discountInfoRow}
                                    <tr>
                                        <td align='right' style='padding: 8px 0; color: #555; font-size: 14px;'>Spedizione:</td>
                                        <td align='right' style='padding: 8px 0; color: #2c3e50; font-weight: bold; font-size: 15px;'>{(shippingCost == 0 ? "Gratuita" : "&euro; " + shippingCost.ToString("0.00"))}</td>
                                    </tr>
                                    <tr>
                                        <td align='right' style='padding: 15px 0 0 0; color: #2c3e50; font-size: 16px; font-weight: bold; border-top: 2px solid #eee;'>TOTALE:</td>
                                        <td align='right' style='padding: 15px 0 0 0; color: #2c3e50; font-size: 20px; font-weight: bold; border-top: 2px solid #eee;'>&euro; {DaPagare.ToString("0.00")}</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    {bankInfo}

                    <div style='margin-top: 40px; padding-top: 20px; border-top: 1px solid #eee;'>
                        <h3 style='color: #2c3e50; font-size: 16px; text-transform: uppercase;'>Indirizzo di Spedizione</h3>
                        <p style='color: #555; font-size: 14px; line-height: 1.5; margin: 0;'>
                            {nome} {cognome}<br>
                            {indirizzo}<br>
                            {cap} - {città} ({nazione})
                        </p>
                    </div>
                </div>

                <div style='background-color: #2c3e50; color: #ffffff; text-align: center; padding: 20px; font-size: 13px;'>
                    <p style='margin: 0;'>{ConfigurationManager.AppSettings["nomeCliente"]}</p>
                    <p style='margin: 5px 0 0 0; color: #95a5a6;'>Hai bisogno di aiuto? Contattaci a {ConfigurationManager.AppSettings["emailFooter"]}</p>
                </div>

            </div>
        </body>
        </html>";

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


        public static string SendMailAdmin(string nome, string cognome, string email, double totale, string indirizzo, string città, string cap, string pagamento, List<DettaglioProdottiOrdine> prodotti, int ordine, double? discountValue, double? discountPercentage, string nazione, double shippingCost)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                var mailInvio = "emanueleavitabile1@gmail.com"; 
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["nomeCliente"]);
                mail.To.Add(mailInvio);
                mail.Subject = $"NUOVO ORDINE #{ordine} - {ConfigurationManager.AppSettings["nomeCliente"]}";

                string productList = @"
            <table width='100%' cellpadding='12' cellspacing='0' style='border-collapse: collapse; margin-bottom: 20px;'>
                <thead>
                    <tr>
                        <th align='left' style='border-bottom: 2px solid #2c3e50; color: #2c3e50; font-size: 14px; text-transform: uppercase;'>Prodotto</th>
                        <th align='center' style='border-bottom: 2px solid #2c3e50; color: #2c3e50; font-size: 14px; text-transform: uppercase;'>Qtà</th>
                        <th align='right' style='border-bottom: 2px solid #2c3e50; color: #2c3e50; font-size: 14px; text-transform: uppercase;'>Prezzo</th>
                    </tr>
                </thead>
                <tbody>";

                DAL.Repository.DettagliProdottiRepository dettRep = new DAL.Repository.DettagliProdottiRepository();
                foreach (var prodotto in prodotti)
                {
                    string stringaTaglia = "";
                    if (prodotto.id_taglia != null && prodotto.id_taglia > 0)
                    {
                        var objTaglia = dettRep.getTagliaById(prodotto.id_taglia);
                        if (objTaglia != null)
                        {
                            stringaTaglia = $"<br/><small style='color: #777; font-size: 12px;'>Taglia: {objTaglia.Descrizione_taglia}</small>";
                        }
                    }


                    string NomePrd = GetProdNomeById(prodotto.id_prodotto);
                    productList += $@"
                <tr>
                    <td style='border-bottom: 1px solid #eee; color: #333; font-size: 14px; padding: 10px 5px;'>{NomePrd} {stringaTaglia}
</td>
                    <td align='center' style='border-bottom: 1px solid #eee; color: #333; font-size: 14px; padding: 10px 5px;'>{prodotto.Quantita}</td>
                    <td align='right' style='border-bottom: 1px solid #eee; color: #2c3e50; font-weight: bold; font-size: 14px; padding: 10px 5px;'>&euro; {((prodotto.Quantita * prodotto.prezzoScontato).ToString("0.00"))}</td>
                </tr>";
                }
                productList += "</tbody></table>";



                string discountInfoRow = "";
                double valoreScontoDaSottrarre = 0;

                if (discountValue.HasValue && discountValue.Value > 0)
                {
                    valoreScontoDaSottrarre = discountValue.Value;
                    discountInfoRow = $"<tr><td align='right' style='padding: 5px 0;'>Sconto Valore:</td><td align='right' style='padding: 5px 0; color:#8B0000;'>- &euro; {valoreScontoDaSottrarre.ToString("0.00")}</td></tr>";
                }
                else if (discountPercentage.HasValue && discountPercentage.Value > 0)
                {
                    valoreScontoDaSottrarre = (totale / 100) * discountPercentage.Value;
                    discountInfoRow = $"<tr><td align='right' style='padding: 5px 0;'>Sconto ({discountPercentage.Value}%):</td><td align='right' style='padding: 5px 0; color:#8B0000;'>- &euro; {valoreScontoDaSottrarre.ToString("0.00")}</td></tr>";
                }

                totale -= valoreScontoDaSottrarre;
                double DaPagare = totale + shippingCost;

                string emailBody = $@"
        <!DOCTYPE html>
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f1f2f6; margin: 0; padding: 20px;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; padding: 30px; border-top: 5px solid #2c3e50;'>
                <h2 style='color: #2c3e50; margin-top: 0;'>Nuovo Ordine Ricevuto</h2>
                <p style='color: #555; font-size: 15px;'>Il cliente <strong>{nome} {cognome}</strong> ha appena completato un ordine.</p>
                
                <div style='background-color: #f8f9fa; padding: 15px; border-radius: 8px; margin-bottom: 20px;'>
                    <p style='margin: 5px 0;'><strong>ID Ordine:</strong> #{ordine}</p>
                    <p style='margin: 5px 0;'><strong>Metodo Pagamento:</strong> <span style='text-transform:uppercase; color:#8B0000;'>{pagamento}</span></p>
                    <p style='margin: 5px 0;'><strong>Email Cliente:</strong> {email}</p>
                </div>

                <h3 style='color: #2c3e50; font-size: 16px; border-bottom: 1px solid #eee; padding-bottom: 10px;'>Dettagli Spedizione</h3>
                <p style='color: #555; font-size: 14px; line-height: 1.5;'>
                    {indirizzo}<br>
                    {cap} - {città} ({nazione})
                </p>

                <h3 style='color: #2c3e50; font-size: 16px; border-bottom: 1px solid #eee; padding-bottom: 10px; margin-top: 30px;'>Riepilogo Carrello</h3>
                {productList}

                <table width='100%' cellpadding='0' cellspacing='0'>
                    <tr>
                        <td width='50%'></td>
                        <td width='50%'>
                            <table width='100%' cellpadding='0' cellspacing='0' style='font-size: 14px;'>
                                {discountInfoRow}
                                <tr>
                                    <td align='right' style='padding: 5px 0;'>Spedizione:</td>
                                    <td align='right' style='padding: 5px 0;'>&euro; {shippingCost.ToString("0.00")}</td>
                                </tr>
                                <tr>
                                    <td align='right' style='padding: 10px 0; font-weight: bold; font-size: 16px;'>TOTALE PAGATO:</td>
                                    <td align='right' style='padding: 10px 0; font-weight: bold; font-size: 16px; color: #2c3e50;'>&euro; {DaPagare.ToString("0.00")}</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </body>
        </html>";

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
        private static string GetColoreNomeById(int idColore)
        {

            AnyeLabelEntities db = new AnyeLabelEntities();
            var taglia = db.Colori.FirstOrDefault(t => t.id_Colori == idColore);
            return taglia != null ? taglia.Descrizione_colore : "Standard";

        }
        private static string GetSpessoreNomeById(int idSpessore)
        {

            AnyeLabelEntities db = new AnyeLabelEntities();
            var taglia = db.Spessori.FirstOrDefault(t => t.id_spessore == idSpessore);
            return taglia != null ? taglia.descrizione_spessore : "Standard";

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


        public static string SendMailRecuperaPassword(string email, string body)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                var miaMail = email; //email di prova
                //il mittente della email
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"]);
                //i destinatari inseriti nel form
                //mail.To.Add(email);
                mail.To.Add(miaMail);
                mail.Subject = "Sartoria Forrest - Recupero Password";
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Host = ConfigurationManager.AppSettings["SmtpServer"];
                SmtpServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["Port"]);
                SmtpServer.AuthenticationMode = AuthenticationType.Base64;
                SmtpServer.SslType = SslMode.Ssl;
                SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["Password"]);
                SmtpServer.EnableImplicitSsl = true;

                //invio la mail
                SmtpServer.Send(mail);

                return "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return "KO";
            }

        }


    }
}