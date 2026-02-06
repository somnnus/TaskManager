using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Сервис авторизации.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserDto?> LoginAsync(string login, string password);
    }
}
