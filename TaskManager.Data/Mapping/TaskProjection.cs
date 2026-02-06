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
    public static class TaskProjection
    {
        public static readonly Expression<Func<TaskModel, TaskDto>> Projection = 
            x => new TaskDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            CreateDate = x.CreateDate,
            Status = (TaskStatusEnum)x.Status,
            AuthorId = x.AuthorId,
            ExecutorId = x.ExecutorId,

            AuthorLogin =x.Author != null
                ? x.Author.Login
                : string.Empty,
            ExecutorLogin = x.Executor != null
                ? x.Executor.Login
                : string.Empty
            };
    }   
}
