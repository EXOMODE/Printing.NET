namespace Printing.NET
{
    /// <summary>
    /// Представляет базовый интерфейс для реализации мониторов печати.
    /// </summary>
    public interface IMonitor : IPrintableDevice
    {
        /// <summary>
        /// Окружение, для которого был написан драйвер (например, Windows NT x86, Windows IA64 или Windows x64).
        /// </summary>
        Environment Environment { get; }

        /// <summary>
        /// Имя файла *.dll монитора.
        /// </summary>
        string Dll { get; }
    }
}