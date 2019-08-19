using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IRepository<Costumer> costumers;
        IBasketService basketService;
        IOrderService orderService;
        public BasketController(IBasketService BasketService,IOrderService OrderService,IRepository<Costumer> Costumers) {
            this.basketService = BasketService;
            this.orderService = OrderService;
            this.costumers = Costumers;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id) {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary() {
            var basketSummary = basketService.GetBasketSummary(HttpContext);
            return PartialView(basketSummary);
        }

        [Authorize]//checks if the user is loged in, IF NOT, it would send the user to the login page
        public ActionResult Checkout() {
            //User.Identity.Name (a built in asp .net security principle)-> from here we get the loged in email
            Costumer costumer = costumers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            if (costumer != null) {
                Order order = new Order()
                {
                    Email=costumer.Email,
                    City=costumer.City,
                    State=costumer.State,
                    Street=costumer.Street,
                    FirstName=costumer.FirstName,
                    Surname=costumer.LastName,
                    ZipCode=costumer.ZipCode
                };

                return View(order);//for autofilling the user data in the checkout page
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order){

            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            //Just to make sure that we are linking the actual costumer's email with the current order
            order.Email = User.Identity.Name;

            //process payment
            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);
            return RedirectToAction("Thankyou", new { OrderId=order.Id});
        }

        public ActionResult ThankYou(string OrderId){
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}