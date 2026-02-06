using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// Сервис для Окна подтверждения.
    /// </summary>
    public interface IConfirmService
    {
        /// <summary>
        /// Подтвердить
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        bool Confirm(string message, string title);
    }
}
