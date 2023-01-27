using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingBuddyWebApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public IdentityUser? Owner { get; set; }
    }
}
