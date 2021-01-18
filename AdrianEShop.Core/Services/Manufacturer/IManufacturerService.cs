using System;
using System.Collections.Generic;

namespace AdrianEShop.Core.Services.Manufacturer
{
    public interface IManufacturerService : IEntityService<Models.Manufacturer>
    {
        void Upsert(Models.Manufacturer manufacturer);
        void Save();
    }
}
