using SelectPdf;
using System;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;


namespace Admin.Utility
{
    public class PngUtility
    {
        public FileStreamResult PngComplete(string nomeProdotto, string htmlString)
        {
            HtmlToImage converter = new HtmlToImage();

            // Converti l'HTML in immagine
            Image img = converter.ConvertHtmlString(htmlString);

            // Ridimensiona l'immagine a 300x300 px
            using (Bitmap resizedImg = new Bitmap(img, new Size(300, 300)))
            {
                // Salva l'immagine ridimensionata in un MemoryStream
                MemoryStream stream = new MemoryStream();
                resizedImg.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;

                // Crea il nome del file
                string fileName = $"Barcode_{HttpUtility.HtmlEncode(nomeProdotto.Replace(" ", ""))}.png";

                return new FileStreamResult(stream, "image/png")
                {
                    FileDownloadName = fileName
                };
            }
        }
    }
}
