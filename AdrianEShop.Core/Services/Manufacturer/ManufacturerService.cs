using AdrianEShop.Core.DAInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.Manufacturer
{
    public class ManufacturerService : IManufacturerService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ManufacturerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Models.Manufacturer> GetAllManufacturers()
        {
            return _unitOfWork.Manufacturer.GetAll();
        }

        public Models.Manufacturer GetManufacturer(int id)
        {
            return _unitOfWork.Manufacturer.Get(id);
        }

        public void Insert(Models.Manufacturer manufacturer)
        {
            _unitOfWork.Manufacturer.Add(manufacturer);
        }

        public void Update(Models.Manufacturer manufacturer)
        {
            _unitOfWork.Manufacturer.Update(manufacturer);
        }

        public void Upsert(Models.Manufacturer manufacturer)
        {
            if (manufacturer.Id == 0)
            {
                Insert(manufacturer);

            }
            else
            {
                Update(manufacturer);
            }
            _unitOfWork.Save();
        }
    }
}
