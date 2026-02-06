using TaskManager.Domain.Dto;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Хранилище задач.
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Получить задачу по Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskDto?> GetTaskByIdAsync(int id);
        /// <summary>
        /// Получить все задачи.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        /// <summary>
        /// Получить задачу по автору.
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<IEnumerable<TaskDto>> GetTasksByAuthorIdAsync(int authorId);
        /// <summary>
        /// Получить задачи по исполнителю.
        /// </summary>
        /// <param name="executorId"></param>
        /// <returns></returns>
        Task<IEnumerable<TaskDto>> GetTasksByExecutorIdAsync(int executorId);
        /// <summary>
        /// Создать задачу.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateTaskAsync(TaskDto dto);
        /// <summary>
        /// Обновить задачу.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateTaskAsync(TaskDto dto);
        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteTaskAsync(int id);
    }
}
