namespace Printing.NET
{
    /// <summary>
    /// Представляет интерфейс для реализации сущностей драйверов печати.
    /// </summary>
    public interface IDriver : IPrintableDevice
    {
        /// <summary>
        /// Монитор печати.
        /// </summary>
        IMonitor Monitor { get; }

        /// <summary>
        /// Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).
        /// </summary>
        uint Version { get; }

        /// <summary>
        /// Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).
        /// </summary>
        Environment Environment { get; }

        /// <summary>
        /// Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").
        /// </summary>
        string Dll { get; }

        /// <summary>
        /// Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").
        /// </summary>
        string DataFile { get; }

        /// <summary>
        /// Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").
        /// </summary>
        string ConfigFile { get; }

        /// <summary>
        /// Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").
        /// </summary>
        string HelpFile { get; }

        /// <summary>
        /// Зависимые файлы.
        /// </summary>
        string DependentFiles { get; }

        /// <summary>
        /// Тип данных принтера по умолчанию.
        /// </summary>
        DataType DefaultDataType { get; }
    }
}