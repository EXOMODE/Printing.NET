using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Printing.NET.Tests
{
    /// <summary>
    /// ѕредставл€ет тестовый модуль класса <see cref="Monitor"/>.
    /// </summary>
    [TestClass]
    public class MonitorTests
    {
        /// <summary>
        /// Ќаименование монитора.
        /// </summary>
        protected const string MonitorName = "Test Monitor";

        /// <summary>
        /// ѕуть к dll монитора.
        /// </summary>
        protected const string MonitorDll = "D:/Printing Tests/mfilemon.dll";

        /// <summary>
        /// Ќеправильный путь к dll монитора.
        /// </summary>
        protected const string FailedMonitorDll = "noexist.dll";

        /// <summary>
        /// “ест локальной установки монитора.
        /// </summary>
        [TestMethod]
        public void InstallTest()
        {
            Monitor monitor = new Monitor(MonitorName, MonitorDll);
            monitor.Install();

            Assert.IsTrue(Monitor.All.Select(m => m.Name).Contains(MonitorName));
        }

        /// <summary>
        /// “ест локального удалени€ монитора.
        /// </summary>
        [TestMethod]
        public void UninstallTest()
        {
            Monitor monitor = new Monitor(MonitorName, MonitorDll);
            monitor.Uninstall();

            Assert.IsFalse(Monitor.All.Select(m => m.Name).Contains(MonitorName));
        }

        /// <summary>
        /// “ест локальной установки монитора с перехватом состо€ни€ установки.
        /// </summary>
        [TestMethod]
        public void TryInstallTest()
        {
            Monitor monitor = new Monitor(MonitorName, MonitorDll);
            bool f = monitor.TryInstall();

            Assert.IsTrue(f);
            Assert.IsTrue(Monitor.All.Select(m => m.Name).Contains(MonitorName));
        }

        /// <summary>
        /// “ест локального удалени€ монитора с перехватом состо€ни€ удалени€.
        /// </summary>
        [TestMethod]
        public void TryUninstallTest()
        {
            Monitor monitor = new Monitor(MonitorName, MonitorDll);
            bool f = monitor.TryUninstall();

            Assert.IsTrue(f);
            Assert.IsFalse(Monitor.All.Select(m => m.Name).Contains(MonitorName));
        }

        /// <summary>
        /// “ест неправильной локальной установки монитора.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(PrintingException))]
        public void InstallFailedTest()
        {
            Monitor monitor = new Monitor(MonitorName, FailedMonitorDll);
            monitor.Install();

            Assert.IsFalse(Monitor.All.Select(m => m.Name).Contains(MonitorName));
        }
        
        /// <summary>
        /// “ест неправильной локальной установки монитора с перехватом состо€ни€ установки.
        /// </summary>
        [TestMethod]
        public void TryInstallFailedTest()
        {
            Monitor monitor = new Monitor(MonitorName, FailedMonitorDll);
            bool f = monitor.TryInstall();

            Assert.IsFalse(f);
            Assert.IsFalse(Monitor.All.Select(m => m.Name).Contains(MonitorName));
        }
    }
}