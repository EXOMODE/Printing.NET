namespace Printing.NET
{
    /// <summary>
    /// Представляет интерфейс для реализации принтеров.
    /// </summary>
    public interface IPrinter : IPrintableDevice
    {
        /// <summary>
        /// Порт, к которому привязан принтер.
        /// </summary>
        IPort Port { get; }

        /// <summary>
        /// Драйвер, который связан с принтером.
        /// </summary>
        IDriver Driver { get; }

        /// <summary>
        /// Публичное наименование принтера.
        /// </summary>
        string ShareName { get; }

        /// <summary>
        /// Имя сервера, на котором запущен принтер.
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Описание устройства принтера.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Расположение принтера.
        /// </summary>
        string Location { get; }
        
        string SepFile { get; }

        /// <summary>
        /// Параметры принтера.
        /// </summary>
        string Parameters { get; }

        /// <summary>
        /// Тип данных печати.
        /// </summary>
        DataType DataType { get; }
    }
}