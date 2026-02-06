using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Сервис хешироваения паролей.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Хешировать.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);
        /// <summary>
        /// Проверить пароль по хешу.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool Verify(string password, string hash);
    }
}
