using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.EntityFramework;
using fast_api.EntityFramework.Entities;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace fast_api.Services
{
    class ContainerService : IContainerService
    {
        private readonly FastContext _context;
        private readonly ICurrencyService _currencyService;

        public ContainerService(FastContext context, ICurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public async Task<List<Container>> GetAsync()
        {
            //TODO: paging etc.
            return await _context.Containers.Take(5)
                .Include(x => x.ContainerItems)
                .Include(x => x.ContainerContainers)
                .Include(x => x.ContainerCategories)
                .Include(x => x.ContainerCurrencies)
                .Include(x => x.ContainerSelectionContainers)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(Container container)
        {
            await DeleteByIdAsync(container.ContainerId);
            await _context.Containers.AddAsync(container);
            await _context.SaveChangesAsync();
            await _context.Entry(container).Reference(x => x.ContainerItems).LoadAsync();
            await _context.Entry(container).Reference(x => x.ContainerCategories).LoadAsync();
            await _context.Entry(container).Reference(x => x.ContainerCurrencies).LoadAsync();
            await _context.Entry(container).Reference(x => x.ContainerContainers).LoadAsync();
            await _context.Entry(container).Reference(x => x.ContainerSelectionContainers).LoadAsync();
            CalculateContainerPrice(container);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePricesAsync()
        {
            var containers = await _context.Containers
                .Include(container => container.ContainerItems)
                .Include(container => container.ContainerCategories)
                .Include(container => container.ContainerCurrencies)
                .Include(container => container.ContainerContainers)
                .Include(container => container.ContainerSelectionContainers)
                .ToListAsync();
            containers.ForEach(CalculateContainerPrice);
            await _context.SaveChangesAsync();
        }

        private void CalculateContainerPrice(Container container)
        {
            var currencyValues = container.ContainerCurrencies.ToDictionary(x => x.Currency,
                x => _currencyService.GetCurrencyValueAsync(x.Currency).Result);

            container.Buy = (int) Math.Ceiling(
                container.ContainerItems.Sum(x => x.Item.Buy * x.DropRate) +
                container.ContainerCategories.Sum(x => x.Category.Buy * x.DropRate) +
                container.ContainerCurrencies.Sum(x => currencyValues[x.Currency].Buy * x.DropRate) +
                container.ContainerContainers.Sum(x => x.Container.Buy * x.DropRate) +
                container.ContainerSelectionContainers.Sum(x => x.SelectionContainer.Buy * x.DropRate));

            container.Sell = (int) Math.Ceiling(
                container.ContainerItems.Sum(x => x.Item.Sell * x.DropRate) +
                container.ContainerCategories.Sum(x => x.Category.Sell * x.DropRate) +
                container.ContainerCurrencies.Sum(x => currencyValues[x.Currency].Sell * x.DropRate) +
                container.ContainerContainers.Sum(x => x.Container.Sell * x.DropRate) +
                container.ContainerSelectionContainers.Sum(x => x.SelectionContainer.Sell * x.DropRate));
        }

        private async Task DeleteByIdAsync(int id)
        {
            var entry = await _context.Containers.FindAsync(id);
            if (entry != null)
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}