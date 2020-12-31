using AdrianEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        void Update(Manufacturer manufacturer);
    }
}
