using DDDCommon.Domain.Interfaces;
using DDDCommon.Domain.Types;
using DDDCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Remotion.Linq.Clauses.ResultOperators;

namespace DDDCommon.Infrastructure.Types
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly ISessionFactory SessionFactory;

        public Repository(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        public virtual async Task<T> GetByAsync<TId>(TId id, CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
                return await session.GetAsync<T>(id, cancellationToken);
        }

        public virtual async Task<List<T>> GetByAsync<TKey>(int skip, int take, Expression<Func<T, bool>> where,
            Expression<Func<T, TKey>> orderBy, CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
                return await session.Query<T>().Where(where).OrderBy(orderBy).Skip(skip).Take(take)
                    .ToListAsync(cancellationToken);
        }

        public virtual async Task<List<T>> GetAsync<TKey>(int skip, int take, Expression<Func<T, TKey>> orderBy,
            CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
                return await session.Query<T>().OrderBy(orderBy).Skip(skip).Take(take)
                    .ToListAsync(cancellationToken);
        }

        public virtual async Task<object> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var res = await session.SaveAsync(entity, cancellationToken);
                await tx.CommitAsync(cancellationToken);
                return res;
            }
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                await session.UpdateAsync(entity, cancellationToken);
                await tx.CommitAsync(cancellationToken);
            }
        }
        
        public virtual async Task UpdateEntityAsync<TId>(TId entityId, Func<T, Task<bool>> updateFunc,
            CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var entity = await session.GetAsync<T>(entityId, cancellationToken);
                if (!cancellationToken.IsCancellationRequested && await updateFunc(entity))
                {
                    await session.UpdateAsync(entity, cancellationToken);
                    await tx.CommitAsync(cancellationToken);
                }
            }
        }

        public virtual async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                await session.DeleteAsync(entity, cancellationToken);
                await tx.CommitAsync(cancellationToken);
            }
        }

        public virtual async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            using (var session = SessionFactory.OpenSession())
                return await session.Query<T>().LongCountAsync(cancellationToken: cancellationToken);
        }
    }
}
