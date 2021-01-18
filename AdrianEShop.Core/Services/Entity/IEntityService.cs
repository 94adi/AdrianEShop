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
        public void Add(T entity);

        public T Get(int id);

        public IEnumerable<T> GetAll();

        public void Remove(int id);

    }
}
