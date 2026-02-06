using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;
using TaskManager.Interfaces;
using TaskManager.ViewModels.Admin;

namespace TaskManager.Views.Admin
{
    /// <summary>
    /// ViewModel пользователей.
    /// </summary>
    public class UsersViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IConfirmService _confirmService;

        public ObservableCollection<UserDto> Users { get; } = new ObservableCollection<UserDto>();

        public ObservableCollection<UserDto> FilteredUsers { get; } = new ObservableCollection<UserDto>();

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

        public CreateEditUserFormViewModel Form { get; }

        public AsyncRelayCommand LoadCommand { get; }
        public RelayCommand<UserDto?> EditCommand { get; }
        public AsyncRelayCommand<UserDto?> DeleteCommand { get; }

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
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="confirmService"></param>
        public UsersViewModel(IUserService userService, IConfirmService confirmService)
        {
            _userService = userService;
            _confirmService = confirmService;

            Form = new CreateEditUserFormViewModel(_userService, ReloadUsersAsync);

            LoadCommand = new AsyncRelayCommand(LoadAsync, CanRun);
            EditCommand = new RelayCommand<UserDto?>(Edit, CanEdit);
            DeleteCommand = new AsyncRelayCommand<UserDto?>(DeleteAsync, CanDelete);

            LoadCommand.ExecuteAsync(null);
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            private set => SetProperty(ref _errorText, value);
        }

        private bool CanRun()
            => !IsExecute;

        private bool CanEdit(UserDto? user)
            => !IsExecute && user != null;

        private bool CanDelete(UserDto? user)
            => !IsExecute && user != null;
        /// <summary>
        /// Загрузить пользователей.
        /// </summary>
        /// <returns></returns>
        private async Task LoadAsync()
        {
            try
            {
                IsExecute = true;
                await ReloadUsersAsync();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при загрузке пользователей. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }

        private void Edit(UserDto? user)
        {
            if (user is null) return;
            Form.ShowEdit(user);
        }

        private async Task DeleteAsync(UserDto? user)
        {
            ClearError();
            if (user ==null)
                return;

            var confirm = _confirmService.Confirm(
                $"Удалить пользователя '{user.Login}'?",
                "Подтверждение удаления");

            if (!confirm) return;

            try
            {
                IsExecute = true;

                await _userService.DeleteUserAsync(user.Id);
                await ReloadUsersAsync();
            }
            catch (Exception ex)
            {
                SetError($"Ошибка при удалении пользователя. {ex.Message}");
            }
            finally
            {
                IsExecute = false;
            }
        }
        /// <summary>
        /// Перезагрузить список пользователей.
        /// </summary>
        /// <returns></returns>
        private async Task ReloadUsersAsync()
        {
            Users.Clear();

            var usersList = await _userService.GetAllUsersAsync();
            foreach (var user in usersList)
                Users.Add(user);

            ApplyFilter();
        }
        /// <summary>
        /// Применить фильтр.
        /// </summary>
        private void ApplyFilter()
        {
            FilteredUsers.Clear();

            var search = SearchText ?? string.Empty;
            if (string.IsNullOrWhiteSpace(search))
            {
                foreach (var user in Users)
                    FilteredUsers.Add(user);
                return;
            }

            search = search.ToLowerInvariant();

            foreach (var user in Users)
            {
                var name = (user.Name ?? string.Empty).ToLowerInvariant();
                var login = (user.Login ?? string.Empty).ToLowerInvariant();

                if (name.Contains(search) || login.Contains(search))
                    FilteredUsers.Add(user);
            }
        }

        private void SetError(string erroeMessage)
            => _errorText = erroeMessage;

        private void ClearError()
            => _errorText = string.Empty;
    }
}
