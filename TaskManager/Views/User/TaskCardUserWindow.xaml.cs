using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.ViewModels.User;

namespace TaskManager.Views.User
{
    public partial class TaskCardUserWindow : Window
    {
        public TaskCardUserWindow(TaskCardUserViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
