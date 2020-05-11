using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.Contracts.DTO;

namespace fast_api.Services.interfaces
{
    public interface ICurrencyService
    {
        Task<PriceDataDTO> GetCurrencyValueAsync(string currency);
        Task<List<CurrencyTradeDTO>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(CurrencyTradeDTO currencyTradeDto);
        Task UpdatePrices();
    }
}