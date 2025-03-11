using HtmlAgilityPack;
using System.Diagnostics.Metrics;
using ZlatnaRekolta.Data;

namespace ZlatnaRekolta.Services
{
    public class WikipediaScrapper
    {
        private readonly ApplicationDbContext _dbContext;

        public WikipediaScrapper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ScrapeAndSaveOriginsAsync()
        {
            string url = "https://bg.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D1%8A%D0%BA_%D0%BD%D0%B0_%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B8%D1%82%D0%B5";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            var countryNodes = doc.DocumentNode.SelectNodes("//table[contains(@class, 'wikitable')]//tr/td[1]/a");
            if (countryNodes == null)
                throw new Exception("Не бяха намерени държави!");

            var countryNames = countryNodes
                .Select(node => node.InnerText.Trim())
                .Distinct()
                .ToList();

            foreach (var name in countryNames)
            {
                if (!_dbContext.Origins.Any(c => c.Name == name))
                {
                    _dbContext.Origins.Add(new Origin { Name = name ,Description=" "});
                    await _dbContext.SaveChangesAsync();
                }
            }

            
        }
    }
}
