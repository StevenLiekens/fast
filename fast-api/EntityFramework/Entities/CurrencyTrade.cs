using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using fast_api.Enums;
using Microsoft.EntityFrameworkCore;

namespace fast_api.EntityFramework.Entities
{
    public static class CurrencyTradeConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyTrade>()
                .HasMany(ct => ct.CurrencyTradeCost)
                .WithOne(ctc => ctc.CurrencyTrade)
                .HasForeignKey(ct => ct.CurrencyTradeId);
        }
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
        //public int ItemAmount { get; set; }
        
        [ForeignKey("SelectionContainerId")]
        public virtual SelectionContainer SelectionContainer { get; set; }
        public int? SelectionContainerId { get; set; }
        //public int SelectionContainerAmount { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        //public int CategoryAmount { get; set; }

        [ForeignKey("ContainerId")]
        public Container Container { get; set; }
        public int? ContainerId { get; set; }

        public int Amount { get; set; }

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