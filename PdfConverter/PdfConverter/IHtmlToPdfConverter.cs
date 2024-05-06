
namespace PdfConverter
{
    public interface IHtmlToPdfConverter
    {
        byte[] ConvertHtmlToPdf(string htmlContent);
    }
}
