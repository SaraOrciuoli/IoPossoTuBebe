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
    public class SendEmailContattiProdotto
    {

        public static string SendMailContatti(string email, string nome, string cognome, string telefono, string messaggio, string nomeProdotto)
        {
            try
            {
                var SmtpServer = new MimeMailer();
                var mail = new MimeMailMessage();
                var mailInvio = "emanueleavitabile1@gmail.com";
                mail.From = new MailAddress(ConfigurationManager.AppSettings["User"]);
                mail.To.Add(mailInvio);
                mail.Subject = $"PlexEvolution - Richiesta di contatto - {nomeProdotto}";

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
                <p>Richiesta di Contatto dal sito</p>
                <table>
                    <tr>
                        <th>
                            <p class='font-size'>Nome</p>
                        </th>
                       
                         <th>
                             <p class='font-size'>Cognome</p>
                        </th>
    
                         <th>
                             <p class='font-size'>Email</p>
                        </th>
                            
                        <th>
                             <p class='font-size'>Telefono</p>
                        </th>
    
                        
                    </tr>
    
                     <tr>
                        <td>
                            <p class='font-size'>{nome}</p>
                        </td>
                       
                        <td>
                            <p class='font-size'>{cognome}</p>
                        </td>
                       
                       <td>
                            <p class='font-size'>{email}</p>
                        </td>
    
                       <td>
                            <p class='font-size'>{telefono}</p>
                        </td>
                     
                    </tr>
                </table>
            <table>
                <tr>
                        <th>
                             <p class='font-size'>Messaggio</p>
                        </th>
    
                        
                        
                </tr>
                      <tr>
                        
                         <td>
                            <p class='font-size'>{messaggio}</p>
                        </td>
                </tr>
            </table>
                 <h1>{nomeProdotto}</h1>
           </div>
        </body>
        </html>
        ";

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