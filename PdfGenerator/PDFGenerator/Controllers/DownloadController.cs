using Microsoft.AspNetCore.Mvc;
using PdfConverter;
using Razor.Templating.Core;

namespace PDFGenerator.Controllers
{
    /// <summary>
    /// Controller for handling PDF downloads.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DownloadController"/> class.
    /// </remarks>
    /// <param name="pdfConverter">The PDF converter service to be injected.</param>
    public class DownloadController(IHtmlToPdfConverter pdfConverter) : Controller
    {
        private readonly IHtmlToPdfConverter _pdfConverter = pdfConverter;

        /// <summary>
        /// Renders the index view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Converts an HTML partial view to a PDF document and returns it as a file download.
        /// </summary>
        /// <returns>A file download containing the generated PDF.</returns>
        [HttpGet]
        public async Task<IActionResult> DownloadPdfAsync()
        {

            // Specify the partial view path
            string partialViewPath = "Views/Download/ExamplePartialView.cshtml";

            string htmlContent =await RazorTemplateEngine.RenderAsync(partialViewPath);

            // Convert the partial view to PDF using the PDF converter service
            byte[] pdfBytes = await _pdfConverter.ConvertHtmlToPdf(htmlContent);

            // Return the generated PDF as a file download
            return File(pdfBytes, "application/pdf", "download.pdf");
        }
    }
}
