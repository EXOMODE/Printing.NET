using Printing.NET.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Printing.NET
{
    /// <summary>
    /// Представляет монитор печати для открытия портов.
    /// </summary>
    public class Monitor : IMonitor
    {
        /// <summary>
        /// Наименование монитора печати.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Окружение, для которого был написан драйвер (например, Windows NT x86, Windows IA64 или Windows x64).
        /// </summary>
        public virtual Environment Environment { get; protected set; }

        /// <summary>
        /// Имя файла *.dll монитора.
        /// </summary>
        public virtual string Dll { get; protected set; }

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
        /// <exception cref="FileNotFoundException" />
        public Monitor(string name, string dll, Environment environment)
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
        public Monitor(string name, string dll) : this(name, dll, Environment.Current) { }

        /// <summary>
        /// Устанавливает монитор печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>True, если процедура установки прошла успешно, иначе False.</returns>
        public virtual void Install(string serverName)
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
        /// Устанавливает монитор печати на локальной машине.
        /// </summary>
        /// <returns>True, если процедура установки прошла успешно, иначе False.</returns>
        public void Install() => Install(null);

        public bool TryInstall(string serverName, out PrintingException e)
        {
            e = null;

            try
            {
                Install(serverName);
            }
            catch (PrintingException ex)
            {
                e = ex;
                return false;
            }

            return true;
        }

        public bool TryInstall(string serverName) => TryInstall(serverName, out PrintingException e);

        public bool TryInstall() => TryInstall(null);

        /// <summary>
        /// Удалает монитор печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>True, если процедура удаления прошла успешно, иначе False.</returns>
        public virtual void Uninstall(string serverName)
        {
            try
            {
                //IEnumerable<string> ports = Ports.Where(p => p.MonitorName == monitorName).Select(p => p.MonitorName);

                //foreach (string port in ports) RemovePort(port, monitorName);

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

        /// <summary>
        /// Удаляет монитор печати на локальной машине.
        /// </summary>
        /// <returns>True, если процедура удаления прошла успешно, иначе False.</returns>
        public void Uninstall() => Uninstall(null);

        public bool TryUninstall(string serverName, out PrintingException e)
        {
            e = null;

            try
            {
                Uninstall(serverName);
            }
            catch (PrintingException ex)
            {
                e = ex;
                return false;
            }

            return true;
        }

        public bool TryUninstall(string serverName) => TryUninstall(serverName, out PrintingException e);

        public bool TryUninstall() => TryUninstall(null);

        #region Native
        /// <summary>
        /// Производит установку монитора принтера в систему.
        /// </summary>
        /// <param name="serverName">Имя сервера, на который необходимо произвести установку. Если равно null - устанавливает на локальную машину.</param>
        /// <param name="level">Номер версии структуры. Должен быть равен 2.</param>
        /// <param name="monitor">Экземпляр структуры <see cref="MonitorInfo"/>.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EnumMonitors(string serverName, uint level, IntPtr monitors, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);

        /// <summary>
        /// Производит удаление монитора принтера из системы.
        /// </summary>
        /// <param name="serverName">Имя сервера, на который необходимо произвести установку. Если равно null - устанавливает на локальную машину.</param>
        /// <param name="environment">Окружение, для которого был написан драйвер (например, Windows x86, Windows IA64 или Windows x64).</param>
        /// <param name="monitorName">Имя удаляемого монитора.</param>
        /// <returns></returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeleteMonitor(string serverName, string environment, string monitorName);
        #endregion
    }
}