using MiniOrderProcessingApi.Interfaces;

namespace MiniOrderProcessingApi.Services
{
    public class PromoDiscountStrategy : IDiscountStrategy
    {
        private const decimal DiscountRate = 0.10m;

        public string DiscountType => "promo";

        public decimal GetDiscountPrice(decimal price)
        {
            return price - price * DiscountRate;
        }
    }
}
