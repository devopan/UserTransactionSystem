using Microsoft.AspNetCore.Mvc;

namespace UserTransactionSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto, TCreateDto, TIdType> : ControllerBase
        where TDto : class
        where TCreateDto : class
    {
        /// <summary>
        /// Gets an entity by its id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(TIdType id)
        {
            var entity = await ReadSingleAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="createDto">The new entity data.</param>
        [HttpPost]
        public virtual async Task<IActionResult> Create(TCreateDto createDto)
        {
            var createdEntity = await CreateAsync(createDto);
            if (createdEntity == null)
                return NotFound();

            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var entities = await GetAllAsync();
            return Ok(entities);
        }

        protected abstract Task<TDto> ReadSingleAsync(TIdType id);
        protected abstract Task<TDto> CreateAsync(TCreateDto createDto);
        protected abstract TIdType GetEntityId(TDto entity);
        protected abstract Task<IEnumerable<TDto>> GetAllAsync();
    }
}