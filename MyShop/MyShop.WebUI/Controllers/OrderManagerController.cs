using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class OrderManagerController : Controller
    {
        IOrderService orderService;

        public OrderManagerController(IOrderService OrderService)
        {
            this.orderService = OrderService;
        }
        // GET: Order
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            return View(orders);
        }

        //Actual page
        public ActionResult UpdateOrder(string Id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };
            Order order = orderService.GetOrder(Id);
            return View(order);
        }
        //The page that is being updated
        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string Id) {
            Order order = orderService.GetOrder(Id);
            order.OrderStatus = updatedOrder.OrderStatus;
            orderService.UpdateOrder(order);

            return RedirectToAction("Index");
        }
    }
}