using Printing.NET.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Printing.NET
{
    /// <summary>
    /// Представляет порт для запуска принтера.
    /// </summary>
    public class Port : PrintableDevice, IPort
    {
        /// <summary>
        /// Монитор, на котором открыт порт.
        /// </summary>
        public virtual IMonitor Monitor { get; protected set; }

        /// <summary>
        /// Описание порта.
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Тип порта.
        /// </summary>
        public virtual PortType Type { get; protected set; }

        /// <summary>
        /// Возвращает список всех установленных в системе портов печати.
        /// </summary>
        public static Port[] All
        {
            get
            {
                if (!PrintingApi.TryGetInfo(EnumPorts, out PortInfo[] portInfo)) return null;

                Port[] ports = new Port[portInfo.Length];

                for (int i = 0; i < portInfo.Length; i++)
                    ports[i] = new Port(portInfo[i].PortName, portInfo[i].Description, portInfo[i].Type, portInfo[i].MonitorName);

                return ports;
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <param name="monitorName">Наименование монитора печати, на котором открыт порт.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, string description, PortType type, string monitorName) : base(name)
        {
            Description = description;
            Type = type;

            Monitor[] monitors = PrintingApi.Monitors;

            if (monitors.Select(m => m.Name).Contains(monitorName)) Monitor = monitors.Where(m => m.Name == monitorName).FirstOrDefault();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <param name="monitor">Монитор печати, на котором открыт порт.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, string description, PortType type, IMonitor monitor) : this(name, description, type, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, string description, PortType type) : this(name, description, type, null as string) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="monitorName">Наименование монитора печати, на котором открыт порт.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, string description, string monitorName) : this(name, description, PortType.Redirected, monitorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="monitor">Монитор печати, на котором открыт порт.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, string description, IMonitor monitor) : this(name, description, PortType.Redirected, monitor) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, string description) : this(name, description, null as string) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="monitor">Монитор печати, на котором открыт порт.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, IMonitor monitor) : this(name, null, monitor) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name, PortType type) : this(name, null, type) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Port"/>.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="PrintingException"/>
        public Port(string name) : this(name, null as string) { }

        /// <summary>
        /// Открывает порт на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="PrintingException"/>
        public override void Install(string serverName)
        {
            try
            {
                if (All.Select(p => p.Name).Contains(Name)) Uninstall(serverName);

                PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.ServerAdmin };

                if (!OpenPrinter($",XcvMonitor {Monitor.Name}", out IntPtr printerHandle, ref defaults)) throw new PrintingException(Marshal.GetLastWin32Error());

                PortData portData = new PortData
                {
                    Version = 1,
                    Protocol = DataType.RAW,
                    PortNumber = 9100,  // 9100 = RAW, 515 = LPR.
                    ReservedSize = 0,
                    PortName = Name,
                    IPAddress = serverName,
                    SNMPCommunity = "public",
                    SNMPEnabled = 1,
                    SNMPDevIndex = 1,
                };

                uint size = (uint)Marshal.SizeOf(portData);
                portData.BufferSize = size;
                IntPtr pointer = Marshal.AllocHGlobal((int)size);
                Marshal.StructureToPtr(portData, pointer, true);

                try
                {
                    IntPtr outputData = IntPtr.Zero;
                    uint outputDataSize = 0;

                    if (!XcvData(printerHandle, Enum.GetName(typeof(XcvDataType), XcvDataType.AddPort), pointer, size, outputData, outputDataSize, out uint outputNeeded, out uint status))
                        throw new PrintingException(Marshal.GetLastWin32Error());
                }
                catch (Exception e)
                {
                    throw new PrintingException(e.Message, e);
                }
                finally
                {
                    Marshal.FreeHGlobal(pointer);
                    ClosePrinter(printerHandle);
                }
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        /// <summary>
        /// Закрывает порт на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="PrintingException"/>
        public override void Uninstall(string serverName)
        {
            try
            {
                if (!All.Select(p => p.Name).Contains(Name)) return;

                IEnumerable<Printer> printers = Printer.All.Where(p => p.Driver?.Name == Name);
                foreach (Printer printer in printers) printer.Uninstall(serverName);

                PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.ServerAdmin };

                if (!OpenPrinter($",XcvPort {Name}", out IntPtr printerHandle, ref defaults)) throw new PrintingException(Marshal.GetLastWin32Error());

                PortData portData = new PortData
                {
                    Version = 1,
                    Protocol = DataType.RAW,
                    PortNumber = 9100,
                    ReservedSize = 0,
                    PortName = Name,
                    IPAddress = serverName,
                    SNMPCommunity = "public",
                    SNMPEnabled = 1,
                    SNMPDevIndex = 1,
                };

                uint size = (uint)Marshal.SizeOf(portData);
                portData.BufferSize = size;
                IntPtr pointer = Marshal.AllocHGlobal((int)size);
                Marshal.StructureToPtr(portData, pointer, true);

                try
                {
                    IntPtr outputData = IntPtr.Zero;
                    uint outputDataSize = 0;

                    if (!XcvData(printerHandle, Enum.GetName(typeof(XcvDataType), XcvDataType.DeletePort), pointer, size, outputData, outputDataSize, out uint outputNeeded, out uint status))
                        throw new PrintingException(Marshal.GetLastWin32Error());
                }
                catch (Exception e)
                {
                    throw new PrintingException(e.Message, e);
                }
                finally
                {
                    Marshal.FreeHGlobal(pointer);
                    ClosePrinter(printerHandle);
                }
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        #region Native
        /// <summary>
        /// Получает указатель на принтер.
        /// </summary>
        /// <param name="printerName">Имя принтера.</param>
        /// <param name="printer">Указатель на принтер.</param>
        /// <param name="printerDefaults">Установки для <see cref="XcvData(IntPtr, string, IntPtr, uint, IntPtr, uint, out uint, out uint)"/>.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool OpenPrinter(string printerName, out IntPtr printer, ref PrinterDefaults printerDefaults);

        /// <summary>
        /// Освобождает ресурсы принтера.
        /// </summary>
        /// <param name="printer">Указатель принтера.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool ClosePrinter(IntPtr printer);

        /// <summary>
        /// Производит оперции с принтером.
        /// </summary>
        /// <param name="printer">Указатель на принтер.</param>
        /// <param name="dataType">Тип операции.</param>
        /// <param name="inputData">Входные данные.</param>
        /// <param name="inputDataSize">Размер буфера входных данных.</param>
        /// <param name="outputData">Выходные данные.</param>
        /// <param name="outputDataSize">Размер буфера выходных данных.</param>
        /// <param name="outputNeeded">Размер указателя на выходные данные.</param>
        /// <param name="status">Статус операции.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool XcvData(IntPtr printer, string dataType, IntPtr inputData, uint inputDataSize, IntPtr outputData, uint outputDataSize,
            out uint outputNeeded, out uint status);

        /// <summary>
        /// Возвращает указатель на буфер экземпляров структур <see cref="PortInfo"/>.
        /// </summary>
        /// <param name="serverName">Имя сервера, с которого требуется получить список портов. Если равно null - получает на локальной машине.</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 1 или 2.</param>
        /// <param name="ports">Указатель на буфер экземпляров структур <see cref="PortInfo"/>.</param>
        /// <param name="bufferSize">Размер буфера экземпляров структур <see cref="PortInfo"/> (в байтах).</param>
        /// <param name="bytesNeeded">Число полученных байт размера буфера.</param>
        /// <param name="bufferReturnedLength">Число экземпляров структур.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EnumPorts(string serverName, uint level, IntPtr ports, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);
        #endregion
    }
}