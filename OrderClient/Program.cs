using InventoryService;
using PaymentRPC.Protos;

namespace OrderClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddGrpcClient<Inventory.InventoryClient>(o =>
            {
                o.Address = new Uri("http://localhost:5188");
            });

            builder.Services.AddGrpcClient<Payment.PaymentClient>(o =>
            {
                o.Address = new Uri("http://localhost:5020"); 
            });
            var app = builder.Build();


            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
