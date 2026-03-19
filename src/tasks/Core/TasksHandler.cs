using Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Handlers
{
    public class TasksHandler
    {
        private readonly AppDbContext _db;
        private readonly ILogger<TasksHandler> _logger;

        public TasksHandler(AppDbContext db, ILogger<TasksHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TaskItem>>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all tasks");
            var tasks = await _db.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync<TaskItem>();
            _logger.LogInformation("Retrieved {Count} tasks", tasks.Count);
            return Result<IEnumerable<TaskItem>>.Ok(tasks);
        }

        public async Task<Result<TaskItem>> GetAsync(Guid id)
        {
            _logger.LogInformation("Retrieving task with ID: {TaskId}", id);
            var task = await _db.Tasks.FindAsync(id);
            if (task is not null)
            {
                _logger.LogInformation("Task found: {TaskId}", id);
                return Result<TaskItem>.Ok(task);
            }
            else
            {
                _logger.LogWarning("Task not found: {TaskId}", id);
                return Result<TaskItem>.Fail("Task not found");
            }
        }

        public async Task<Result<TaskItem>> CreateAsync(TaskItem input)
        {
            _logger.LogInformation("Creating new task with title: {Title}", input.Title);

            if (string.IsNullOrWhiteSpace(input.Title))
            {
                _logger.LogWarning("Task creation failed: empty title");
                return Result<TaskItem>.Fail("Task title cannot be empty.");
            }

            var newTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = input.Title,
                Description = input.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _db.Tasks.Add(newTask);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Task created successfully with ID: {TaskId}", newTask.Id);
            return Result<TaskItem>.Ok(newTask);
        }

        public async Task<Result<bool>> UpdateAsync(Guid id, TaskItem input)
        {
            _logger.LogInformation("Updating task with ID: {TaskId}", id);
            var existing = await _db.Tasks.FindAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Task not found for update: {TaskId}", id);
                return Result<bool>.Fail("Task not found");
            }

            existing.Title = string.IsNullOrWhiteSpace(input.Title) ? existing.Title : input.Title;
            existing.Description = input.Description;
            existing.IsCompleted = input.IsCompleted;
            await _db.SaveChangesAsync();
            _logger.LogInformation("Task updated successfully: {TaskId}", id);
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting task with ID: {TaskId}", id);
            var existing = await _db.Tasks.FindAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Task not found for deletion: {TaskId}", id);
                return Result<bool>.Fail("Task not found");
            }

            _db.Tasks.Remove(existing);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Task deleted successfully: {TaskId}", id);
            return Result<bool>.Ok(true);
        }
    }
}
