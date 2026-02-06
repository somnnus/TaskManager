using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TaskManager.ViewModels.Admin;

namespace TaskManager.Views.Admin
{
    public partial class TasksView : UserControl
    {
        public TasksView(TasksViewModel viewModel)
        { 
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
