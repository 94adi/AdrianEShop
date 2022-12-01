using AdrianEShop.Core.DAInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services
{
    //defines the basic CRUD operations that can be applied on an entity
    public interface IEntityService<T> where T : class, new()
    {
        public Task AddAsync(T entity);

        public Task<T> GetAsync(Guid id);

        public Task<IEnumerable<T>> GetAllAsync();

        public Task RemoveAsync(Guid id);

    }
}
