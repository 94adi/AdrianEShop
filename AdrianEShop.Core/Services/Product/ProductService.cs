using AdrianEShop.Core.DAInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;

namespace AdrianEShop.Core.Services.Product
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Models.Product> GetAllProducts(string includeProperties = null)
        {
            string prop = includeProperties;
            return _unitOfWork.Product.GetAll(includeProperties: prop);
        }

        public Models.Product GetProduct(Guid id)
        {
            return _unitOfWork.Product.Get(id);
        }

        public void Upsert(Models.Product product)
        {
            if(product.Id == Guid.Empty)
            {
                _unitOfWork.Product.Add(product);
            }
            else
            {
                _unitOfWork.Product.Update(product);
            }
        }
        public void Remove(Models.Product product)
        {
            _unitOfWork.Product.Remove(product);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}
