using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.OrderHeader
{
    public interface IOrderHeaderService
    {
        IEnumerable<Models.OrderHeader> GetAllOrders(string includeProperties = null);

        IEnumerable<Models.OrderHeader> GetAllOrders(string id, string includeProperties = null);

        IEnumerable<Models.OrderHeader> GetAllOrders(ClaimsPrincipal User, string includeProperties = null);

        Models.OrderHeader GetOrder(string id);

        Models.OrderHeader GetOrder(string id, string includeProperties = null);

        void Upsert(Models.OrderHeader orderHeader);

        void Remove(Models.OrderHeader orderHeader);

        void Save();
    }
}
