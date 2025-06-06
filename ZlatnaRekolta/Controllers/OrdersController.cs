﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ZlatnaRekolta.Data;

namespace ZlatnaRekolta.Controllers
{
    [Authorize(Roles ="User")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult FinalizeOrder(string listOrder)
        {
            List<int> listId = listOrder.Split(',').Select(int.Parse).ToList();
            foreach (int id in listId)
            {

                Order order = _context.Orders.FirstOrDefault(x => x.Id == id);
                if (order == null) break;


                Basket b = new Basket();
                b.UserId = order.UserId;
                b.ProductId = order.ProductId;
                b.Quantity = order.Quantity;
                b.Description = order.Description;
                b.RegisterOn = DateTime.Now;

                _context.Baskets.Add(b);
                _context.Orders.Remove(order);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var borsaDbContext = _context.Orders
                                    .Include(o => o.Users)
                                    .Include(o => o.Products);
                return View(await borsaDbContext.ToListAsync());
            }
            else
            {
                var borsaDbContext = _context.Orders
                                    .Include(o => o.Users)
                                    .Include(o => o.Products)
                                    .Where(x => x.UserId == _userManager.GetUserId(User));
                return View(await borsaDbContext.ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(int? ProductId, string? Unim)
        {
            ViewBag.ProductId = new SelectList(_context.Products, "Id", "Name", ProductId);
            ViewBag.Unim = Unim;
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Quantity,Description")] Order order)
        {
            if (order.Description == null)
            {
                order.Description = "-";
            }

            var product = _context.Products.Find(order.ProductId);
            if (order.Quantity > product.Quantity)
            {
                ModelState.AddModelError("Quantity", $"Наличното количество е само {product.Quantity:f3} {product.UnitOfMe}.");
            }
            if (ModelState.IsValid)
            {
                product.Quantity -= order.Quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                order.RegisterOn = DateTime.Now;
                order.UserId = _userManager.GetUserId(User);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", order.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", order.UserId);
            return View(order);
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id, string? Unim)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", order.ProductId);
            ViewBag.Unim = Unim;
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", order.UserId);

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductId,Quantity,Description,RegisterOn")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    order.UserId = _userManager.GetUserId(User);
                    order.RegisterOn = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", order.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }

            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
