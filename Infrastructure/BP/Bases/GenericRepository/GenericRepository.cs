using System.Linq.Expressions;
using Common.BP.Exceptions;
using Infrastructure.BP.Bases.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BP.Bases.GenericRepository;

using Common.BP.Exceptions.Entity;
using Migrations;

/// <inheritdoc/>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// The table operation manager of the db.
        /// </summary>
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger<IGenericRepository<TEntity>> _logger;

        /// <summary>
        /// Context class instance.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Create the instance of the repository for <see cref="TEntity"/>.
        /// </summary>
        /// <param name="context">The context of the Db.</param>
        /// <param name="logger">logger instance.</param>
        public GenericRepository(DataContext context, ILogger<GenericRepository<TEntity>> logger)
        {
            this._dbSet = context.Set<TEntity>();
            this._logger = logger;
            this._context = context;
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this._dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }
        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this._dbSet;

            if (filter != null)
                query = query.Where(filter).AsNoTracking();

            query = includeProperties.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        /// <inheritdoc/>
        public TEntity GetById(int id)
        {
            var entity = this._dbSet.Find(id);
            if (entity != null)
            {
                this._logger.LogInformation("Fetched {TEntity} of id {Id} at {DT}", typeof(TEntity), id, DateTime.UtcNow.ToLongTimeString());
                return entity;
            }

            this._logger.LogWarning("No {TEntity} of id {Id} found", typeof(TEntity), id);
            throw new EntityNotFoundException($"No {typeof(TEntity)} of Id {id} found.");
        }

        /// <inheritdoc/>
        public void Insert(TEntity entity)
        {
            this._logger.LogInformation("{TEntity} of id {Id} to be inserted at {DT}", typeof(TEntity), entity.Id, DateTime.UtcNow.ToLongTimeString());
            this._dbSet.Add(entity);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            var entityToDelete = this._dbSet.Find(id);

            if (entityToDelete != null)
            {
                this.Delete(entityToDelete);
            }
            else
            {
                this._logger.LogWarning("Won't delete entity of type {TEntity} because it doesn't exist in database", typeof(TEntity));
                throw new EntityNotFoundException($"No {typeof(TEntity)} of Id {id} found.");
            }
        }

        /// <inheritdoc/>
        public void Delete(TEntity entityToDelete)
        {
            if (this._dbSet.Entry(entityToDelete).State == EntityState.Detached)
            {
                this._dbSet.Attach(entityToDelete);
            }

            this._dbSet.Remove(entityToDelete);
            this._logger.LogInformation(" {TEntity} Id: {Id} to be deleted", typeof(TEntity), entityToDelete.Id);
        }

        /// <inheritdoc/>
        public void Update(TEntity entityToUpdate)
        {
            var existingEntity = this._dbSet.Find(entityToUpdate.Id);
            
            if (existingEntity == null)
            {
                throw new EntityNotFoundException($"No {typeof(TEntity)} of Id {entityToUpdate.Id} found.");
            }

            this._dbSet.Attach(entityToUpdate);
            this._context.Entry(entityToUpdate).State = EntityState.Modified;
            
            this._logger.LogInformation("{TEntity} Id: {Id} to be updated", typeof(TEntity), entityToUpdate.Id);
        }
    }