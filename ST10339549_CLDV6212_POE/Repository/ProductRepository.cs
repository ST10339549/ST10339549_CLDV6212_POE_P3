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
    }
}