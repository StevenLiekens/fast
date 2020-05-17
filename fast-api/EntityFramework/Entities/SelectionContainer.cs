using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace fast_api.EntityFramework.Entities
{
    public static class SelectionContainerConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SelectionContainerItem>()
                .HasKey(sci => new { sci.SelectionContainerId, sci.ItemId });
            modelBuilder.Entity<SelectionContainer>()
                .HasMany(sc => sc.SelectionContainerItems)
                .WithOne(sci => sci.SelectionContainer)
                .HasForeignKey(sci => sci.SelectionContainerId);
            modelBuilder.Entity<SelectionContainerContainer>()
                .HasKey(sci => new { sci.SelectionContainerId, sci.ContainerId });
            modelBuilder.Entity<SelectionContainer>()
                .HasMany(sc => sc.SelectionContainerContainers)
                .WithOne(sci => sci.SelectionContainer)
                .HasForeignKey(sci => sci.SelectionContainerId);
            modelBuilder.Entity<SelectionContainerCategory>()
                .HasKey(sci => new { sci.SelectionContainerId, sci.CategoryId });
            modelBuilder.Entity<SelectionContainer>()
                .HasMany(sc => sc.SelectionContainerCategories)
                .WithOne(sci => sci.SelectionContainer)
                .HasForeignKey(sci => sci.SelectionContainerId);
            modelBuilder.Entity<SelectionContainerCurrency>()
                .HasKey(sci => new { sci.SelectionContainerId, sci.Currency });
            modelBuilder.Entity<SelectionContainer>()
                .HasMany(sc => sc.SelectionContainerCurrencies)
                .WithOne(sci => sci.SelectionContainer)
                .HasForeignKey(sci => sci.SelectionContainerId);
        }
    }
    
    public class SelectionContainer 
    {
        public int SelectionContainerId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public ICollection<SelectionContainerItem> SelectionContainerItems { get; set; }
        public ICollection<SelectionContainerContainer> SelectionContainerContainers { get; set; }
        public ICollection<SelectionContainerCategory> SelectionContainerCategories { get; set; }
        public ICollection<SelectionContainerCurrency> SelectionContainerCurrencies { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class SelectionContainerItem
    {
        public SelectionContainer SelectionContainer { get; set; }
        public int SelectionContainerId { get; set; }
        
        public Item Item { get; set; }
        public int ItemId { get; set; }

        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
         
        [NotMapped]
        public int BuyPrice => Item.Buy * Amount;
    }

    public class SelectionContainerContainer
    {
        public SelectionContainer SelectionContainer { get; set; }
        public int SelectionContainerId { get; set; }

        public Container Container { get; set; }
        public int ContainerId { get; set; }

        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }

    public class SelectionContainerCategory
    {
        public SelectionContainer SelectionContainer { get; set; }
        public int SelectionContainerId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }

    public class SelectionContainerCurrency
    {
        public SelectionContainer SelectionContainer { get; set; }
        public int SelectionContainerId { get; set; }

        public string Currency { get; set; }

        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }
}