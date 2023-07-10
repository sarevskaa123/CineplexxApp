using System;

namespace CineplexxAdminApp.Models
{
    public class ProductInOrder
    {
        public Guid Id { get; set; }
        public Product OrderedProduct { get; set; }
        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
