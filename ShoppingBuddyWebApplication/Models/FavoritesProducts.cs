namespace ShoppingBuddyWebApplication.Models
{
    public class FavoritesProducts
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public Favorites Favorite { get; set; }
        public Product Product { get; set; }
    }
}
