using System.Linq.Expressions;
using Application.BP.Bases.Service;
using Common.BP.Bases;
using Domain.BP.Bases.Crud;
using Domain.BP.Bases.Mapper;
using Infrastructure.BP.Bases.Entity;
using Microsoft.Extensions.Logging;

namespace Application.BP.Bases.Crud;

    /// <inheritdoc cref="ICrudAppService{TEntity,TDto,TMapper}"/>
    public class CrudAppService<TEntity, TDto, TMapper> : AppService, ICrudAppService<TEntity, TDto, TMapper> 
        where TEntity : BaseEntity 
        where TDto : BaseDto
        where TMapper : BaseMapper<TDto, TEntity>, new()
    {
        /// <summary>
        /// the injected instance of a crud domain service.
        /// </summary>
        private readonly ICrudDomainService<TEntity, TDto, TMapper> _domainService;

        /// <summary>
        /// Create an instance of a generic crud app service.
        /// </summary>
        /// <param name="logger">logger instance.</param>
        /// <param name="domainService">The instance of a <see cref="CrudDomainService{TEntity,TDto,TMapper}"/>.</param>
        public CrudAppService(ILogger<CrudAppService<TEntity, TDto, TMapper>> logger, ICrudDomainService<TEntity, TDto, TMapper> domainService) : base(logger)
        {
            this._domainService = domainService;
        }

        /// <inheritdoc/>
        public IEnumerable<TDto> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            return this._domainService.Get(filter, orderBy, includeProperties);
        }

        /// <inheritdoc/>
        public IEnumerable<TDto> GetAll()
        {
            return this._domainService.GetAll();
        }

        /// <inheritdoc/>
        public TDto GetById(int id)
        {
            return this._domainService.GetById(id);
        }

        /// <inheritdoc/>
        public TDto Insert(TDto dto)
        {
            return this._domainService.Insert(dto);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            this._domainService.Delete(id);
        }

        /// <inheritdoc/>
        public void Delete(TDto toDelete)
        {
            this._domainService.Delete(toDelete);
        }

        /// <inheritdoc/>
        public TDto Update(TDto toUpdate)
        {
            return this._domainService.Update(toUpdate);
        }
    }