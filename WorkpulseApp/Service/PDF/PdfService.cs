using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CORTNE.Service.PDF
{
    public class PdfService
    {
    }

    public interface IStampService
    {
        Stream ApplyStamp(StampRequestForm stampRequest);
    }

    public class StampService : IStampService
    {
        private const int FontSize = 7;
        private readonly BaseFont Font = BaseFont.CreateFont();
        private readonly BaseColor Color = BaseColor.Red;
        private const int VerticalSpaceBetweenLines = 15;

        private IHostingEnvironment _env;

        public StampService(IHostingEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Stamp a PDF with the specific message and position as specified in the <see cref="StampRequest"/>
        /// https://stackoverflow.com/questions/2372041/c-sharp-itextsharp-pdf-creation-with-watermark-on-each-page#
        /// </summary>
        /// <param name="stampRequest"></param>
        /// <returns></returns>
        public Stream ApplyStamp(StampRequestForm stampRequest)
        {
            MemoryStream pdfOutStream;
            Stream pdfInstream = null;
            PdfReader reader = null;
            PdfStamper stamper = null;
            try
            {
                pdfOutStream = new MemoryStream();
                pdfInstream = stampRequest.Pdf.OpenReadStream();
                reader = new PdfReader(pdfInstream);
                stamper = new PdfStamper(reader, pdfOutStream);
                var dc = stamper.GetOverContent(1);
                AddWaterMark(dc, reader, stampRequest);
                return pdfOutStream;
            }
            finally
            {
                if (pdfInstream != null)
                {
                    pdfInstream.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (stamper != null)
                {
                    stamper.Close();
                }
            }
        }

        private void AddWaterMark(PdfContentByte dc, PdfReader reader, StampRequestForm stampRequest)
        {
            var gstate = new PdfGState
            {
                FillOpacity = 0.61f,
                StrokeOpacity = 0.61f
            };
            dc.SaveState();
            dc.SetGState(gstate);
            dc.SetColorFill(Color);
            dc.BeginText();
            dc.SetFontAndSize(Font, FontSize);
            var x = (stampRequest.LowerLeftX + stampRequest.UpperRightX) / 2;
            var y = ((stampRequest.LowerLeftY + stampRequest.UpperRightY) / 2);
            var lines = new string[] {stampRequest.Line1, stampRequest.Line2,
                                      stampRequest.Line3}
            .Where(line => !string.IsNullOrEmpty(line));
            foreach (var line in lines)
            {
                dc.ShowTextAligned(Element.ALIGN_CENTER, line, x, y, stampRequest.RotationDegree);
                y -= VerticalSpaceBetweenLines;
            }
            dc.EndText();
            dc.RestoreState();
        }

        private Document CreatePdf(StampRequestForm stampRequest)
        {
           var pdfInstream = stampRequest.Pdf.OpenReadStream();

            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfInstream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk("Your Credit Card Statement Report has been Generated", FontFactory.GetFont("Arial", 20, 1, BaseColor.Magenta));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            //0=Left, 1=Centre, 2=Right
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell no 1
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            var preImage = System.Drawing.Image.FromFile("imagePath");
            var image = Image.GetInstance(preImage, ImageFormat.Png);
            //var image = Image.GetInstance("blobUri");

            image.ScaleAbsolute(200, 150);
            cell.AddElement(image);
            table.AddCell(cell);

            //Cell no 2
            chunk = new Chunk("Name: Mrs. Salma Mukherji,\nAddress: Latham Village, Latham, New York, US, \nOccupation: Nurse, \nAge: 35 years", FontFactory.GetFont("Arial", 15,2, BaseColor.Pink));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            //Add table to document
            pdfDoc.Add(table);

            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell
            cell = new PdfPCell();
            chunk = new Chunk("This Month's Transactions of your Credit Card");
            cell.AddElement(chunk);
            cell.Colspan = 5;
            cell.BackgroundColor = BaseColor.Pink;
            table.AddCell(cell);

            table.AddCell("S.No");
            table.AddCell("NYC Junction");
            table.AddCell("Item");
            table.AddCell("Cost");
            table.AddCell("Date");

            table.AddCell("1");
            table.AddCell("David Food Store");
            table.AddCell("Fruits & Vegetables");
            table.AddCell("$100.00");
            table.AddCell("June 1");

            table.AddCell("2");
            table.AddCell("Child Store");
            table.AddCell("Diaper Pack");
            table.AddCell("$6.00");
            table.AddCell("June 9");

            table.AddCell("3");
            table.AddCell("Punjabi Restaurant");
            table.AddCell("Dinner");
            table.AddCell("$29.00");
            table.AddCell("June 15");

            table.AddCell("4");
            table.AddCell("Wallmart Albany");
            table.AddCell("Grocery");
            table.AddCell("$299.50");
            table.AddCell("June 25");

            table.AddCell("5");
            table.AddCell("Singh Drugs");
            table.AddCell("Back Pain Tablets");
            table.AddCell("$14.99");
            table.AddCell("June 28");

            pdfDoc.Add(table);

            Paragraph para = new Paragraph();
            para.Add("Hello Salma,\n\nThank you for being our valuable customer. We hope our letter finds you in the best of health and wealth.\n\nYours Sincerely, \nBank of America");
            pdfDoc.Add(para);

            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            para = new Paragraph();
            para.Add("This PDF is generated using iTextSharp. You can read the turorial:");
            para.SpacingBefore = 20f;
            para.SpacingAfter = 20f;
            pdfDoc.Add(para);

            //Creating link
            chunk = new Chunk("How to Create a Pdf File");
            chunk.Font = FontFactory.GetFont("Arial", 25, 1, BaseColor.Red);
            chunk.SetAnchor("https://www.yogihosting.com/create-pdf-asp-net-mvc/");
            pdfDoc.Add(chunk);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();

            return pdfDoc;

            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=Credit-Card-Report.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Write(pdfDoc);
            //Response.End();

        }
    }
    public class StampRequestForm
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public float LowerLeftX { get; set; }
        public float LowerLeftY { get; set; }
        public float UpperRightX { get; set; }
        public float UpperRightY { get; set; }
        // ASP.NET core has support to bind a multipart/form-data request 
        // to a model via model binding using the IFormFile interface.
        [Required]
        public IFormFile Pdf { get; set; }
        public int RotationDegree { get; set; }

    }
}
