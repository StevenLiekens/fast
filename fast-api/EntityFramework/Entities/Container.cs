using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace fast_api.EntityFramework.Entities
{
    public static class ContainerConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContainerItem>()
                .HasKey(sci => new { sci.ContainerId, sci.ItemId });
            modelBuilder.Entity<Container>()
                .HasMany(sc => sc.ContainerItems)
                .WithOne(sci => sci.Container)
                .HasForeignKey(sci => sci.ContainerId);
            modelBuilder.Entity<ContainerContainer>()
                .HasKey(sci => new { sci.ParentContainerId, sci.ContainerId });
            modelBuilder.Entity<Container>()
                .HasMany(sc => sc.ContainerContainers)
                .WithOne(sci => sci.Container)
                .HasForeignKey(sci => sci.ContainerId);
            modelBuilder.Entity<ContainerSelectionContainer>()
                .HasKey(sci => new { sci.ContainerId, sci.SelectionContainerId });
            modelBuilder.Entity<Container>()
                .HasMany(sc => sc.ContainerSelectionContainers)
                .WithOne(sci => sci.Container)
                .HasForeignKey(sci => sci.ContainerId);
            modelBuilder.Entity<ContainerCategory>()
                .HasKey(sci => new { sci.ContainerId, sci.CategoryId });
            modelBuilder.Entity<Container>()
                .HasMany(sc => sc.ContainerCategories)
                .WithOne(sci => sci.Container)
                .HasForeignKey(sci => sci.ContainerId);
            modelBuilder.Entity<ContainerCurrency>()
                .HasKey(sci => new { sci.ContainerId, sci.Currency });
            modelBuilder.Entity<Container>()
                .HasMany(sc => sc.ContainerCurrencies)
                .WithOne(sci => sci.Container)
                .HasForeignKey(sci => sci.ContainerId);
        }
    }

    public class Container
    {
        public int ContainerId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Tag { get; set; }

        public ICollection<ContainerItem> ContainerItems { get; set; }
        public ICollection<ContainerContainer> ContainerContainers { get; set; }
        public ICollection<ContainerSelectionContainer> ContainerSelectionContainers { get; set; }
        public ICollection<ContainerCategory> ContainerCategories { get; set; }
        public ICollection<ContainerCurrency> ContainerCurrencies { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class ContainerItem
    {
        public Container Container { get; set; }
        public int ContainerId { get; set; }

        public Item Item { get; set; }
        public int ItemId { get; set; }

        public double DropRate { get; set; }
    }

    public class ContainerContainer
    {
        public Container ParentContainer { get; set; }
        public int ParentContainerId { get; set; }

        public Container Container { get; set; }
        public int ContainerId { get; set; }

        public double DropRate { get; set; }
    }

    public class ContainerSelectionContainer
    {
        public Container Container { get; set; }
        public int ContainerId { get; set; }

        public SelectionContainer SelectionContainer { get; set; }
        public int SelectionContainerId { get; set; }

        public double DropRate { get; set; }
    }

    public class ContainerCategory
    {
        public Container Container { get; set; }
        public int ContainerId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public double DropRate { get; set; }
    }
    
    public class ContainerCurrency
    {
        public Container Container { get; set; }
        public int ContainerId { get; set; }

        public string Currency { get; set; }

        public double DropRate { get; set; }
    }
}