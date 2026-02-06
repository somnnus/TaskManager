using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.ViewModels.Admin
{
    /// <summary>
    /// Форма для редактирования пользователя.
    /// </summary>
    public sealed class CreateEditUserFormViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly Func<Task> _afterSaveAsync;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="afterSaveAsync"></param>
        public CreateEditUserFormViewModel(IUserService userService, Func<Task> afterSaveAsync)
        {
            _userService = userService;
            _afterSaveAsync = afterSaveAsync;

            Roles = new List<UserRoleEnum>
            {
                UserRoleEnum.Admin,
                UserRoleEnum.Manager,
                UserRoleEnum.User
            };

            ShowCommand = new RelayCommand(ShowCreate, CanShow);
            CancelCommand = new RelayCommand(Hide, CanCancel);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }

        public List<UserRoleEnum> Roles { get; }
        public RelayCommand ShowCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

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

        private int _editUserId;
        public int EditUserId
        {
            get => _editUserId;
            private set => SetProperty(ref _editUserId, value);
        }

        public string Title => IsEditMode
            ? "Редактировать информацию о пользователе"
            : "Добавить нового пользователя";

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

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set
            {
                if (SetProperty(ref _login, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private UserRoleEnum _role = UserRoleEnum.User;
        public UserRoleEnum Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        /// <summary>
        /// Показать форму для создания.
        /// </summary>
        public void ShowCreate()
        {
            IsEditMode = false;
            EditUserId = -1;

            Name = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            Role = UserRoleEnum.User;

            IsVisible = true;
            SaveCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Показать форму для редактирования.
        /// </summary>
        /// <param name="user"></param>
        public void ShowEdit(UserDto user)
        {
            IsEditMode = true;
            EditUserId = user.Id;

            Name = user.Name;
            Login = user.Login;
            Role = user.Role;
            Password = string.Empty;

            IsVisible = true;
            SaveCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Скрыть форму.
        /// </summary>
        private void Hide()
        {
            IsVisible = false;

            Name = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            Role = UserRoleEnum.User;
        }

        private bool CanShow()
            => !IsExecute && !IsVisible;

        private bool CanCancel()
            => !IsExecute && IsVisible;

        private bool CanSave()
            => !IsExecute
            && IsVisible
            && !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Login)
            && (IsEditMode || !string.IsNullOrWhiteSpace(Password));

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {
            ClearError();
            try
            {
                IsExecute = true;

                if (!IsEditMode)
                {
                    await _userService.CreateUserAsync(new UserDto
                    {
                        Name = Name,
                        Login = Login,
                        Password = Password,
                        Role = Role
                    });
                }
                else
                {
                    await _userService.UpdateUserAsync(new UserDto
                    {
                        Id = EditUserId,
                        Name = Name,
                        Login = Login,
                        Password = Password,
                        Role = Role
                    });
                }

                Hide();
                await _afterSaveAsync();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при сохранении пользователя. {ex.Message}");
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
            SaveCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
        }
    }
}
