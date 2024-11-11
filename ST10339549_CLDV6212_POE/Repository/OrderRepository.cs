using Dapper;
using ST10339549_CLDV6212_POE.Helpers;
using ST10339549_CLDV6212_POE.Interfaces;
using ST10339549_CLDV6212_POE.Models;
using System.Data;

namespace ST10339549_CLDV6212_POE.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public OrderRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddOrderAsync(OrderMessage order)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "INSERT INTO Orders (OrderId, ProductId, Quantity, Action, Timestamp) VALUES (@OrderId, @ProductId, @Quantity, @Action, @Timestamp)";
            await connection.ExecuteAsync(query, order);
        }

        public async Task<OrderMessage> GetOrderByIdAsync(string orderId)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            return await connection.QueryFirstOrDefaultAsync<OrderMessage>(query, new { OrderId = orderId });
        }

        public async Task<IEnumerable<OrderMessage>> GetAllOrdersAsync()
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT * FROM Orders";
            return await connection.QueryAsync<OrderMessage>(query);
        }

        public async Task UpdateOrderAsync(OrderMessage order)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "UPDATE Orders SET ProductId = @ProductId, Quantity = @Quantity, Action = @Action, Timestamp = @Timestamp WHERE OrderId = @OrderId";
            await connection.ExecuteAsync(query, order);
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "DELETE FROM Orders WHERE OrderId = @OrderId";
            await connection.ExecuteAsync(query, new { OrderId = orderId });
        }
    }
}
