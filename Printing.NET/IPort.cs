namespace Printing.NET
{
    /// <summary>
    /// Представляет базовый интерфейс для реализации портов печати.
    /// </summary>
    public interface IPort : IPrintableDevice
    {
        /// <summary>
        /// Монитор, на котором открыт порт.
        /// </summary>
        IMonitor Monitor { get; }

        /// <summary>
        /// Описание порта.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Тип порта.
        /// </summary>
        PortType Type { get; }
    }
}