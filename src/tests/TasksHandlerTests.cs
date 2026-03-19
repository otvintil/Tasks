using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tasks;
using Tasks.Handlers;
using Xunit;

public class TasksHandlerTests
{
    private AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    private TasksHandler GetHandler(AppDbContext db)
    {
        var logger = new Mock<ILogger<TasksHandler>>();
        return new TasksHandler(db, logger.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenTitleIsEmpty()
    {
        var db = GetDbContext(Guid.NewGuid().ToString());
        var handler = GetHandler(db);
        var input = new TaskItem { Title = "" };
        var result = await handler.CreateAsync(input);
        Assert.False(result.Success);
        Assert.Equal("Task title cannot be empty.", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTask_WhenValid()
    {
        var db = GetDbContext(Guid.NewGuid().ToString());
        var handler = GetHandler(db);
        var input = new TaskItem { Title = "Test Task", Description = "desc" };
        var result = await handler.CreateAsync(input);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("Test Task", result.Value!.Title);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenMissing()
    {
        var db = GetDbContext(Guid.NewGuid().ToString());
        var handler = GetHandler(db);
        var result = await handler.GetAsync(Guid.NewGuid());
        Assert.False(result.Success);
        Assert.Equal("Task not found", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenMissing()
    {
        var db = GetDbContext(Guid.NewGuid().ToString());
        var handler = GetHandler(db);
        var result = await handler.UpdateAsync(Guid.NewGuid(), new TaskItem { Title = "x" });
        Assert.False(result.Success);
        Assert.Equal("Task not found", result.Error);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenMissing()
    {
        var db = GetDbContext(Guid.NewGuid().ToString());
        var handler = GetHandler(db);
        var result = await handler.DeleteAsync(Guid.NewGuid());
        Assert.False(result.Success);
        Assert.Equal("Task not found", result.Error);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        var db = GetDbContext(Guid.NewGuid().ToString());
        db.Tasks.Add(new TaskItem { Id = Guid.NewGuid(), Title = "A", CreatedAt = DateTime.UtcNow });
        db.Tasks.Add(new TaskItem { Id = Guid.NewGuid(), Title = "B", CreatedAt = DateTime.UtcNow });
        db.SaveChanges();
        var handler = GetHandler(db);
        var result = await handler.GetAllAsync();
        Assert.True(result.Success);
        Assert.Equal(2, ((ICollection<TaskItem>)result.Value!).Count);
    }
}
