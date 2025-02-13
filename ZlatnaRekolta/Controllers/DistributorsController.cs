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
    public class DistributorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistributorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Distributors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Distributors.ToListAsync());
        }

        // GET: Distributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // GET: Distributors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distributors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distributor);
        }

        // GET: Distributors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return View(distributor);
        }

        // POST: Distributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Distributor distributor)
        {
            if (id != distributor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distributor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributorExists(distributor.Id))
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
            return View(distributor);
        }

        // GET: Distributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // POST: Distributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor != null)
            {
                _context.Distributors.Remove(distributor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(int id)
        {
            return _context.Distributors.Any(e => e.Id == id);
        }
    }
}
