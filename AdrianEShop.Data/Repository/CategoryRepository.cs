using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.DataAccess.Data;

namespace AdrianEShop.DataAccess.Repository
{
    public class CategoryRepository : RepositoryAsync<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Category category)
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == category.Id);

            if(categoryFromDb != null)
            {
                categoryFromDb.Name = category.Name;
            }
        }
    }
}
