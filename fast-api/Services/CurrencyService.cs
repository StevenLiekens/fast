using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.EntityFramework;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace fast_api.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly FastContext _context;
        private readonly IMapper _mapper;

        public CurrencyService(FastContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PriceDataDTO> GetCurrencyValueAsync(string currency)
        {
            var data = await _context.CurrencyTradeCosts.Where(x => x.Currency == currency).Include(x => x.CurrencyTrade)
                .ToListAsync();

            return new PriceDataDTO
            {
                BuyPrice = data.Select(x => x.CurrencyTrade.Buy / x.Amount * currency switch {"karma" => 1000, "luck" => 1000, _ => 1}).Max().ToString(),
                SellPrice = data.Select(x => x.CurrencyTrade.Sell / x.Amount * currency switch { "karma" => 1000, "luck" => 1000, _ => 1 }).Max().ToString()
            }; ;
        }

        public async Task<List<CurrencyTradeDTO>> GetAsync()
        {
            //TODO: Remove the Take(5)
            var data = await _context.CurrencyTrades
                .Include(currencyTrade => currencyTrade.CurrencyTradeCost).Take(5).ToListAsync();
            return _mapper.Map<List<CurrencyTradeDTO>>(data);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(CurrencyTradeDTO currencyTradeDto)
        {
            await DeleteByIdAsync(currencyTradeDto.CurrencyTradeId);

            var currencyTrade = new CurrencyTrade()
            {
                CurrencyTradeId = currencyTradeDto.CurrencyTradeId,
                Name = currencyTradeDto.Name,
                Info = currencyTradeDto.Info,
                CoinCost = currencyTradeDto.CoinCost,
                ItemType = Enum.Parse<ItemType>(currencyTradeDto.Type)
            };

            currencyTrade.CurrencyTradeCost = currencyTradeDto.CurrencyTradeCost.Select(x => new CurrencyTradeCost
            {
                Amount = x.Amount,
                Currency = x.Currency,
                CurrencyTradeId = currencyTrade.CurrencyTradeId,
                CurrencyTrade = currencyTrade
            }).ToArray();

            switch (currencyTrade.ItemType)
            {
                case ItemType.Item:
                    currencyTrade.ItemId = currencyTradeDto.ItemId;
                    currencyTrade.ItemAmount = currencyTradeDto.ItemAmount;
                    currencyTrade.Item = await _context.Items.FindAsync(currencyTrade.ItemId);
                    break;
                case ItemType.SelectionContainer:
                    currencyTrade.SelectionContainerId = currencyTradeDto.SelectionContainerId;
                    currencyTrade.SelectionContainerAmount = currencyTradeDto.SelectionContainerAmount;
                    currencyTrade.SelectionContainer =
                        await _context.SelectionContainers.FindAsync(currencyTrade.SelectionContainerId);
                    break;
                case ItemType.Container:
                    break;
                case ItemType.Category:
                    break;
                case ItemType.Currency:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            CalculateCurrencyTradePrices(currencyTrade);

            await _context.CurrencyTrades.AddAsync(currencyTrade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePrices()
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
                    currencyTrade.Buy = currencyTrade.Item.Buy * currencyTrade.ItemAmount - currencyTrade.CoinCost;
                    currencyTrade.Sell = currencyTrade.Item.Sell * currencyTrade.ItemAmount - currencyTrade.CoinCost;
                    break;
                case ItemType.SelectionContainer:
                    currencyTrade.Buy = currencyTrade.SelectionContainer.Buy * currencyTrade.SelectionContainerAmount - currencyTrade.CoinCost;
                    currencyTrade.Sell = currencyTrade.SelectionContainer.Sell * currencyTrade.SelectionContainerAmount - currencyTrade.CoinCost;
                    break;
                case ItemType.Container:
                    break;
                case ItemType.Category:
                    break;
                case ItemType.Currency:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}