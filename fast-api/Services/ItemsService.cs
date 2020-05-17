using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.Interfaces;
using fast_api.EntityFramework;
using fast_api.EntityFramework.Entities;
using fast_api.Http;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;

namespace fast_api.Services
{
    public class ItemsService : IItemsService
    {
        private readonly FastContext _context;
        private readonly IMapper _mapper;
        private readonly ITpItemService _tpItemService;
        private readonly Gw2ApiClient _gw2ApiClient;
        private readonly ISelectionContainerService _selectionContainerService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly IContainerService _containerService;

        public ItemsService(FastContext context, IMapper mapper, ITpItemService tpItemService,
            Gw2ApiClient gw2ApiClient, ISelectionContainerService selectionContainerService,
            ICategoryService categoryService, ICurrencyService currencyService, IContainerService containerService)
        {
            _context = context;
            _mapper = mapper;
            _tpItemService = tpItemService;
            _gw2ApiClient = gw2ApiClient;
            _selectionContainerService = selectionContainerService;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _containerService = containerService;
        }

        public async Task<List<Item>> GetAsync()
        {
            return await _context.Items.Take(50).ToListAsync();
        }

        public async Task FetchItemsFromApiAsync()
        {
            var items = await _tpItemService.GetItemPricesFromApi();
            await _context.AddRangeAsync(_mapper.Map<List<Item>>(items));
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePricesAsync()
        {
            var items = _context.Items.ToList();

            var itemPriceDictionary =
                (await Task.WhenAll(items.Batch(150).Select(batch =>
                    _gw2ApiClient.FetchItemPricesForIdsAsync(CancellationToken.None, batch.Select(x => x.ItemId)))))
                .SelectMany(x => x).ToDictionary(x => x.ItemPriceId, x => x);

            items.ForEach(x =>
            {
                x.Buy = itemPriceDictionary[x.ItemId].BuyData.BuyPrice;
                x.Sell = itemPriceDictionary[x.ItemId].SellData.SellPrice;
            });

            //TODO: Unfuck this. needs more thinking. Build hierarchy? Iterate until all updated?
            await _context.SaveChangesAsync();
            await _categoryService.UpdatePricesAsync();

            await _currencyService.UpdatePricesAsync();
            await _selectionContainerService.UpdatePricesAsync();
            await _containerService.UpdatePricesAsync();
        }
    }
}