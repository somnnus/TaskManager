using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Models;
using TaskManager.Domain.Dto;

namespace TaskManager.Data.Mapping
{
    public static class UserProjection
    {
        public static readonly Expression<Func<UserModel, UserDto>> Projection =
            x => new UserDto
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Role = (UserRoleEnum)x.Role,
                Password = x.Password,
            };
    }
}
