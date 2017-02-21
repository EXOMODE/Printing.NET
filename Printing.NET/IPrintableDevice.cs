namespace Printing.NET
{
    /// <summary>
    /// Представляет базовый интерфейс для реализации устройств, связанных с печатью (мониторы печати, порты принтеров, принтеры).
    /// </summary>
    public interface IPrintableDevice
    {
        /// <summary>
        /// Наименование устройства печати.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Устанавливает устройство печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        void Install(string serverName);

        /// <summary>
        /// Удаляет устройство печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        void Uninstall(string serverName);
    }
}