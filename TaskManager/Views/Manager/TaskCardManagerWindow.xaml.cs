using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.ViewModels.Manager;

namespace TaskManager.Views.Manager
{
    public partial class TaskCardManagerWindow: Window
    {
        public TaskCardManagerWindow(TaskCardManagerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
