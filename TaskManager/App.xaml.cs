using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskManager.Data.Db;
using TaskManager.Data.Repositories;
using TaskManager.Domain.Services;
using TaskManager.Views.Admin;
using TaskManager.Views;
using TaskManager.ViewModels.Admin;
using TaskManager.Domain.Interfaces;
using TaskManager.ViewModels.Login;
using TaskManager.Views.Login;
using TaskManager.Views.Manager;
using TaskManager.ViewModels.Manager;
using TaskManager.ViewModels.User;
using TaskManager.Views.User;
using TaskManager.Interfaces;
using TaskManager.Factories;
using TaskManager.Services;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlite("Data Source=taskmanager.db"));

            services.AddSingleton<ITaskCardManagerService, TaskCardManagerService>();
            services.AddSingleton<ITaskCardUserService, TaskCardUserService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddSingleton<IPasswordHasher, PasswordHasherSHA256>();
            services.AddSingleton<IConfirmService, ConfirmService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();
          
            services.AddTransient<LoginViewModel>();
            services.AddTransient<UsersViewModel>();
            services.AddTransient<TasksViewModel>();
            services.AddTransient<TaskViewManagerModel>();
            services.AddTransient<TaskViewUserModel>();

            services.AddTransient<TaskViewManager>();
            services.AddTransient<TaskViewUser>();
            services.AddTransient<UsersView>();
            services.AddTransient<TasksView>();
            services.AddTransient<HomePageView>();
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();

            Services = services.BuildServiceProvider();

            var loginWindow = Services.GetRequiredService<LoginWindow>();
            var user = await loginWindow.ShowDialogAsync();

            if (user == null)
            {
                Shutdown(); 
                return;
            }

            var current = Services.GetRequiredService<ICurrentUserService>();
            current.Set(user);

            var mainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow = mainWindow;             
            mainWindow.Show();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }

}
