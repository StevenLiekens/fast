using System;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.EntityFramework;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fast_api.Services
{
    public class SelectionContainerService : ISelectionContainerService
    {
        private readonly FastContext _context;
        private readonly IMapper _mapper;

        public SelectionContainerService(FastContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SelectionContainerDTO>> GetAsync()
        {
            //TODO: Remove the random Take(5)
            var data = await _context.SelectionContainers
                .Include(selectionContainer => selectionContainer.SelectionContainerItems).Take(5).ToListAsync();
            return _mapper.Map<List<SelectionContainerDTO>>(data);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(SelectionContainerDTO selectionContainerDto)
        {
            await DeleteByIdAsync(selectionContainerDto.Id);

            var selectionContainer = new SelectionContainer
            {
                SelectionContainerId = selectionContainerDto.Id,
                Name = selectionContainerDto.Name,
            };

            selectionContainer.SelectionContainerItems = selectionContainerDto.Items.Select(x =>
                new SelectionContainerItem
                {
                    Amount = x.Amount,
                    Guaranteed = x.Guaranteed,
                    Item = _context.Items.Find(x.Id) ??
                           throw new ArgumentException($"Item with id {x.Id} could not be found"),
                    ItemId = x.Id,
                    SelectionContainer = selectionContainer,
                    SelectionContainerId = selectionContainerDto.Id
                }).ToList();
            CalculateSelectionContainerPrices(selectionContainer);
            await _context.SelectionContainers.AddAsync(selectionContainer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePrices()
        {
            var selectionContainers = await _context.SelectionContainers.Include(selectionContainer => selectionContainer.SelectionContainerItems).ToListAsync();
            selectionContainers.ForEach(CalculateSelectionContainerPrices);
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

        private static void CalculateSelectionContainerPrices(SelectionContainer selectionContainer)
        {
            //The price of an item is determined by the sum of the guaranteed items + the maximum of the optional items
            selectionContainer.Buy =
                selectionContainer.SelectionContainerItems.Aggregate(0,
                    (total, current) => total += current.Guaranteed ? current.Amount * current.Item.Buy : 0) +
                selectionContainer.SelectionContainerItems.Where(x => !x.Guaranteed).Select(x => x.Item.Buy).Max();
            selectionContainer.Sell =
                selectionContainer.SelectionContainerItems.Aggregate(0,
                    (total, current) => total += current.Guaranteed ? current.Amount * current.Item.Sell : 0) +
                selectionContainer.SelectionContainerItems.Where(x => !x.Guaranteed).Select(x => x.Item.Sell).Max();
        }
    }
}