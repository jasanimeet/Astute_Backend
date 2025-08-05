using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.IO;
using System.Threading.Tasks;

namespace astute.CoreServices
{
    public class PuppeteerSharpExport
    {
        public static async Task<string> CreatePdfWithPuppet(string htmlContent, string _strFolderPath, string _strFilePath)
        {
            try
            {
                if (!Directory.Exists(_strFolderPath))
                {
                    Directory.CreateDirectory(_strFolderPath);
                }

                // Output PDF file path
                string outputPdfPath = Path.Combine(_strFilePath);

                await new BrowserFetcher().DownloadAsync();

                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                };

                using var browser = await Puppeteer.LaunchAsync(launchOptions);
                using var page = await browser.NewPageAsync();

                await page.SetContentAsync(htmlContent, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } });

                await page.PdfAsync(outputPdfPath, new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true
                });

                //var pdfStream = await page.PdfStreamAsync(outputPdfPath,new PdfOptions
                //{
                //    Format = PaperFormat.A4,
                //    PrintBackground = true
                //});

                await browser.CloseAsync();
                // Optionally, return the file as download
                //byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(outputPdfPath);
            }
            catch (System.Exception)
            {
                throw;
            }
            return _strFilePath;
        }
    }
}