using Application.BP.Bases.Crud;
using Common.BP.Bases;
using Common.BP.Exceptions;
using Domain.BP.Bases.Mapper;
using Infrastructure.BP.Bases.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.BP.Bases;

using Common.BP.Exceptions.Entity;

/// <summary>
    /// Generic controller class.
    /// </summary>
    /// <typeparam name="TEntity">The entity to fetch.</typeparam>
    /// <typeparam name="TDto">The out format</typeparam>
    /// <typeparam name="TMapper">The mapping between the entity and the out format.</typeparam>
    public abstract class CrudController<TEntity, TDto, TMapper> : BaseController
    where TEntity : BaseEntity
    where TDto : BaseDto, new()
    where TMapper : BaseMapper<TDto, TEntity>, new()
    {
        /// <summary>
        /// The Instance of the service responsible for the crud operations.
        /// </summary>
        protected readonly ICrudAppService<TEntity, TDto, TMapper> CrudAppService;

        /// <summary>
        /// DI constructor.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="crudAppService">The crud service instance.</param>
        protected CrudController(ILogger<CrudController<TEntity, TDto, TMapper>> logger, ICrudAppService<TEntity, TDto, TMapper> crudAppService) : base(logger)
        {
            this.CrudAppService = crudAppService;
        }
        
        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>All entities as <see cref="TDto"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual IActionResult GetAll()
        {
            this.Logger.LogInformation("[GET] /{Controller} hit at {DT}", typeof(TEntity), DateTime.UtcNow.ToLongTimeString());
            return this.Ok(this.CrudAppService.GetAll());
        }
        
        /// <summary>
        /// Get one entity by Id.
        /// </summary>
        /// <param name="id">The id of the entity to fetch.</param>
        /// <returns>the entity as a <see cref="TDto"/></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual IActionResult GetById(int id)
        {
            this.Logger.LogInformation("[GET] /{Controller}/{Id} hit at {DT}", typeof(TEntity), id, DateTime.UtcNow.ToLongTimeString());

            try
            {
                return this.Ok(this.CrudAppService.GetById(id));
            }
            catch (EntityNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Insert a dto into db.
        /// </summary>
        /// <param name="dto">The db to insert.</param>
        /// <returns>The dto inserted with db values.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual IActionResult Insert(TDto dto)
        {
            this.Logger.LogInformation("[POST] /{Controller} hit at {DT}", typeof(TEntity), DateTime.UtcNow.ToLongTimeString());

            try
            {
                return this.Ok(this.CrudAppService.Insert(dto));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Update an entity based on dto information.
        /// </summary>
        /// <param name="dto">The information dto.</param>
        /// <returns>The updated entity as dto.</returns>
        [Authorize]
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual IActionResult Update(TDto dto)
        {
            this.Logger.LogInformation("[POST] /{Controller}/update hit at {DT}", typeof(TEntity), DateTime.UtcNow.ToLongTimeString());

            try
            {
                return this.Ok(this.CrudAppService.Update(dto));
            }
            catch (EntityNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Delete an entity based on dto information.
        /// </summary>
        /// <param name="id">The id of entity.</param>
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual IActionResult Delete(int id)
        {
            this.Logger.LogInformation("[DELETE] /{Controller}/{Id} hit at {DT}", typeof(TEntity), id, DateTime.UtcNow.ToLongTimeString());

            try
            {
                this.CrudAppService.Delete(id);
            }
            catch (EntityNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
            
            return this.Ok();
        }

    }