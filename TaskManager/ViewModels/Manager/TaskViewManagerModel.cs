using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;
using TaskManager.Interfaces;

namespace TaskManager.ViewModels.Manager
{
    /// <summary>
    /// ViewModel Задач (Роль: Менеджер)
    /// </summary>
    public class TaskViewManagerModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITaskCardManagerService _taskCardService;
        private readonly IConfirmService _confirmService;

        public ObservableCollection<TaskDto> Tasks { get; } 
            = new ObservableCollection<TaskDto>();

        public AsyncRelayCommand LoadCommand { get; }
        public AsyncRelayCommand CreateCommand { get; }
        public AsyncRelayCommand<TaskDto?> EditCommand { get; }
        public AsyncRelayCommand<TaskDto?> DeleteCommand { get; }
        public AsyncRelayCommand<TaskDto?> CardCommand { get; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="taskService"></param>
        /// <param name="userService"></param>
        /// <param name="currentUserService"></param>
        /// <param name="taskCardService"></param>
        /// <param name="confirmService"></param>
        public TaskViewManagerModel(
            ITaskService taskService,
            IUserService userService,
            ICurrentUserService currentUserService,
            ITaskCardManagerService taskCardService,
            IConfirmService confirmService)
        {
            _taskService = taskService;
            _currentUserService = currentUserService;
            _taskCardService = taskCardService;
            _confirmService = confirmService;

            LoadCommand = new AsyncRelayCommand(LoadAsync, CanRun);
            CreateCommand = new AsyncRelayCommand(CreateAsync, CanRun);
            EditCommand = new AsyncRelayCommand<TaskDto?>(EditAsync, CanTaskRun);
            DeleteCommand = new AsyncRelayCommand<TaskDto?>(DeleteAsync, CanTaskRun);
            CardCommand = new AsyncRelayCommand<TaskDto?>(EditAsync, CanTaskRun);

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
                    CreateCommand.NotifyCanExecuteChanged();
                    EditCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
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

        private bool CanTaskRun(TaskDto? task)
            => !IsExecute && task != null;

        /// <summary>
        /// Создание новой задачи.
        /// </summary>
        /// <returns></returns>
        private async Task CreateAsync()
        {
            await _taskCardService.CreateAsync(ReloadTasksAsync);
        }

        /// <summary>
        /// Редактирование задачи.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task EditAsync(TaskDto? task)
        {
            ClearError();
            if (task == null)
                return;

            try
            {
                IsExecute = true;
                await _taskCardService.EditAsync(task, ReloadTasksAsync);
            }
            catch (Exception ex)
            {
                SetError($"Не удалось изменить задачу. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        /// <summary>
        /// Загрузить задачи.
        /// </summary>
        /// <returns></returns>
        private async Task LoadAsync()
        {
            ClearError();
            try
            {
                IsExecute = true;
                await ReloadTasksAsync();
            }
            catch (Exception ex)
            {
                SetError($"Не удалось загрузить задачи. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        /// <summary>
        /// Удаление задачи.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task DeleteAsync(TaskDto? task)
        {
            ClearError();
            if (task == null)
                return;

            var confirm = _confirmService.Confirm($"Удалить задачу '{task.Title}'?", "Подтверждение удаления");
            if (!confirm)
                return;

            try
            {
                IsExecute = true;
                await _taskService.DeleteTaskAsync(task.Id);
                await ReloadTasksAsync();
            }
            catch (Exception ex)
            {
                SetError($"Не удалось удалить задачу. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        /// <summary>
        /// Перезагрузить список задач.
        /// </summary>
        /// <returns></returns>
        private async Task ReloadTasksAsync()
        {
            Tasks.Clear();

            var currentId = _currentUserService.UserId;
            var tasksCreatedByCurrentUser = await _taskService.GetTasksByAuthorIdAsync(currentId);
            var tasksAssignedCurrentUser = await _taskService.GetTasksByExecutorIdAsync(currentId);
            tasksCreatedByCurrentUser.AddRange(tasksAssignedCurrentUser);

            foreach (var task in tasksCreatedByCurrentUser)
                Tasks.Add(task);
        }

        private void SetError(string erroeMessage)
                => _errorText = erroeMessage;

        private void ClearError()
                => _errorText = string.Empty;
    }
}
