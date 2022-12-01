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

        private readonly IRepositoryAsync<T> _repository;

        public EntityService(IRepositoryAsync<T> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var objToDelete = await GetAsync(id);
            await _repository.RemoveAsync(objToDelete);
        }

    }
}
