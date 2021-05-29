using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        Models.ShoppingCart GetCurrentCart(string userId, Guid productId);

        void Upsert(Models.ShoppingCart cart);

        int GetProductsCount(string userId);

        void Save();
    }
}
