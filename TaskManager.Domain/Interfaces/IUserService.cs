using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Сервис Пользователя
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns></returns>
        Task<List<UserDto>> GetAllUsersAsync();
        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateUserAsync(UserDto dto);
        /// <summary>
        /// Обновить пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UserDto dto);
        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserAsync(int userId);
        /// <summary>
        /// Получеть пользователей по роли.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetUsersByRoleAsyn(UserRoleEnum role);

    }
}
