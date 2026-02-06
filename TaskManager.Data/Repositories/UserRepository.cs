using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Db;
using TaskManager.Data.Mapping;
using TaskManager.Data.Models;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Data.Repositories
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="db"></param>
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public async Task<UserDto> CreateUserAsync(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var loginExists = await _db.Users
                .AsNoTracking()
                .AnyAsync(x => x.Login == dto.Login);

            if (loginExists)
                throw new InvalidDataException($"Польхователь с логином {dto.Login} уже зарегестрирован");

            var userModel = new UserModel
            {
                Name = dto.Name,
                Login = dto.Login,
                Password = dto.Password,
                Role = (int)dto.Role
            };

            await _db.Users.AddAsync(userModel);
            await _db.SaveChangesAsync();

            return new UserDto
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Login = userModel.Login,
                Role = (UserRoleEnum)userModel.Role
            };
        }
        /// <summary>
        /// Обновить пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task UpdateUserAsync(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.Id <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id пользователя: {dto.Id}");

            var model = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (model == null)
                return;

            model.Name = dto.Name;
            model.Login = dto.Login;
            if (!string.IsNullOrEmpty(dto.Password))
            {
                model.Password = dto.Password;
            }         
            model.Role = (int)dto.Role;

            await _db.SaveChangesAsync();
        }
        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task DeleteUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException($"Некорректный Id пользователя: {id}");

            var model = await _db.Users
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
                return;

            _db.Users.Remove(model);
            await _db.SaveChangesAsync();
        }
        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _db.Users
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(UserProjection.Projection)
                .ToListAsync();
        }

        /// <summary>
        /// Получить пользоваетеля по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _db.Users
               .AsNoTracking()
               .Where(x => x.Id == id)
               .Select(UserProjection.Projection)
               .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Получить пользователей по логину.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<UserDto?> GetUserByLoginAsync(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return null;

            return await _db.Users
                .AsNoTracking()
                .Where(x => x.Login == login)
                .Select(UserProjection.Projection)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Получить пользователей по роли.
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserDto>> GetByRoleAsync(UserRoleEnum userRole)
        {
            var roleInt = (int)userRole;

            return await _db.Users
                .AsNoTracking()
                .Where(x => x.Role == roleInt)
                .OrderBy(x => x.Name)
                .Select(UserProjection.Projection)
                .ToListAsync();
        }      
    }
}
