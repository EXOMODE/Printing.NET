using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Printing.NET.Tests
{
    [TestClass]
    public class PrintingApiTests
    {
        protected const string MonitorName = "mfilemon";
        protected const string PortName = "TESTPORT:";
        protected const string DriverName = "Test Driver";
        protected const string PrinterName = "Test Printer";

        protected const string MonitorFile = "D:/Printing Tests/mfilemon.dll";
        protected const string DriverFile = "D:/Printing Tests/pscript5.dll";
        protected const string DriverDataFile = "D:/Printing Tests/testprinter.ppd";
        protected const string DriverConfigFile = "D:/Printing Tests/ps5ui.dll";
        protected const string DriverHelpFile = "D:/Printing Tests/pscript.hlp";

        [TestMethod]
        public void PrinterInstallationTest()
        {
            PrintingApi.TryRestart();

            Monitor monitor = PrintingApi.Factory.CreateMonitor(MonitorName, MonitorFile);
            Port port = PrintingApi.Factory.OpenPort(PortName, monitor);
            Driver driver = PrintingApi.Factory.InstallDriver(DriverName, DriverFile, DriverDataFile, DriverConfigFile, DriverHelpFile, 3, Environment.Current, DataType.RAW, null, monitor);
            Printer printer = PrintingApi.Factory.RunPrinter(PrinterName, port, driver);

            PrintingApi.TryRestart();

            Assert.IsNotNull(printer);
        }
    }
}