using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;

namespace TesseractOCR
{
    internal class PdfManager
    {
        public static bool IsSearchable(string path) 
        {
            bool isSearchable;

            using (PdfReader reader = new PdfReader(path))
            {
                isSearchable = reader.IsTagged();
            }

            return isSearchable;
        }

        public static string ExtractTextFromRectangle(string fileName, int pageNumber, 
            float x, float y, float width, float height)
        {
            string text;
            using (PdfReader reader = new PdfReader(fileName))
            {
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(x, y, x + width, y + height);

                RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
                ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);

                text = PdfTextExtractor.GetTextFromPage(reader, pageNumber, strategy);
            }

            return text;
        }

        public static string CreatePdfFromRectangle(string fileName, int pageNumber,
            float x, float y, float width, float height)
        {

            // Load the original PDF document
            PdfReader reader = new PdfReader(fileName);

            // Select page number and coordinates for the area to be copied
            float leftX = x;
            float bottomY = y;

            int rotation = reader.GetPageRotation(1);

            // Create a new PDF document and add a new page
            using (Document document = new Document(new iTextSharp.text.Rectangle(width, height, 45)))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("new_document.pdf", FileMode.Create));
                document.Open();

                // Import the selected area of the original page into the new page
                PdfImportedPage importedPage = writer.GetImportedPage(reader, pageNumber);
                PdfContentByte contentByte = writer.DirectContent;
                contentByte.AddTemplate(importedPage, -leftX, -bottomY);

                document.Close();
            }
            return "new_document.pdf";
        }

        public static byte[] CreatePdfFromArea(string fileName, int pageNumber,
            float x, float y, float width, float height)
        {

            // Load the original PDF document
            PdfReader reader = new PdfReader(fileName);

            // Select page number and coordinates for the area to be copied
            float leftX = x;
            float bottomY = y;

            // Create a new PDF document and add a new page
            using (MemoryStream stream = new MemoryStream())
            {
                using (Document document = new Document(new iTextSharp.text.Rectangle(width, height)))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Import the selected area of the original page into the new page
                    PdfImportedPage importedPage = writer.GetImportedPage(reader, pageNumber);
                    PdfContentByte contentByte = writer.DirectContent;
                    contentByte.AddTemplate(importedPage, -leftX, -bottomY);

                    document.Close();
                }

                // Return the resulting byte array
                return stream.ToArray();
            }
        }

        public static int GetRotation(string fileName)
        {
            // Cargar el archivo PDF
            PdfReader reader = new PdfReader(fileName);

            // Obtener la rotación de la página especificada (en este ejemplo, la página 1)
            int rotation = reader.GetPageRotation(1);

            // Determinar la orientación de la página en función de la rotación
            if (rotation == 0 || rotation == 180)
            {
                Console.WriteLine("La página está en orientación horizontal.");
            }
            else
            {
                Console.WriteLine("La página está en orientación vertical.");
            }

            // Cerrar el archivo PDF
            reader.Close();
            return rotation;
        }
    }
}

