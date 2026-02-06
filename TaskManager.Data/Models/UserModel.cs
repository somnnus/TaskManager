using System;

namespace TaskManager.Data.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Role { get; set; }

        public List<TaskModel> AuthoredTasks { get; set; } = new();
        public List<TaskModel> ExecutedTasks { get; set; } = new();

    }
}
