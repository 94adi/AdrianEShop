using AdrianEShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
    
}
