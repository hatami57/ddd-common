using DDDCommon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDDCommon.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> GetBy<TId>(TId id);
        Task<List<T>> Get(int skip, int take);
        Task<int> Add(T entity);
        Task<int> Update(T entity);
        Task<int> Remove(T entity);
    }
}
