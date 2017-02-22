using System;

namespace Printing.NET.Native
{
    /// <summary>
    /// Флаги выборки принтеров при получении их списка.
    /// </summary>
    [Flags]
    internal enum PrinterEnumFlag
    {
        Default = 0x00000001,
        Local = 0x00000002,
        Connections = 0x00000004,
        Favorite = 0x00000004,
        Name = 0x00000008,
        Remote = 0x00000010,
        Shared = 0x00000020,
        Network = 0x00000040,
        Expand = 0x00004000,
        Container = 0x00008000,
        IconMask = 0x00ff0000,
        Icon1 = 0x00010000,
        Icon2 = 0x00020000,
        Icon3 = 0x00040000,
        Icon4 = 0x00080000,
        Icon5 = 0x00100000,
        Icon6 = 0x00200000,
        Icon7 = 0x00400000,
        Icon8 = 0x00800000,
        Hide = 0x01000000,
        All = 0x02000000,
        Category3D = 0x04000000,
    }
}