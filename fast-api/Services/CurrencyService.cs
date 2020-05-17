using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.EntityFramework;
using fast_api.EntityFramework.Entities;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace fast_api.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly FastContext _context;

        public CurrencyService(FastContext context)
        {
            _context = context;
        }

        public async Task<(int Buy, int Sell)> GetCurrencyValueAsync(string currency)
        {
            var data = await _context.CurrencyTradeCosts.Where(x => x.Currency == currency).Include(x => x.CurrencyTrade)
                .ToListAsync();

            return (
                data.Select(x => x.CurrencyTrade.Buy / x.Amount * currency switch {"karma" => 1000, "luck" => 1000, _ => 1}).Max(),
                data.Select(x => x.CurrencyTrade.Sell / x.Amount * currency switch { "karma" => 1000, "luck" => 1000, _ => 1 }).Max()
            );
        }

        public async Task<List<CurrencyTrade>> GetAsync()
        {
            //TODO: paging etc.
            return await _context.CurrencyTrades
                .Include(currencyTrade => currencyTrade.CurrencyTradeCost).Take(5).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(CurrencyTrade currencyTrade)
        {
            await DeleteByIdAsync(currencyTrade.CurrencyTradeId);
            CalculateCurrencyTradePrices(currencyTrade);
            await _context.SaveChangesAsync();
            await _context.Entry(currencyTrade).Reference(x => x.Item).LoadAsync();
            await _context.Entry(currencyTrade).Reference(x => x.SelectionContainer).LoadAsync();
            await _context.Entry(currencyTrade).Reference(x => x.Container).LoadAsync();
            await _context.Entry(currencyTrade).Reference(x => x.Category).LoadAsync();
            await _context.CurrencyTrades.AddAsync(currencyTrade);
        }

        public async Task UpdatePricesAsync()
        {
            var currencyTrades = await _context.CurrencyTrades.Include(currencyTrade => currencyTrade.CurrencyTradeCost).ToListAsync();
            currencyTrades.ForEach(CalculateCurrencyTradePrices);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteByIdAsync(int id)
        {
            var entry = await _context.CurrencyTrades.FindAsync(id);
            if (entry != null)
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        private static void CalculateCurrencyTradePrices(CurrencyTrade currencyTrade)
        {
            switch (currencyTrade.ItemType)
            {
                case ItemType.Item:
                    currencyTrade.Buy = currencyTrade.Item.Buy * currencyTrade.Amount - currencyTrade.CoinCost;
                    currencyTrade.Sell = currencyTrade.Item.Sell * currencyTrade.Amount - currencyTrade.CoinCost;
                    break;
                case ItemType.SelectionContainer:
                    currencyTrade.Buy = currencyTrade.SelectionContainer.Buy * currencyTrade.Amount - currencyTrade.CoinCost;
                    currencyTrade.Sell = currencyTrade.SelectionContainer.Sell * currencyTrade.Amount - currencyTrade.CoinCost;
                    break;
                case ItemType.Container:
                    currencyTrade.Buy = currencyTrade.Container.Buy * currencyTrade.Amount - currencyTrade.CoinCost;
                    currencyTrade.Sell = currencyTrade.Container.Sell * currencyTrade.Amount - currencyTrade.CoinCost;
                    break;
                case ItemType.Category:
                    currencyTrade.Buy = currencyTrade.Category.Buy * currencyTrade.Amount - currencyTrade.CoinCost;
                    currencyTrade.Sell = currencyTrade.Category.Sell * currencyTrade.Amount - currencyTrade.CoinCost;
                    break;
                case ItemType.Currency:
                    throw new DataException("Currency rewarding other currency isn't supported. Instead enter an entry with the rewards of the currency you are converting to.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}