using ST10339549_CLDV6212_POE.Models;

namespace ST10339549_CLDV6212_POE.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(OrderMessage order);
        Task<OrderMessage> GetOrderByIdAsync(string orderId);
        Task<IEnumerable<OrderMessage>> GetAllOrdersAsync();
        Task UpdateOrderAsync(OrderMessage order);
        Task DeleteOrderAsync(string orderId);
    }
}
