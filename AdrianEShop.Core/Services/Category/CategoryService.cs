using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Core.Services.Entity;

namespace AdrianEShop.Core.Services.Category
{
    public class CategoryService : EntityService<Models.Category>, ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork.Category)
        {
            _unitOfWork = unitOfWork;
        }

        public void Upsert(Models.Category category)
        {
            if (category.Id == 0)
            {
                Add(category);

            }
            else
            {
                Update(category);
            }
        }

        public void Save()
        {
            _unitOfWork.Save();
        }

#region private_methods
        private void Update(Models.Category category)
        {
            _unitOfWork.Category.Update(category);
        }
#endregion

    }
}
