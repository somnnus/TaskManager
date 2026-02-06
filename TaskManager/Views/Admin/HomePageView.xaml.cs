using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TaskManager.Views.Admin
{
    public partial class HomePageView : UserControl
    {
        public HomePageView(UsersView usersView, TasksView tasksView)
        {
            InitializeComponent();

            UsersHost.Content = usersView;
            TasksHost.Content = tasksView;
        }
    }
}
