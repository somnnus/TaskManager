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
    /// Сервис для работы с пользователями.
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="hasher"></param>
        public UserService(IUserRepository repository, IPasswordHasher hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();
            return users.ToList();
        }

        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateUserAsync(UserDto dto)
        {
            dto.Password = _hasher.HashPassword(dto.Password);
            await _repository.CreateUserAsync(dto);
        }

        /// <summary>
        /// Обновить пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(UserDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Password))
                dto.Password = _hasher.HashPassword(dto.Password);
            await _repository.UpdateUserAsync(dto);
        }

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(int userId)
        {
            await _repository.DeleteUserAsync(userId);
        }

        /// <summary>
        /// Получить пользователей по роли.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<List<UserDto>> GetUsersByRoleAsyn(UserRoleEnum role)
        {
            var users = await _repository.GetByRoleAsync(role);
            return users.ToList();
        }
    }
}
