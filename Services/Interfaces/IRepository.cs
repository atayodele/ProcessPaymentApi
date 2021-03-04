using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize);
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending);

        IQueryable<TEntity> GetAllString(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties);

        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        TEntity GetSingle(Guid id);
        TEntity GetSingleIncluding(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetAllAsync();

        Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByIdIncludingAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        TEntity GetById(object id);
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
