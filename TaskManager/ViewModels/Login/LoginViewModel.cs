using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;

namespace TaskManager.ViewModels.Login
{
    /// <summary>
    /// ViewModel для Авторизации.
    /// </summary>
    public class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly TaskCompletionSource<UserDto?> _tcs;

        public AsyncRelayCommand LoginCommand { get; }
        public RelayCommand CancelCommand { get; }
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="authService"></param>
        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            _tcs = new TaskCompletionSource<UserDto?>();

            LoginCommand = new AsyncRelayCommand(LoginAsync, CanRun);
            CancelCommand = new RelayCommand(Cancel, CanRun);
        }

        public Task<UserDto?> WaitResultAsync() => _tcs.Task;

        private bool _isExecute;
        public bool IsExecute
        {
            get => _isExecute;
            private set
            {
                if (SetProperty(ref _isExecute, value))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                    CancelCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private bool CanRun()
            => !IsExecute;

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            ClearError();
            try
            {
                IsExecute = true;
                ErrorText = string.Empty;

                var user = await _authService.LoginAsync(Login, Password);
                if (user == null)
                {
                    ErrorText = "Неправильно введен логин или пароль.";
                    return;
                }

                _tcs.TrySetResult(user);
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при авторизации. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        private void Cancel()
        {
            _tcs.TrySetResult(null);
        }

        private void SetError(string erroeMessage)
            => _errorText = erroeMessage;

        private void ClearError()
            => _errorText = string.Empty;
    }
}
