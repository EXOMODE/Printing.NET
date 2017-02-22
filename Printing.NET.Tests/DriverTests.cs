using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Printing.NET.Tests
{
    /// <summary>
    /// Представляет тестовый модуль класса <see cref="Driver"/>.
    /// </summary>
    [TestClass]
    public class DriverTests
    {
        /// <summary>
        /// Наименование драйвера.
        /// </summary>
        protected const string DriverName = "Test Driver";

        /// <summary>
        /// Наименование монитора.
        /// </summary>
        protected const string MonitorName = "mfilemon";

        /// <summary>
        /// Наименование несуществующего монитора.
        /// </summary>
        protected const string FailedMonitorName = "noexist";

        protected const string DllPath = "D:/Printing Tests/pscript5.dll";
        protected const string DataPath = "D:/Printing Tests/testprinter.ppd";
        protected const string ConfigPath = "D:/Printing Tests/ps5ui.dll";
        protected const string HelpPath = "D:/Printing Tests/pscript.hlp";

        /// <summary>
        /// Тест локальной установки драйвера.
        /// </summary>
        [TestMethod]
        public void InstallTest()
        {
            Driver driver = new Driver(DriverName, DllPath, DataPath, ConfigPath, HelpPath, MonitorName);
            driver.Install();

            Assert.IsTrue(Driver.All.Select(d => d.Name).Contains(DriverName));
        }

        /// <summary>
        /// Тест локального удаления драйвера.
        /// </summary>
        [TestMethod]
        public void UninstallTest()
        {
            Driver driver = new Driver(DriverName, DllPath, DataPath, ConfigPath, HelpPath, MonitorName);
            driver.Uninstall();

            Assert.IsFalse(Driver.All.Select(d => d.Name).Contains(DriverName));
        }

        /// <summary>
        /// Тест локальной установки драйвера с перехватом состояния установки.
        /// </summary>
        [TestMethod]
        public void TryInstallTest()
        {
            Driver driver = new Driver(DriverName, DllPath, DataPath, ConfigPath, HelpPath, MonitorName);
            bool f = driver.TryInstall();

            Assert.IsTrue(f);
            Assert.IsTrue(Driver.All.Select(d => d.Name).Contains(DriverName));
        }

        /// <summary>
        /// Тест локального удаления драйвера с перехватом состояния удаления.
        /// </summary>
        [TestMethod]
        public void TryUninstallTest()
        {
            Driver driver = new Driver(DriverName, DllPath, DataPath, ConfigPath, HelpPath, MonitorName);
            bool f = driver.TryUninstall();

            Assert.IsTrue(f);
            Assert.IsFalse(Driver.All.Select(d => d.Name).Contains(DriverName));
        }

        /// <summary>
        /// Тест неправильной локальной установки драйвера.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(PrintingException))]
        public void InstallFailedTest()
        {
            Driver driver = new Driver(DriverName, DllPath + "failed", DataPath, ConfigPath, HelpPath, FailedMonitorName);
            driver.Install();

            Assert.IsFalse(Driver.All.Select(d => d.Name).Contains(DriverName));
        }

        /// <summary>
        /// Тест неправильной локальной установки драйвера с перехватом состояния установки.
        /// </summary>
        [TestMethod]
        public void TryInstallFailedTest()
        {
            Driver driver = new Driver(DriverName, DllPath + "failed", DataPath, ConfigPath, HelpPath, FailedMonitorName);
            bool f = driver.TryInstall();

            Assert.IsTrue(f);
            Assert.IsFalse(Driver.All.Select(d => d.Name).Contains(DriverName));
        }
    }
}