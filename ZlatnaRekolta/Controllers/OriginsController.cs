using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZlatnaRekolta.Data;
using ZlatnaRekolta.Services;

namespace ZlatnaRekolta.Controllers
{
    public class OriginsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly WikipediaScrapper _scraper;

        public OriginsController(ApplicationDbContext dbContext, WikipediaScrapper scraper)
        {
            _dbContext = dbContext;
            _scraper = scraper;
        }

        public async Task<IActionResult> Index()
        {
            var origin = await _dbContext.Origins.ToListAsync();
            return View(origin);
        }

        public async Task<IActionResult> Scrape()
        {
            try
            {
                await _scraper.ScrapeAndSaveOriginsAsync();
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

            var origin = await _dbContext.Origins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (origin == null)
            {
                return NotFound();
            }

            return View(origin);
        }

        // POST: Origin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Origins == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Origin'  is null.");
            }
            var origin = await _dbContext.Distributors.FindAsync(id);
            if (origin != null)
            {
                _dbContext.Distributors.Remove(origin);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OriginExists(int id)
        {
            return (_dbContext.Distributors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}