using System.Linq.Expressions;
using Application.BP.Bases.Service;
using Common.BP.Bases;
using Domain.BP.Bases.Crud;
using Domain.BP.Bases.Mapper;
using Infrastructure.BP.Bases.Entity;
using Microsoft.Extensions.Logging;

namespace Application.BP.Bases.Crud;

using Common.BP.Response;

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
        public DtoListResponse<TDto> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            return new DtoListResponse<TDto>
            {
                Data = this._domainService.Get(filter, orderBy, includeProperties)
            };
        }

        /// <inheritdoc/>
        public DtoListResponse<TDto> GetAll()
        {
            return new DtoListResponse<TDto>
            {
                Data = this._domainService.GetAll()
            };
        }

        /// <inheritdoc/>
        public DtoResponse<TDto> GetById(int id)
        {
            return new DtoResponse<TDto>
            {
                Data = this._domainService.GetById(id)
            };
        }

        /// <inheritdoc/>
        public DtoResponse<TDto> Insert(TDto dto)
        {
            return new DtoResponse<TDto>
            {
                Data = this._domainService.Insert(dto)
            };
        }

        /// <inheritdoc/>
        public BaseResponse Delete(int id)
        {
            this._domainService.Delete(id);
            return new BaseResponse();
        }

        /// <inheritdoc/>
        public BaseResponse Delete(TDto toDelete)
        {
            this._domainService.Delete(toDelete);
            return new BaseResponse();
        }

        /// <inheritdoc/>
        public DtoResponse<TDto> Update(TDto toUpdate)
        {
            return new DtoResponse<TDto>
            {
                Data = this._domainService.Update(toUpdate)
            };
        }
    }