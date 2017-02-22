using System.Runtime.InteropServices;

namespace Printing.NET.Native
{
    /// <summary>
    /// Представляет данные принтера для <see cref="Port.XcvData(System.IntPtr, string, System.IntPtr, uint, System.IntPtr, uint, out uint, out uint)"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct PortData
    {
        /// <summary>
        /// Наименование порта.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string PortName;

        /// <summary>
        /// Номер версии (по умолчанию равен 1).
        /// </summary>
        public uint Version;

        /// <summary>
        /// Протокол.
        /// </summary>
        public DataType Protocol;

        /// <summary>
        /// Размер буфера данных.
        /// </summary>
        public uint BufferSize;

        /// <summary>
        /// Размер зарезервированного буфера.
        /// </summary>
        public uint ReservedSize;

        /// <summary>
        /// Адрес хоста.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 49)]
        public string HostAddress;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string SNMPCommunity;

        public uint DoubleSpool;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string Queue;

        /// <summary>
        /// IP-адрес.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;

        /// <summary>
        /// Зарезервированный буфер.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
        public byte[] Reserved;

        /// <summary>
        /// Номер порта.
        /// </summary>
        public uint PortNumber;

        public uint SNMPEnabled;

        public uint SNMPDevIndex;
    }
}