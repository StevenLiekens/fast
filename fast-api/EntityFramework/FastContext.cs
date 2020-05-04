using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;

namespace fast_api.EntityFramework
{
    public class FastContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<SelectionContainer> SelectionContainers { get; set; }
        public DbSet<SelectionContainerItem> SelectionContainerItems { get; set; }

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
            modelBuilder.Entity<SelectionContainerItem>()
                .HasOne(sci => sci.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId);
            modelBuilder.Entity<CurrencyTrade>()
                .HasMany(ct => ct.CurrencyTradeCost)
                .WithOne(ctc => ctc.CurrencyTrade)
                .HasForeignKey(ct => ct.CurrencyTradeId);
            modelBuilder.Entity<CurrencyTrade>()
                .HasOne<SelectionContainer>()
                .WithMany()
                .HasForeignKey(ct => ct.SelectionContainerId)
                .IsRequired(false);
            modelBuilder.Entity<CurrencyTrade>()
                .HasOne<Item>()
                .WithMany()
                .HasForeignKey(ct => ct.ItemId)
                .IsRequired(false);
        }
    }

    public enum ItemType
    {
        Item,
        SelectionContainer,
        Container,
        Category,
        Currency
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

        public int ItemId { get; set; }
        public Item Item { get; set; }

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

        public Item Item { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }

        public SelectionContainer SelectionContainer { get; set; }
        public int SelectionContainerId { get; set; }
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
}