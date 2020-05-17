using AutoMapper;
using fast_api.EntityFramework;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;

namespace fast_api.Services
{
    public class SelectionContainerService : ISelectionContainerService
    {
        private readonly FastContext _context;
        private readonly ICurrencyService _currencyService;

        public SelectionContainerService(FastContext context, ICurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public async Task<List<SelectionContainer>> GetAsync()
        {
            //TODO: paging etc.
            return await _context.SelectionContainers
                .Include(selectionContainer => selectionContainer.SelectionContainerItems).Take(5).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(SelectionContainer selectionContainer)
        {
            await DeleteByIdAsync(selectionContainer.SelectionContainerId);
            await _context.SelectionContainers.AddAsync(selectionContainer);
            await _context.SaveChangesAsync();
            await _context.Entry(selectionContainer).Reference(x => x.SelectionContainerItems).LoadAsync();
            await _context.Entry(selectionContainer).Reference(x => x.SelectionContainerCategories).LoadAsync();
            await _context.Entry(selectionContainer).Reference(x => x.SelectionContainerCurrencies).LoadAsync();
            await _context.Entry(selectionContainer).Reference(x => x.SelectionContainerContainers).LoadAsync();
            CalculateSelectionContainerPrice(selectionContainer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePricesAsync()
        {
            var selectionContainers = await _context.SelectionContainers
                .Include(selectionContainer => selectionContainer.SelectionContainerItems)
                .Include(selectionContainer => selectionContainer.SelectionContainerCategories)
                .Include(selectionContainer => selectionContainer.SelectionContainerCurrencies)
                .Include(selectionContainer => selectionContainer.SelectionContainerContainers)
                .ToListAsync();
            selectionContainers.ForEach(CalculateSelectionContainerPrice);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteByIdAsync(int id)
        {
            var entry = await _context.SelectionContainers.FindAsync(id);
            if (entry != null)
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        private void CalculateSelectionContainerPrice(SelectionContainer selectionContainer)
        {
            var currencyValues = selectionContainer.SelectionContainerCurrencies.ToDictionary(x => x.Currency,
                x => _currencyService.GetCurrencyValueAsync(x.Currency).Result);

            var buyGuaranteed = selectionContainer.SelectionContainerCategories.Where(x => x.Guaranteed)
                                    .Sum(x => x.Category.Buy * x.Amount) +
                                selectionContainer.SelectionContainerContainers.Where(x => x.Guaranteed)
                                    .Sum(x => x.Container.Buy * x.Amount) +
                                selectionContainer.SelectionContainerItems.Where(x => x.Guaranteed)
                                    .Sum(x => x.Item.Buy * x.Amount) +
                                selectionContainer.SelectionContainerCurrencies.Where(x => x.Guaranteed)
                                    .Sum(x => currencyValues[x.Currency].Buy * x.Amount);

            var sellGuaranteed = selectionContainer.SelectionContainerCategories.Where(x => x.Guaranteed)
                                     .Sum(x => x.Category.Sell * x.Amount) +
                                 selectionContainer.SelectionContainerContainers.Where(x => x.Guaranteed)
                                     .Sum(x => x.Container.Sell * x.Amount) +
                                 selectionContainer.SelectionContainerItems.Where(x => x.Guaranteed)
                                     .Sum(x => x.Item.Sell * x.Amount) +
                                 selectionContainer.SelectionContainerCurrencies.Where(x => x.Guaranteed)
                                     .Sum(x => currencyValues[x.Currency].Sell * x.Amount);
            
            var buySelection = new[]
            {
                selectionContainer.SelectionContainerCategories.Where(x => !x.Guaranteed)
                    .Max(x => x.Category.Buy * x.Amount),
                selectionContainer.SelectionContainerContainers.Where(x => !x.Guaranteed)
                    .Max(x => x.Container.Buy * x.Amount),
                selectionContainer.SelectionContainerItems.Where(x => !x.Guaranteed)
                    .Max(x => x.Item.Buy * x.Amount),
                selectionContainer.SelectionContainerCurrencies.Where(x => !x.Guaranteed)
                    .Max(x => currencyValues[x.Currency].Buy * x.Amount)
            }.Max();

            var sellSelection = new[]
            {
                selectionContainer.SelectionContainerCategories.Where(x => !x.Guaranteed)
                    .Max(x => x.Category.Sell * x.Amount),
                selectionContainer.SelectionContainerContainers.Where(x => !x.Guaranteed)
                    .Max(x => x.Container.Sell * x.Amount),
                selectionContainer.SelectionContainerItems.Where(x => !x.Guaranteed)
                    .Max(x => x.Item.Sell * x.Amount),
                selectionContainer.SelectionContainerCurrencies.Where(x => !x.Guaranteed)
                    .Max(x => currencyValues[x.Currency].Sell * x.Amount)
            }.Max();

            selectionContainer.Buy = buyGuaranteed + buySelection;
            selectionContainer.Sell = sellGuaranteed + sellSelection;
        }
    }
}