using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace fast_api.Http
{
    public class Gw2ApiClient : IGw2ApiClient
    {
        private readonly HttpClient _httpClient;

        public Gw2ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<int>> FetchAllItemIdsFromApi(CancellationToken cancellationToken, string endpoint)
        {
            var request = CreateRequest(endpoint);
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            using (var contentStream = await result.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<List<int>>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
            }
        }

        public async Task<List<Item>> FetchAllItemsFromApi(CancellationToken cancellationToken, string endpoint)
        {
            var request = CreateRequest(endpoint);
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                return new List<Item>();
            }
            using (var contentStream = await result.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<List<Item>>(contentStream, null, cancellationToken);
            }
        }

        public async Task<List<ItemPrice>> FetchAllItemPricesFromApi(CancellationToken cancellationToken, string endpoint)
        {
            var request = CreateRequest(endpoint);
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                return new List<ItemPrice>();
            }

            using (var contentStream = await result.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<List<ItemPrice>>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
            }
        }

        private static HttpRequestMessage CreateRequest(string endpoint)
        {
            return new HttpRequestMessage(HttpMethod.Get, endpoint);
        }
    }
}
