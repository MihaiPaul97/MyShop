using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket:BaseEntity
    {

        //virtual is used for lazy-loading:when loads the basket, entity framework
        //will know to load the items from the basket also
        public virtual ICollection<BasketItem> BasketItems { get; set; }
        public Basket() {
            this.BasketItems = new List<BasketItem>();
        }


    }
}
