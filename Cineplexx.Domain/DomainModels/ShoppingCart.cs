using Cineplexx.Domain.Identity;
using System.Collections.Generic;

namespace Cineplexx.Domain.DomainModels
{
    public class ShoppingCart:BaseEntity
    {
        public string OwnerId { get; set; }
        public CineplexxUser Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }

    }
}
