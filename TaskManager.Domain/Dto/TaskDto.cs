using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }  
        public TaskStatusEnum Status { get; set; }
        public int AuthorId { get; set; }
        public int? ExecutorId { get; set; }

        public string AuthorLogin { get; set; } = string.Empty;
        public string ExecutorLogin { get; set; } = string.Empty;

    }
}
