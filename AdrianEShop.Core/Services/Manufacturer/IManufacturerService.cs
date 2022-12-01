using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.Manufacturer
{
    public interface IManufacturerService : IEntityService<Models.Manufacturer>
    {
        Task UpsertAsync(Models.Manufacturer manufacturer);
        void Save();
    }
}
