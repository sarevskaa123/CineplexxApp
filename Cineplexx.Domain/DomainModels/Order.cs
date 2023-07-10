using Cineplexx.Domain.Identity;
using System.Collections.Generic;

namespace Cineplexx.Domain.DomainModels
{
    public class Order: BaseEntity
    {
        public string UserId { get; set; }
        public CineplexxUser User { get; set; }
        public IEnumerable<ProductInOrder> ProductInOrders { get; set; }
    }
}
