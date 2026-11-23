using MiniOrderProcessingApi.Interfaces;

namespace MiniOrderProcessingApi.Services
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public string DiscountType => "standard";

        public decimal GetDiscountPrice(decimal price)
        {
            return price;
        }
    }
}
