using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Order : BaseEntity
    {
        public Order() {
            //make sure that the list items is initialised otherwise, when we will try to add products
            //to the list it will throw an error
            this.OrderItems = new List<OrderItem>();
        }

        //In the order object we will keep the copy of the costumer details
        //We could keep a link to the costumer himself, but sometimes user wants
        //to update personal details within the actual order creation
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string OrderStatus { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }


}
