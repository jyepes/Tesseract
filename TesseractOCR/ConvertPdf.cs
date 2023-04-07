using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TesseractOCR
{
    internal class ConvertPdf
    {
        public static string ConvertPdfToImage(string pdfFilePath)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfFilePath);
            Image bmp = doc.SaveAsImage(0);

            bmp.Save("convertToTiff.tiff", ImageFormat.Tiff);

            return "convertToTiff.tiff";
        }

        public static byte[] ConvertPdfToImageBytes(byte[] pdfBytes)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromBytes(pdfBytes);
            Image bmp = doc.SaveAsImage(0);

            using (var stream = new System.IO.MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Tiff);
                return stream.ToArray();
            }
        }

        public static SizeF PdfSize(string fileName)
        {
            PdfDocument doc = new PdfDocument(fileName);
            PdfPageBase page = doc.Pages[0];
            return page.Size;
        }


    }
}
