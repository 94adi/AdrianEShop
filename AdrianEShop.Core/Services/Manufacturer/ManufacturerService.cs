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

        public void Upsert(Models.Manufacturer manufacturer)
        {
            if (manufacturer.Id == 0)
            {
                Add(manufacturer);

            }
            else
            {
                Update(manufacturer);
            }
        }

        public void Remove(Models.Manufacturer manufacturer)
        {
            _unitOfWork.Manufacturer.Remove(manufacturer);
        }

        private void Update(Models.Manufacturer manufacturer)
        {
            _unitOfWork.Manufacturer.Update(manufacturer);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}
