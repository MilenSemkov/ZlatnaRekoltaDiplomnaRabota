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
            options.AddArgument("--headless"); // Без графичен интерфейс
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            using (var driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl("https://fortune.com/ranking/global500/");
                await Task.Delay(5000); // Изчакайте 5 секунди за зареждане

                var companyElements = driver.FindElements(By.CssSelector("a.row-link.hidden-text"));
                if (companyElements.Count == 0)
                    throw new Exception("Не бяха намерени компании!");

                var companyNames = companyElements
    .Select(e => e.Text.Trim())
    .Where(name => !string.IsNullOrEmpty(name))
    .Select(name => string.Join(" ", name.Split(" ").Skip(4))) // Премахва първите 4 думи
    .Distinct()
    .ToList();

                driver.Quit(); // Затваря браузъра

                return companyNames;
            }
        }
    }
}
