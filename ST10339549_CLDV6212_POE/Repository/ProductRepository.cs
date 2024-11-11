using Dapper;
using ST10339549_CLDV6212_POE.Helpers;
using ST10339549_CLDV6212_POE.Interfaces;
using ST10339549_CLDV6212_POE.Models;
using System.Data;

namespace ST10339549_CLDV6212_POE.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ProductRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddProductAsync(Product product)
        {
            // Generate the ProductId if it is null or empty
            if (string.IsNullOrEmpty(product.ProductId))
            {
                product.ProductId = await GenerateProductIdAsync();
            }

            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "INSERT INTO Products (ProductId, ProductName, ProductDescription, ProductPrice) VALUES (@ProductId, @ProductName, @ProductDescription, @ProductPrice)";
            await connection.ExecuteAsync(query, product);
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT * FROM Products WHERE ProductId = @ProductId";
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { ProductId = productId });
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT * FROM Products";
            return await connection.QueryAsync<Product>(query);
        }

        public async Task UpdateProductAsync(Product product)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "UPDATE Products SET ProductName = @ProductName, ProductDescription = @ProductDescription, ProductPrice = @ProductPrice WHERE ProductId = @ProductId";
            await connection.ExecuteAsync(query, product);
        }

        public async Task DeleteProductAsync(string productId)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "DELETE FROM Products WHERE ProductId = @ProductId";
            await connection.ExecuteAsync(query, new { ProductId = productId });
        }

        private async Task<string> GenerateProductIdAsync()
        {
            using IDbConnection connection = _dbConnection.CreateConnection();

            var query = "SELECT TOP 1 ProductId FROM Products ORDER BY ProductId DESC";
            var lastProductId = await connection.QueryFirstOrDefaultAsync<string>(query);

            if (string.IsNullOrEmpty(lastProductId))
            {
                return "P001";
            }

            int numericPart = int.Parse(lastProductId.Substring(1));
            numericPart++;

            return $"P{numericPart.ToString("D3")}";
        }
    }
}
