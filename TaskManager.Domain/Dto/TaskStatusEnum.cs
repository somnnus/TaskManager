using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Dto
{
    public enum TaskStatusEnum
    {
        New = 0,
        InProgress = 1,
        Paused = 2,
        Completed = 3,
        Cancelled = 4
    }
}
