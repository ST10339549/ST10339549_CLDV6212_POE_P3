using Microsoft.AspNetCore.Mvc;
using ST10339549_CLDV6212_POE.Interfaces;
using ST10339549_CLDV6212_POE.Models;

namespace ST10339549_CLDV6212_POE.Controllers
{
    public class OrderProcessingController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderProcessingController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> OrderProcessing(OrderMessage orderMessage)
        {
            if (string.IsNullOrEmpty(orderMessage.OrderId))
            {
                orderMessage.OrderId = await _orderRepository.GenerateOrderIdAsync();
            }

            orderMessage.Action ??= "In Progress";
            orderMessage.Timestamp = DateTimeOffset.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepository.AddOrderAsync(orderMessage);
                    TempData["Success"] = $"Order {orderMessage.OrderId} processed and saved successfully!";
                }
                catch (System.Exception ex)
                {
                    TempData["Error"] = $"Failed to process and save the order: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Invalid order details. Please provide valid inputs.";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderMessage order)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.UpdateOrderActionAsync(order.OrderId, order.Action);
                TempData["Success"] = "Order action updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }
    }
}
