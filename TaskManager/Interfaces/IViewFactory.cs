using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// Сервис фабрики View.
    /// </summary>
    public interface IViewFactory
    {
        /// <summary>
        /// Создать View.
        /// </summary>
        /// <returns></returns>
        object CreateView();
    }
}
