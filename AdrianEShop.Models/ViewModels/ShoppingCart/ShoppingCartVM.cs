using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;

namespace AdrianEShop.Models.ViewModels.ShoppingCart
{
    public class ShoppingCartVM
    {
        public IEnumerable<AdrianEShop.Models.ShoppingCart> ListCart { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
