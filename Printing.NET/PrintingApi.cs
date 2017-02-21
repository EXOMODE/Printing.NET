using System;
using System.Runtime.InteropServices;

namespace Printing.NET
{
    /// <summary>
    /// Представляет API для работы со службой печати WinAPI.
    /// </summary>
    public static class PrintingApi
    {
        internal delegate bool EnumInfo(string serverName, uint level, IntPtr structs, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);
        internal delegate bool EnumInfo2(string serverName, string environment, uint level, IntPtr structs, uint bufferSize, ref uint bytesNeeded, ref uint bufferReturnedLength);

        public static Monitor[] Monitors => Monitor.All;

        internal static T[] GetInfo<T>(EnumInfo handler, string serverName, uint level) where T : struct
        {
            uint bytesNeeded = 0;
            uint bufferReturnedLength = 0;

            if (handler(serverName, level, IntPtr.Zero, 0, ref bytesNeeded, ref bufferReturnedLength)) return null;

            int lastWin32Error = Marshal.GetLastWin32Error();

            if (lastWin32Error != PrintingException.ErrorInsufficientBuffer) throw new PrintingException(lastWin32Error);

            IntPtr pointer = Marshal.AllocHGlobal((int)bytesNeeded);

            try
            {
                if (handler(serverName, level, pointer, bytesNeeded, ref bytesNeeded, ref bufferReturnedLength))
                {
                    IntPtr currentPointer = pointer;
                    T[] dataCollection = new T[bufferReturnedLength];
                    Type type = typeof(T);

                    for (int i = 0; i < bufferReturnedLength; i++)
                    {
                        dataCollection[i] = (T)Marshal.PtrToStructure(currentPointer, type);
                        currentPointer = (IntPtr)(currentPointer.ToInt64() + Marshal.SizeOf(type));
                    }

                    return dataCollection;
                }

                throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }

        internal static T[] GetInfo<T>(EnumInfo handler, string serverName) where T : struct => GetInfo<T>(handler, serverName, 2);

        internal static T[] GetInfo<T>(EnumInfo handler, uint level) where T : struct => GetInfo<T>(handler, null, level);

        internal static T[] GetInfo<T>(EnumInfo handler) where T : struct => GetInfo<T>(handler, null);
        
        internal static T[] GetInfo<T>(EnumInfo2 handler, string serverName, string arg, uint level) where T : struct
        {
            uint bytesNeeded = 0;
            uint bufferReturnedLength = 0;

            if (handler(serverName, arg, level, IntPtr.Zero, 0, ref bytesNeeded, ref bufferReturnedLength)) return null;

            int lastWin32Error = Marshal.GetLastWin32Error();

            if (lastWin32Error != PrintingException.ErrorInsufficientBuffer) throw new PrintingException(lastWin32Error);

            IntPtr pointer = Marshal.AllocHGlobal((int)bytesNeeded);

            try
            {
                if (handler(serverName, arg, level, pointer, bytesNeeded, ref bytesNeeded, ref bufferReturnedLength))
                {
                    IntPtr currentPointer = pointer;
                    T[] dataCollection = new T[bufferReturnedLength];
                    Type type = typeof(T);

                    for (int i = 0; i < bufferReturnedLength; i++)
                    {
                        dataCollection[i] = (T)Marshal.PtrToStructure(currentPointer, type);
                        currentPointer = (IntPtr)(currentPointer.ToInt64() + Marshal.SizeOf(type));
                    }

                    return dataCollection;
                }

                throw new PrintingException(Marshal.GetLastWin32Error());
            }
            catch (Exception e)
            {
                throw new PrintingException(e.Message, e);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }

        internal static T[] GetInfo<T>(EnumInfo2 handler, string serverName, string arg) where T : struct => GetInfo<T>(handler, serverName, arg, 2);

        internal static T[] GetInfo<T>(EnumInfo2 handler, string arg, uint level) where T : struct => GetInfo<T>(handler, null, arg, level);

        internal static T[] GetInfo<T>(EnumInfo2 handler, string arg) where T : struct => GetInfo<T>(handler, null, arg);

        internal static T[] GetInfo<T>(EnumInfo2 handler, uint level) where T : struct => GetInfo<T>(handler, null, level);

        internal static T[] GetInfo<T>(EnumInfo2 handler) where T : struct => GetInfo<T>(handler, null);

        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, uint level, out T[] dataCollection, out PrintingException e) where T : struct
        {
            dataCollection = null;
            e = null;

            try
            {
                dataCollection = GetInfo<T>(handler, serverName, level);
                return true;
            }
            catch (PrintingException ex)
            {
                e = ex;
            }

            return false;
        }

        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, level, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, serverName, 2, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo handler, string serverName, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, 2, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo handler, uint level, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo handler, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo handler, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo handler, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, uint level, out T[] dataCollection, out PrintingException e) where T : struct
        {
            dataCollection = null;
            e = null;

            try
            {
                dataCollection = GetInfo<T>(handler, serverName, arg, level);
                return true;
            }
            catch (PrintingException ex)
            {
                e = ex;
            }

            return false;
        }

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, arg, level, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, serverName, arg, 2, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string serverName, string arg, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, serverName, arg, 2, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, uint level, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, arg, level, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, arg, level, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, arg, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, string arg, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, arg, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, uint level, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, uint level, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, level, out dataCollection, out PrintingException e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, out T[] dataCollection, out PrintingException e) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out e);

        internal static bool TryGetInfo<T>(EnumInfo2 handler, out T[] dataCollection) where T : struct
            => TryGetInfo(handler, null, out dataCollection, out PrintingException e);

        /// <summary>
        /// Возвращает имя окружение системы, совместимое с WinAPI.
        /// </summary>
        /// <param name="environment">Окружение системы.</param>
        /// <returns></returns>
        internal static string GetEnvironmentName(this Environment environment)
        {
            switch (environment)
            {
                default: return null;
                case Environment.X86: return "Windows x86";
                case Environment.X64: return "Windows x64";
                case Environment.IA64: return "Windows IA64";
            }
        }

        internal static Environment GetEnvironment(this string environmentString)
        {
            environmentString = environmentString.ToLower();

            if (environmentString.Contains("x86")) return Environment.X86;
            if (environmentString.Contains("x64")) return Environment.X64;
            if (environmentString.Contains("ia64")) return Environment.IA64;

            return Environment.Current;
        }

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <param name="environment">Окружение, для которого был написан монитор печати.</param>
        /// <param name="serverName">Наименование сервера, на котором производится установка монитора печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public static Monitor CreateMonitor(string name, string dll, Environment environment, string serverName)
        {
            Monitor monitor = new Monitor(name, dll, environment);
            monitor.Install(serverName);

            return monitor;
        }

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <param name="environment">Окружение, для которого был написан монитор печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public static Monitor CreateMonitor(string name, string dll, Environment environment) => CreateMonitor(name, dll, environment, null);

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <param name="serverName">Наименование сервера, на котором производится установка монитора печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public static Monitor CreateMonitor(string name, string dll, string serverName) => CreateMonitor(name, dll, Environment.Current, null);

        /// <summary>
        /// Создаёт новый монитор печати в системе.
        /// </summary>
        /// <param name="name">Наименование монитора печати.</param>
        /// <param name="dll">Путь к файлу dll монитора печати.</param>
        /// <returns>Экземпляр монитора печати.</returns>
        public static Monitor CreateMonitor(string name, string dll) => CreateMonitor(name, dll, null);
    }
}