using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Сервис для работы с задачами.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Получить все задачи.
        /// </summary>
        /// <returns></returns>
        Task<List<TaskDto>> GetAllTasksAsync();
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
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task DeleteTaskAsync(int taskId);
        /// <summary>
        /// Получить все задачи по автору.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<TaskDto>> GetTasksByAuthorIdAsync(int userId);
        /// <summary>
        /// Получить все задачи по исполнителю.
        /// </summary>
        /// <param name="executorId"></param>
        /// <returns></returns>
        Task<List<TaskDto>> GetTasksByExecutorIdAsync(int executorId);
        /// <summary>
        /// Обновить статус задачи.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        Task UpdateTaskStatusAsync(int taskId, TaskStatusEnum taskStatus);
    }
}
