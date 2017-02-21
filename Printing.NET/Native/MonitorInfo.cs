using System.Runtime.InteropServices;

namespace Printing.NET.Native
{
    /// <summary>
    /// Представляет структуру для хранения информации о мониторе принтера.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct MonitorInfo
    {
        /// <summary>
        /// Наименование монитора.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Name;

        /// <summary>
        /// Окружение, для которого был написан драйвер (например, Windows NT x86, Windows IA64 или Windows x64).
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Environment;

        /// <summary>
        /// Имя файла *.dll монитора.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DllName;
    }
}