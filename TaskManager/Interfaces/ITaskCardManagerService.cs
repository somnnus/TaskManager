using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// Сервис Карточки задачи (Менеджер).
    /// </summary>
    public interface ITaskCardManagerService
    {
        /// <summary>
        /// Создание новой задачи.
        /// </summary>
        /// <param name="afterSaveAsync"></param>
        /// <returns></returns>
        Task CreateAsync(Func<Task> afterSaveAsync);
        /// <summary>
        /// Редактирование задачи.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="afterSaveAsync"></param>
        /// <returns></returns>
        Task EditAsync(TaskDto task, Func<Task> afterSaveAsync);
    }
}
