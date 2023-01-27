using Microsoft.AspNetCore.Identity;
using ShoppingBuddyWebApplication.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition;

namespace ShoppingBuddyWebApplication.Models
{
    public class ShoppingLists
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CheckedProductIds { get;set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        public ICollection<ProductShoppingLists> ProductShoppingLists { get; set; } = new List<ProductShoppingLists>();
        [NotMapped]
        public ICollection<int>? ProductIds { get; set; } = new List<int>();

        public ShoppingLists FillProductShoppingList(ApplicationDbContext context)
        {
            var list = ProductShoppingLists.ToList();
            ProductIds.ToList().ForEach(id =>
            {
                list.Add(new ProductShoppingLists()
                {
                    ProductId = id,
                    ShoppingListId = this.Id,
                    ShoppingList = this,
                    Product = context.Product.Where(p=>p.Id == id).First()
                });  
            });
            ProductShoppingLists = list;
            return this;
        }
        public ShoppingLists FillProducts()
        {
            ProductShoppingLists.ToList().ForEach(ps =>
            {
                ProductIds.Add(ps.Product.Id);
            });
            return this;
        }
    }
}
