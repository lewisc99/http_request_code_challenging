using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Entities;
using Polly.CircuitBreaker;
using System.Text.Json;

namespace CodeChallenging.ClientServices.Contracts
{
    public class FakeStoreService : IFakeStoreService
    {
        private readonly IHttpClientFactory _clientFactory;

        public FakeStoreService(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IEnumerable<Product>> FindProductsAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient("FakeStore");

                var response = await client.GetAsync("products");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<IEnumerable<Product>>(content);

                return result;
            }
            catch (BrokenCircuitException)
            {
                IEnumerable<Product> emptyProduct = Enumerable.Empty<Product>();
                return emptyProduct;
            }

        }
    }
}
