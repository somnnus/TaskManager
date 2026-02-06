using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Domain.Dto;
using TaskManager.ViewModels.Login;

namespace TaskManager.Views.Login
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _loginViewModel = viewModel;
            DataContext = _loginViewModel;

            Closed += (x, y) =>
            {
                _loginViewModel.CancelCommand.Execute(null);
            };


        }

        public async Task<UserDto?> ShowDialogAsync()
        {
            Show();
            var result = await _loginViewModel.WaitResultAsync();
            Close();
            return result;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                _loginViewModel.Password = pb.Password;
        }
    }
}
