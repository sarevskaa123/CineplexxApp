using Cineplexx.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cineplexx.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDTO GetShoppingCartInfo(string userId);
        bool DeleteProductFromShoppingCart(string userId, Guid Id);
        bool Order(string userId);
    }
}
