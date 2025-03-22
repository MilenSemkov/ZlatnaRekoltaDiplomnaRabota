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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string? category)
        {
            var allData = _context.Products.Include(p => p.Categories).
                Include(p => p.Distributors).Include(p => p.Origins);
            if (category != null)
            {
                var applicationDbContext = allData.Where(c => c.Categories.Name == category);
                return View(await applicationDbContext.ToListAsync());


            }
            return View(await allData.ToListAsync());


        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Distributors)
                .Include(p => p.Origins)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["DistributorID"] = new SelectList(_context.Distributors, "Id", "Name");
            ViewData["OriginID"] = new SelectList(_context.Origins, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CategoryID,OriginID,DistributorID,Description,URLimage,Price,Quantity,UnitOfMe")] Product product)
        {
            product.DateRegister = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryID);
            ViewData["DistributorID"] = new SelectList(_context.Distributors, "Id", "Name", product.DistributorID);
            ViewData["OriginID"] = new SelectList(_context.Origins, "Id", "Name", product.OriginID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryID);
            ViewData["DistributorID"] = new SelectList(_context.Distributors, "Id", "Name", product.DistributorID);
            ViewData["OriginID"] = new SelectList(_context.Origins, "Id", "Name", product.OriginID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryID,OriginID,DistributorID,Description,URLimage,Price,Quantity,UnitOfMe")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            product.DateRegister = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryID);
            ViewData["DistributorID"] = new SelectList(_context.Distributors, "Id", "Name", product.DistributorID);
            ViewData["OriginID"] = new SelectList(_context.Origins, "Id", "Name", product.OriginID);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Distributors)
                .Include(p => p.Origins)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
