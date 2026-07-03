using DAL.Repository;
using DAL.Model;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vetrina.Models.CustomModel;
using Vetrina.Utility;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Graph;
using System.Data.Entity;
using DevExpress.XtraReports.UI;
using static Vetrina.Classi.Configuration;

namespace Vetrina.Controllers
{
    public class LoginController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Utility.Util.CheckCulture();
            base.Initialize(requestContext);
        }
        public void RedirectToCulture(string culture)
        {
            Response.Cookies.Remove("Language");

            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            if (languageCookie == null) languageCookie = new HttpCookie("Language");

            languageCookie.Value = culture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);

            languageCookie.Expires = DateTime.Now.AddDays(10);

            Response.SetCookie(languageCookie);

            Response.Redirect(Request.UrlReferrer.ToString());
        }

        UtentiRepository rep = new UtentiRepository();
        private readonly AnyeLabelEntities dbContext;
        public LoginController()
        {
            dbContext = new AnyeLabelEntities(); 
        }
        public ActionResult Registrazione(string returnUrl)
        {
            UtentiRepository repo = new UtentiRepository();
            CombinedViewModel model = new CombinedViewModel();
            model.ModelUser = new ModelUser();
            model.ModelUser.Utente = new Utenti();
            Session["ReturnUrl"] = returnUrl;

            return View(model);
        }

        public ActionResult Recupera()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.RecuperaModel = new RecuperaModel();
            return View(model);
        }

        public ActionResult RecuperaPassword(RecuperaModel r)
        {
            UtentiRepository rep = new UtentiRepository();
            Utenti u = new Utenti();
            u = rep.getUtenteRecupera(r.Email);
            if (u != null)
            {
                string passwordTemporanea = GenericUtil.RandomPassword(6);
                u.Password = passwordTemporanea;

                rep.UpdateUtente(u, true);

                string NomeCompleto = u.Nome + " " + u.Cognome;

                Vetrina.Utility.SendMailRecupera.RecuperaPasswordEmail(NomeCompleto, passwordTemporanea, r.Email);

                TempData["message"] = "Password recuperata correttamente a breve riceverai una mail con la nuova password.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Utente non riconosciuto, riprovare";
            }
            return View("Recupera");

        }

        public ActionResult Profilo(int id, int pageSize = 5)
        {
            CombinedViewModel model = new CombinedViewModel();
            UtentiRepository repo = new UtentiRepository();
            model.ModelUser = new ModelUser();
            model.ModelUser.ListaComuniIsole = repo.GetListaComuniIsole();
            model.ModelUser.Utente = new Utenti();
            model.ModelUser.Utente = repo.GetUtenteById(id);
            model.ModelUser.Ordini = repo.GetOrdiniUtente(id);

            return View(model);
        }

        public ActionResult GetOrdiniByUtente(DataSourceLoadOptions loadOptions, string id)
        {
            UtentiRepository repository = new UtentiRepository();
            var ordini = repository.GetOrdiniByUtente(id);

            foreach (var ordine in ordini)
            {
                ordine.DescrizioneStato = repository.GetDescrizioneStato(ordine.id_stato);
                ordine.DescrizioneStatoPagamento = repository.GetDescrizioneStatoPagamento(ordine.id_stato_pagmento);
            }

            var result = DataSourceLoader.Load(ordini, loadOptions);
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }


        [HttpPost]
        public ActionResult AggiornaUtente(CombinedViewModel model)
        {
            UtentiRepository utentiRepository = new UtentiRepository();

            // Recuperiamo l'utente esistente dal DB
            Utenti userOld = utentiRepository.GetUtenteById(model.ModelUser.Utente.id_utente);

            // Aggiorniamo i dati anagrafici
            userOld.Nome = model.ModelUser.Utente.Nome;
            userOld.Cognome = model.ModelUser.Utente.Cognome;
            userOld.Mail = model.ModelUser.Utente.Mail;
            userOld.Cellulare = model.ModelUser.Utente.Cellulare;
            userOld.indirizzo = model.ModelUser.Utente.indirizzo;
            userOld.Citta = model.ModelUser.Utente.Citta;
            userOld.Cap = model.ModelUser.Utente.Cap;
            userOld.Nazione = model.ModelUser.Utente.Nazione;

            bool cifraPassword = false;

            // Se il campo password non è vuoto, significa che l'utente vuole cambiarla
            if (!string.IsNullOrEmpty(model.ModelUser.Utente.Password))
            {
                userOld.Password = model.ModelUser.Utente.Password;
                cifraPassword = true; // Alziamo il flag per dire al Repository di criptarla!
            }

            // Richiamiamo il metodo del repository passando l'oggetto e il flag
            utentiRepository.UpdateUtente(userOld, cifraPassword);

            return RedirectToAction("Profilo", new { id = model.ModelUser.Utente.id_utente });
        }

        public ActionResult Logout()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvaUtente(CombinedViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.ModelUser.Utente.Nome) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.Cognome) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.Mail) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.Cellulare) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.indirizzo) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.Citta) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.Cap) ||
                    string.IsNullOrEmpty(model.ModelUser.Utente.Password))
                {
                    TempData["ErrCampiObbligatori"] = "Tutti i campi obbligatori devono essere compilati.";
                    return View("Registrazione", model);
                }

                if (!IsValidEmail(model.ModelUser.Utente.Mail))
                {
                    TempData["ErrEmailNonValida"] = "L'indirizzo email non è valido.";
                    return View("Registrazione", model);
                }

                if (!CheckEmailEsistente(model.ModelUser.Utente.Mail))
                {
                    try
                    {
                        model.ModelUser.Utente.Password = BCrypt.Net.BCrypt.HashPassword(model.ModelUser.Utente.Password);

                        model.ModelUser.Utente.id_Ruolo = 2; // Utente Standard
                        rep.SalvaUtente(model.ModelUser.Utente);

                        int userId = model.ModelUser.Utente.id_utente;
                        if (userId > 0)
                        {
                            string userNome = model.ModelUser.Utente.Nome;
                            string userCognome = model.ModelUser.Utente.Cognome;

                            HttpCookie userInfo = new HttpCookie("ForrestUser");
                            userInfo["IdUtente"] = userId.ToString();
                            userInfo["Nome"] = userNome;
                            userInfo["Cognome"] = userCognome;
                            HttpContext.Response.Cookies.Add(userInfo);

                            SendEmailRegistrazione.EmailRegistrazione(model.ModelUser.Utente.Nome, model.ModelUser.Utente.Cognome, model.ModelUser.Utente.Mail, model.ModelUser.Utente.Cellulare, model.ModelUser.Utente.indirizzo, model.ModelUser.Utente.Citta, model.ModelUser.Utente.Cap);

                            TempData["SuccessMessage"] = App_GlobalResources.labels.Registrazione_avvenuta;

                            if (!string.IsNullOrEmpty((string)Session["ReturnUrl"]))
                            {
                                return Redirect((string)Session["ReturnUrl"]);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            TempData["ErrAge"] = "Errore nel salvataggio dell'utente.";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrAge"] = "Si è verificato un errore durante il salvataggio dell'utente.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["EmailInUso"] = "La mail scelta risulta già registrata";
                    return RedirectToAction("Registrazione", "Login");
                }
            }
            else
            {
                return View("Registrazione", model);
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.Contains("@");
            }
            catch
            {
                return false;
            }
        }

        private bool CheckEmailEsistente(string email)
        {
            return rep.CheckEmailEsistente(email);
        }

        public ActionResult Login(string returnUrl)
        {
            if (Session["ErrLog"] != null)
            {
                Session["ErrLog"] = null;
            }
            Session["ReturnUrl"] = returnUrl;

            return View();
        }

        public ActionResult LoginUtente(LoginModel l, string returnUrl)
        {
            Session["ErrLog"] = null;
            UtentiRepository rep = new UtentiRepository();
            Utenti u = new Utenti();
            u = rep.login(l.Email, l.Password);
            if (u != null)
            {
                HttpCookie userInfo = new HttpCookie("ForrestUser");
                userInfo["IdUtente"] = u.id_utente.ToString();
                if (u.id_Ruolo != null && u.id_Ruolo > 0)
                {
                    userInfo["IdRuolo"] = u.id_Ruolo.ToString();
                }
                userInfo["Nome"] = u.Nome.ToString();
                userInfo["Cognome"] = u.Cognome.ToString();
                userInfo["Email"] = u.Mail.ToString();

                if (l.Rememberme)
                {
                    userInfo.Expires = DateTime.Now.AddYears(1);
                }
                else
                {
                    userInfo.Expires = DateTime.Now.AddHours(2);
                }

                Response.Cookies.Add(userInfo);
                if (!string.IsNullOrEmpty((string)Session["ReturnUrl"]))
                {
                    return Redirect((string)Session["ReturnUrl"]);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["EmailNo"] = App_GlobalResources.labels.email_pass_no;

                Session["ErrLog"] = App_GlobalResources.labels.email_pass_no;
            }
            return View("Login");
        }



    }
}
