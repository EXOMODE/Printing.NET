namespace Printing.NET
{
    /// <summary>
    /// Представляет статический класс расширений типов.
    /// </summary>
    public static class PrintingExtensions
    {
        /// <summary>
        /// Возвращает имя окружения системы, совместимое с WinAPI.
        /// </summary>
        /// <param name="environment">Окружение системы.</param>
        /// <returns>Строковое представление имени окружения системы.</returns>
        internal static string GetEnvironmentName(this Environment environment)
        {
            switch (environment)
            {
                default:
                case Environment.Current:
                    return null;

                case Environment.X86: return "Windows x86";
                case Environment.X64: return "Windows x64";
                case Environment.IA64: return "Windows IA64";
            }
        }

        /// <summary>
        /// Возвращает <see cref="Environment"/>, эквивалентный входной строке имени окружения.
        /// </summary>
        /// <param name="environmentString">Входная строка имени окружения.</param>
        /// <returns><see cref="Environment"/>, эквивалентный входной строке имени окружения.</returns>
        internal static Environment GetEnvironment(this string environmentString)
        {
            environmentString = environmentString.ToLower();

            if (environmentString.Contains("x86")) return Environment.X86;
            if (environmentString.Contains("x64")) return Environment.X64;
            if (environmentString.Contains("ia64")) return Environment.IA64;

            return Environment.Current;
        }
    }
}