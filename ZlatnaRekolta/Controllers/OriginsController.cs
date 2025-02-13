using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZlatnaRekolta.Data;

namespace ZlatnaRekolta.Controllers
{
    public class OriginsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OriginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Origins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Origins.ToListAsync());
        }

        // GET: Origins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var origin = await _context.Origins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (origin == null)
            {
                return NotFound();
            }

            return View(origin);
        }

        // GET: Origins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Origins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Origin origin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(origin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(origin);
        }

        // GET: Origins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var origin = await _context.Origins.FindAsync(id);
            if (origin == null)
            {
                return NotFound();
            }
            return View(origin);
        }

        // POST: Origins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Origin origin)
        {
            if (id != origin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(origin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OriginExists(origin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(origin);
        }

        // GET: Origins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var origin = await _context.Origins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (origin == null)
            {
                return NotFound();
            }

            return View(origin);
        }

        // POST: Origins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var origin = await _context.Origins.FindAsync(id);
            if (origin != null)
            {
                _context.Origins.Remove(origin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OriginExists(int id)
        {
            return _context.Origins.Any(e => e.Id == id);
        }
    }
}
