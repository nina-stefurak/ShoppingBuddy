using ShoppingBuddyWebApplication.Models;

namespace ShoppingBuddyWebApplication.Controllers.viewModels
{
    public class FavoritesViewModel
    {
        public Favorites Favorites { get; set; }
        public List<Product> Products { get; set; }
    }
}
