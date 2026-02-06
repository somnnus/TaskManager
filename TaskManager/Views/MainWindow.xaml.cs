using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;
using TaskManager.Interfaces;
using TaskManager.Views.Admin;
using TaskManager.Views.Manager;
using TaskManager.Views.User;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ICurrentUserService currentUser, IViewFactory viewFactory)
        {
            InitializeComponent();
            View.Content = viewFactory.CreateView();
        }         
    }
}