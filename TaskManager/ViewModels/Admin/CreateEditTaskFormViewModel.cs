using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.ViewModels.Admin
{
    /// <summary>
    /// Форма создания редактирования задачи.
    /// </summary>
    public sealed class CreateEditTaskFormViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly Func<Task> _afterSaveAsync;

        private readonly int CurrentAdminId = 0;

        public AsyncRelayCommand ShowCommand { get; }
        public RelayCommand CancelCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="taskService"></param>
        /// <param name="userService"></param>
        /// <param name="currentUserService"></param>
        /// <param name="afterSaveAsync"></param>
        public CreateEditTaskFormViewModel(
            ITaskService taskService,
            IUserService userService,
            ICurrentUserService currentUserService,
            Func<Task> afterSaveAsync)
        {
            _taskService = taskService;
            _userService = userService;
            _afterSaveAsync = afterSaveAsync;

            CurrentAdminId = currentUserService.UserId;

            ShowCommand = new AsyncRelayCommand(ShowCreateAsync, CanShow);
            CancelCommand = new RelayCommand(Hide, CanCancel);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);

            Statuses = new List<TaskStatusEnum>
            {
                TaskStatusEnum.New,
                TaskStatusEnum.InProgress,
                TaskStatusEnum.Paused,
                TaskStatusEnum.Completed,
                TaskStatusEnum.Cancelled
            };
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

        public List<TaskStatusEnum> Statuses { get; }

        private TaskStatusEnum _selectedStatus;
        public TaskStatusEnum SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            private set
            {
                if (SetProperty(ref _isVisible, value))
                {
                    NotifyAllCommands();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            private set
            {
                if (SetProperty(ref _isEditMode, value))
                {
                    OnPropertyChanged(nameof(Title));
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private int _editTaskId;
        public int EditTaskId
        {
            get => _editTaskId;
            private set => SetProperty(ref _editTaskId, value);
        }

        public string Title => IsEditMode
            ? "Редактировать задачу"
            : "Новая задача";

        private bool _isExecute;
        public bool IsExecute
        {
            get => _isExecute;
            private set
            {
                if (SetProperty(ref _isExecute, value))
                    NotifyAllCommands();
            }
        }

        private string _titleTask = string.Empty;
        public string TitleTask
        {
            get => _titleTask;
            set
            {
                if (SetProperty(ref _titleTask, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private TaskStatusEnum _status = TaskStatusEnum.New;
        public TaskStatusEnum Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        /// <summary>
        /// Показать форму для создания.
        /// </summary>
        /// <returns></returns>
        public async Task ShowCreateAsync()
        {
            IsEditMode = false;
            EditTaskId = 0;

            TitleTask = string.Empty;
            Description = string.Empty;
            Status = TaskStatusEnum.New;

            IsVisible = true;

            await LoadExecutorsAsync(CurrentAdminId);
        }
        /// <summary>
        /// Показать форму для редактирования.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task ShowEditAsync(TaskDto task)
        {
            IsEditMode = true;
            EditTaskId = task.Id;

            TitleTask = task.Title;
            Description = task.Description;
            Status = task.Status;

            IsVisible = true;

            await LoadExecutorsAsync(task.ExecutorId ?? CurrentAdminId);
        }
        /// <summary>
        /// Скрыть форму.
        /// </summary>
        private void Hide()
        {
            IsVisible = false;

            TitleTask = string.Empty;
            Description = string.Empty;
            Status = TaskStatusEnum.New;
            Executors = new List<UserDto>();
            SelectedExecutor = null;
            IsEditMode = false;
            EditTaskId = 0;
        }

        private bool CanShow()
            => !IsExecute && !IsVisible;

        private bool CanCancel()
            => !IsExecute && IsVisible;

        private bool CanSave() =>
            !IsExecute
            && IsVisible
            && !string.IsNullOrWhiteSpace(TitleTask);

        /// <summary>
        /// Сохранить форму
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {
            ClearError();
            try
            {
                IsExecute = true;

                if (SelectedExecutor == null)
                    throw new ArgumentException("Не выбран исполнитель задачи");

                if (!IsEditMode)
                {
                    await _taskService.CreateTaskAsync(new TaskDto
                    {
                        Title = TitleTask,
                        Description = string.IsNullOrWhiteSpace(Description) ? string.Empty : Description,
                        Status = TaskStatusEnum.New,
                        AuthorId = CurrentAdminId,
                        ExecutorId = SelectedExecutor.Id
                    });
                }
                else
                {
                    await _taskService.UpdateTaskAsync(new TaskDto
                    {
                        Id = EditTaskId,
                        Title = TitleTask,
                        Description = string.IsNullOrWhiteSpace(Description) ? string.Empty : Description,
                        Status = Status,
                        AuthorId = CurrentAdminId,
                        ExecutorId = SelectedExecutor.Id
                    });
                }

                Hide();
                await _afterSaveAsync();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка сохранения задачи. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
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

                Executors = await _userService.GetAllUsersAsync();
                SelectedExecutor = Executors.FirstOrDefault(u => u.Id == defaultExecutorId)
                                   ?? Executors.FirstOrDefault();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка загрузки задач. {ex.Message}");
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

        private void NotifyAllCommands()
        {
            ShowCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
            SaveCommand.NotifyCanExecuteChanged();
        }
    }
}
