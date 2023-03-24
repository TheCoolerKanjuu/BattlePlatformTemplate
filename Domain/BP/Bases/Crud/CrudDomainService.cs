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
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Instantiate an new instance of the mapper.
        /// Not satisfied the way it is now, might change it to use static method instead of instance later.
        /// </summary>
        private readonly TMapper _mapper = new();
        
        /// <summary>
        /// Instantiate the repository of the corresponding entity.
        /// </summary>
        private readonly IGenericRepository<TEntity> _repository;

        /// <summary>
        /// Create an instance of a generic crud domain service.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="unitOfWork">The instance of a <see cref="IUnitOfWork"/>.</param>
        public CrudDomainService(ILogger<CrudDomainService<TEntity, TDto, TMapper>> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            this._unitOfWork = unitOfWork;
            this._repository = this._unitOfWork.GetRepository<TEntity>();
        }


        /// <inheritdoc/>
        public IEnumerable<TDto> Get(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            var results = this._repository.Get(filter, orderBy, includeProperties);
            var ret = results.Select(x => this._mapper.EntityToDto(x)).ToList();

            return ret;
        }

        /// <inheritdoc/>
        public IEnumerable<TDto> GetAsNoTracking(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            var results = this._repository.GetAsNoTracking(filter, orderBy, includeProperties);
            var ret = results.Select(x => this._mapper.EntityToDto(x)).ToList();
            
            return ret;
        }

        /// <inheritdoc/>
        public IEnumerable<TDto> GetAll()
        {
            var results = this._repository.Get();
            return results.Select(x => this._mapper.EntityToDto(x)).ToList();
        }

        /// <inheritdoc/>
        public TDto GetById(int id)
        {
            return this._mapper.EntityToDto(this._repository.GetById(id));
        }

        /// <inheritdoc/>
        public TDto Insert(TDto dto)
        {
            var entity = this._mapper.DtoToEntity(dto);
            
            this._repository.Insert(entity);
            this._unitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} inserted", typeof(TEntity), dto.Id);

            return this._mapper.EntityToDto(entity);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            this._repository.Delete(id);
            this._unitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} deleted", typeof(TEntity), id);

        }

        /// <inheritdoc/>
        public void Delete(TDto toDelete)
        {
            var entity = this._mapper.DtoToEntity(toDelete);

            this._repository.Delete(entity);
            this._unitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} deleted", typeof(TEntity), toDelete.Id);

        }

        /// <inheritdoc/>
        public TDto Update(TDto toUpdate)
        {
            var entity = this._mapper.DtoToEntity(toUpdate);
            
            this._repository.Update(entity);
            this._unitOfWork.Save();
            Logger.LogInformation(" {TEntity} Id: {Id} updated", typeof(TEntity), toUpdate.Id);
            
            return this._mapper.EntityToDto(entity);
        }
    }