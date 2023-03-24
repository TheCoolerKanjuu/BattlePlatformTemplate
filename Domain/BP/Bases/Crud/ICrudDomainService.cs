using System.Linq.Expressions;
using Common.BP.Bases;
using Domain.BP.Bases.Mapper;
using Domain.BP.Bases.Service;
using Infrastructure.BP.Bases.Entity;

namespace Domain.BP.Bases.Crud;

    /// <summary>
    /// Generic crud domain service.
    /// You can either instantiate if you want to use a basic crud, or inherit it and override some / all of its methods to add custom behaviours.
    /// Do not call it directly in the controller, pass through an AppService.
    /// </summary>
    /// <typeparam name="TEntity">The entity fetched by your service (aka table in db).</typeparam>
    /// <typeparam name="TDto">The DTO returned by the service to the controller that call it.</typeparam>
    /// <typeparam name="TMapper">The mapper that convert the entity to the specified DTO.</typeparam>
    public interface ICrudDomainService<TEntity, TDto, TMapper> : IDomainService 
        where TEntity : BaseEntity
        where TDto : BaseDto
        where TMapper : BaseMapper<TDto, TEntity>
    {

        /// <summary>
        /// Fetch a list of <see cref="TEntity"/> from custom filter, a specific order and some of its properties.
        /// Be sure to read the doc in microsoft website, i'm not totally sure how it works.
        /// </summary>
        /// <param name="filter">Linq function that expresses a filter, applied on the db</param>
        /// <param name="orderBy">Linq function that expresses an order, applied to the return of the request.</param>
        /// <param name="includeProperties">string containing all properties.</param>
        /// <returns>A <see cref="IEnumerable{TDTO}"/>.</returns>
        public IEnumerable<TDto> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        
        /// <summary>
        /// Fetch a list of <see cref="TEntity"/> from custom filter, a specific order and some of its properties without tracking.
        /// Be sure to read the doc in microsoft website, i'm not totally sure how it works.
        /// </summary>
        /// <param name="filter">Linq function that expresses a filter, applied on the db</param>
        /// <param name="orderBy">Linq function that expresses an order, applied to the return of the request.</param>
        /// <param name="includeProperties">string containing all properties.</param>
        /// <returns>A <see cref="IEnumerable{TDTO}"/>.</returns>
        public IEnumerable<TDto> GetAsNoTracking(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Fetch all <see cref="TEntity"/> then map and return them as <see cref="TDto"/>
        /// </summary>
        /// <returns>A List of <see cref="TDto"/>.</returns>
        public IEnumerable<TDto> GetAll();

        /// <summary>
        /// Fetch a <see cref="TEntity"/> then map and return it as <see cref="TDto"/>
        /// </summary>
        /// <param name="id">The Id of the <see cref="TDto"/> to fetch.</param>
        /// <returns>A <see cref="TDto"/></returns>
        public TDto GetById(int id);

        /// <summary>
        /// Map a <see cref="TDto"/> into a <see cref="TEntity"/> and insert it into the localdb.
        /// </summary>
        /// <param name="dto">the data of the <see cref="TEntity"/> to be inserted.</param>
        public TDto Insert(TDto dto);

        /// <summary>
        /// Delete a <see cref="TEntity"/> from local db.
        /// </summary>
        /// <param name="id">The Id of the <see cref="TDto"/> to be deleted.</param>
        public void Delete(int id);

        /// <summary>
        /// Delete a <see cref="TEntity"/> from local db.
        /// Prefer the usage of <see cref="Delete(int)"/>.
        /// </summary>
        /// <param name="toDelete"><see cref="TDto"/> equivalent of the <see cref="TEntity"/> to delete.</param>
        public void Delete(TDto toDelete);

        /// <summary>
        /// Map a <see cref="TDto"/> into a <see cref="TEntity"/> and update it in the localdb.
        /// </summary>
        /// <param name="toUpdate">the data of the <see cref="TEntity"/> to be updated.</param>
        public TDto Update(TDto toUpdate);
}