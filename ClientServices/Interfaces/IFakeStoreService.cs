using CodeChallenging.Entities;

namespace CodeChallenging.ClientServices.Interfaces
{
    public interface IFakeStoreService
    {
        Task<IEnumerable<Product>> FindProductsAsync();
    }
}
