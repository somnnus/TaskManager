using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.ViewModels.Manager
{
    /// <summary>
    /// ViewModel Карточки задачи (Роль: Менеджер)
    /// </summary>
    public sealed class TaskCardManagerViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;
        private readonly Func<Task> _afterSaveAsync;

        public event Action? Close;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="taskService"></param>
        /// <param name="userService"></param>
        /// <param name="currentUser"></param>
        /// <param name="afterSaveAsync"></param>
        public TaskCardManagerViewModel(
            ITaskService taskService,
            IUserService userService,
            ICurrentUserService currentUser,
            Func<Task> afterSaveAsync)
        {
            _taskService = taskService;
            _userService = userService;
            _currentUser = currentUser;
            _afterSaveAsync = afterSaveAsync;

            Statuses = new List<TaskStatusEnum>
            {
                TaskStatusEnum.New,
                TaskStatusEnum.InProgress,
                TaskStatusEnum.Paused,
                TaskStatusEnum.Completed,
                TaskStatusEnum.Cancelled
            };

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            CancelCommand = new RelayCommand(() => Close?.Invoke(), CanCancel);
        }

        public string FormTitle => IsCreateMode 
            ? "Создать задачу" 
            : "Редактировать задачу";

        public List<TaskStatusEnum> Statuses { get; }

        public AsyncRelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        private bool _isCreateMode;
        public bool IsCreateMode
        {
            get => _isCreateMode;
            private set
            {
                if (SetProperty(ref _isCreateMode, value))
                    OnPropertyChanged(nameof(FormTitle));
            }
        }

        private List<UserDto> _executors = new List<UserDto>();
        public List<UserDto> Executors
        {
            get => _executors;
            private set => SetProperty(ref _executors, value);
        }

        private UserDto? _selectedExecutor;
        public UserDto? SelectedExecutor
        {
            get => _selectedExecutor;
            set
            {
                if (SetProperty(ref _selectedExecutor, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private int _taskId;
        public int TaskId
        {
            get => _taskId;
            private set
            {
                if (SetProperty(ref _taskId, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private int _authorId;
        public int AuthorId
        {
            get => _authorId;
            private set => SetProperty(ref _authorId, value);
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get => _createDate;
            private set => SetProperty(ref _createDate, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                if (SetProperty(ref _title, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (SetProperty(ref _description, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private TaskStatusEnum _selectedStatus = TaskStatusEnum.New;
        public TaskStatusEnum SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isExecute;
        public bool IsExecute
        {
            get => _isExecute;
            private set
            {
                if (SetProperty(ref _isExecute, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    CancelCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private bool CanCancel()
            => !IsExecute;

        private bool CanSave() =>
            !IsExecute
            && !string.IsNullOrWhiteSpace(Title)
            && SelectedExecutor != null
            && (IsCreateMode || TaskId > 0);

        /// <summary>
        /// Загрузить данные задачи на операцию создание.
        /// </summary>
        /// <returns></returns>
        public async Task LoadCreateAsync()
        {
            IsCreateMode = true;

            TaskId = 0;
            AuthorId = _currentUser.UserId;
            CreateDate = DateTime.Now;

            Title = string.Empty;
            Description = string.Empty;
            SelectedStatus = TaskStatusEnum.New;

            await LoadExecutorsAsync(_currentUser.UserId);
        }

        /// <summary>
        /// Загрузить данные задачи на операцию редактирования.
        /// </summary>
        /// <returns></returns>
        public async Task LoadEditAsync(TaskDto task)
        {
            IsCreateMode = false;

            TaskId = task.Id;
            AuthorId = task.AuthorId;
            CreateDate = task.CreateDate;

            Title = task.Title;
            Description = task.Description ?? string.Empty;
            SelectedStatus = task.Status;

            await LoadExecutorsAsync(task.ExecutorId ?? 0);
        }

        /// <summary>
        /// Загрузить исполнителей.
        /// </summary>
        /// <param name="defaultExecutorId"></param>
        /// <returns></returns>
        private async Task LoadExecutorsAsync(int defaultExecutorId)
        {
            ClearError();
            try
            {
                IsExecute = true;

                Executors = await _userService.GetUsersByRoleAsyn(UserRoleEnum.User);
                SelectedExecutor = Executors.FirstOrDefault(x => x.Id == defaultExecutorId)
                                   ?? Executors.FirstOrDefault();

                SaveCommand.NotifyCanExecuteChanged();
            }
            catch (Exception ex)
            {
                SetError($" Не удалось загрузить исполнителей. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        /// <summary>
        /// Сохранение задачи.
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {
            ClearError();
            try
            {
                IsExecute = true;

                var dto = new TaskDto
                {
                    Id = TaskId,
                    Title = Title,
                    Description = Description,
                    Status = SelectedStatus,
                    AuthorId = AuthorId,
                    ExecutorId = SelectedExecutor!.Id,
                    CreateDate = CreateDate
                };

                if (IsCreateMode)
                    await _taskService.CreateTaskAsync(dto);
                else
                    await _taskService.UpdateTaskAsync(dto);

                await _afterSaveAsync();
                Close?.Invoke();
            }
            catch (Exception ex)
            {
                SetError($"Не уадлось сохранить задачу. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        private void SetError(string erroeMessage)
            => _errorText = erroeMessage;

        private void ClearError()
            => _errorText = string.Empty;
    }
}
