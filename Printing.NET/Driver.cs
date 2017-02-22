using Printing.NET.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Printing.NET
{
    /// <summary>
    /// Представляет данные для работы с драйвером печати.
    /// </summary>
    public class Driver : PrintableDevice, IDriver
    {
        /// <summary>
        /// Монитор печати.
        /// </summary>
        public virtual IMonitor Monitor { get; protected set; }

        /// <summary>
        /// Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).
        /// </summary>
        public virtual uint Version { get; protected set; }

        /// <summary>
        /// Окружение, для которого был написан драйвер.
        /// </summary>
        public virtual Environment Environment { get; protected set; }

        /// <summary>
        /// Полный или относительный путь к файлу драйвера устройства.
        /// </summary>
        public virtual string Dll { get; protected set; }

        /// <summary>
        /// Полный или относительный путь к файлу данных драйвера.
        /// </summary>
        public virtual string DataFile { get; protected set; }

        /// <summary>
        /// Полный или относительный путь к dll данных конфигурации драйвера.
        /// </summary>
        public virtual string ConfigFile { get; protected set; }

        /// <summary>
        /// Полный или относительный путь к dll данных HLP-файла драйвера.
        /// </summary>
        public virtual string HelpFile { get; protected set; }

        /// <summary>
        /// Зависимые файлы.
        /// </summary>
        public virtual string DependentFiles { get; protected set; }
        
        /// <summary>
        /// Тип данных принтера по умолчанию.
        /// </summary>
        public virtual DataType DefaultDataType { get; protected set; }

        /// <summary>
        /// Путь к системному каталогу драйверов печати.
        /// </summary>
        public static string Directory { get; protected set; }

        /// <summary>
        /// Возвращает коллекцию всех установленных драйверов печати в системе.
        /// </summary>
        public static Driver[] All
        {
            get
            {
                if (!PrintingApi.TryGetInfo(EnumPrinterDrivers, 3, out DriverInfo[] driverInfo)) return null;

                Driver[] drivers = new Driver[driverInfo.Length];

                for (int i = 0; i < driverInfo.Length; i++)
                    drivers[i] = new Driver(driverInfo[i].Name, driverInfo[i].DriverPath, driverInfo[i].DataFile, driverInfo[i].ConfigFile, driverInfo[i].HelpFile,
                        driverInfo[i].Version, driverInfo[i].Environment.GetEnvironment(), (DataType)Enum.Parse(typeof(DataType), driverInfo[i].DefaultDataType ?? "RAW", true),
                        driverInfo[i].DependentFiles, driverInfo[i].MonitorName);

                return drivers;
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="defaultDataType">Тип данных принтера по умолчанию.</param>
        /// <param name="dependentFiles">Зависимые файлы.</param>
        /// <param name="monitorName">Наименование монитора печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment, DataType defaultDataType,
            string dependentFiles, string monitorName) : base(name)
        {
            Dll = dll;
            DataFile = dataFile;
            ConfigFile = configFile;
            HelpFile = helpFile;
            Version = version;
            Environment = environment;
            DefaultDataType = defaultDataType;
            DependentFiles = dependentFiles;

            Monitor[] monitors = PrintingApi.Monitors;

            if (monitors.Select(m => m.Name).Contains(monitorName)) Monitor = monitors.Where(m => m.Name == monitorName).FirstOrDefault();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="defaultDataType">Тип данных принтера по умолчанию.</param>
        /// <param name="dependentFiles">Зависимые файлы.</param>
        /// <param name="monitor">Монитор печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment, DataType defaultDataType,
            string dependentFiles, IMonitor monitor)
            : this(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, dependentFiles, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="defaultDataType">Тип данных принтера по умолчанию.</param>
        /// <param name="monitorName">Наименование монитора печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment, DataType defaultDataType,
            string monitorName)
            : this(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, null, monitorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="defaultDataType">Тип данных принтера по умолчанию.</param>
        /// <param name="monitor">Монитор печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment, DataType defaultDataType,
            IMonitor monitor)
            : this(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="monitorName">Наименование монитора печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment, string monitorName)
            : this(name, dll, dataFile, configFile, helpFile, version, environment, DataType.RAW, monitorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="monitor">Монитор печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment, IMonitor monitor)
            : this(name, dll, dataFile, configFile, helpFile, version, environment, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="monitorName">Наименование монитора печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, string monitorName)
            : this(name, dll, dataFile, configFile, helpFile, version, Environment.Current, monitorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="monitor">Монитор печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, IMonitor monitor)
            : this(name, dll, dataFile, configFile, helpFile, version, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="monitorName">Наименование монитора печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, string monitorName)
            : this(name, dll, dataFile, configFile, helpFile, 3, monitorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="monitor">Монитор печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string helpFile, IMonitor monitor)
            : this(name, dll, dataFile, configFile, helpFile, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="monitorName">Наименование монитора печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, string monitorName)
            : this(name, dll, dataFile, configFile, null, monitorName) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="monitor">Монитор печати.</param>
        public Driver(string name, string dll, string dataFile, string configFile, IMonitor monitor)
            : this(name, dll, dataFile, configFile, monitor?.Name) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Driver"/>.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        public Driver(string name, string dll, string dataFile, string configFile)
            : this(name, dll, dataFile, configFile, null as string) { }

        /// <summary>
        /// Статический инициализатор класса <see cref="Driver"/>.
        /// </summary>
        static Driver()
        {
            uint length = 1024;
            StringBuilder driverDirectory = new StringBuilder((int)length);
            uint bytesNeeded = 0;

            if (!GetPrinterDriverDirectory(null, null, 1, driverDirectory, length, ref bytesNeeded)) throw new PrintingException(Marshal.GetLastWin32Error());

            Directory = driverDirectory.ToString();
        }

        /// <summary>
        /// Устанавливает драйвер удалённо в системе.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        public override void Install(string serverName)
        {
            try
            {
                if (!File.Exists(Dll)) throw new PrintingException($"Не удалось найти файл драйвера '{Dll}'");
                if (!File.Exists(DataFile)) throw new PrintingException($"Не удалось найти файл драйвера '{DataFile}'");
                if (!File.Exists(ConfigFile)) throw new PrintingException($"Не удалось найти файл драйвера '{ConfigFile}'");

                if (All.Select(d => d.Name).Contains(Name)) Uninstall(serverName);
                
                string systemDriverPath = Path.Combine(Directory, Path.GetFileName(Dll));
                string systemDataPath = Path.Combine(Directory, Path.GetFileName(DataFile));
                string systemConfigPath = Path.Combine(Directory, Path.GetFileName(ConfigFile));
                string systemHelpPath = Path.Combine(Directory, Path.GetFileName(HelpFile));

                File.Copy(Dll, systemDriverPath, true);
                File.Copy(DataFile, systemDataPath, true);
                File.Copy(ConfigFile, systemConfigPath, true);
                
                if (File.Exists(HelpFile)) File.Copy(HelpFile, systemHelpPath, true);

                DriverInfo driverInfo = new DriverInfo
                {
                    Version = Version,
                    Name = Name,
                    Environment = Environment.GetEnvironmentName(),
                    DriverPath = File.Exists(systemDriverPath) ? systemDriverPath : Dll,
                    DataFile = File.Exists(systemDataPath) ? systemDataPath : DataFile,
                    ConfigFile = File.Exists(systemConfigPath) ? systemConfigPath : ConfigFile,
                    HelpFile = File.Exists(systemHelpPath) ? systemHelpPath : HelpFile,
                    DependentFiles = DependentFiles,
                    MonitorName = Monitor?.Name,
                    DefaultDataType = Enum.GetName(typeof(DataType), DefaultDataType),
                };

                if (AddPrinterDriver(serverName, Version, ref driverInfo)) return;

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
        /// Удаляет драйвер удалённо в системе.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        public override void Uninstall(string serverName)
        {
            try
            {
                if (!All.Select(d => d.Name).Contains(Name)) return;

                IEnumerable<Printer> printers = Printer.All.Where(p => p.Driver?.Name == Name);
                foreach (Printer printer in printers) printer.Uninstall(serverName);

                if (DeletePrinterDriver(serverName, Environment.GetEnvironmentName(), Name)) return;

                throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        #region Native
        /// <summary>
        /// Возвращает путь к системной директории с установленными драйверами принтера.
        /// </summary>
        /// <param name="serverName">Имя сервера, с которого требуется получить путь к директории драйвера принтера. Если равно null - получает локальный путь.</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 1.</param>
        /// <param name="driverDirectory">Путь к драйверу принтера.</param>
        /// <param name="bufferSize">Размер буфера для вывода пути.</param>
        /// <param name="bytesNeeded">Число полученных байт пути.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetPrinterDriverDirectory(string serverName, string environment, uint level, [Out] StringBuilder driverDirectory, uint bufferSize,
            ref uint bytesNeeded);

        /// <summary>
        /// Добавляет драйвер в систему.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 1, 2, 3, 4, 5, 6 или 8.</param>
        /// <param name="driverInfo">Данные драйвера.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool AddPrinterDriver(string serverName, uint level, ref DriverInfo driverInfo);

        /// <summary>
        /// Возвращает указатель на буфер экземпляров структур <see cref="DriverInfo"/>.
        /// </summary>
        /// <param name="serverName">Имя сервера, с которого требуется получить список портов. Если равно null - получает на локальной машине.</param>
        /// <param name="environment">Окружение (например, Windows x86, Windows IA64, Windows x64, или Windows NT R4000). Если параметр равен null,
        /// используется вызываемое окружение клиента (не сервера). Если параметр равен "all", метод верёт список драйверов для всех платформ,
        /// для которых они были установлены на сервере.</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 1, 2, 3, 4, 5, 6 или 8.</param>
        /// <param name="drivers">Указатель на буфер экземпляров структур <see cref="DriverInfo"/>.</param>
        /// <param name="bufferSize">Размер буфера экземпляров структур <see cref="DriverInfo"/> (в байтах).</param>
        /// <param name="bytesNeeded">Число полученных байт размера буфера.</param>
        /// <param name="bufferReturnedLength">Число экземпляров структур.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EnumPrinterDrivers(string serverName, string environment, uint level, IntPtr drivers, uint bufferSize, ref uint bytesNeeded,
            ref uint bufferReturnedLength);

        /// <summary>
        /// Удаляет драйвер из системы.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="environment">Окружение (например, Windows x86, Windows IA64, Windows x64, или Windows NT R4000). Если параметр равен null,
        /// используется вызываемое окружение клиента (не сервера). Если параметр равен "all", метод верёт список драйверов для всех платформ,
        /// для которых они были установлены на сервере.</param>
        /// <param name="driverName">Наименование драйвера.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeletePrinterDriver(string serverName, string environment, string driverName);
        #endregion
    }
}