using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Data.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public int AuthorId { get; set; }
        public int? ExecutorId { get; set; }

        public UserModel? Author { get; set; }
        public UserModel? Executor { get; set; }
    }
}
