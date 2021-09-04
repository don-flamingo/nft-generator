using System.Threading;
using System.Threading.Tasks;
using NftGenerator.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace NftGenerator.OpenSea
{
    public class WebScrapperUploader : IUploader
    {
        private readonly WebScrapperConfiguration _webScrapperConfiguration;
        private readonly IWebDriver _webDriver;

        public WebScrapperUploader(WebScrapperConfiguration webScrapperConfiguration)
        {
            _webScrapperConfiguration = webScrapperConfiguration;
            var options = new FirefoxOptions
            {
                BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe"
            };

            _webDriver = new FirefoxDriver(options);
            _webDriver.Navigate().GoToUrl(webScrapperConfiguration.CollectionAddress);
        }

        public async Task Upload(Nft nft, CancellationToken cancellationToken)
        {

        }
    }
}
