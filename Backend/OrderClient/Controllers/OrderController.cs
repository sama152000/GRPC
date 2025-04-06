using InventoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderClient.Model;
using OrderClient.Protos;
using static InventoryService.Inventory;
using static PaymentRPC.Protos.Payment;

namespace OrderClient.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class OrderController : ControllerBase
        {
        private readonly InventoryClient _inventoryClient;
        private readonly PaymentClient _paymentClient;

        public OrderController(InventoryClient inventoryClient, PaymentClient paymentClient)
        {
            _inventoryClient = inventoryClient;
            _paymentClient = paymentClient;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto order)
        {
            // 1. التحقق من الكمية
            var invResponse = await _inventoryClient.CheckQuantityAsync(
                new InventoryRequest { ProductName = order.ProductName, Quantity = order.Quantity }
            );

            if (!invResponse.Success)
                return BadRequest("Inventory error: " + invResponse.Message);

            // 2. التحقق من الرصيد
            var payResponse = await _paymentClient.CheckBalanceAsync(
                new PaymentRequest { ClientId = order.ClientId, Amount = order.Amount }
            );

            if (!payResponse.Success)
                return BadRequest("Payment error: " + payResponse.Message);

            return Ok("Order created successfully!");
        }
    }
}
        



