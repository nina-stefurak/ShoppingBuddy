using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingBuddyWebApplication.Controllers.viewModels;
using ShoppingBuddyWebApplication.Data;
using ShoppingBuddyWebApplication.Models;

namespace ShoppingBuddyWebApplication.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Products = GetProducts();
            return View(await _context.Favorites.Include(f=>f.FavoritesProducts).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Favorites == null)
            {
                return NotFound();
            }

            var favorites = await _context.Favorites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorites == null)
            {
                return NotFound();
            }

            return View(favorites);
        }

        public IActionResult Create()
        {
            ViewBag.Products = GetProducts();
            var viewModel = new FavoritesViewModel
            {
                Products = GetProducts()
            };

            return View(viewModel);
        }

        private List<Product> GetProducts()
        {
            return _context.Product.ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProductIds")] Favorites favorites)
        {
            favorites.FillFavoritesProducts();
            try
            {
                _context.Add(favorites);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } catch(Exception e)
            {
                return View(favorites);
            }           
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var favorite = _context.Favorites.Include(f => f.FavoritesProducts)
                .SingleOrDefault(f => f.Id == id)
                .FillProductIds();
            if (favorite == null)
            {
                return NotFound();
            }
            var viewModel = new FavoritesViewModel
            {
                Favorites = favorite,
                Products = GetProducts()
            };
          
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProductIds")] Favorites favorites)
        {
            if (id != favorites.Id)
            {
                return NotFound();
            }
            // Remove all existing FavoritesProducts records associated with the Favorites object
            var existingFavoritesProducts = _context.FavoritesProducts.Where(fp => fp.FavoriteId== id);
            _context.FavoritesProducts.RemoveRange(existingFavoritesProducts);
            // Add new FavoritesProducts records with the new product IDs
            foreach (var productId in favorites.ProductIds)
            {
                _context.FavoritesProducts.Add(new FavoritesProducts { FavoriteId = favorites.Id, ProductId = productId });
            }
            favorites.FillFavoritesProducts();
            await _context.SaveChangesAsync();

            _context.Update(favorites.ToDbEntity());
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Favorites == null)
            {
                return NotFound();
            }

            var favorites = await _context.Favorites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorites == null)
            {
                return NotFound();
            }

            return View(favorites);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Favorites == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Favorites'  is null.");
            }
            var favorites = await _context.Favorites.FindAsync(id);
            if (favorites != null)
            {
                _context.Favorites.Remove(favorites);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoritesExists(int id)
        {
          return _context.Favorites.Any(e => e.Id == id);
        }
    }
}
