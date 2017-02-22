using Printing.NET.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Printing.NET
{
    /// <summary>
    /// Представляет монитор печати для открытия портов.
    /// </summary>
    public class Monitor : PrintableDevice, IMonitor
    {
        /// <summary>
        /// Окружение, для которого был написан драйвер (например, Windows NT x86, Windows IA64 или Windows x64).
        /// </summary>
        public virtual Environment Environment { get; protected set; }

        /// <summary>
        /// Имя файла *.dll монитора.
        /// </summary>
        public virtual string Dll { get; protected set; }

        /// <summary>
        /// Возвращает список всех установленных в системе мониторов печати.
        /// </summary>
        public static Monitor[] All
        {
            get
            {
                if (!PrintingApi.TryGetInfo(EnumMonitors, out MonitorInfo[] monitorInfo)) return null;

                Monitor[] monitors = new Monitor[monitorInfo.Length];

                for (int i = 0; i < monitorInfo.Length; i++)
                    monitors[i] = new Monitor(monitorInfo[i].Name, monitorInfo[i].DllName, monitorInfo[i].Environment.GetEnvironment());

                return monitors;
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Monitor"/>.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Имя файла *.dll монитора.</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows NT x86, Windows IA64 или Windows x64).</param>
        /// <exception cref="ArgumentNullException" />
        public Monitor(string name, string dll, Environment environment) : base(name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(dll)) throw new ArgumentNullException("dll");

            Name = name;
            Environment = environment;
            Dll = dll;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Monitor"/>.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Имя файла *.dll монитора.</param>
        /// <exception cref="ArgumentNullException" />
        public Monitor(string name, string dll) : this(name, dll, Environment.Current) { }

        /// <summary>
        /// Устанавливает монитор печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="PrintingException"/>
        public override void Install(string serverName)
        {
            try
            {
                if (!File.Exists(Dll)) throw new FileNotFoundException("Не удалось найти файл монитора печати", Dll);

                string dllName = Path.GetFileName(Dll);
                string dllPath = Path.Combine(System.Environment.SystemDirectory, dllName);

                File.Copy(Dll, dllPath, true);

                MonitorInfo monitorInfo = new MonitorInfo
                {
                    Name = Name,
                    Environment = Environment.GetEnvironmentName(),
                    DllName = File.Exists(dllPath) ? dllName : Dll,
                };

                if (AddMonitor(serverName, 2, ref monitorInfo)) return;

                if (Marshal.GetLastWin32Error() == PrintingException.ErrorMonitorAlreadyInstalled && TryUninstall(serverName)
                    && AddMonitor(serverName, 2, ref monitorInfo))
                    return;
                else
                    throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        /// <summary>
        /// Удалает монитор печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <exception cref="PrintingException"/>
        public override void Uninstall(string serverName)
        {
            try
            {
                if (!All.Select(m => m.Name).Contains(Name)) return;

                IEnumerable<Port> openPorts = Port.All.Where(p => p.Monitor?.Name == Name);
                IEnumerable<Driver> drivers = Driver.All.Where(d => d.Monitor?.Name == Name);

                foreach (Port openPort in openPorts) openPort.Uninstall(serverName);
                foreach (Driver driver in drivers) driver.Uninstall(serverName);

                if (DeleteMonitor(serverName, Environment.GetEnvironmentName(), Name)) return;
                if (Marshal.GetLastWin32Error() == PrintingException.ErrorMonitorUnknown) return;
                if (DeleteMonitor(serverName, Environment.GetEnvironmentName(), Name)) return;

                throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
        }

        #region Native
        /// <summary>
        /// Производит установку монитора принтера в систему.
        /// </summary>
        /// <param name="serverName">Имя сервера, на который необходимо произвести установку. Если равно null - устанавливает на локальную машину.</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 2.</param>
        /// <param name="monitor">Экземпляр структуры <see cref="MonitorInfo"/>.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool AddMonitor(string serverName, uint level, ref MonitorInfo monitor);

        /// <summary>
        /// Возвращает указатель на буфер экземпляров структур <see cref="MonitorInfo"/>.
        /// </summary>
        /// <param name="serverName">Имя сервера, с которого требуется получить список мониторов. Если равно null - получает на локальной машине.</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 1 или 2.</param>
        /// <param name="monitors">Указатель на буфер экземпляров структур <see cref="MonitorInfo"/>.</param>
        /// <param name="bufferSize">Размер буфера экземпляров структур <see cref="MonitorInfo"/> (в байтах).</param>
        /// <param name="bytesNeeded">Число полученных байт размера буфера.</param>
        /// <param name="bufferReturnedLength">Число экземпляров структур.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EnumMonitors(string serverName, uint level, IntPtr monitors, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);

        /// <summary>
        /// Производит удаление монитора принтера из системы.
        /// </summary>
        /// <param name="serverName">Имя сервера, на который необходимо произвести установку. Если равно null - устанавливает на локальную машину.</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="monitorName">Имя удаляемого монитора.</param>
        /// <returns>True, если операция выполнена успешно, иначе False.</returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeleteMonitor(string serverName, string environment, string monitorName);
        #endregion
    }
}