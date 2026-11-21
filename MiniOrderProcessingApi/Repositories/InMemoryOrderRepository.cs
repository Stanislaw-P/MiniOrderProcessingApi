using MiniOrderProcessingApi.Interfaces;
using MiniOrderProcessingApi.Models;

namespace MiniOrderProcessingApi.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private List<Order> orders = new List<Order>();

        public void Add(Order newOrder)
        {
            orders.Add(newOrder);
        }

        public Order? GetById(Guid id)
        {
            return orders.FirstOrDefault(order => order.Id == id);
        }
    }
}