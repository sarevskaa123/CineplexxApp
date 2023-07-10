using Cineplexx.Domain.DomainModels;
using Cineplexx.Domain.DTO;
using Cineplexx.Repository.Interface;
using Cineplexx.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cineplexx.Services.Implementation
{
    
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IUserReopsitory _userRepository;
        public ShoppingCartService(IRepository<EmailMessage> mailRepository, IRepository<ShoppingCart> shoppingCartRepository,IUserReopsitory userRepository, IRepository<Order> orderRepository, IRepository<ProductInOrder> productInOrderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _mailRepository = mailRepository;
        }

        public bool DeleteProductFromShoppingCart(string userId, Guid Id)
        {
            if (!string.IsNullOrEmpty(userId) && Id != null)
            {
                var loggedInUser = _userRepository.Get(userId);
                var userShoppingCart = loggedInUser.UserCart;
                var itemToDelete = userShoppingCart.ProductInShoppingCarts.Where(e => e.ProductId.Equals(Id)).FirstOrDefault();
                userShoppingCart.ProductInShoppingCarts.Remove(itemToDelete);
                _shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }
        public ShoppingCartDTO GetShoppingCartInfo(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = _userRepository.Get(userId);
                var userShoppingCart = loggedInUser.UserCart;
                var AllProducts = userShoppingCart.ProductInShoppingCarts.ToList();
                var allProductPrice = AllProducts.Select(e => new
                {
                    ProductPrice = e.Product.Price,
                    Quantity = e.Quantity
                }).ToList();
                double totalPrice = 0.0;
                foreach (var item in allProductPrice)
                {
                    totalPrice += item.ProductPrice * item.Quantity;
                }
                ShoppingCartDTO cart = new ShoppingCartDTO
                {
                    Products = AllProducts,
                    TotalPrice = totalPrice
                };
                return cart;
            }
            return new ShoppingCartDTO();
        }

        public bool Order(string userId)
        {
            if(!string.IsNullOrEmpty(userId)) { 
            var loggedInUser = _userRepository.Get(userId);
            var userShoppingCart = loggedInUser.UserCart;
            EmailMessage mail = new EmailMessage();
            mail.MailTo = loggedInUser.Email;
            mail.Subject = "Order successfully created";
            mail.Status = false;

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                User = loggedInUser,
                UserId = userId,
            };
            _orderRepository.Insert(order);
            List<ProductInOrder> productInOrders = new List<ProductInOrder>();
            var result = userShoppingCart.ProductInShoppingCarts.Select(e => new ProductInOrder
            {
                Id = Guid.NewGuid(),
                ProductId = e.Product.Id,
                OrderedProduct = e.Product,
                OrderId = order.Id,
                UserOrder = order,
                Quantity = e.Quantity
            }).ToList();
            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;
            sb.AppendLine("Успешна нарачка и содржи: ");
            for (int i = 1; i <= result.Count(); i++)
            {
                var product = result[i-1];
                totalPrice +=  product.Quantity * product.OrderedProduct.Price;
                sb.AppendLine(i.ToString() + ". " + product.OrderedProduct.ProductName + " со цена: " + product.OrderedProduct.Price + " и количина: " + product.Quantity);
            }
            sb.AppendLine("Вкупно: " + totalPrice.ToString());
            mail.Content = sb.ToString();

            productInOrders.AddRange(result);

            foreach (var item in productInOrders)
            {
                _productInOrderRepository.Insert(item);
            }
            loggedInUser.UserCart.ProductInShoppingCarts.Clear();
            _userRepository.Update(loggedInUser);
            _mailRepository.Insert(mail);
            return true;
        }
        return false;
        }
        
    }
}
