using System.Runtime.InteropServices;

namespace Printing.NET.Native
{
    /// <summary>
    /// представляет информацию о порте монитора принтера.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PortInfo
    {
        /// <summary>
        /// Наименование поддерживаемого порта (например, "LPT1:").
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string PortName;

        /// <summary>
        /// Наименование установленного монитора принтера (например, "PJL monitor"). Может быть равно null.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string MonitorName;

        /// <summary>
        /// Описание порта (например, если <see cref="PortName"/> равен "LPT1:", <see cref="Description"/> будет равен "printer port"). Может быть равно null.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Description;

        /// <summary>
        /// Тип порта.
        /// </summary>
        public PortType Type;

        /// <summary>
        /// Зарезервировано. Должен быть равен 0.
        /// </summary>
        internal uint Reserved;
    }
}