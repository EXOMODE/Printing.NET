using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printing.NET
{
    /// <summary>
    /// Представляет порт для запуска принтера.
    /// </summary>
    public class Port
    {
        /// <summary>
        /// Имя порта.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        public Port()
        {

        }
    }
}