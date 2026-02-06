using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;
using TaskManager.Interfaces;

namespace TaskManager.ViewModels.Admin
{
    /// <summary>
    /// ViewModel задач.
    /// </summary>
    public class TasksViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly IConfirmService _confirmService;

        public ObservableCollection<TaskDto> Tasks { get; } 
            = new ObservableCollection<TaskDto>();

        public ObservableCollection<TaskDto> FilteredTasks { get; } 
            = new ObservableCollection<TaskDto>();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    ApplyFilter();
            }
        }

        public CreateEditTaskFormViewModel Form { get; }

        public AsyncRelayCommand LoadCommand { get; }
        public AsyncRelayCommand<TaskDto?> EditCommand { get; }
        public AsyncRelayCommand<TaskDto?> DeleteCommand { get; }

        private bool _isExecute;
        public bool IsExecute
        {
            get => _isExecute;
            private set
            {
                if (SetProperty(ref _isExecute, value))
                {
                    LoadCommand.NotifyCanExecuteChanged();
                    EditCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public TasksViewModel(
            ITaskService taskService,
            IUserService userService,
            ICurrentUserService currentUserService,
            IConfirmService confirmService)
        {
            _taskService = taskService;
            _confirmService = confirmService;

            Form = new CreateEditTaskFormViewModel(_taskService, userService, currentUserService, ReloadTasksAsync);

            LoadCommand = new AsyncRelayCommand(LoadAsync, CanRun);
            EditCommand = new AsyncRelayCommand<TaskDto?>(EditAsync, CanEdit);
            DeleteCommand = new AsyncRelayCommand<TaskDto?>(DeleteAsync, CanDelete);

            _ = LoadCommand.ExecuteAsync(null);
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private bool CanRun()
            => !IsExecute;

        private bool CanEdit(TaskDto? task)
            => !IsExecute && task != null;

        private bool CanDelete(TaskDto? task)
            => !IsExecute && task != null;
        /// <summary>
        /// Редактировать пользователя.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task EditAsync(TaskDto? task)
        {
            if (task == null)
                return;

            try
            {
                IsExecute = true;
                await Form.ShowEditAsync(task);
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при редактировании задачи. {ex.Message}");
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
            try
            {
                IsExecute = true;
                await ReloadTasksAsync();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при загрузке задач. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }
        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task DeleteAsync(TaskDto? task)
        {
            ClearError();
            if (task == null)
                return;

            var confirm = _confirmService
                .Confirm($"Удалить задачу '{task.Title}'?", "Подтверждение удаления");

            if (!confirm) return;

            try
            {
                IsExecute = true;

                await _taskService.DeleteTaskAsync(task.Id);
                await ReloadTasksAsync();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при удалении задачи. {ex.Message}");
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

            var tasksLst = await _taskService.GetAllTasksAsync();
            foreach (var task in tasksLst)
                Tasks.Add(task);

            ApplyFilter();
        }
        /// <summary>
        /// Применить фильтр.
        /// </summary>
        private void ApplyFilter()
        {
            FilteredTasks.Clear();

            var search = SearchText ?? string.Empty;
            if (string.IsNullOrWhiteSpace(search))
            {
                foreach (var task in Tasks)
                    FilteredTasks.Add(task);
                return;
            }
            search = search.ToLowerInvariant();

            foreach (var task in Tasks)
            {
                var title = (task.Title ?? string.Empty).ToLowerInvariant();
                var description = (task.Description ?? string.Empty).ToLowerInvariant();
                var executorLogin = (task.ExecutorLogin ?? string.Empty).ToLowerInvariant();

                if (title.Contains(search) || description.Contains(search) || executorLogin.Contains(search))
                    FilteredTasks.Add(task);
            }
        }


        private void SetError(string erroeMessage)
            => _errorText = erroeMessage;

        private void ClearError()
            => _errorText = string.Empty;
    }
}
