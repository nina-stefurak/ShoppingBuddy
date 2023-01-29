using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingBuddyWebApplication.Data;
using ShoppingBuddyWebApplication.Models;
using System.Diagnostics;

namespace ShoppingBuddyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ShoppingListsController _shoppingListsController;
        public HomeController(ILogger<HomeController> logger,
             ApplicationDbContext context,
             UserManager<IdentityUser> userManager,
             SignInManager<IdentityUser> SignInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = SignInManager;
            _shoppingListsController = new ShoppingListsController(context, userManager);
        }

        public IActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                var shoppingLists = _context.ShoppingLists
                    .Where(shoppingList => shoppingList.UserId == user.Id)
                    .Include(s => s.ProductShoppingLists)
                    .ToList()
                    .Select(sl =>
                    {
                        sl.ProductShoppingLists = _context.ProductShoppingLists
                            .Where(p => sl.Id == p.ShoppingListId)
                            .Include(p => p.Product)
                            .ToList();
                        return sl;
                    })
                    .ToList();
                ViewBag.ShoppingList = shoppingLists;
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _shoppingListsController.DeleteConfirmed(id);
            return RedirectToRoute(new
            {
                controller = "Home",
                action = "Index"
            });
        }
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            return await _shoppingListsController.Edit(id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}