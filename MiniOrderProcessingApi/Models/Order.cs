namespace MiniOrderProcessingApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
    }    
}