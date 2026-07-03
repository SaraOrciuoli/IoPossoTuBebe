using Admin.Classi;
using Admin.Models.CustomModels;
using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Admin.Classi.Configuration;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie reqCookies = Request.Cookies["userRetailCassa"];
            if (reqCookies != null)
            {
                var model = new ModelHome();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult GetOrderStats(int days = 7)
        {
            var stats = new List<object>
    {
        new { date = DateTime.Today.AddDays(-6).Ticks, count = 5 },
        new { date = DateTime.Today.AddDays(-5).Ticks, count = 8 },
        new { date = DateTime.Today.AddDays(-4).Ticks, count = 3 },
        new { date = DateTime.Today.AddDays(-3).Ticks, count = 12 },
        new { date = DateTime.Today.AddDays(-2).Ticks, count = 7 },
        new { date = DateTime.Today.AddDays(-1).Ticks, count = 9 },
        new { date = DateTime.Today.Ticks, count = 6 }
    };

            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrdersByStatus()
        {
            var repo = new OrdiniRepository();
            var ordini = repo.GetOrdini()?.ToList() ?? new List<DettagliOrdine>();

            var statusGroups = ordini
                .GroupBy(o => o.StatoOrdine ?? "Sconosciuto")
                .Select(g => new
                {
                    status = g.Key,
                    count = g.Count()
                })
                .ToList();

            return Json(statusGroups, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrdersByPaymentType()
        {
            var repo = new OrdiniRepository();
            var ordini = repo.GetOrdini()?.ToList() ?? new List<DettagliOrdine>();

            var paymentGroups = ordini
                .GroupBy(o => o.Tipo_Pagamento ?? "Sconosciuto")
                .Select(g => new
                {
                    paymentType = g.Key,
                    count = g.Count()
                })
                .ToList();

            return Json(paymentGroups, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            var model = new LoginModel(); // nuova istanza

            if (TempData["message"] != null && !string.IsNullOrEmpty(TempData["message"].ToString()))
            {
                ViewBag.Message = TempData["message"];
            }
            return View(model);
        }

        public ActionResult Recupera()
        {
            return View();
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
            return RedirectToAction("Login");
        }

        public ActionResult LoginUtente(LoginModel l)
        {
            UtentiRepository rep = new UtentiRepository();
            Utenti u = new Utenti();
            u = rep.login(l.Email, l.Password);
            if (u != null && u.id_Ruolo == 1)
            {
                HttpCookie userInfo = new HttpCookie("userRetailCassa");
                userInfo["IdUtente"] = u.id_utente.ToString();
                userInfo["IdRuolo"] = u.id_Ruolo.ToString();
                userInfo["NomeCompleto"] = u.Nome.ToString() +" "+ u.Cognome.ToString();
                userInfo["email"] = u.Mail.ToString();
                if (l.Rememberme)
                {
                    userInfo.Expires = DateTime.Now.AddYears(1);
                }
                else
                {
                    userInfo.Expires = DateTime.Now.AddHours(2);
                }

                Response.Cookies.Add(userInfo);

                if(u.id_Ruolo == (int)EnumRuoli.Ruoli.Admin)
                {
                    return RedirectToAction("Index","Home");
                }

                
            }
            else
            {
                ViewBag.Message = "Email o Password non riconosciute";
            }
            return View("Login");

        }
        public ActionResult RecuperaPassword(RecuperaModel r)
        {
            UtentiRepository rep = new UtentiRepository();
            Utenti u = rep.getUtenteRecupera(r.Email);

            if (u != null)
            {
                string passwordTemporanea = GenericUtil.RandomPassword(6);
                u.Password = passwordTemporanea;

                // Criptiamo a DB
                rep.UpdateUtente(u, true);

                string NomeCompleto = u.Nome + " " + u.Cognome;

                Admin.Utility.SendEmailAdmin.RecuperaPasswordEmail(NomeCompleto, passwordTemporanea, r.Email);

                TempData["message"] = "Password recuperata correttamente a breve riceverai una mail con la nuova password.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Utente non riconosciuto, riprovare";
            }
            return View("Recupera");
        }
    }
}