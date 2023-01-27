using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingBuddyWebApplication.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FavoritesProducts> FavoritesProducts { get; set; }

        [NotMapped]
        public List<int> ProductIds { get; set; }

        public Favorites ToDbEntity()
        {
            return new Favorites()
            {
                Id = Id,
                Name = Name
            };
        }

        public Favorites FillFavoritesProducts()
        {
            if (FavoritesProducts == null || FavoritesProducts.Count == 0)
            {
                FavoritesProducts = new List<FavoritesProducts>();
            }

            if (ProductIds != null || ProductIds.Count > 0)
            {
                ProductIds.ForEach(id =>
                {
                    FavoritesProducts.Add(new FavoritesProducts()
                    {
                        ProductId = id,
                        FavoriteId = Id
                    });
                });
            }
            return this;
        }

        public Favorites FillProductIds()
        {
            ProductIds = new List<int>();
            if (FavoritesProducts != null || FavoritesProducts.Count > 0)
            {
                FavoritesProducts.ToList().ForEach(favoriteProduts =>
                {
                    ProductIds.Add(favoriteProduts.ProductId);
                });
            }
            return this;
        }

        public bool HasProducts()
        {
            return FavoritesProducts != null && FavoritesProducts.Count > 0;
        }
        public Favorites()
        {

        }
    }
}
