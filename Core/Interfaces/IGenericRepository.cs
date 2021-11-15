using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);


    }
}