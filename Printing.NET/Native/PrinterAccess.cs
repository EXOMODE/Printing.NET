namespace Printing.NET.Native
{
    /// <summary>
    /// Права доступа к принтеру.
    /// </summary>
    internal enum PrinterAccess
    {
        /// <summary>
        /// Полный доступ к данным принтера.
        /// </summary>
        ServerAdmin = 0x01,
        /// <summary>
        /// Доступ к чтению данных принтера.
        /// </summary>
        ServerEnum = 0x02,
        /// <summary>
        /// Полный доступ к использованию принтера.
        /// </summary>
        PrinterAdmin = 0x04,
        /// <summary>
        /// Ограниченный доступ к использованию принтера.
        /// </summary>
        PrinterUse = 0x08,
        /// <summary>
        /// Полный доступ к данным очереди печати.
        /// </summary>
        JobAdmin = 0x10,
        /// <summary>
        /// Чтение данных очереди печати.
        /// </summary>
        JobRead = 0x20,
        /// <summary>
        /// Стандартные права доступа.
        /// </summary>
        StandardRightsRequired = 0x000F0000,
        /// <summary>
        /// Самый полный доступ.
        /// </summary>
        PrinterAllAccess = (StandardRightsRequired | PrinterAdmin | PrinterUse),
    }
}