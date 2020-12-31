using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.Product
{
    public interface IProductService
    {
        IEnumerable<Models.Product> GetAllProducts();

        Models.Product GetProduct(int id);

    }
}
