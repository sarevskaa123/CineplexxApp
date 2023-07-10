using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using Stripe;
using Cineplexx.Services.Interface;

namespace Cineplexx.Web.Controllers 
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
            StripeConfiguration.ApiKey = "sk_test_51NIzhaCu38DStnw8kR2YWVpa2UcceyKTMfySprqAbsVRSry8fFG5dmpwehLXpmnTZV5esSD5Gwt5eAPMdkAOJihg00qisRbTh7";
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_shoppingCartService.GetShoppingCartInfo(userId));
        }



        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _shoppingCartService.DeleteProductFromShoppingCart(userId, id);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index","ShoppingCart");
            }
        }


        private Boolean Order()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _shoppingCartService.Order(userId);

            return result;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _shoppingCartService.GetShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Cineplexx Application Payment",
                Currency = "mkd",
                Customer = customer.Id
            });

            if (charge.Status.Equals( "succeeded"))
            {
                var result = Order();
                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                
                   
              
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

    }
}
