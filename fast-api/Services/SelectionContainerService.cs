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
            var data = _context.SelectionContainers.Include(SelectionContainer => SelectionContainer.SelectionContainerItems).Take(5).ToList();
            return _mapper.Map<List<SelectionContainerDTO>>(await _context.SelectionContainers.Take(5).ToListAsync());
        }

        public async Task AddAsync(SelectionContainerDTO selectionContainer)
        {
            var entry = new SelectionContainer
            {
                SelectionContainerId = selectionContainer.Id,
                Name = selectionContainer.Name,
            };
            entry.SelectionContainerItems = selectionContainer.Items.Select(x => new SelectionContainerItem
            {
                Amount = x.Amount, 
                Guaranteed = x.Guaranteed,
                Item = _context.Items.Find(x.Id) ??
                       throw new ArgumentException($"Item with id {x.Id} could not be found"),
                ItemId = x.Id,
                SelectionContainer = entry,
                SelectionContainerId = selectionContainer.Id
            }).ToList();
            entry.Buy = entry.SelectionContainerItems.Aggregate(0, (total, current) => total += current.Guaranteed ? current.Amount * current.Item.Buy : 0) + entry.SelectionContainerItems.Where(x => !x.Guaranteed).Select(x => x.Item.Buy).Max();
            entry.Sell = entry.SelectionContainerItems.Aggregate(0, (total, current) => total += current.Guaranteed ? current.Amount * current.Item.Sell : 0) + entry.SelectionContainerItems.Where(x => !x.Guaranteed).Select(x => x.Item.Sell).Max();

            await _context.SelectionContainers.AddAsync(entry);
            await _context.SaveChangesAsync();
        }
    }
}

