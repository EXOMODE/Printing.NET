using System;
using System.Runtime.InteropServices;

namespace Printing.NET.Native
{
    /// <summary>
    /// Представляет установки принтера для <see cref="Port.XcvData(IntPtr, string, IntPtr, uint, IntPtr, uint, out uint, out uint)"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PrinterDefaults
    {
        /// <summary>
        /// Тип данных (по умолчанию равен null).
        /// </summary>
        public IntPtr DataType;

        /// <summary>
        /// Режим работы (по умолчанию равен null).
        /// </summary>
        public IntPtr DevMode;

        /// <summary>
        /// Права доступа к принтеру.
        /// </summary>
        public PrinterAccess DesiredAccess;
    }
}