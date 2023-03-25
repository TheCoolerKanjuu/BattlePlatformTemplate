namespace Presentation.BP.Bases;

using Application.BP.Bases.Caching;
using Application.BP.Bases.Crud;
using Common.BP.Bases;
using Common.BP.Response;
using Domain.BP.Bases.Mapper;
using Infrastructure.BP.Bases.Entity;
using Microsoft.AspNetCore.Mvc;

public abstract class CachedCrudController<TEntity, TDto, TMapper> : CrudController<TEntity, TDto, TMapper>
        where TEntity : BaseEntity
        where TDto : BaseDto, new()
        where TMapper : BaseMapper<TDto, TEntity>, new()
{
        private readonly IApiCachingAppService<DtoListResponse<TDto>> _getAllCacheService;
        
        private readonly IApiCachingAppService<DtoResponse<TDto>> _getByIdCacheService;

        protected CachedCrudController(
                ILogger<CachedCrudController<TEntity, TDto, TMapper>> logger, 
                ICrudAppService<TEntity, TDto, TMapper> crudAppService,
                IApiCachingAppService<DtoListResponse<TDto>> getAllCacheService,
                IApiCachingAppService<DtoResponse<TDto>> getByIdCacheService
        ) 
                : base(logger, crudAppService)
        {
                this._getAllCacheService = getAllCacheService;
                this._getByIdCacheService = getByIdCacheService;
        }

        public override IActionResult GetAll()
        {
                this.Logger.LogInformation("[GET] /{Controller} hit at {DT}", typeof(TEntity), DateTime.UtcNow.ToLongTimeString());
                var route = $"/{typeof(TEntity)}";
                return this.Ok(this._getAllCacheService.Cache(this.CrudAppService.GetAll,route));
        }

        public override IActionResult GetById(int id)
        {
                this.Logger.LogInformation("[GET] /{Controller} hit at {DT}", typeof(TEntity), DateTime.UtcNow.ToLongTimeString());
                var route = $"/{typeof(TEntity)}";
                return this.Ok(this._getByIdCacheService.Cache(this.CrudAppService.GetById, id, route));
        }
}