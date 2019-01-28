using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;

namespace MINDOnContainers.Vif.API.Data
{

    /// <summary>
    /// Provides CRUD access to the repository.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal SigmaContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(SigmaContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            Func<IQueryable<TEntity>, IQueryable<TEntity>> query = null,
            bool AsTrackable = true)
        {
            IQueryable<TEntity> q = dbSet;

            if (filter != null)
            {
                q = q.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                q = q.Include(includeProperty);
            }

            if (query != null)
            {
                q = query.Invoke(q);
            }

            if (! AsTrackable)
            {
                q = q.AsNoTracking();
            }

            if (orderBy != null)
            {
                return await orderBy(q).ToListAsync();
            }
            else
            {
                return await q.ToListAsync();
            }
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Insert(IList<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}