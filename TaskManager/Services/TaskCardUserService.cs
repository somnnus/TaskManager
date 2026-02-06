using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskManager.Domain.Dto;
using TaskManager.Interfaces;
using TaskManager.ViewModels.User;
using TaskManager.Views.User;

namespace TaskManager.Services
{
    /// <summary>
    /// Сервис Карточки задачи (Пользователь).
    /// </summary>
    public class TaskCardUserService : ITaskCardUserService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public TaskCardUserService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Редактирование задачи.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="afterSaveAsync"></param>
        /// <returns></returns>
        public void Edit(TaskDto task, Func<Task> afterSaveAsync)
        {
            var viewModel = ActivatorUtilities.CreateInstance<TaskCardUserViewModel>(_serviceProvider, afterSaveAsync);
            viewModel.LoadEdit(task);

            ShowWindow(viewModel);
        }

        /// <summary>
        /// Показать карточку.
        /// </summary>
        /// <param name="viewModel"></param>
        private static void ShowWindow(TaskCardUserViewModel viewModel)
        {
            var window = new TaskCardUserWindow(viewModel)
            {
                Owner = Application.Current.MainWindow
            };

            viewModel.Close += () =>
            {
                if (window.IsVisible)
                    window.Close();
            };

            window.ShowDialog();
        }
    }
}
