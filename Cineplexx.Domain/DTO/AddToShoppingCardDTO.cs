using Cineplexx.Domain.DomainModels;
using System;

namespace Cineplexx.Domain.DTO
{
    public class AddToShoppingCardDTO
    {
        public Product SelectedProduct { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
