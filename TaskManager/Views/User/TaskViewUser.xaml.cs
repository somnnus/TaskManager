using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TaskManager.ViewModels.User;

namespace TaskManager.Views.User
{
    public partial class TaskViewUser: UserControl
    {
        public TaskViewUser()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<TaskViewUserModel>();
        }
    }
}
