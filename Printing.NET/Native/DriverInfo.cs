using System.Runtime.InteropServices;

namespace Printing.NET.Native
{
    /// <summary>
    /// Представляет структуру для хранения информации о драйвере устройства принтера.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DriverInfo
    {
        /// <summary>
        /// Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).
        /// </summary>
        public uint Version;

        /// <summary>
        /// Имя драйвера (например, "QMS 810").
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Name;

        /// <summary>
        /// Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Environment;

        /// <summary>
        /// Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DriverPath;

        /// <summary>
        /// Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DataFile;

        /// <summary>
        /// Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ConfigFile;

        /// <summary>
        /// Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string HelpFile;

        /// <summary>
        /// Зависимые файлы.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DependentFiles;

        /// <summary>
        /// Наименование монитора.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string MonitorName;

        /// <summary>
        /// Тип данных принтера по умолчанию.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DefaultDataType;
    }
}