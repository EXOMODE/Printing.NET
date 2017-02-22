using System;

namespace Printing.NET
{
    /// <summary>
    /// Тип порта для монитора печати.
    /// </summary>
    [Flags]
    public enum PortType
    {
        /// <summary>
        /// Запись данных.
        /// </summary>
        Write = 0x1,
        /// <summary>
        /// Чтение данных.
        /// </summary>
        Read = 0x2,
        /// <summary>
        /// Перенаправление данных.
        /// </summary>
        Redirected = 0x4,
        /// <summary>
        /// Отправка данных на сервер.
        /// </summary>
        NetAttached = 0x8,
    }
}