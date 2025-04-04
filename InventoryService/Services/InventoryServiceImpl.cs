using Grpc.Core;
using InventoryService;
using System.Collections.Generic;

public class InventoryServiceImpl : Inventory.InventoryBase
{
    private static readonly Dictionary<string, int> InventoryData = new Dictionary<string, int>
    {
        { "Laptop", 10 },
        { "Mouse", 20 },
        { "Keyboard", 15 }
    };

    public override Task<DeductResponse> Deduct(DeductRequest request, ServerCallContext context)
    {
        if (!InventoryData.ContainsKey(request.ProductName))
        {
            return Task.FromResult(new DeductResponse
            {
                Success = false,
                Message = $" Laptop {request.ProductName} "
            });
        }

        if (InventoryData[request.ProductName] < request.Quantity)
        {
            return Task.FromResult(new DeductResponse
            {
                Success = false,
                Message = $"Not enough;{InventoryData[request.ProductName]}"
            });
        }

        InventoryData[request.ProductName] -= request.Quantity;

        return Task.FromResult(new DeductResponse
        {
            Success = true,
            Message = $"Deducted{request.Quantity} from {request.ProductName}"
        });
    }
}
