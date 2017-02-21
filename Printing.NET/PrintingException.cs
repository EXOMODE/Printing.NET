using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Printing.NET
{
    /// <summary>
    /// Представляет ошибку менеджера печати.
    /// </summary>
    [Serializable]
    public class PrintingException : Win32Exception
    {
        #region Error Codes
        /// <summary>
        /// Код ошибки "Файл не найден".
        /// </summary>
        public const int ErrorFileNotFound = 2;

        /// <summary>
        /// Код ошибки "Неинициализированный буфер".
        /// </summary>
        public const int ErrorInsufficientBuffer = 122;

        /// <summary>
        /// Код ошибки "Модуль не найден".
        /// </summary>
        public const int ErrorModuleNotFound = 126;

        /// <summary>
        /// Код ошибки "Имя принтера задано неверно".
        /// </summary>
        public const int ErrorInvalidPrinterName = 1801;

        /// <summary>
        /// Код ошибки "Указан неизвестный монитор печати".
        /// </summary>
        public const int ErrorMonitorUnknown = 3000;

        /// <summary>
        /// Код ошибки "Указанный драйвер принтера занят".
        /// </summary>
        public const int ErrorPrinterDriverIsReadyUsed = 3001;

        /// <summary>
        /// Код ошибки "Не найден файл диспетчера очереди".
        /// </summary>
        public const int ErrorPrinterJobFileNotFound = 3002;

        /// <summary>
        /// Код ошибки "Не был произведен вызов StartDocPrinter".
        /// </summary>
        public const int ErrorStartDocPrinterNotCalling = 3003;

        /// <summary>
        /// Код ошибки "Не был произведен вызов AddJob".
        /// </summary>
        public const int ErrorAddJobNotCalling = 3004;

        /// <summary>
        /// Код ошибки "Указанный процессор печати уже установлен".
        /// </summary>
        public const int ErrorPrinterProcessorAlreadyInstalled = 3005;

        /// <summary>
        /// Код ошибки "Указанный монитор печати уже установлен".
        /// </summary>
        public const int ErrorMonitorAlreadyInstalled = 3006;

        /// <summary>
        /// Код ошибки "Указанный монитор печати не имеет требуемых функций".
        /// </summary>
        public const int ErrorInvalidMonitor = 3007;

        /// <summary>
        /// Код ошибки "Указанный монитор печати сейчас уже используется".
        /// </summary>
        public const int ErrorMonitorIsReadyUsed = 3008;
        #endregion

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintingException"/>.
        /// </summary>
        public PrintingException() : base() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintingException"/>.
        /// </summary>
        /// <param name="nativeErrorCode">Код ошибки Win32.</param>
        public PrintingException(int nativeErrorCode) : base(nativeErrorCode) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintingException"/>.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public PrintingException(string message) : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintingException"/>.
        /// </summary>
        /// <param name="nativeErrorCode">Код ошибки Win32.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        public PrintingException(int nativeErrorCode, string message) : base(nativeErrorCode, message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintingException"/>.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="innerException"></param>
        public PrintingException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintingException"/>.
        /// </summary>
        /// <param name="info">Данные для сериализации.</param>
        /// <param name="context">Контекст потока сериализации.</param>
        public PrintingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}