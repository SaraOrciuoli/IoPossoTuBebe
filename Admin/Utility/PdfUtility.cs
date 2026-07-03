using SelectPdf;
using System;
using System.IO;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Admin.Utility
{
    public class PdfUtility
    {

        public FileStreamResult PdfSingoloBarcode(string nomeProdotto, string descrizioneTaglia, string barcode, string barcodeUrl)
        {
            string htmlString = $@"
<html>
<head>
   <style>
        body {{

            margin-top: 30px;
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
        }}
        table {{
            width: 100%;
            height: 100%;
            border-collapse: collapse;
        }}
        td {{
            vertical-align: middle;
            text-align: center;
        }}
        h3 {{
            font-size: 44px; /* Dimensione del font per il nome del prodotto */
            margin: 0;
        }}
       
        img {{
            width: 80%; /* L'immagine occuperà l'intera larghezza della cella */
            height: auto; /* Mantiene le proporzioni */
            max-height: 100%; /* Limita l'altezza massima dell'immagine */
        }}
    </style>
</head>
<body>
    <table>
        <tr>
            <td>
<br>
<br>
<br>
<br>
<br>
                <h3>{HttpUtility.HtmlEncode(nomeProdotto)} - {HttpUtility.HtmlEncode(descrizioneTaglia)}</h3>
<br>
<br>
                <img src='{HttpUtility.HtmlEncode(barcodeUrl)}' alt='Barcode' />
<br>
<br>
<br>
            </td>
        </tr>
    </table>
</body>
</html>";


            HtmlToPdf converter = new HtmlToPdf
            {
                Options =
        {
            PdfPageSize = PdfPageSize.Custom,
            PdfPageCustomSize = new System.Drawing.SizeF(189, 113),
            PdfPageOrientation = PdfPageOrientation.Landscape
        }
            };

            PdfDocument doc = converter.ConvertHtmlString(htmlString);

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            stream.Position = 0;

            // Chiudi il documento PDF
            doc.Close();

            // Crea il nome del file
            string fileName = $"Barcode_{DateTime.Now:yyyyMMddHHmmssffff}_{HttpUtility.HtmlEncode(nomeProdotto.Replace(" ",""))}_{HttpUtility.HtmlEncode(descrizioneTaglia)}.pdf";

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = fileName
            };
        }

        public FileStreamResult PdfComplete(string nomeProdotto, string htmlString)
        {


            HtmlToPdf converter = new HtmlToPdf
            {
                Options =
                {
                    PdfPageSize = PdfPageSize.Custom,
                    PdfPageCustomSize = new System.Drawing.SizeF(189, 113), 
                    PdfPageOrientation = PdfPageOrientation.Landscape
                }
            };

            PdfDocument doc = converter.ConvertHtmlString(htmlString);

            // Salva il documento PDF in un MemoryStream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            stream.Position = 0;

            // Chiudi il documento PDF
            doc.Close();

            // Crea il nome del file
            string fileName = $"Barcode_{HttpUtility.HtmlEncode(nomeProdotto.Replace(" ", ""))}.pdf";

            // Restituisci il PDF come FileStreamResult
            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = fileName
            };
        }
    }
}
