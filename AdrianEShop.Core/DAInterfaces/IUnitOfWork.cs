using System;
using System.Collections.Generic;
using System.Text;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Product { get; }

        IManufacturerRepository Manufacturer { get; }

        ICategoryRepository Category { get; }

        IApplicationUserRepository ApplicationUser { get; }

        IShoppingCartRepository ShoppingCart { get; }

        IOrderDetailsRepository OrderDetails { get; }

        IOrderHeaderRepository OrderHeader { get;
        }
        void Save();
    }
}
