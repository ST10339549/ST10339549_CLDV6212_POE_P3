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
            if (string.IsNullOrEmpty(order.OrderId))
            {
                order.OrderId = await GenerateOrderIdAsync();
            }
            order.Timestamp = DateTimeOffset.UtcNow;

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

        public async Task UpdateOrderActionAsync(string orderId, string action)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "UPDATE Orders SET Action = @Action WHERE OrderId = @OrderId";
            await connection.ExecuteAsync(query, new { OrderId = orderId, Action = action });
        }

        public async Task<string> GenerateOrderIdAsync()
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT TOP 1 OrderId FROM Orders ORDER BY OrderId DESC";
            var lastOrderId = await connection.QueryFirstOrDefaultAsync<string>(query);

            if (string.IsNullOrEmpty(lastOrderId))
            {
                return "O001";
            }

            int numericPart = int.Parse(lastOrderId.Substring(1));
            numericPart++;

            return $"O{numericPart.ToString("D3")}";
        }
    }
}
