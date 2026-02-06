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
    /// Сервис текущего пользователя.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId { get; private set; }
        public string Login { get; private set; } = string.Empty;
        public UserRoleEnum Role { get; private set; }

        public bool IsAuthenticated => UserId > 0;

        public void Set(UserDto user)
        {
            UserId = user.Id;
            Login = user.Login;
            Role = user.Role;
        }

        public void Clear()
        {
            UserId = 0;
            Login = string.Empty;
            Role = UserRoleEnum.User;
        }
    }
}
