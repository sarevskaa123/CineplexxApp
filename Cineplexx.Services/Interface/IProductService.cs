using Cineplexx.Domain.DomainModels;
using Cineplexx.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cineplexx.Services.Interface
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetDetailsForProduct(Guid? id);
        void CreateNewProduct(Product product);
        AddToShoppingCardDTO GetShoppingCardInfo(Guid? id);
        void DeleteProduct(Guid? id);
        bool AddToShoppingCart(AddToShoppingCardDTO item, string userID);
        public void UpdateExisingProduct(Product product);

    }
}
