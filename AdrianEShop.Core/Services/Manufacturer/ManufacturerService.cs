using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Core.Services.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.Manufacturer
{
    public class ManufacturerService : EntityService<Models.Manufacturer>, IManufacturerService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ManufacturerService(IUnitOfWork unitOfWork) : base(unitOfWork.Manufacturer)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpsertAsync(Models.Manufacturer manufacturer)
        {
            if (manufacturer.Id == Guid.Empty)
            {
                await AddAsync(manufacturer);

            }
            else
            {
                await UpdateAsync (manufacturer);
            }
        }

        public async Task RemoveAsync(Models.Manufacturer manufacturer)
        {
            await _unitOfWork.Manufacturer.RemoveAsync(manufacturer);
        }

        private async Task UpdateAsync(Models.Manufacturer manufacturer)
        {
             await _unitOfWork.Manufacturer.UpdateAsync(manufacturer);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}
