using System;
using System.Runtime.InteropServices;

namespace Printing.NET.Native
{
    /// <summary>
    /// Представляет структуру данных принтера.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PrinterInfo
    {
        /// <summary>
        /// Имя сервера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ServerName;

        /// <summary>
        /// Наименование принтера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string PrinterName;

        /// <summary>
        /// Публичное наименование принтера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ShareName;

        /// <summary>
        /// Наименование порта, привязанного к принтеру.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string PortName;

        /// <summary>
        /// Наименование драйвера принтера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DriverName;

        /// <summary>
        /// Описание принтера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Comment;

        /// <summary>
        /// Местоположение принтера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Location;

        public IntPtr DevMode;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string SepFile;

        /// <summary>
        /// Процессор печати, связанный с принтером.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string PrintProcessor;

        /// <summary>
        /// Тип данных печати принтера.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DataType;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Parameters;

        public IntPtr SecurityDescriptor;

        public uint Attributes;
        public uint Priority;
        public uint DefaultPriority;
        public uint StartTime;
        public uint UntilTime;
        public uint Status;
        public uint cJobs;
        public uint AveragePPM;
    }
}