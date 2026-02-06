using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskManager.Domain.Dto;
using TaskManager.Interfaces;
using TaskManager.ViewModels.Manager;
using TaskManager.Views.Manager;

namespace TaskManager.Services
{
    /// <summary>
    /// Сервис Карточки задачи (Менеджер).
    /// </summary>
    public class TaskCardManagerService : ITaskCardManagerService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public TaskCardManagerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Создание новой задачи.
        /// </summary>
        /// <param name="afterSaveAsync"></param>
        /// <returns></returns>
        public async Task CreateAsync(Func<Task> afterSaveAsync)
        {
            var viewModel = ActivatorUtilities.CreateInstance<TaskCardManagerViewModel>(_serviceProvider, afterSaveAsync);
            await viewModel.LoadCreateAsync();

            ShowWindow(viewModel);
        }

        /// <summary>
        /// Редактирование задачи.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="afterSaveAsync"></param>
        /// <returns></returns>
        public async Task EditAsync(TaskDto task, Func<Task> afterSaveAsync)
        {
            if (task == null)
                return;

            var viewModel = ActivatorUtilities.CreateInstance<TaskCardManagerViewModel>(_serviceProvider, afterSaveAsync);
            await viewModel.LoadEditAsync(task);

            ShowWindow(viewModel);
        }

        /// <summary>
        /// Показать карточку.
        /// </summary>
        /// <param name="viewModel"></param>
        private static void ShowWindow(TaskCardManagerViewModel viewModel)
        {
            var window = new TaskCardManagerWindow(viewModel)
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
