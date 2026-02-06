using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;
using TaskManager.Interfaces;

namespace TaskManager.ViewModels.User
{
    /// <summary>
    /// ViewModel Задач (Роль: Пользователь)
    /// </summary>
    public class TaskViewUserModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITaskCardUserService _taskCardUserService;

        public ObservableCollection<TaskDto> Tasks { get; } 
            = new ObservableCollection<TaskDto>();

        public AsyncRelayCommand LoadCommand { get; }
        public RelayCommand<TaskDto?> CardCommand { get; }
        
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="taskService"></param>
        /// <param name="currentUserService"></param>
        /// <param name="taskCardUserService"></param>
        public TaskViewUserModel(
            ITaskService taskService,
            ICurrentUserService currentUserService,
            ITaskCardUserService taskCardUserService)
        {
            _taskService = taskService;
            _currentUserService = currentUserService;
            _taskCardUserService = taskCardUserService;

            LoadCommand = new AsyncRelayCommand(LoadAsync, CanRun);
            CardCommand = new RelayCommand<TaskDto?>(ShowCardTask, CanCard);

            LoadCommand.ExecuteAsync(null);
        }

        private bool _isExecute;
        public bool IsExecute
        {
            get => _isExecute;
            private set
            {
                if (SetProperty(ref _isExecute, value))
                {
                    LoadCommand.NotifyCanExecuteChanged();
                    CardCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private bool CanRun() 
            => !IsExecute;

        private bool CanCard(TaskDto? task)
            => !IsExecute && task != null;

        /// <summary>
        /// Загрузить задачи.
        /// </summary>
        /// <returns></returns>
        private async Task LoadAsync()
        {
            try
            {
                ClearError();
                IsExecute = true;
                await ReloadTasksAsync();
            }
            catch (Exception ex)
            {
                SetError("Не удалось загрузить пользователей");
            }
            finally
            {
                IsExecute = false;
            }
        }

        /// <summary>
        /// Показать карточку задачи.
        /// </summary>
        /// <param name="task"></param>
        private void ShowCardTask(TaskDto? task)
        {
            if (task is null)
                return;

            _taskCardUserService.Edit(task, ReloadTasksAsync);
        }

        private void SetError(string erroeMessage) 
            => _errorText = erroeMessage;

        private void ClearError()
            => _errorText = string.Empty;

        /// <summary>
        /// Обновить список задач.
        /// </summary>
        /// <returns></returns>
        private async Task ReloadTasksAsync()
        {
            Tasks.Clear();

            var currentId = _currentUserService.UserId;
            var tasksList = await _taskService.GetTasksByExecutorIdAsync(currentId);

            foreach (var task in tasksList)
                Tasks.Add(task);
        }
    }
}
