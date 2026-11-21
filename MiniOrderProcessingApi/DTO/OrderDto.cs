namespace MiniOrderProcessingApi.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
    }
}