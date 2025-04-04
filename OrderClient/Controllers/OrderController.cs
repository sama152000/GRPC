using InventoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderClient.Model;
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
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var invResponse = await _inventoryClient.DeductAsync(new DeductRequest
            {
                ProductName = "Laptop",
                //ProductId = order.ProductId,
                Quantity = order.Quantity
            });

            if (!invResponse.Success)
                return BadRequest("Inventory error: " + invResponse.Message);

            var payResponse = await _paymentClient.PayAsync(new PaymentRPC.Protos.PayRequest
            {
                ClientId = order.ClientId,
                Amount = order.Amount
            });

            if (!payResponse.Success)
                return BadRequest("Payment error: " + payResponse.Message);

            return Ok("Order created successfully!");
        }
    }
}
        



