using System.Linq.Expressions;
using Common.BP.Bases;
using Domain.BP.Bases.Mapper;
using Domain.BP.Bases.Service;
using Infrastructure.BP.Bases.Entity;
using Infrastructure.BP.Bases.GenericRepository;
using Infrastructure.BP.Bases.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Domain.BP.Bases.Crud;

    /// <summary>
    /// Base class for crud service on domain layer.
    /// </summary>
    /// <typeparam name="TEntity">The Entity to be fetched from DB.</typeparam>
    /// <typeparam name="TDto">The return format</typeparam>
    /// <typeparam name="TMapper">The mapper that will convert them.</typeparam>
    public class CrudDomainService<TEntity, TDto, TMapper> : DomainService, ICrudDomainService<TEntity, TDto, TMapper>
        where TEntity : BaseEntity
        where TDto : BaseDto, new()
        where TMapper : BaseMapper<TDto, TEntity>, new()
    {
        /// <summary>
        /// The unit of work instance. too long to explain. Ask google the role of this pattern.
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Instantiate an new instance of the mapper.
        /// Not satisfied the way it is now, might change it to use static method instead of instance later.
        /// </summary>
        protected readonly TMapper Mapper = new();
        
        /// <summary>
        /// Instantiate the repository of the corresponding entity.
        /// </summary>
        protected IGenericRepository<TEntity> Repository;

        /// <summary>
        /// Create an instance of a generic crud domain service.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="unitOfWork">The instance of a <see cref="IUnitOfWork"/>.</param>
        public CrudDomainService(ILogger<DomainService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            this.UnitOfWork = unitOfWork;
            this.Repository = this.UnitOfWork.GetRepository<TEntity>();
        }


        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TDto>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            var results = await this.Repository.GetAsync(filter, orderBy, includeProperties);
            var ret = results.Select(x => this.Mapper.EntityToDto(x)).ToList();

            return ret;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDto>> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            var results = await this.Repository.GetAsNoTrackingAsync(filter, orderBy, includeProperties);
            var ret = results.Select(x => this.Mapper.EntityToDto(x)).ToList();
            
            return ret;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var results = await this.Repository.GetAsync();
            return results.Select(x => this.Mapper.EntityToDto(x)).ToList();
        }

        /// <inheritdoc/>
        public async Task<TDto?> GetByIdAsync(int id)
        {
            return this.Mapper.EntityToDto(await this.Repository.GetByIdAsync(id) ??
                                           throw new InvalidOperationException());
        }

        /// <inheritdoc/>
        public TDto Insert(TDto dto)
        {
            var entity = this.Mapper.DtoToEntity(dto);
            
            this.Repository.Insert(entity);
            this.UnitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} inserted.", typeof(TEntity), dto.Id);

            return this.Mapper.EntityToDto(entity);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            this.Repository.Delete(id);
            this.UnitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} deleted.", typeof(TEntity), id);

        }

        /// <inheritdoc/>
        public void Delete(TDto toDelete)
        {
            var entity = this.Mapper.DtoToEntity(toDelete);

            this.Repository.Delete(entity);
            this.UnitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} deleted.", typeof(TEntity), toDelete.Id);

        }

        /// <inheritdoc/>
        public TDto Update(TDto toUpdate)
        {
            var entity = this.Mapper.DtoToEntity(toUpdate);
            
            this.Repository.Update(entity);
            this.UnitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} updated.", typeof(TEntity), toUpdate.Id);
            
            return this.Mapper.EntityToDto(entity);
        }
    }