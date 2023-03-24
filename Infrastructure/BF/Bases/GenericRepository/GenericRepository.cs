using System.Linq.Expressions;
using Common.BF.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BF.Bases.GenericRepository;

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
        private readonly ILogger _logger;

        /// <summary>
        /// Create the instance of the repository for <see cref="TEntity"/>.
        /// </summary>
        /// <param name="context">The context of the Db.</param>
        /// <param name="logger">logger instance.</param>
        public GenericRepository(DbContext context, ILogger logger)
        {
            this._dbSet = context.Set<TEntity>();
            this._logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this._dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            var result = orderBy != null ? orderBy(query).ToList() : query.ToList();
            var count = result.Count;

            // Log the number of entities found
            this._logger.LogInformation("Fetched {Count} {TEntity} at {DT}.", count, typeof(TEntity), DateTime.UtcNow.ToLongTimeString());

            return await Task.FromResult(result);
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this._dbSet;

            if (filter != null)
            {
                query = query.Where(filter).AsNoTracking();
            }

            query = includeProperties.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            var result = orderBy != null ? orderBy(query).ToList() : query.ToList();
            var count = result.Count;

            // Log the number of entities found
            this._logger.LogInformation("Fetched {Count} {TEntity} at {DT}.", count, typeof(TEntity), DateTime.UtcNow.ToLongTimeString());

            return await Task.FromResult(result);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await this._dbSet.FindAsync(id);
            if (entity != null)
            {
                this._logger.LogInformation("Fetched {TEntity} of id {Id} at {DT}.", typeof(TEntity), id, DateTime.UtcNow.ToLongTimeString());
                return entity;
            }

            this._logger.LogWarning("No {TEntity} of id {Id} found", typeof(TEntity), id);
            throw new EntityNotFoundException($"No {typeof(TEntity)} of Id {id} found.");
        }

        /// <inheritdoc/>
        public void Insert(TEntity entity)
        {
            this._logger.LogInformation("{TEntity} of id {Id} to be inserted at {DT}.", typeof(TEntity), entity.Id, DateTime.UtcNow.ToLongTimeString());
            this._dbSet.Entry(entity).State = EntityState.Added;
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
                this._logger.LogWarning("Won't delete entity of type {TEntity} because it doesn't exist in database.", typeof(TEntity));
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
            this._logger.LogInformation(" {TEntity} Id: {Id} to be deleted.", typeof(TEntity), entityToDelete.Id);
        }

        /// <inheritdoc/>
        public void Update(TEntity entityToUpdate)
        {
            // Check if the entity exists in the database
            var existingEntity = this._dbSet.Find(entityToUpdate.Id);
            if (existingEntity == null)
            {
                throw new EntityNotFoundException($"No {typeof(TEntity)} of Id {entityToUpdate.Id} found.");
            }

            // Update the entity
            this._dbSet.Entry(existingEntity).CurrentValues.SetValues(entityToUpdate);
            this._logger.LogInformation("{TEntity} Id: {Id} updated.", typeof(TEntity), entityToUpdate.Id);
        }
    }