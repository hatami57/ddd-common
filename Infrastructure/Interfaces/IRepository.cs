using DDDCommon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCommon.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> GetByAsync<TId>(TId id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAsync<TKey>(int skip, int take, Expression<Func<T, TKey>> orderBy,
            CancellationToken cancellationToken = default);
        Task<object> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateEntityAsync<TId>(TId entityId, Func<T, Task<bool>> updateFunc,
            CancellationToken cancellationToken = default);
        Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
        Task<long> CountAsync(CancellationToken cancellationToken = default);
    }
}
