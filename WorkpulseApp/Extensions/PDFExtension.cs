using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WorkpulseApp.Extensions
{
    public class PDFExtension
    {
        private IHostingEnvironment _env;
        public PDFExtension(IHostingEnvironment environment)
        {
            _env = environment;
        }
        private static void MergePDF(string File1, string File2)
        {
            string[] fileArray = new string[3];
            fileArray[0] = File1;
            fileArray[1] = File2;

            PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = @"E:/newFile.pdf";

            sourceDocument = new iTextSharp.text.Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length - 1; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        public void InsertImage()
        {
            // create filestream object
            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "test.pdf");
            FileStream fs = new FileStream(file, FileMode.Create);
            // create document object
            Document doc = new Document();
            // create PdfWriter instance which will write at file filestream
            PdfWriter.GetInstance(doc, fs);
            // opening the dociment
            doc.Open();
            // creating paragraph object
            Paragraph para = new Paragraph("Insert an image into pdf using C#.");
            para.Alignment = Element.ALIGN_CENTER;
            // adding pargraph to document
            doc.Add(para);

            // setting image path
            var imagePath = System.IO.Path.Combine(webRoot+"\\Images", "demo.png");
            // string imagePath = Server.MapPath("Images\\demo.PNG") + "";
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
            image.Alignment = Element.ALIGN_CENTER;
            // set width and height
            image.ScaleToFit(180f, 250f);

            // adding image to document
            doc.Add(image);
            // closing the document
            doc.Close();
        }
    }
}
