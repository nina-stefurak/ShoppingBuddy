﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBuddyWebApplication.Models;
using System.Reflection.Emit;

namespace ShoppingBuddyWebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ShoppingBuddyWebApplication.Models.Product> Product { get; set; }
        public DbSet<ShoppingBuddyWebApplication.Models.Favorites> Favorites { get; set; }

        public DbSet<ShoppingBuddyWebApplication.Models.FavoritesProducts> FavoritesProducts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Favorites>()
                .HasMany(f => f.FavoritesProducts)
                .WithOne(fp => fp.Favorite)
                .HasForeignKey(fp => fp.FavoriteId);
            modelBuilder.Entity<FavoritesProducts>()
                .HasKey(fp => new { fp.FavoriteId, fp.ProductId });

        }
    }
}