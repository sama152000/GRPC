namespace OrderClient.Controllers
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }

    }
}