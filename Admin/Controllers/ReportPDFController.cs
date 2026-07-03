using Admin.Models.CustomModels;
using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

using System.Text;
using DevExpress.Web.ASPxScheduler;
using System.IO;
using SelectPdf;


namespace Admin.Controllers
{
    public class ReportPDFController : Controller
    {

        public FileStreamResult GeneratePDF()
        {
            HttpCookie data_da = Request.Cookies["data_da"];
            HttpCookie data_a = Request.Cookies["data_a"];
            CultureInfo ita = new CultureInfo("it-IT");
            DateTime da = DateTime.Parse(data_da.Value.ToString(), ita);
            DateTime a = DateTime.Parse(data_a.Value.ToString(), ita);
            ReportPDF model = new ReportPDF();

            ConfigurazioniRepository conf_repo = new ConfigurazioniRepository();
            ReportPDFRepository pdf_repo = new ReportPDFRepository();
            List<InvoiceCategory> list_invoice_cat = new List<InvoiceCategory>();

            foreach (CategorieFattura cat in conf_repo.GetCategorieFatture())
            {
                List<Invoice> list_invoice = new List<Invoice>();
                List<object> fatture = pdf_repo.GetAllInvoiceByCategory(da, a, cat.id);
                if (fatture.Count >= 1)
                {
                    foreach (var inv in fatture)
                    {
                        Invoice invoice = new Invoice();
                        invoice.business_name = (string)inv.GetType().GetProperty("RagioneSociale").GetValue(inv);
                        invoice.total = Math.Round((double)inv.GetType().GetProperty("Totale").GetValue(inv), 2);
                        list_invoice.Add(invoice);
                    }
                    InvoiceCategory invoiceCategory = new InvoiceCategory();
                    invoiceCategory.category = cat;
                    invoiceCategory.ReportFrom = da;
                    invoiceCategory.ReportTo = a;
                    invoiceCategory.invoices = list_invoice;
                    list_invoice_cat.Add(invoiceCategory);
                }
                else
                {
                    continue;
                }
            }

            model.ReportFrom = da;
            model.ReportTo = a;
            model.InvoiceCategories = list_invoice_cat;
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.WebPageWidth = 850;
            string html = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Resources/GeneratePDF.html"), Encoding.UTF8);

            string table = "";
            foreach (InvoiceCategory el in list_invoice_cat)
            {
                table += "<div class='categoria'>" + el.category.Descrizione + "</div>";

                foreach (Invoice inv in el.invoices)
                {
                    table += "<div class='riga azienda'><div class='azienda'>" + inv.business_name + "</div><div class='totale_azienda'>" + inv.total + " €</div></div>";
                }
            }
            html = html
                .Replace("[total]", Math.Round(model.total, 2).ToString())
                .Replace("[ReportFrom.Day]", da.Day.ToString())
                .Replace("[ReportFrom.Month]", da.Month.ToString())
                .Replace("[ReportFrom.Year]", da.Year.ToString())
                .Replace("[ReportTo.Day]", a.Day.ToString())
                .Replace("[ReportTo.Month]", a.Month.ToString())
                .Replace("[ReportTo.Year]", a.Year.ToString())
                .Replace("[Profitti]", Math.Round((model.total - model.total_expense), 2).ToString())
                .Replace("[Logo]", "~/Content/Img/logo-grande.png")
                .Replace("[Tabella]", table);

            PdfDocument doc = converter.ConvertHtmlString(html);
            string fileName = "Report_" + da.Day.ToString() + "/" + da.Month.ToString() + "/" + da.Year.ToString() + "_" + a.Day.ToString() + "/" + a.Month.ToString() + "/" + a.Year.ToString() + ".pdf";

            MemoryStream stream = new MemoryStream();

            doc.Save(stream);
            stream.Position = 0;

            doc.Close();

            //Download the PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = fileName;

            return File(stream, "application/pdf", fileName);
        }
    }
}