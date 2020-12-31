using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AdrianEShop.Core.Services.Manufacturer
{
    public interface IManufacturerService
    {
        IEnumerable<Models.Manufacturer> GetAllManufacturers();
    }
}
