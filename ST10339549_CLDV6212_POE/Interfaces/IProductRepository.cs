using ST10339549_CLDV6212_POE.Models;

namespace ST10339549_CLDV6212_POE.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(string productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string productId);
    }
}
