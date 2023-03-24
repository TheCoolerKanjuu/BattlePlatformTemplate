namespace Infrastructure.BF.Bases.GenericRepository;

   using System.Linq.Expressions;
    using System.Linq;

    /// <summary>
    /// Generic repository that 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get a list of <see cref="TEntity"/> that match the filter.
        /// </summary>
        /// <param name="filter">The filter applied.</param>
        /// <param name="orderBy">How the object should be ordered.</param>
        /// <param name="includeProperties">What child properties should be included.</param>
        /// <returns>a <see cref="IEnumerable{TEntity}"/></returns>
        public Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        
        /// <summary>
        /// Get a list of <see cref="TEntity"/> that match the filter.
        /// </summary>
        /// <param name="filter">The filter applied.</param>
        /// <param name="orderBy">How the object should be ordered.</param>
        /// <param name="includeProperties">What child properties should be included.</param>
        /// <returns>a <see cref="IEnumerable{TEntity}"/></returns>
        public Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        
        /// <summary>
        /// Get a <see cref="TEntity"/> by it's Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="TEntity"/>.</param>
        /// <returns>The <see cref="TEntity"/> found.</returns>
        public Task<TEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Insert a <see cref="TEntity"/> into db.
        /// </summary>
        /// <param name="entity">the entity to insert.</param>
        public void Insert(TEntity entity);

        /// <summary>
        /// Delete a <see cref="TEntity"/> by its Id.
        /// </summary>
        /// <param name="id">The Id of the entity to delete.</param>
        public void Delete(int id);

        /// <summary>
        /// Delete a <see cref="TEntity"/> of the db.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public void Delete(TEntity entityToDelete);

        /// <summary>
        /// Update a <see cref="TEntity"/> of the db.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        public void Update(TEntity entityToUpdate);
    }