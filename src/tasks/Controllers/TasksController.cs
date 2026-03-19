using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Tasks.Controllers
{

    using Tasks.Handlers;
        using Tasks;

    [ApiController]
    [Route("api/[controller]")]

    public class TasksController : ControllerBase
    {
        private readonly TasksHandler _handler;

        public TasksController(AppDbContext db, ILogger<TasksController> logger, ILogger<TasksHandler> handlerLogger)
        {
            _handler = new TasksHandler(db, handlerLogger);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var result = await _handler.GetAllAsync();
            if (!result.Success)
                return StatusCode(500, result.Error);
            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskItem>> Get(Guid id)
        {
            var result = await _handler.GetAsync(id);
            if (!result.Success)
                return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem input)
        {
            var result = await _handler.CreateAsync(input);
            if (!result.Success)
                return BadRequest(result.Error);
            return CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, TaskItem input)
        {
            var result = await _handler.UpdateAsync(id, input);
            if (!result.Success)
                return NotFound(result.Error);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _handler.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Error);
            return NoContent();
        }
    }
}
