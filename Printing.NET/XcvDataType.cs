namespace Printing.NET
{
    /// <summary>
    /// Тип операции для <see cref="Port.XcvData(IntPtr, string, IntPtr, uint, IntPtr, uint, out uint, out uint)"/>.
    /// </summary>
    internal enum XcvDataType
    {
        /// <summary>
        /// Добавить новый порт.
        /// </summary>
        AddPort,
        /// <summary>
        /// Удалить существующий порт.
        /// </summary>
        DeletePort,
    }
}