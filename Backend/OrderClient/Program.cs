using InventoryService;
using PaymentRPC.Protos;

namespace OrderClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // إضافة الـ gRPC Services
            builder.Services.AddGrpc();

            // إعداد الـ gRPC Clients
            builder.Services.AddGrpcClient<Inventory.InventoryClient>(o =>
            {
                o.Address = new Uri("http://localhost:5188");
            });

            builder.Services.AddGrpcClient<Payment.PaymentClient>(o =>
            {
                o.Address = new Uri("http://localhost:5020");
            });

            var app = builder.Build();

            // تعيين الـ gRPC Services
            app.MapGrpcService<OrderService>();
            app.MapControllers();

            app.Run();
        }
    }
}
