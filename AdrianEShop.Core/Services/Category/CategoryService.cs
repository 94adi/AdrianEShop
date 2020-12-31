using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Core.DAInterfaces;
namespace AdrianEShop.Core.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Models.Category> GetAllCategories()
        {
            return _unitOfWork.Category.GetAll();
        }
    }
}
