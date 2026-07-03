using AegisImplicitMail;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Vetrina.Utility
{
    public class SendEmailRegistrazione
    {

        public static string EmailRegistrazione(string nome, string cognome, string email, string cellulare, string indirizzo, string citta, string cap)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["nomeCliente"] );
                mail.To.Add(email);

                mail.Subject = $"{ConfigurationManager.AppSettings["nomeCliente"]} - Registrazione completata con successo";

                string emailBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    color: #333333;
                    padding: 0;
                    margin: 0;
                    background-color: #f4f6f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    overflow: hidden;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.05);
                    border: 1px solid #eaeaea;
                }}
                .header {{
                    background-color: #2c3e50; /* Blu scuro Bettanini */
                    padding: 30px 20px;
                    text-align: center;
                }}
                .header img {{
                    max-height: 60px;
                    display: block;
                    margin: 0 auto;
                }}
                .content {{
                    padding: 40px 30px;
                }}
                h1 {{
                    color: #8B0000; /* Rosso scuro Bettanini */
                    text-align: center;
                    font-size: 24px;
                    margin-top: 0;
                    text-transform: uppercase;
                    letter-spacing: 1px;
                }}
                p {{
                    font-size: 16px;
                    line-height: 1.6;
                    color: #4a4a4a;
                    text-align: center;
                    margin-bottom: 30px;
                }}
                .details-box {{
                    background-color: #f8f9fa;
                    border-radius: 8px;
                    padding: 20px;
                    margin-bottom: 30px;
                    border-left: 4px solid #c49a6c; /* Tocco dorato elegante */
                }}
                table {{
                    width: 100%;
                    border-collapse: collapse;
                }}
                th, td {{
                    padding: 10px 5px;
                    text-align: left;
                    font-size: 14px;
                    border-bottom: 1px solid #eeeeee;
                }}
                th {{
                    color: #2c3e50;
                    font-weight: 600;
                    width: 35%;
                }}
                td {{
                    color: #666666;
                }}
                tr:last-child th, tr:last-child td {{
                    border-bottom: none;
                }}
                .btn-container {{
                    text-align: center;
                    margin-top: 30px;
                }}
                .btn {{
                    background-color: #2c3e50;
                    color: #ffffff !important;
                    padding: 14px 35px;
                    text-decoration: none;
                    border-radius: 30px;
                    font-weight: bold;
                    text-transform: uppercase;
                    letter-spacing: 1px;
                    display: inline-block;
                }}
                .footer {{
                    background-color: #f8f9fa;
                    text-align: center;
                    padding: 20px;
                    font-size: 12px;
                    color: #999999;
                    border-top: 1px solid #eaeaea;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <img src='https://www.otticabettanini.it/images/thumbs/0000206_logo.jpeg' alt='Ottica Bettanini' />
                </div>
                
                <div class='content'>
                    <h1>Benvenuto in {ConfigurationManager.AppSettings["nomeCliente"]}</h1>
                    <p>Gentile <strong>{nome} {cognome}</strong>,<br>la tua registrazione è avvenuta con successo! Ecco il riepilogo dei tuoi dati:</p>

                    <div class='details-box'>
                        <table>
                            <tr><th>Nome e Cognome</th><td>{nome} {cognome}</td></tr>
                            <tr><th>E-mail</th><td>{email}</td></tr>
                            <tr><th>Cellulare</th><td>{cellulare}</td></tr>
                            <tr><th>Indirizzo</th><td>{indirizzo}</td></tr>
                            <tr><th>Città</th><td>{citta}</td></tr>
                            <tr><th>CAP</th><td>{cap}</td></tr>
                        </table>
                    </div>

                    <div class='btn-container'>
                        <a href='' class='btn'>Inizia lo shopping</a>
                    </div>
                </div>

                <div class='footer'>
                   {ConfigurationManager.AppSettings["nomeCliente"]} &copy; {DateTime.Now.Year} - Tutti i diritti riservati.<br>
  
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

        private static string GetTagliaNomeById(int idTaglia)
        {

            AnyeLabelEntities db = new AnyeLabelEntities();
            var taglia = db.Taglie.FirstOrDefault(t => t.id_taglia == idTaglia);
            return taglia != null ? taglia.Descrizione_taglia : "Taglia non trovata";

        }


        
    }
}