using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using fast_api.Config;
using Microsoft.Extensions.Options;

namespace fast_api.Http
{
    public class Gw2ApiClient : IGw2ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly Gw2ApiEndpoints _options;

        public Gw2ApiClient(HttpClient httpClient, IOptions<Gw2ApiEndpoints> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
        }

        public async Task<List<int>> FetchAllItemIdsAsync(CancellationToken cancellationToken)
        {
            var request = CreateRequest(_options.Gw2ApiCommercePricesEndpoint);
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            await using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<int>>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
        }

        public async Task<List<Item>> FetchItemsForIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids)
        {
            var request = CreateRequest($"{_options.Gw2ApiItemEndpoint}?ids={string.Join(",", ids)}");
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                return new List<Item>();
            }

            await using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Item>>(contentStream, null, cancellationToken);
        }

        public async Task<List<ItemPrice>> FetchItemPricesForIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids)
        {
            var request = CreateRequest($"{_options.Gw2ApiCommercePricesEndpoint}?ids={string.Join(",", ids)}");
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                return new List<ItemPrice>();
            }

            await using var contentStream = await result.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<ItemPrice>>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
        }

        private static HttpRequestMessage CreateRequest(string endpoint)
        {
            return new HttpRequestMessage(HttpMethod.Get, endpoint);
        }
    }
}
