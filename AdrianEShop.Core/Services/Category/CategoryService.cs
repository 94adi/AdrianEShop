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

        public async Task UpsertAsync(Models.Category category)
        {
            if (category.Id == Guid.Empty)
            {
                AddAsync(category);

            }
            else
            {
                UpdateAsync(category);
            }
        }

        public void Save()
        {
            _unitOfWork.Save();
        }

#region private_methods
        private async Task UpdateAsync(Models.Category category)
        {
            await _unitOfWork.Category.UpdateAsync(category);
        }
#endregion

    }
}
