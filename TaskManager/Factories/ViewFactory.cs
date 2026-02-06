using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Dto;
using TaskManager.Domain.Interfaces;
using TaskManager.Interfaces;
using TaskManager.Views.Admin;
using TaskManager.Views.Manager;
using TaskManager.Views.User;

namespace TaskManager.Factories
{
    /// <summary>
    /// Фабрика для создания View.
    /// </summary>
    public class ViewFactory: IViewFactory
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="currentUserService"></param>
        /// <param name="serviceProvider"></param>
        public ViewFactory(ICurrentUserService currentUserService, IServiceProvider serviceProvider)
        {
            _currentUserService = currentUserService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Содать View.
        /// </summary>
        /// <returns></returns>
        public object CreateView()
        {
            switch (_currentUserService.Role)
            {
                case (UserRoleEnum.Admin):
                    return _serviceProvider.GetRequiredService<HomePageView>();
                case (UserRoleEnum.Manager):
                    return _serviceProvider.GetRequiredService<TaskViewManager>();
                default:
                    return _serviceProvider.GetRequiredService<TaskViewUser>();

            }
        }
    }
}
