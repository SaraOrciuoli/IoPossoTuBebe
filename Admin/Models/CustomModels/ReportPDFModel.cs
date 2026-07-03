using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.CustomModels
{
    public class ReportPDF
    {
        public double total
        {
            get
            {
                DAL.Repository.StoricoFatturatoRepository repo = new DAL.Repository.StoricoFatturatoRepository();
                return repo.getTotale(ReportFrom, ReportTo);
            }
        }
        public List<InvoiceCategory> InvoiceCategories { get; set; }
        public DateTime ReportFrom { get; set; }
        public DateTime ReportTo { get; set; }

        public double total_expense
        {
            get
            {
                DAL.Repository.StoricoFatturatoRepository repo = new DAL.Repository.StoricoFatturatoRepository();
                return repo.getSpese(ReportFrom, ReportTo);
            }
        }
    }
    public class InvoiceCategory
    {
        public CategorieFattura category { get; set; }
        public List<Invoice> invoices { get; set; }
        public double total_expense
        {
            get
            {
                DAL.Repository.ReportPDFRepository repo = new DAL.Repository.ReportPDFRepository();
                return repo.GetTotalExpensesByCategory(ReportFrom, ReportTo, category.id);
            }
        }
        public DateTime ReportFrom { get; set; }
        public DateTime ReportTo { get; set; }
    }
    public class Invoice
    {
        public string business_name { get; set; }
        public double total { get; set; }
    }
}