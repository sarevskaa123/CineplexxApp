using Cineplexx.Domain.DomainModels;
using System.Collections.Generic;

namespace Cineplexx.Domain.DTO
{
    public class ShoppingCartDTO
    {
        public List<ProductInShoppingCart> Products { get; set; }
        public double TotalPrice { get; set; }
    }
}
