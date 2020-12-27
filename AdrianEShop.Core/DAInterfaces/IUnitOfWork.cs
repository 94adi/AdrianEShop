using System;
using System.Collections.Generic;
using System.Text;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Product { get; }

        void Save();
    }
}
