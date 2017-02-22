using Printing.NET.Native;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Printing.NET
{
    /// <summary>
    /// Представляет устройство принтера.
    /// </summary>
    public class Printer : PrintableDevice, IPrinter
    {
        /// <summary>
        /// Порт, к которому привязан принтер.
        /// </summary>
        public virtual IPort Port { get; protected set; }

        /// <summary>
        /// Драйвер, который связан с принтером.
        /// </summary>
        public virtual IDriver Driver { get; protected set; }

        /// <summary>
        /// Публичное наименование принтера.
        /// </summary>
        public virtual string ShareName { get; protected set; }

        /// <summary>
        /// Описание устройства принтера.
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Тип данных печати.
        /// </summary>
        public virtual DataType DataType { get; protected set; }

        /// <summary>
        /// Процессор очереди печати.
        /// </summary>
        public virtual string Processor { get; protected set; }

        /// <summary>
        /// Имя сервера, на котором запущен принтер.
        /// </summary>
        public virtual string ServerName { get; protected set; }

        /// <summary>
        /// Расположение принтера.
        /// </summary>
        public virtual string Location { get; protected set; }

        /// <summary>
        /// Параметры принтера.
        /// </summary>
        public virtual string Parameters { get; protected set; }

        public virtual string SepFile { get; protected set; }

        /// <summary>
        /// Задаёт или возвращает принтер по умолчанию.
        /// </summary>
        public static Printer Default
        {
            get
            {
                uint length = 0;

                if (GetDefaultPrinter(null, ref length)) return null;

                int lastWin32Error = Marshal.GetLastWin32Error();

                if (lastWin32Error != PrintingException.ErrorInsufficientBuffer) throw new PrintingException(lastWin32Error);

                StringBuilder printerName = new StringBuilder((int)length);

                if (!GetDefaultPrinter(printerName, ref length)) throw new PrintingException(Marshal.GetLastWin32Error());

                string name = printerName.ToString();

                return All.Where(p => p.Name == name).FirstOrDefault();
            }

            set
            {
                if (!SetDefaultPrinter(value?.Name)) throw new PrintingException(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Список всех установленных принтеров в системе.
        /// </summary>
        public static Printer[] All
        {
            get
            {
                uint bytesNeeded = 0;
                uint bufferReturnedLength = 0;
                uint level = 2;
                PrinterEnumFlag flags = PrinterEnumFlag.Local | PrinterEnumFlag.Network;

                if (EnumPrinters(flags, null, level, IntPtr.Zero, 0, ref bytesNeeded, ref bufferReturnedLength)) return null;

                int lastWin32Error = Marshal.GetLastWin32Error();

                if (lastWin32Error != PrintingException.ErrorInsufficientBuffer) throw new PrintingException(lastWin32Error);

                IntPtr printersPtr = Marshal.AllocHGlobal((int)bytesNeeded);

                try
                {
                    if (EnumPrinters(flags, null, level, printersPtr, bytesNeeded, ref bytesNeeded, ref bufferReturnedLength))
                    {
                        IntPtr currentPrinterPtr = printersPtr;
                        PrinterInfo[] printerInfo = new PrinterInfo[bufferReturnedLength];
                        Printer[] printers = new Printer[bufferReturnedLength];
                        Type type = typeof(PrinterInfo);

                        for (int i = 0; i < bufferReturnedLength; i++)
                        {
                            printerInfo[i] = (PrinterInfo)Marshal.PtrToStructure(currentPrinterPtr, type);
                            currentPrinterPtr = (IntPtr)(currentPrinterPtr.ToInt64() + Marshal.SizeOf(type));

                            printers[i] = new Printer(printerInfo[i].PrinterName, printerInfo[i].PortName, printerInfo[i].DriverName, printerInfo[i].PrintProcessor,
                                printerInfo[i].ShareName, printerInfo[i].ServerName, printerInfo[i].Comment,
                                (DataType)Enum.Parse(typeof(DataType), printerInfo[i].DataType), printerInfo[i].Location, printerInfo[i].Parameters,
                                printerInfo[i].SepFile);
                        }

                        return printers;
                    }

                    throw new PrintingException(Marshal.GetLastWin32Error());
                }
                catch
                {
                    return null;
                }
                finally
                {
                    Marshal.FreeHGlobal(printersPtr);
                }
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        /// <param name="location">Местоположение принтера.</param>
        /// <param name="parameters">Параметры принтера.</param>
        /// <param name="sepFile"></param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName, string serverName, string description, DataType dataType,
            string location, string parameters, string sepFile) : base(name)
        {
            Port[] ports = PrintingApi.Ports;
            Driver[] drivers = PrintingApi.Drivers;

            if (ports.Select(p => p.Name).Contains(portName)) Port = ports.Where(p => p.Name == portName).FirstOrDefault();
            if (drivers.Select(d => d.Name).Contains(driverName)) Driver = drivers.Where(d => d.Name == driverName).FirstOrDefault();

            Processor = processorName;
            ShareName = shareName;
            ServerName = serverName;
            Description = description;
            DataType = dataType;
            Location = location;
            Parameters = parameters;
            SepFile = sepFile;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        /// <param name="location">Местоположение принтера.</param>
        /// <param name="parameters">Параметры принтера.</param>
        /// <param name="sepFile"></param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName, string serverName, string description, DataType dataType,
            string location, string parameters, string sepFile)
            : this(name, port?.Name, driver?.Name, processorName, shareName, serverName, description, dataType, location, parameters, sepFile) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        /// <param name="location">Местоположение принтера.</param>
        /// <param name="parameters">Параметры принтера.</param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName, string serverName, string description, DataType dataType,
            string location, string parameters)
            : this(name, portName, driverName, processorName, shareName, serverName, description, dataType, location, parameters, null) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        /// <param name="location">Местоположение принтера.</param>
        /// <param name="parameters">Параметры принтера.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName, string serverName, string description, DataType dataType,
            string location, string parameters)
            : this(name, port?.Name, driver?.Name, processorName, shareName, serverName, description, dataType, location, parameters) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        /// <param name="location">Местоположение принтера.</param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName, string serverName, string description, DataType dataType,
            string location)
            : this(name, portName, driverName, processorName, shareName, serverName, description, dataType, location, null) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        /// <param name="location">Местоположение принтера.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName, string serverName, string description, DataType dataType,
            string location)
            : this(name, port?.Name, driver?.Name, processorName, shareName, serverName, description, dataType, location) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName, string serverName, string description, DataType dataType)
            : this(name, portName, driverName, processorName, shareName, serverName, description, dataType, null) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        /// <param name="dataType">Тип данных печати.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName, string serverName, string description, DataType dataType)
            : this(name, port?.Name, driver?.Name, processorName, shareName, serverName, description, dataType) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName, string serverName, string description)
            : this(name, portName, driverName, processorName, shareName, serverName, description, DataType.RAW) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="description">Описание принтера.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName, string serverName, string description)
            : this(name, port?.Name, driver?.Name, processorName, shareName, serverName, description) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName, string serverName)
            : this(name, portName, driverName, processorName, shareName, serverName, null) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName, string serverName)
            : this(name, port?.Name, driver?.Name, processorName, shareName, serverName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        public Printer(string name, string portName, string driverName, string processorName, string shareName)
            : this(name, portName, driverName, processorName, shareName, null) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName, string shareName) : this(name, port?.Name, driver?.Name, processorName, shareName)
        { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        public Printer(string name, string portName, string driverName, string processorName) : this(name, portName, driverName, processorName, null) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        public Printer(string name, IPort port, IDriver driver, string processorName) : this(name, port?.Name, driver?.Name, processorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="portName">Наименование порта.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        public Printer(string name, string portName, string driverName) : this(name, portName, driverName, "WinPrint") { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        public Printer(string name, IPort port, IDriver driver) : this(name, port?.Name, driver?.Name) { }

        /// <summary>
        /// Устанавливает принтер в системе.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        public override void Install(string serverName)
        {
            try
            {
                if (All.Select(p => p.Name).Contains(Name)) Uninstall(serverName);

                PrinterInfo printerInfo = new PrinterInfo
                {
                    ServerName = serverName,
                    PrinterName = Name,
                    ShareName = ShareName,
                    PortName = Port?.Name,
                    DriverName = Driver?.Name,
                    Comment = Description,
                    Location = Location,
                    DevMode = new IntPtr(0),
                    SepFile = SepFile,
                    PrintProcessor = Processor,
                    DataType = Enum.GetName(typeof(DataType), DataType),
                    Parameters = Parameters,
                    SecurityDescriptor = new IntPtr(0),
                };

                if (AddPrinter(serverName, 2, ref printerInfo)) return;

                int lastWin32ErrorCode = Marshal.GetLastWin32Error();

                if (lastWin32ErrorCode == 0) return;

                throw new PrintingException(lastWin32ErrorCode);
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        /// <summary>
        /// Удаляет принтер из системы.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        public override void Uninstall(string serverName)
        {
            try
            {
                if (!All.Select(p => p.Name).Contains(Name)) return;

                PrinterDefaults defaults = new PrinterDefaults { DesiredAccess = PrinterAccess.PrinterAllAccess };

                if (!NET.Port.OpenPrinter(Name, out IntPtr printerHandle, ref defaults)) throw new PrintingException(Marshal.GetLastWin32Error());

                if (!DeletePrinter(printerHandle))
                {
                    int lastWin32ErrorCode = Marshal.GetLastWin32Error();

                    if (lastWin32ErrorCode == PrintingException.ErrorInvalidPrinterName) return;

                    throw new PrintingException(lastWin32ErrorCode);
                }

                NET.Port.ClosePrinter(printerHandle);
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        #region Native
        /// <summary>
        /// Устанавливает принтер в системе.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="printerInfo">Структура данных.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool AddPrinter(string serverName, uint level, [In] ref PrinterInfo printerInfo);

        /// <summary>
        /// Возвращает имя принтера, установленного в системе по умолчанию.
        /// </summary>
        /// <param name="printerName">Имя принтера.</param>
        /// <param name="bytesNeeded">Размер символьного буфера имени.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetDefaultPrinter([Out] StringBuilder printerName, ref uint bytesNeeded);

        /// <summary>
        /// Устанавливает имя принтера по умолчанию.
        /// </summary>
        /// <param name="printerName">Имя устанавливаемого по умолчанию принтера.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetDefaultPrinter(string printerName);

        /// <summary>
        /// Получает список установленных в системе принтеров.
        /// </summary>
        /// <param name="flags">Флаги для выборки результатов.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="printers">Указатель на буфер структур.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <param name="bytesNeeded">Число требуемых байт для выделения памяти под буфер.</param>
        /// <param name="bufferReturnedLength">Размер полученного буфера.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EnumPrinters(PrinterEnumFlag flags, string serverName, uint level, IntPtr printers, uint bufferSize, ref uint bytesNeeded,
            ref uint bufferReturnedLength);

        /// <summary>
        /// Удаляет принтер из системы.
        /// </summary>
        /// <param name="printer">Указатель на принтер.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool DeletePrinter(IntPtr printer);
        #endregion
    }
}