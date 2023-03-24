using System.Linq.Expressions;
using Application.BF.Bases.Service;
using Common.BF.Bases;
using Domain.BF.Bases.Crud;
using Domain.BF.Bases.Mapper;
using Infrastructure.BF.Bases;
using Microsoft.Extensions.Logging;

namespace Application.BF.Bases.Crud;

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
        public CrudAppService(ILogger<AppService> logger, ICrudDomainService<TEntity, TDto, TMapper> domainService) : base(logger)
        {
            this._domainService = domainService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDto>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            return await this._domainService.GetAsync(filter, orderBy, includeProperties);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            return await this._domainService.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<TDto?> GetByIdAsync(int id)
        {
            return await this._domainService.GetByIdAsync(id);
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