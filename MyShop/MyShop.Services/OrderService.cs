﻿using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService:IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach(var item in basketItems){
                baseOrder.OrderItems.Add(new OrderItem() {
                    ProductId=item.Id,
                    Image=item.Image,
                    Price=item.Price,
                    ProductName=item.ProductName,
                    Quantity=item.Quantity
                });
            }

            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }

        //Return the list of orders
        public List<Order> GetOrderList() {
            return orderContext.Collection().ToList();
        }

        //Return individual order
        public Order GetOrder(string Id) {
            return orderContext.Find(Id);
        }

        //Update individual order, JUST the Status of the order
        public void UpdateOrder(Order updatedOrder)
        {
            orderContext.Update(updatedOrder);
            orderContext.Commit();
        }
    }
}
