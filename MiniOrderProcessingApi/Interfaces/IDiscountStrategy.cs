namespace MiniOrderProcessingApi.Interfaces
{
    public interface IDiscountStrategy
    {
        string DiscountType { get; }
        decimal GetDiscountPrice(decimal price);
    }
}
