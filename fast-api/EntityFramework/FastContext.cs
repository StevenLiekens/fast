using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace fast_api.EntityFramework
{
    public class FastContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<SelectionContainer> SelectionContainers { get; set; }
        public DbSet<SelectionContainerItem> SelectionContainerItems { get; set; }
        public DbSet<CurrencyTrade> CurrencyTrades { get; set; }
        public DbSet<CurrencyTradeCost> CurrencyTradeCosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }

        public FastContext(DbContextOptions<FastContext> dbContext) : base(dbContext)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SelectionContainerItem>()
                .HasKey(sci => new {sci.SelectionContainerId, sci.ItemId});
            modelBuilder.Entity<SelectionContainer>()
                .HasMany(sc => sc.SelectionContainerItems)
                .WithOne(sci => sci.SelectionContainer)
                .HasForeignKey(sci => sci.SelectionContainerId);
            modelBuilder.Entity<CurrencyTrade>()
                .HasMany(ct => ct.CurrencyTradeCost)
                .WithOne(ctc => ctc.CurrencyTrade)
                .HasForeignKey(ct => ct.CurrencyTradeId);
            modelBuilder.Entity<CategoryItem>()
                .HasKey(sci => new { sci.CategoryId, sci.ItemId });
            modelBuilder.Entity<Category>()
                .HasMany(c => c.CategoryItems)
                .WithOne(ci => ci.Category)
                .HasForeignKey(c => c.CategoryId);
        }
    }

    public enum ItemType
    {
        Item,
        SelectionContainer,
        Container,
        Category,
        Currency,
        //TODO: crafting??
        //TODO: Mystic Forge??
    }

    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class SelectionContainer
    {
        public int SelectionContainerId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public ICollection<SelectionContainerItem> SelectionContainerItems { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class SelectionContainerItem
    {
        public int SelectionContainerId { get; set; }
        public SelectionContainer SelectionContainer { get; set; }

        public ItemType ItemType { get; set; }
        
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
        public int? ItemId { get; set; }

        public string Currency { get; set; }

        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }

    public class CurrencyTrade
    {
        public int CurrencyTradeId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        
        public ICollection<CurrencyTradeCost> CurrencyTradeCost { get; set; }
        public int CoinCost { get; set; }

        public ItemType ItemType { get; set; }
        
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
        public int? ItemId { get; set; }
        public int ItemAmount { get; set; }
        
        [ForeignKey("SelectionContainerId")]
        public virtual SelectionContainer SelectionContainer { get; set; }
        public int? SelectionContainerId { get; set; }
        public int SelectionContainerAmount { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class CurrencyTradeCost
    {
        public int CurrencyTradeCostId { get; set; }
        
        public string Currency { get; set; }
        public int Amount { get; set; }

        public CurrencyTrade CurrencyTrade { get; set; }
        public int CurrencyTradeId { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public ICollection<CategoryItem> CategoryItems { get; set; }
    }

    public class CategoryItem
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
        public int? ItemId { get; set; }
    }
}