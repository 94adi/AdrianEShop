using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.Product
{
    public interface IProductService
    {
        IEnumerable<Models.Product> GetAllProducts(string includeProperties = null);

        Models.Product GetProduct(Guid id);

        Models.Product GetProduct(Guid id, string includeProperties = null);

        void Upsert(Models.Product product);

        void Remove(Models.Product product);

        void Save();

    }
}
