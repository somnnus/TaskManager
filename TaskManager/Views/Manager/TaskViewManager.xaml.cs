using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TaskManager.ViewModels.Admin;
using TaskManager.ViewModels.Manager;

namespace TaskManager.Views.Manager
{
    public partial class TaskViewManager: UserControl
    {
        public TaskViewManager()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<TaskViewManagerModel>();
        }
    }
}
