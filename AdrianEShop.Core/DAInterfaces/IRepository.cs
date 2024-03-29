﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(Guid id);

        T Get(Guid id, Expression<Func<T, bool>> filter, string includeProperties);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );

        void Add(T entity);
        void Remove(T entity);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> entities);
    }
}
