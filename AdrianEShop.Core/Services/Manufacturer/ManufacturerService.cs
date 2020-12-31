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
    }
}
