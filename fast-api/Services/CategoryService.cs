using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using fast_api.EntityFramework;
using fast_api.EntityFramework.Entities;
using fast_api.Services.interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fast_api.Services
{
    class CategoryService : ICategoryService
    {
        private readonly FastContext _context;

        public CategoryService(FastContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAsync()
        {
            //TODO: paging etc.
            return await _context.Categories.Take(5).Include(x => x.CategoryItems).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(Category category)
        {
            await DeleteByIdAsync(category.CategoryId);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            await _context.Entry(category).Reference(x => x.CategoryItems).LoadAsync();
            CalculateCategoryPrice(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePricesAsync()
        {
            var categories = await _context.Categories.Include(x => x.CategoryItems).ToListAsync();
            categories.ForEach(CalculateCategoryPrice);
            await _context.SaveChangesAsync();
        }

        private static void CalculateCategoryPrice(Category category)
        {
            category.Buy = (int)Math.Ceiling(category.CategoryItems.Average(x => x.Item.Buy));
            category.Sell = (int)Math.Ceiling(category.CategoryItems.Average(x => x.Item.Sell));
        }

        private async Task DeleteByIdAsync(int id)
        {
            var entry = await _context.Categories.FindAsync(id);
            if (entry != null)
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}