using System;
using System.Collections.Generic;

namespace CineplexxAdminApp.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId  { get; set; }
        public CineplexxUser User { get; set; }
        public ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}
