using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.ViewModels.User
{
    /// <summary>
    /// ViewModel Карточки задачи (Роль: Пользователь)
    /// </summary>
    public class TaskCardUserViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly Func<Task> _afterSaveAsync;

        public event Action? Close;
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="taskService"></param>
        /// <param name="afterSaveAsync"></param>
        public TaskCardUserViewModel(ITaskService taskService, Func<Task> afterSaveAsync)
        {
            _taskService = taskService;
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

        public List<TaskStatusEnum> Statuses { get; }

        public AsyncRelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// Загрузить данные задачи на операцию редактирования.
        /// </summary>
        /// <param name="task"></param>
        public void LoadEdit(TaskDto task)
        {
            TaskId = task.Id;
            Title = task.Title;
            Description = task.Description;
            ExecutorLogin = task.ExecutorLogin;
            CreateDate = task.CreateDate;
            _originalStatus = task.Status;
            SelectedStatus = task.Status;

            SaveCommand.NotifyCanExecuteChanged();
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

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            private set => SetProperty(ref _description, value);
        }

        private string _executorLogin = string.Empty;
        public string ExecutorLogin
        {
            get => _executorLogin;
            private set => SetProperty(ref _executorLogin, value);
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get => _createDate;
            private set => SetProperty(ref _createDate, value);
        }

        private TaskStatusEnum _originalStatus;

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

        private bool CanCancel() 
            => !IsExecute;

        private bool CanSave()
            => !IsExecute && TaskId > 0
                && SelectedStatus != _originalStatus;

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }
        /// <summary>
        /// Сохранить данные карточки.
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {
            ClearError();
            try
            {
                
                IsExecute = true;

                await _taskService.UpdateTaskStatusAsync(TaskId, SelectedStatus);
                await _afterSaveAsync();

                Close?.Invoke();
            }
            catch (Exception ex)
            {
                SetError($"Не удалось сохранить задачу. {ex.Message}");
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
