using System;
using System.Collections.Generic;

namespace AdrianEShop.Core.Services.Manufacturer
{
    public interface IManufacturerService
    {
        IEnumerable<Models.Manufacturer> GetAllManufacturers();

        Models.Manufacturer GetManufacturer(int id);

        void Upsert(Models.Manufacturer manufacturer);

        void Insert(Models.Manufacturer manufacturer);

        void Update(Models.Manufacturer manufacturer);
    }
}
