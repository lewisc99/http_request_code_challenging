using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Entities;
using System.Text.Json;

namespace CodeChallenging.ClientServices.Contracts
{
    public class FakeStoreService : IFakeStoreService
    {
        private readonly IHttpClientFactory _clientFactory;

        public FakeStoreService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Product>> FindProductsAsync()
        {
            var client = _clientFactory.CreateClient("FakeStore");

            var response = await client.GetAsync("products");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<IEnumerable<Product>>(content);

            return result;
        }
    }
}
