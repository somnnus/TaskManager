using TaskManager.Interfaces;

namespace TaskManager.Services
{
    /// <summary>
    /// Сервис окна подтверждения.
    /// </summary>
    public class ConfirmService : IConfirmService
    {
        /// <summary>
        /// Подтвердить.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool Confirm(string message, string title)
        {
            var result = System.Windows.MessageBox.Show(
                 message,
                 title,
                 System.Windows.MessageBoxButton.YesNo,
                 System.Windows.MessageBoxImage.Warning);

            return result == System.Windows.MessageBoxResult.Yes;
        }
    }
}
