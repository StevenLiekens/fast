using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fast_api.EntityFramework;
using fast_api.Enums;
using fast_api.Helpers;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace fast_api.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly FastContext _context;
        private readonly IItemsService _itemsService;
        private readonly IContainerService _containerService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly ISelectionContainerService _selectionContainerService;

        public UpdateService(FastContext context, IItemsService itemsService, IContainerService containerService,
            ICategoryService categoryService,
            ICurrencyService currencyService, ISelectionContainerService selectionContainerService)
        {
            _context = context;
            _itemsService = itemsService;
            _containerService = containerService;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _selectionContainerService = selectionContainerService;
        }

        public async Task UpdateAll()
        {
            await _itemsService.UpdatePricesAsync();
            await _categoryService.UpdatePricesAsync();
            var entities = new List<EntityWrapper>(_context.Containers.Include(x => x.ContainerSelectionContainers)
                .Include(x => x.ContainerContainers).Include(x => x.ContainerCurrencies)
                .Select(x => new EntityWrapper(x)));
            entities.AddRange(_context.SelectionContainers.Include(x => x.SelectionContainerContainers).Include(x => x.SelectionContainerCurrencies).Select(x => new EntityWrapper(x)));
            entities.AddRange(_context.CurrencyTrades.Include(x => x.CurrencyTradeCost).Select(x => new EntityWrapper(x)));
            var currencyTradeIdsByCurrency = entities.Where(x => x.ItemType == ItemType.Currency)
                .Select(x => x.CurrencyTrade).SelectMany(x => x.CurrencyTradeCost).ToLookup(x => x.Currency, x => x.CurrencyTrade.CurrencyTradeId);
            entities.ForEach(x => x.CalculateDependencies(entities, currencyTradeIdsByCurrency));
            var sortedEntities = TopologicalSort.Sort(entities, x => x.Dependencies);
            sortedEntities.ForEach(x =>
            {
                switch (x.ItemType)
                {
                    case ItemType.SelectionContainer:
                        _selectionContainerService.UpdatePriceAsync(x.SelectionContainer);
                        break;
                    case ItemType.Container:
                        _containerService.UpdatePriceAsync(x.Container);
                        break;
                    case ItemType.Currency:
                        _currencyService.UpdatePriceAsync(x.CurrencyTrade);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}