using AegisImplicitMail;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Vetrina.Classi
{
    public class EmailUtility
    {

        public static string error { get; set; }
        
        private static string getHtmlMail(string nomeUtente,string password)
        {
            string htmlString = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Resources/MailLayout.html"), Encoding.UTF8);

            htmlString = htmlString.Replace("[[NomeCognome]]", nomeUtente);
            htmlString = htmlString.Replace("[[NEWPASS]]", password);
            return htmlString;
        }

        public static void SendMail_Utente(string nomeUtente, string password,string email)
        {
            string htmlString = "";
            string Subject = "";
            htmlString = getHtmlMail(nomeUtente, password);
            Subject = "Recupero password";
            var mail = new MimeMailMessage();


            mail.From = new MailAddress(ConfigurationManager.AppSettings["MailUser"]);
            mail.To.Add(email);
            mail.Subject = Subject;
            mail.Body = htmlString;
            mail.IsBodyHtml = true;


            // mail.Attachments.Add(new Attachment(path));
            var SmtpServer = new MimeMailer();
            SmtpServer.Host = ConfigurationManager.AppSettings["SmtpServer"];
            SmtpServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["MailPort"]);
            SmtpServer.AuthenticationMode = AuthenticationType.Base64;
            SmtpServer.SslType = SslMode.Ssl;
            SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailUser"], ConfigurationManager.AppSettings["MailPassword"]);
            SmtpServer.EnableImplicitSsl = true;
            //Set a delegate function for call back
            SmtpServer.SendCompleted += compEvent;
          
            try
            {
                SmtpServer.SendMail(mail);
            }
            catch (Exception ex)
            {

                error = "Si è presentato un errore nell'invio dell'Email. Riprova più tardi";
            }




        }

        //Call back function
        private static void compEvent(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState != null)
                Console.Out.WriteLine(e.UserState.ToString());

            Console.Out.WriteLine("is it canceled? " + e.Cancelled);

            if (e.Error != null)
                Console.Out.WriteLine("Error : " + e.Error.Message);
        }

        internal static void SendMail_Utente(object nomeCompleto, string password, string mail)
        {
            throw new NotImplementedException();
        }
    }
}
