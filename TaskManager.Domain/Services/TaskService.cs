using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Services
{
    /// <summary>
    /// Сервис для работы с задачами.
    /// </summary>
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _repository;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="repository"></param>
        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Создать задачу.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task CreateTaskAsync(TaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("..");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("..");

            await _repository.CreateTaskAsync(dto);
        }

        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task DeleteTaskAsync(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("..");
            await _repository.DeleteTaskAsync(taskId);
        }

        /// <summary>
        /// Получить все задачию
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _repository.GetAllTasksAsync();
            return tasks.ToList();
        }

        /// <summary>
        /// Получить задачу по автору.
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<TaskDto>> GetTasksByAuthorIdAsync(int authorId)
        {
            if(authorId <= 0) throw new ArgumentException("..");

            var tasks = await _repository.GetTasksByAuthorIdAsync(authorId);
            return tasks.ToList();
        }

        /// <summary>
        /// Получить задачу по исполнителям.
        /// </summary>
        /// <param name="executorId"></param>
        /// <returns></returns>
        public async Task<List<TaskDto>> GetTasksByExecutorIdAsync(int executorId)
        {
            var tasks = await _repository.GetTasksByExecutorIdAsync(executorId);
            return tasks.ToList();
        }

        /// <summary>
        /// Обновить задачу.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateTaskAsync(TaskDto dto)
        {
            await _repository.UpdateTaskAsync(dto);
        }

        /// <summary>
        /// Обновить статус задачи.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public async Task UpdateTaskStatusAsync(int taskId, TaskStatusEnum taskStatus)
        {
            var model = await _repository.GetTaskByIdAsync(taskId);
            if (model == null)
                return;

            model.Status = taskStatus;
            await _repository.UpdateTaskAsync(model);
        }
    }
}
