using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
namespace AdrianEShop.Core.Services.Product
{
    public interface IProductService
    {
        IEnumerable<AdrianEShop.Models.Product> GetAllProducts();
    }
}
