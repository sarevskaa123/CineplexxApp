using Cineplexx.Domain.DomainModels;
using System.Collections.Generic;

namespace Cineplexx.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetOrderDetails(BaseEntity model);
    }
}
