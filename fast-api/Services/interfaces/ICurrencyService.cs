using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;

namespace fast_api.Services.interfaces
{
    public interface ICurrencyService
    {
        Task<(int Buy, int Sell)> GetCurrencyValueAsync(string currency);
        Task<List<CurrencyTrade>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(CurrencyTrade currencyTrade);
        Task UpdatePricesAsync();
    }
}