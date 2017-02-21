using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printing.NET
{
    /// <summary>
    /// Представляет устройство принтера.
    /// </summary>
    public class Printer
    {
        /// <summary>
        /// Имя принтера.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        public Printer()
        {

        }
    }
}