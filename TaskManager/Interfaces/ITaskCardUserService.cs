using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// Сервис Карточки задачи (Пользователь).
    /// </summary>
    public interface ITaskCardUserService
    {
        /// <summary>
        /// Редактирование задачи.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="afterSaveAsync"></param>
        /// <returns></returns>
        void Edit(TaskDto task, Func<Task> afterSaveAsync);
    }
}
