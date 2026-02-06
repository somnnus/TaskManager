using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;

namespace TaskManager.Domain.Interfaces
{
    /// <summary>
    /// Сервис текущего пользователя.
    /// </summary>
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Login { get; }
        UserRoleEnum Role { get; }
        bool IsAuthenticated { get; }

        void Set(UserDto user);
        void Clear();
    }
}
