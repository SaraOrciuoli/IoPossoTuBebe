using AegisImplicitMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using DAL.Model;
using System.Configuration;
using System.IO;


namespace Vetrina.Utility
{
    public class SendMailRecupera
    {
        public static string RecuperaPasswordEmail(string nomeUtente, string passwordTemporanea, string emailDestinatario)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"]);
                mail.To.Add(emailDestinatario);

                mail.Subject = "Bettanini - Recupero Accesso Admin";

                string emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color: #333333; padding: 0; margin: 0; background-color: #f4f6f9; }}
                        .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.05); border: 1px solid #eaeaea; }}
                        .header {{ background-color: #2c3e50; padding: 30px 20px; text-align: center; }}
                        .header img {{ max-height: 60px; display: block; margin: 0 auto; }}
                        .content {{ padding: 40px 30px; text-align: center; }}
                        h1 {{ color: #8B0000; font-size: 24px; margin-top: 0; text-transform: uppercase; letter-spacing: 1px; }}
                        p {{ font-size: 16px; line-height: 1.6; color: #4a4a4a; margin-bottom: 20px; }}
                        .password-box {{ background-color: #f8f9fa; border-radius: 8px; padding: 20px; margin: 30px auto; border-left: 4px solid #c49a6c; display: inline-block; font-size: 26px; font-weight: bold; letter-spacing: 4px; color: #2c3e50; }}
                        .warning {{ font-size: 13px; color: #999999; margin-top: 30px; }}
                        .footer {{ background-color: #f8f9fa; text-align: center; padding: 20px; font-size: 12px; color: #999999; border-top: 1px solid #eaeaea; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <img src='https://www.otticabettanini.it/images/thumbs/0000206_logo.jpeg' alt='Bettanini Admin' />
                        </div>
                        <div class='content'>
                            <h1>Recupero Accesso Admin</h1>
                            <p>Gentile <strong>{nomeUtente}</strong>,<br>è stato richiesto un reset della password per il tuo account  sul gestionale Bettanini.</p>
                            <p>Ecco la tua nuova password temporanea:</p>
                            
                            <div class='password-box'>
                                {passwordTemporanea}
                            </div>
                            
                            <p>Ti invitiamo ad accedere al pannello di gestione e modificare questa password il prima possibile per ragioni di sicurezza.</p>
                            <p class='warning'>Se non hai richiesto questo reset, contatta immediatamente il supporto tecnico IT.</p>
                        </div>
                        <div class='footer'>
                            Bettanini Management System &copy; {DateTime.Now.Year}<br>
                            Area ad accesso riservato.
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
                SmtpServer.SendMail(mail);
                return "OK";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore invio mail Admin: " + ex.Message);
                return "KO";
            }
        }
    }
}