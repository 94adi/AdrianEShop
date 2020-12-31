using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
namespace AdrianEShop.Core.Services.Category
{
    public interface ICategoryService
    {
        IEnumerable<Models.Category> GetAllCategories();
    }
}
