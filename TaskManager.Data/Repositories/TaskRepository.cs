using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Db;
using TaskManager.Data.Mapping;
using TaskManager.Data.Models;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateTaskAsync(TaskDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var taskModel = new TaskModel
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = (int)dto.Status,
                AuthorId = dto.AuthorId,
                ExecutorId = dto.ExecutorId,
            };

            await _db.Tasks.AddAsync(taskModel);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.Id <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id задачи: {dto.Id}");

            var model = await _db.Tasks
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (model == null)
                return;

            model.Title = dto.Title;
            model.Description = dto.Description;
            model.Status = (int)dto.Status;
            model.ExecutorId = dto.ExecutorId;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id пользователя: {id}");

            var model = await _db.Tasks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
                return;

            _db.Tasks.Remove(model);
            await _db.SaveChangesAsync();

        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            return await _db.Tasks
                .AsNoTracking()
                .Select(TaskProjection.Projection)
                .ToListAsync();
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id пользователя: {id}");

            return await _db.Tasks
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(TaskProjection.Projection)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByAuthorIdAsync(int authorId)
        {
            if (authorId <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id создателя: {authorId}");

            return await _db.Tasks
                .AsNoTracking()
                .Where(x => x.AuthorId == authorId)
                .Select(TaskProjection.Projection)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByExecutorIdAsync(int executorId)
        {
            if (executorId <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id создателя: {executorId}");

            return await _db.Tasks
                .AsNoTracking()
                .Where(x => x.ExecutorId == executorId)
                .Select(TaskProjection.Projection)
                .ToListAsync();
        }
    }
}
