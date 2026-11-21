using MiniOrderProcessingApi.Models;

namespace MiniOrderProcessingApi.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order newOrder);
        Order? GetById(Guid id);
    }
}