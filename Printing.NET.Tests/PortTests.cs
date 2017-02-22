using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Printing.NET.Tests
{
    /// <summary>
    /// Представляет тестовый модуль класса <see cref="Port"/>.
    /// </summary>
    [TestClass]
    public class PortTests
    {
        /// <summary>
        /// Наименование порта.
        /// </summary>
        protected const string PortName = "TESTPORT:";

        /// <summary>
        /// Описание порта.
        /// </summary>
        protected const string PortDescription = "Description for " + PortName;

        /// <summary>
        /// Наименование монитора.
        /// </summary>
        protected const string MonitorName = "mfilemon";

        /// <summary>
        /// Наименование несуществующего монитора.
        /// </summary>
        protected const string FailedMonitorName = "noexist";

        /// <summary>
        /// Тест локальной установки порта.
        /// </summary>
        [TestMethod]
        public void InstallTest()
        {
            Port port = new Port(PortName, PortDescription, MonitorName);
            port.Install();

            Assert.IsTrue(Port.All.Select(p => p.Name).Contains(PortName));
        }

        /// <summary>
        /// Тест локального удаления порта.
        /// </summary>
        [TestMethod]
        public void UninstallTest()
        {
            Port port = new Port(PortName, PortDescription, MonitorName);
            port.Uninstall();

            Assert.IsFalse(Port.All.Select(p => p.Name).Contains(PortName));
        }

        /// <summary>
        /// Тест локальной установки порта с перехватом состояния установки.
        /// </summary>
        [TestMethod]
        public void TryInstallTest()
        {
            Port port = new Port(PortName, PortDescription, MonitorName);
            bool f = port.TryInstall();

            Assert.IsTrue(f);
            Assert.IsTrue(Port.All.Select(p => p.Name).Contains(PortName));
        }

        /// <summary>
        /// Тест локального удаления порта с перехватом состояния удаления.
        /// </summary>
        [TestMethod]
        public void TryUninstallTest()
        {
            Port port = new Port(PortName, PortDescription, MonitorName);
            bool f = port.TryUninstall();

            Assert.IsTrue(f);
            Assert.IsFalse(Port.All.Select(p => p.Name).Contains(PortName));
        }

        /// <summary>
        /// Тест неправильной локальной установки порта.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(PrintingException))]
        public void InstallFailedTest()
        {
            Port port = new Port(PortName, PortDescription, MonitorName);
            port.Install();

            Assert.IsFalse(Port.All.Select(p => p.Name).Contains(PortName));
        }

        /// <summary>
        /// Тест неправильной локальной установки порта с перехватом состояния установки.
        /// </summary>
        [TestMethod]
        public void TryInstallFailedTest()
        {
            Port port = new Port(PortName, PortDescription, FailedMonitorName);
            bool f = port.TryUninstall();

            Assert.IsTrue(f);
            Assert.IsFalse(Port.All.Select(p => p.Name).Contains(PortName));
        }
    }
}