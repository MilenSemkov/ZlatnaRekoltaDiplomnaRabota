using HtmlAgilityPack;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ZlatnaRekolta.Data;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace ZlatnaRekolta.Services
{
    public class FortuneScraper
    {
        public async Task<List<string>> ScrapeCompaniesAsync()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); 
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            using (var driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl("https://fortune.com/ranking/global500/");
                await Task.Delay(5000);

                var companyElements = driver.FindElements(By.CssSelector("a.row-link.hidden-text"));
                if (companyElements.Count == 0)
                    throw new Exception("Не бяха намерени компании!");

                var companyNames = companyElements
    .Select(e => e.Text.Trim())
    .Where(name => !string.IsNullOrEmpty(name))
    .Select(name => string.Join(" ", name.Split(" ").Skip(4))) 
    .Distinct()
    .ToList();

                driver.Quit();

                return companyNames;
            }
        }
    }
}
