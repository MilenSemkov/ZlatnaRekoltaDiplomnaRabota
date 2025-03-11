using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZlatnaRekolta.Data;
using ZlatnaRekolta.Services;

namespace ZlatnaRekolta.Controllers
{
    public class DistributorsController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly FortuneScraper _scraper;

        public DistributorsController(ApplicationDbContext dbContext, FortuneScraper scraper)
        {
            _dbContext = dbContext;
            _scraper = scraper;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _dbContext.Distributors.ToListAsync();
            return View(companies);
        }

        public async Task<IActionResult> Scrape()
        {
            try
            {
                var scraper = new FortuneScraper();
                var companies = await scraper.ScrapeCompaniesAsync();

                foreach (var company in companies)
                {
                    if (!_dbContext.Distributors.Any(c => c.Name == company))
                    {
                        _dbContext.Distributors.Add(new Distributor { Name = company, Description = " " });
                    }
                }

                await _dbContext.SaveChangesAsync();
                TempData["Success"] = "Данните бяха успешно извлечени и записани!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Грешка: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Origins == null)
            {
                return NotFound();
            }
            
            var distributor = await _dbContext.Distributors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Distributors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var distributor = await _dbContext.Distributors.FindAsync(id);
            if (distributor != null)
            {
                _dbContext.Distributors.Remove(distributor);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(int id)
        {
            return (_dbContext.Distributors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}