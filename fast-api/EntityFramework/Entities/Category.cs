using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace fast_api.EntityFramework.Entities
{
    public static class CategoryConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryItem>()
                .HasKey(sci => new { sci.CategoryId, sci.ItemId });
            modelBuilder.Entity<Category>()
                .HasMany(c => c.CategoryItems)
                .WithOne(ci => ci.Category)
                .HasForeignKey(c => c.CategoryId);
        }
    }
    
    public class Category : IBuy, ISell
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public ICollection<CategoryItem> CategoryItems { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }
    
    public class CategoryItem
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Item Item { get; set; }
        public int ItemId { get; set; }
    }
}