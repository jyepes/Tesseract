using System;
using Tesseract;

namespace TesseractOCR
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string text;
            string pdfFile = "C:\\samples\\Tesseract\\TesseractOCR\\sample.pdf";

            var pageSize = ConvertPdf.PdfSize(pdfFile);

            bool isSearchable = PdfManager.IsSearchable(pdfFile);
            if (isSearchable)
            {
                text = PdfManager.ExtractTextFromRectangle(pdfFile, 1, 0, 0, pageSize.Width, pageSize.Height);
            }
            else
            {
                var pdfBytes = PdfManager.CreatePdfFromArea(pdfFile, 1, 0, 0, pageSize.Width, pageSize.Height);
                var imgBytes = ConvertPdf.ConvertPdfToImageBytes(pdfBytes);
                text = GetTextFromImage(imgBytes,"spa");
            }

            Console.WriteLine(text);
            Console.ReadLine();
        }



        static string GetTextFromImage(byte[] imgBytes, string language = "eng")
        {
            // Obtener el texto de la imagen mediante Tesseract-OCR
            using (var engine = new TesseractEngine(@"./tessdata", language, EngineMode.Default))
            using (var img = Pix.LoadTiffFromMemory(imgBytes))
            {
                using (var page = engine.Process(img))
                {
                    return page.GetText();
                }
            }
        }

    }
}
