using fast_api.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
namespace fast_api.EntityFramework
{
    public class FastContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public DbSet<SelectionContainer> SelectionContainers { get; set; }
        public DbSet<SelectionContainerItem> SelectionContainerItems { get; set; }
        public DbSet<SelectionContainerContainer> SelectionContainerContainers { get; set; }
        public DbSet<SelectionContainerCategory> SelectionContainerCategories { get; set; }
        public DbSet<SelectionContainerCurrency> SelectionContainerCurrencies { get; set; }
        
        public DbSet<Container> Containers { get; set; }
        public DbSet<ContainerItem> ContainerItems { get; set; }
        public DbSet<ContainerContainer> ContainerContainers { get; set; }
        public DbSet<ContainerSelectionContainer> ContainerSelectionContainers { get; set; }
        public DbSet<ContainerCategory> ContainerCategories { get; set; }
        public DbSet<ContainerCurrency> ContainerCurrencies { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }

        public DbSet<CurrencyTrade> CurrencyTrades { get; set; }
        public DbSet<CurrencyTradeCost> CurrencyTradeCosts { get; set; }

        public FastContext(DbContextOptions<FastContext> dbContext) : base(dbContext)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SelectionContainerConfiguration.OnModelCreating(modelBuilder);
            ContainerConfiguration.OnModelCreating(modelBuilder);
            CategoryConfiguration.OnModelCreating(modelBuilder);
            CurrencyTradeConfiguration.OnModelCreating(modelBuilder);
        }
    }
}