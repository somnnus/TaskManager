using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить пользователя по Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto?> GetByIdAsync(int id);
        /// <summary>
        /// Получить пользователя по логину.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<UserDto?> GetUserByLoginAsync(string login);
        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        /// <summary>
        /// Получить пользователя по роли.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IEnumerable<UserDto>> GetByRoleAsync(UserRoleEnum role);
        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UserDto> CreateUserAsync(UserDto dto);
        /// <summary>
        /// Обновить пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UserDto dto);
        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteUserAsync(int id);
    }
}
