using Application.BP.Bases.Crud;
using Common.BP.Bases;
using Common.BP.Exceptions;
using Domain.BP.Bases.Mapper;
using Infrastructure.BP.Bases.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.BP.Bases;

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
        private readonly ICrudAppService<TEntity, TDto, TMapper> _crudAppService;

        /// <summary>
        /// DI constructor.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="crudAppService">The crud service instance.</param>
        protected CrudController(ILogger<CrudController<TEntity, TDto, TMapper>> logger, ICrudAppService<TEntity, TDto, TMapper> crudAppService) : base(logger)
        {
            this._crudAppService = crudAppService;
        }
        
        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>All entities as <see cref="TDto"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual IActionResult GetAll()
        {
            return this.Ok(this._crudAppService.GetAll());
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
            try
            {
                return this.Ok(this._crudAppService.GetById(id));
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
            try
            {
                return this.Ok(this._crudAppService.Insert(dto));
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
            try
            {
                return this.Ok(this._crudAppService.Update(dto));
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
        /// <param name="dto">The information dto.</param>
        [Authorize]
        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual IActionResult Delete(TDto dto)
        {
            try
            {
                this._crudAppService.Delete(dto);
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