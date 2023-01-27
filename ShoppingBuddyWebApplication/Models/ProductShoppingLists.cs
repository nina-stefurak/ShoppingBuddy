namespace ShoppingBuddyWebApplication.Models{
    public class ProductShoppingLists
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ShoppingListId { get; set; }
        public ShoppingLists ShoppingList { get; set; }
    }
}
