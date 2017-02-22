using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace Printing.NET
{
    /// <summary>
    /// Представляет API для работы со службой печати.
    /// </summary>
    public class PrintingApi
    {
        /// <summary>
        /// Вспомагательный делегат для оптимизации кода в P/Invoke.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Уровень структуры данных.</param>
        /// <param name="structs">Указатель на структуру данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <param name="bytesNeeded">Число требуемых байт для выделения памяти.</param>
        /// <param name="bufferReturnedLength">Число выделенных байт памяти.</param>
        /// <returns></returns>
        internal delegate bool EnumInfo(string serverName, uint level, IntPtr structs, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);

        /// <summary>
        /// Вспомагательный делегат для оптимизации кода в P/Invoke.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="environment">Окружение.</param>
        /// <param name="level">Уровень структуры данных.</param>
        /// <param name="structs">Указатель на структуру данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <param name="bytesNeeded">Число требуемых байт для выделения памяти.</param>
        /// <param name="bufferReturnedLength">Число выделенных байт памяти.</param>
        /// <returns></returns>
        internal delegate bool EnumInfo2(string serverName, string environment, uint level, IntPtr structs, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);

        /// <summary>
        /// Фабрика классов для <see cref="PrintingApi"/>.
        /// </summary>
        public static PrintingApi Factory { get; protected set; }

        /// <summary>
        /// Возвращает коллекцию всех установленных мониторов в системе.
        /// </summary>
        public static Monitor[] Monitors => Monitor.All;

        /// <summary>
        /// Возвращает коллекцию всех октрытых портов печати в системе.
        /// </summary>
        public static Port[] Ports => Port.All;

        /// <summary>
        /// Возвращает коллекцию всех установленных драйверов печати в системе.
        /// </summary>
        public static Driver[] Drivers => Driver.All;

        /// <summary>
        /// Возвращает коллекцию всех установленных устройств печати в системе.
        /// </summary>
        public static Printer[] Printers => Printer.All;

        /// <summary>
        /// Статический инициализатор класса <see cref="PrintingApi"/>.
        /// </summary>
        static PrintingApi()
        {
            Factory = new PrintingApi();
        }

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <param name="environment">Окружение, для которого был написан монитор печати.</param>
        /// <param name="serverName">Наименование сервера, на котором производится установка монитора печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public Monitor CreateMonitor(string name, string dll, Environment environment, string serverName)
        {
            Monitor monitor = new Monitor(name, dll, environment);
            monitor.TryInstall(serverName);

            return monitor;
        }

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <param name="environment">Окружение, для которого был написан монитор печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public Monitor CreateMonitor(string name, string dll, Environment environment) => CreateMonitor(name, dll, environment, null);

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <param name="serverName">Наименование сервера, на котором производится установка монитора печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public Monitor CreateMonitor(string name, string dll, string serverName) => CreateMonitor(name, dll, Environment.Current, null);

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public Monitor CreateMonitor(string name, string dll) => CreateMonitor(name, dll, null);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <param name="monitor">Монитор печати, на котором открывается порт.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, PortType type, Monitor monitor, string serverName)
        {
            Port port = new Port(name, description, type, monitor);
            monitor.TryInstall(serverName);

            return port;
        }

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <param name="monitor">Монитор печати, на котором открывается порт.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, PortType type, Monitor monitor) => OpenPort(name, description, type, monitor, null);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, PortType type, string serverName) => OpenPort(name, description, type, null, serverName);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, PortType type) => OpenPort(name, description, type, null as string);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="monitor">Монитор печати, на котором открывается порт.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, Monitor monitor, string serverName)
            => OpenPort(name, description, PortType.Redirected, monitor, serverName);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="monitor">Монитор печати, на котором открывается порт.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, Monitor monitor) => OpenPort(name, description, monitor, null);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="monitor">Монитор печати, на котором открывается порт.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, Monitor monitor, string serverName)
            => OpenPort(name, null, monitor, serverName);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="monitor">Монитор печати, на котором открывается порт.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, Monitor monitor) => OpenPort(name, monitor, null);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, PortType type, string serverName)
            => OpenPort(name, null, type, serverName);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="type">Тип порта.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, PortType type) => OpenPort(name, type, null);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description, string serverName) => OpenPort(name, description, null, serverName);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <param name="description">Описание порта.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name, string description) => OpenPort(name, description, null as string);

        /// <summary>
        /// Открывает новый порт печати в системе.
        /// </summary>
        /// <param name="name">Наименование порта.</param>
        /// <returns>Экземпляр порта печати.</returns>
        public Port OpenPort(string name) => OpenPort(name, null as string);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
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
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment,
            DataType defaultDataType, string dependentFiles, Monitor monitor, string serverName)
        {
            Driver driver = new Driver(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, dependentFiles, monitor);
            driver.TryInstall(serverName);

            return driver;
        }

        /// <summary>
        /// Устанавливает новый драйвер в системе.
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
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment,
            DataType defaultDataType, string dependentFiles, Monitor monitor)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, dependentFiles, monitor, null);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
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
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment,
            DataType defaultDataType, string dependentFiles, string serverName)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, dependentFiles, null, serverName);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
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
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment,
            DataType defaultDataType, string dependentFiles)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, dependentFiles, null as string);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="defaultDataType">Тип данных принтера по умолчанию.</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment,
            DataType defaultDataType)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, environment, defaultDataType, null);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment,
            string serverName)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, environment, DataType.RAW, serverName);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, Environment environment)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, environment, null);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version, string serverName)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, Environment.Current, serverName);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="version">Номер версии операционной системы, для которой был написан драйвер. Поддерживаемые значения - 3 и 4 (V3 и V4 номера версий драйвера соответственно).</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, uint version)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, version, null);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile, string serverName)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, 3, serverName);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <param name="helpFile">Полный или относительный путь к dll данных HLP-файла драйвера (например, "C:\DRIVERS\Pscript.hlp").</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile, string helpFile)
            => InstallDriver(name, dll, dataFile, configFile, helpFile, null);

        /// <summary>
        /// Устанавливает новый драйвер в системе.
        /// </summary>
        /// <param name="name">Наименование драйвера.</param>
        /// <param name="dll">Полный или относительный путь к файлу драйвера устройства (например, "C:\DRIVERS\Pscript.dll").</param>
        /// <param name="dataFile">Полный или относительный путь к файлу данных драйвера (например, "C:\DRIVERS\Qms810.ppd").</param>
        /// <param name="configFile">Полный или относительный путь к dll данных конфигурации драйвера (например, "C:\DRIVERS\Pscriptui.dll").</param>
        /// <returns>Экземпляр драйвера печати.</returns>
        public Driver InstallDriver(string name, string dll, string dataFile, string configFile) => InstallDriver(name, dll, dataFile, configFile, null);

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
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName, string serverName, string description,
            DataType dataType, string location, string parameters, string sepFile)
        {
            Printer printer = new Printer(name, port, driver, processorName, shareName, serverName, description, dataType, location, parameters, sepFile);
            printer.TryInstall(serverName);

            return printer;
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
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName, string serverName, string description,
            DataType dataType, string location, string parameters)
            => RunPrinter(name, port, driver, processorName, shareName, serverName, description, dataType, location, parameters, null);

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
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName, string serverName, string description,
            DataType dataType, string location)
            => RunPrinter(name, port, driver, processorName, shareName, serverName, description, dataType, location, null);

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
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName, string serverName, string description,
            DataType dataType)
            => RunPrinter(name, port, driver, processorName, shareName, serverName, description, dataType, null);

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
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName, string serverName, string description)
            => RunPrinter(name, port, driver, processorName, shareName, serverName, description, DataType.RAW);

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName, string serverName)
            => RunPrinter(name, port, driver, processorName, shareName, serverName, null);

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <param name="shareName">Публичное наименование принтера.</param>
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName, string shareName)
            => RunPrinter(name, port, driver, processorName, shareName, null);

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <param name="processorName">Наименование процессора печати.</param>
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver, string processorName) => RunPrinter(name, port, driver, processorName, null);

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Printer"/>.
        /// </summary>
        /// <param name="name">Наименование принтера.</param>
        /// <param name="port">Порт, к которому привязан принтер.</param>
        /// <param name="driver">Драйвер, который связан с принтером.</param>
        /// <returns>Экземпляр устройства печати.</returns>
        public Printer RunPrinter(string name, Port port, Driver driver) => RunPrinter(name, port, driver, "WinPrint");

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo handler, string serverName, uint level) where T : struct
        {
            uint bytesNeeded = 0;
            uint bufferReturnedLength = 0;

            if (handler(serverName, level, IntPtr.Zero, 0, ref bytesNeeded, ref bufferReturnedLength)) return null;

            int lastWin32Error = Marshal.GetLastWin32Error();

            if (lastWin32Error != PrintingException.ErrorInsufficientBuffer) throw new PrintingException(lastWin32Error);

            IntPtr pointer = Marshal.AllocHGlobal((int)bytesNeeded);

            try
            {
                if (handler(serverName, level, pointer, bytesNeeded, ref bytesNeeded, ref bufferReturnedLength))
                {
                    IntPtr currentPointer = pointer;
                    T[] dataCollection = new T[bufferReturnedLength];
                    Type type = typeof(T);

                    for (int i = 0; i < bufferReturnedLength; i++)
                    {
                        dataCollection[i] = (T)Marshal.PtrToStructure(currentPointer, type);
                        currentPointer = (IntPtr)(currentPointer.ToInt64() + Marshal.SizeOf(type));
                    }
                    
                    return dataCollection;
                }

                throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo handler, string serverName) where T : struct => GetInfo<T>(handler, serverName, 2);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo handler, uint level) where T : struct => GetInfo<T>(handler, null, level);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo handler) where T : struct => GetInfo<T>(handler, null);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo2 handler, string serverName, string arg, uint level) where T : struct
        {
            uint bytesNeeded = 0;
            uint bufferReturnedLength = 0;

            if (handler(serverName, arg, level, IntPtr.Zero, 0, ref bytesNeeded, ref bufferReturnedLength)) return null;

            int lastWin32Error = Marshal.GetLastWin32Error();

            if (lastWin32Error != PrintingException.ErrorInsufficientBuffer) throw new PrintingException(lastWin32Error);

            IntPtr pointer = Marshal.AllocHGlobal((int)bytesNeeded);

            try
            {
                if (handler(serverName, arg, level, pointer, bytesNeeded, ref bytesNeeded, ref bufferReturnedLength))
                {
                    IntPtr currentPointer = pointer;
                    T[] dataCollection = new T[bufferReturnedLength];
                    Type type = typeof(T);

                    for (int i = 0; i < bufferReturnedLength; i++)
                    {
                        dataCollection[i] = (T)Marshal.PtrToStructure(currentPointer, type);
                        currentPointer = (IntPtr)(currentPointer.ToInt64() + Marshal.SizeOf(type));
                    }

                    return dataCollection;
                }

                throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo2 handler, string serverName, string arg) where T : struct => GetInfo<T>(handler, serverName, arg, 2);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo2 handler, string arg, uint level) where T : struct => GetInfo<T>(handler, null, arg, level);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo2 handler, string arg) where T : struct => GetInfo<T>(handler, null, arg);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo2 handler, uint level) where T : struct => GetInfo<T>(handler, null, level);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <returns>Коллекция нативных структур Spooler API.</returns>
        internal static T[] GetInfo<T>(EnumInfo2 handler) where T : struct => GetInfo<T>(handler, null);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, uint level, out T[] dataCollection, out PrintingException e) where T : struct
        {
            dataCollection = null;
            e = null;

            try
            {
                dataCollection = GetInfo<T>(handler, serverName, level);
                return true;
            }
            catch (PrintingException ex)
            {
                e = ex;
            }

            return false;
        }

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, level, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, serverName, 2, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, 2, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, uint level, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo handler, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, uint level, out T[] dataCollection, out PrintingException e) where T : struct
        {
            dataCollection = null;
            e = null;

            try
            {
                dataCollection = GetInfo<T>(handler, serverName, arg, level);
                return true;
            }
            catch (PrintingException ex)
            {
                e = ex;
            }

            return false;
        }

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, arg, level, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, serverName, arg, 2, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, arg, 2, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, uint level, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, arg, level, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, arg, level, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, arg, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="arg">Дополнительный аргумент делегата.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, arg, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, uint level, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="level">Уровень структуры.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out PrintingException e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <param name="e">Исключение, возникшее во время операции.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out e);

        /// <summary>
        /// Получает коллекцию нативных структур Spooler API и в случае успеха возвращает True. Вспомагательный метод для оптимизации кода в P/Invoke.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <param name="handler">Делегат-обработчик нативного метода из Spooler API.</param>
        /// <param name="dataCollection">Коллекция нативных структур Spooler API.</param>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        internal static bool TryGetInfo<T>(EnumInfo2 handler, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out PrintingException e);

        /// <summary>
        /// Перезагружает службу печати.
        /// </summary>
        /// <returns>True, если операция прошла успешно, иначе False.</returns>
        public static bool TryRestart()
        {
            int tryCount = 5;

            while (tryCount > 0)
            {
                try
                {
                    ServiceController sc = new ServiceController("Spooler");

                    if (sc.Status != ServiceControllerStatus.Stopped || sc.Status != ServiceControllerStatus.StopPending)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    }

                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);

                    return sc.Status == ServiceControllerStatus.Running;
                }
                catch
                {
                    tryCount--;
                }
            }

            return false;
        }
    }
}