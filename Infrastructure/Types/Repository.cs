using DDDCommon.Domain.Interfaces;
using DDDCommon.Domain.Types;
using DDDCommon.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCommon.Infrastructure.Types
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly PostgreSqlDbContext DbContext;

        public Repository(PostgreSqlDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual Task<T> GetBy<TId>(TId id)
        {
            return DbSet.SingleOrDefaultAsync(e => (e as Entity<TId>).Id.Equals(id));
        }

        public DbSet<T> DbSet => DbContext.Set<T>();

        public virtual Task<List<T>> Get(int skip, int take)
        {
            return DbSet.Skip(skip).Take(take).ToListAsync();
        }

        public virtual Task<int> Add(T entity)
        {
            DbSet.Add(entity);
            return DbContext.SaveChangesAsync();
        }

        public virtual Task<int> Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            return DbContext.SaveChangesAsync();
        }

        public virtual Task<int> Remove(T entity)
        {
            DbSet.Remove(entity);
            return DbContext.SaveChangesAsync();
        }
    }
}
