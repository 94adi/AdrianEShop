using AdrianEShop.Core.DAInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.Entity
{
    //implements generic functionality for basic CRUD operations on an entity
    public class EntityService<T> : IEntityService<T> where T : class, new()
    {

        private readonly IRepository<T> _repository;

        public EntityService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public void Add(T entity)
        {
            _repository.Add(entity);
        }

        public T Get(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public void Remove(int id)
        {
            _repository.Remove(id);
        }

    }
}
