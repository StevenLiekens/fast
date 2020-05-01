using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                .HasMany(sci => sci.SelectionContainerItems)
                .WithOne(sc => sc.SelectionContainer)
                .HasForeignKey(sci => sci.SelectionContainerId);

            modelBuilder.Entity<SelectionContainerItem>()
                .HasOne(sci => sci.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId);
        }
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
        public int Buy { get; set; }
        public int Sell { get; set; }
        public ICollection<SelectionContainerItem> SelectionContainerItems { get; set; }
    }

    public class SelectionContainerItem
    {
        public int SelectionContainerId { get; set; }
        public SelectionContainer SelectionContainer { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }
}