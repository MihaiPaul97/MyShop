using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        //for accessing the specific data tables from the database
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        //this string is not necessarely required,however when we need to write some cookies
        //we use a string to identify a particular cookie we want and reference this
        //cookie using this string whenever we want to store or load in cookie
        public const string BasketSessionName = "eCommerceBasket";


        //for injecting SQLRepositories of Products and Baksets
        public BasketService(IRepository<Product> ProductContext,IRepository<Basket> BasketContext) {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        //we want to read the user's cookies from httpContext looking for the basketId
        //and to attempt to read that baksetId from Database
        //bool createIfNUll - is the flag that tells us if there is no basket, to create ONE
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNUll) {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null) {//if user has vizited before the requested page,the cookie already exists
                string basketId = cookie.Value;//get the value from this particular cookie
                if (!string.IsNullOrEmpty(basketId)) {
                    //load the basket from the db if basketid exists;
                    basket = basketContext.Find(basketId);
                }
                else{//if the basket does not exist
                    if (createIfNUll) {//check if we want to create one
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else{//if cookie was NULL
                if (createIfNUll){//check if we want to create one
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext) {
            Basket basket = new Basket();//create a new basket
            basketContext.Insert(basket);//insert it into the db
            basketContext.Commit();

            //write the cookie to the user's browser
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);//this can be set flexible - cookie expire in 1 day
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId) {
            Basket basket = GetBasket(httpContext, true);
            //check if there is already a basket item in the user's basket with the same productid
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                basket.BasketItems.Add(item);
            }
            else {
                item.Quantity++;
            }

            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId) {
            Basket basket = GetBasket(httpContext, true);
            //check if there is already a basket item in the user's basket with the same productid
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null) {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }

        }
       
    }
}
