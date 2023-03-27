namespace Presentation.BP.Bases;

using System.Text;
using Application.BP.Bases.Caching;
using Application.BP.Bases.Crud;
using Common.BP.Bases;
using Common.BP.Helpers;
using Common.BP.Request;
using Common.BP.Response;
using Domain.BP.Bases.Mapper;
using Infrastructure.BP.Bases.Entity;
using Microsoft.AspNetCore.Mvc;

public abstract class CachedCrudController<TEntity, TDto, TMapper> : CrudController<TEntity, TDto, TMapper>
        where TEntity : BaseEntity
        where TDto : BaseDto, new()
        where TMapper : BaseMapper<TDto, TEntity>, new()
{
        private readonly IApiCachingAppService<DtoListResponse<TDto>> _listCacheService;
        
        private readonly IApiCachingAppService<DtoResponse<TDto>> _cacheService;

        private readonly string _baseRoute;

        protected CachedCrudController(
                ILogger<CachedCrudController<TEntity, TDto, TMapper>> logger, 
                ICrudAppService<TEntity, TDto, TMapper> crudAppService,
                IApiCachingAppService<DtoListResponse<TDto>> listCacheService,
                IApiCachingAppService<DtoResponse<TDto>> cacheService
        ) 
                : base(logger, crudAppService)
        {
                this._listCacheService = listCacheService;
                this._cacheService = cacheService;
                this._baseRoute = ControllerContext.ActionDescriptor.ControllerName;
        }

        public override IActionResult GetAll()
        {
                var route = $"{_baseRoute}/";
                this.Logger.LogInformation("[GET] /{Controller} hit at {DT}", typeof(TEntity), DateTime.UtcNow.ToLongTimeString());

                var cacheKey = HashRequest.BuildAnonymousKey("Get", route);
                
                return this.Ok(this._listCacheService.Cache(this.CrudAppService.GetAll,cacheKey));
        }

        public override IActionResult GetById(int id)
        {
                var route = $"{_baseRoute}/{id}";
                this.Logger.LogInformation("[GET] {Route} hit at {DT}", route, DateTime.UtcNow.ToLongTimeString());

                var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
                var cacheKey = HashRequest.BuildAnonymousKey("Get", route, reader.ReadToEnd());
                
                return this.Ok(this._cacheService.Cache(this.CrudAppService.GetById, cacheKey, id));
        }

        public override IActionResult Get(LazyLoadRequest request)
        {
                var route = $"{_baseRoute}/get";
                this.Logger.LogInformation("[Post] {Route} hit at {DT}", route, DateTime.UtcNow.ToLongTimeString());

                var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
                var cacheKey = HashRequest.BuildAnonymousKey("Post", route, reader.ReadToEnd());

                return this.Ok(this._listCacheService.Cache(this.CrudAppService.Get, cacheKey, request.Filter, request.OrderBy));
        }
}