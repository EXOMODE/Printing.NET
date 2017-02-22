using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Printing.NET.Tests
{
    /// <summary>
    /// Представляет тестовый модуль класса <see cref="Printer"/>.
    /// </summary>
    [TestClass]
    public class PrinterTests
    {
        /// <summary>
        /// Наименование принтера.
        /// </summary>
        protected const string PrinterName = "Test Printer";

        /// <summary>
        /// Наименование порта.
        /// </summary>
        protected const string PortName = "TESTPORT:";
        
        /// <summary>
        /// Наименование драйвера.
        /// </summary>
        protected const string DriverName = "Test Driver";
        
        /// <summary>
        /// Тест локальной установки принтера.
        /// </summary>
        [TestMethod]
        public void InstallTest()
        {
            Printer printer = new Printer(PrinterName, PortName, DriverName);
            printer.Install();

            Assert.IsTrue(Printer.All.Select(p => p.Name).Contains(PrinterName));
        }

        /// <summary>
        /// Тест локального удаления принтера.
        /// </summary>
        [TestMethod]
        public void UninstallTest()
        {
            Printer printer = new Printer(PrinterName, PortName, DriverName);
            printer.Uninstall();

            Assert.IsFalse(Printer.All.Select(p => p.Name).Contains(PrinterName));
        }

        /// <summary>
        /// Тест локальной установки принтера с перехватом состояния установки.
        /// </summary>
        [TestMethod]
        public void TryInstallTest()
        {
            Printer printer = new Printer(PrinterName, PortName, DriverName);
            bool f = printer.TryInstall();

            Assert.IsTrue(f);
            Assert.IsTrue(Printer.All.Select(p => p.Name).Contains(PrinterName));
        }

        /// <summary>
        /// Тест локального удаления принтера с перехватом состояния удаления.
        /// </summary>
        [TestMethod]
        public void TryUninstallTest()
        {
            Printer printer = new Printer(PrinterName, PortName, DriverName);
            bool f = printer.TryUninstall();

            Assert.IsTrue(f);
            Assert.IsFalse(Printer.All.Select(p => p.Name).Contains(PrinterName));
        }
    }
}