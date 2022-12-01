using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Models;
namespace AdrianEShop.Core.Services.Category
{
    public interface ICategoryService : IEntityService<Models.Category>
    {
        Task UpsertAsync(Models.Category category);
        void Save();
    }
}
