using Grpc.Core;
using OrderClient.Protos; 
using InventoryService;   
using PaymentRPC.Protos;  

public class OrderService : OrderService.OrderServiceBase
{
    private readonly Inventory.InventoryClient _inventoryClient;
    private readonly Payment.PaymentClient _paymentClient;

    public OrderService(
        Inventory.InventoryClient inventoryClient,
        Payment.PaymentClient paymentClient)
    {
        _inventoryClient = inventoryClient;
        _paymentClient = paymentClient;
    }

    public override async Task<OrderResponse> PlaceOrder(OrderRequest request, ServerCallContext context)
    {
        // 1. التحقق من الكمية
        var inventoryResponse = await _inventoryClient.DeductAsync(
            new DeductRequest { ProductName = request.ProductName, Quantity = request.Quantity }
        );

        if (!inventoryResponse.Success)
            return new OrderResponse { Success = false, Message = inventoryResponse.Message };

        // 2. التحقق من الرصيد
        var paymentResponse = await _paymentClient.PayAsync(
            new PayRequest { ClientId = request.ClientId, Amount = request.Amount }
        );

        if (!paymentResponse.Success)
            return new OrderResponse { Success = false, Message = paymentResponse.Message };

        return new OrderResponse { Success = true, Message = "تم الطلب بنجاح!" };
    }
}