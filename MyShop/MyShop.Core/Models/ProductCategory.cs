using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory
    {
        public String Id { get; set; }
        public String Category { get; set; }

        public ProductCategory() {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}
