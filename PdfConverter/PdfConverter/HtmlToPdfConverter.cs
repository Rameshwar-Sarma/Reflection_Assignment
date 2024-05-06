using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;

namespace PdfConverter
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {

        public byte[] ConvertHtmlToPdf(string htmlContent)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12, XFontStyle.Regular);

            XTextFormatter formatter = new XTextFormatter(gfx);

            // Create a PDF layout rectangle
            XRect layoutRectangle = new XRect(10, 10, page.Width - 20, page.Height - 20);

            // Render the HTML content onto the PDF page
            formatter.DrawString(htmlContent, font, XBrushes.Black, layoutRectangle);

            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
