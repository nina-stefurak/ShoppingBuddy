using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingBuddyWebApplication.Data;
using ShoppingBuddyWebApplication.Models;

namespace ShoppingBuddyWebApplication.Controllers
{
    public class ShoppingListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingListsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string id)
        {
            var shoppingList = await _context.ShoppingLists
                .Where(shoppingList => shoppingList.UserId == id)
                .Include(s => s.User)
                .Include(s=> s.ProductShoppingLists)
                .ThenInclude(list => list.Product)
                .ToListAsync();
            return View(shoppingList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingLists = await _context.ShoppingLists
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingLists == null)
            {
                return NotFound();
            }

            return View(shoppingLists);
        }

        public IActionResult Create()
        {
            ViewBag.Products = _context.Product.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId,ProductIds,CheckedProductIds")] ShoppingLists shoppingLists)
        {
            var user = await _userManager.FindByIdAsync(shoppingLists.UserId);
            shoppingLists.User = user;
            shoppingLists.CheckedProductIds = "";
            try
            {
                var transformed = shoppingLists.FillProductShoppingList(_context);
                _context.Add(transformed);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new
                {
                    controller = "ShoppingLists",
                    action = "Index",
                    id = shoppingLists.UserId
                });
            }
            catch (Exception e)
            {
                ViewBag.Products = _context.Product.ToList();
                return View(shoppingLists);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingLists = await _context.ShoppingLists.FindAsync(id);
            if (shoppingLists == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingLists.UserId);
            return View(shoppingLists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CheckedProductIds,UserId")] ShoppingLists shoppingLists)
        {
            if (id != shoppingLists.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingLists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingListsExists(shoppingLists.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingLists.UserId);
            return View(shoppingLists);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingLists = await _context.ShoppingLists
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingLists == null)
            {
                return NotFound();
            }

            return View(shoppingLists);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingLists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShoppingLists'  is null.");
            }
            var shoppingLists = await _context.ShoppingLists.FindAsync(id);
            if (shoppingLists != null)
            {
                _context.ShoppingLists.Remove(shoppingLists);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToRoute(new
            {
                controller = "ShoppingLists",
                action = "Index",
                id = shoppingLists.UserId
            });
        }

        private bool ShoppingListsExists(int id)
        {
          return _context.ShoppingLists.Any(e => e.Id == id);
        }
    }
}
