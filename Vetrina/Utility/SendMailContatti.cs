using AegisImplicitMail;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Vetrina.Utility
{
    public class SendEmailContatti
    {

        public static string SendMail(string nome, string cognome, string email, string telefono, string messaggio)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                var mailInvio = "emanueleavitabile1@gmail.com";
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"], "Ottica Bettanini Sistema");
                mail.To.Add(mailInvio);
                mail.Subject = $"Ottica Bettanini - Nuova richiesta di contatto";

                string emailBody = $@"
        <!DOCTYPE html>
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f1f2f6; margin: 0; padding: 40px 20px;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 5px 20px rgba(0,0,0,0.05);'>
                
                <div style='text-align: center; padding: 30px 20px; background-color: #ffffff; border-bottom: 3px solid #8B0000;'>
                    <img src='https://otticabettanini.it/images/thumbs/0000206_logo.jpeg' alt='Ottica Bettanini' style='max-width: 200px; height: auto;' />
                </div>

                <div style='padding: 40px 30px;'>
                    <h2 style='color: #2c3e50; font-size: 20px; margin-top: 0; border-bottom: 1px solid #eee; padding-bottom: 10px;'>Nuova Richiesta dal Sito Web</h2>
                    <p style='color: #555; font-size: 15px; line-height: 1.6; margin-bottom: 30px;'>
                        Hai ricevuto un nuovo messaggio dal modulo di contatto del sito web. Ecco i dettagli:
                    </p>

                    <table width='100%' cellpadding='12' cellspacing='0' style='border-collapse: collapse; background-color: #f8f9fa; border-radius: 8px;'>
                        <tr>
                            <td width='30%' style='border-bottom: 1px solid #eee; color: #95a5a6; font-size: 14px; font-weight: bold; text-transform: uppercase;'>Nome:</td>
                            <td width='70%' style='border-bottom: 1px solid #eee; color: #2c3e50; font-size: 15px; font-weight: bold;'>{nome} {cognome}</td>
                        </tr>
                        <tr>
                            <td style='border-bottom: 1px solid #eee; color: #95a5a6; font-size: 14px; font-weight: bold; text-transform: uppercase;'>Email:</td>
                            <td style='border-bottom: 1px solid #eee; color: #2c3e50; font-size: 15px;'><a href='mailto:{email}' style='color: #5b8bd0; text-decoration: none;'>{email}</a></td>
                        </tr>
                        <tr>
                            <td style='border-bottom: 1px solid #eee; color: #95a5a6; font-size: 14px; font-weight: bold; text-transform: uppercase;'>Telefono:</td>
                            <td style='border-bottom: 1px solid #eee; color: #2c3e50; font-size: 15px;'>{telefono}</td>
                        </tr>
                        <tr>
                            <td colspan='2' style='color: #95a5a6; font-size: 14px; font-weight: bold; text-transform: uppercase; padding-top: 20px;'>Messaggio:</td>
                        </tr>
                        <tr>
                            <td colspan='2' style='color: #333; font-size: 15px; line-height: 1.6; padding-top: 0; padding-bottom: 20px;'>
                                {messaggio}
                            </td>
                        </tr>
                    </table>
                </div>

                <div style='background-color: #2c3e50; color: #ffffff; text-align: center; padding: 20px; font-size: 13px;'>
                    <p style='margin: 0;'>Sistema Notifiche - Ottica Bettanini</p>
                </div>

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
                Console.WriteLine(ex.Message);
                return "KO";
            }
        }

    }
}