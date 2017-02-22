using System;
using System.IO;

namespace Printing.NET
{
    /// <summary>
    /// Представляет базовый класс для всех компонентов устройства печати.
    /// </summary>
    public abstract class PrintableDevice : IPrintableDevice
    {
        /// <summary>
        /// Наименование компонента устройства печати.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintableDevice"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"/>
        public PrintableDevice(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            Name = name;
        }

        /// <summary>
        /// Устанавливает компонента устройства печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="PrintingException" />
        public abstract void Install(string serverName);

        /// <summary>
        /// Устанавливает компонента устройства печати на локальной машине.
        /// </summary>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="PrintingException" />
        public void Install() => Install(null);

        /// <summary>
        /// Устанавливает компонента устройства печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="e">Исключение, возникшее в процессе установки.</param>
        /// <returns>True, если процедура установки прошла успешно, иначе False.</returns>
        public bool TryInstall(string serverName, out PrintingException e)
        {
            e = null;

            try
            {
                Install(serverName);
            }
            catch (PrintingException ex)
            {
                e = ex;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Устанавливает компонента устройства печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>True, если процедура установки прошла успешно, иначе False.</returns>
        public bool TryInstall(string serverName) => TryInstall(serverName, out PrintingException e);

        /// <summary>
        /// Устанавливает компонента устройства печати на локальной машине.
        /// </summary>
        /// <param name="e">Исключение, возникшее в процессе установки.</param>
        /// <returns>True, если процедура установки прошла успешно, иначе False.</returns>
        public bool TryInstall(out PrintingException e) => TryInstall(null, out e);

        /// <summary>
        /// Устанавливает компонента устройства печати на локальной машине.
        /// </summary>
        /// <returns>True, если процедура установки прошла успешно, иначе False.</returns>
        public bool TryInstall() => TryInstall(out PrintingException e);

        /// <summary>
        /// Удалает компонента устройства печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <exception cref="PrintingException" />
        public abstract void Uninstall(string serverName);

        /// <summary>
        /// Удаляет компонента устройства печати на локальной машине.
        /// </summary>
        /// <exception cref="PrintingException" />
        public void Uninstall() => Uninstall(null);

        /// <summary>
        /// Удалает компонента устройства печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="e">Исключение, возникшее в процессе удаления.</param>
        /// <returns>True, если процедура удаления прошла успешно, иначе False.</returns>
        public bool TryUninstall(string serverName, out PrintingException e)
        {
            e = null;

            try
            {
                Uninstall(serverName);
            }
            catch (PrintingException ex)
            {
                e = ex;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Удалает компонента устройства печати на удалённой машине.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <returns>True, если процедура удаления прошла успешно, иначе False.</returns>
        public bool TryUninstall(string serverName) => TryUninstall(serverName, out PrintingException e);

        /// <summary>
        /// Удалает компонента устройства печати на локальной машине.
        /// </summary>
        /// <param name="e">Исключение, возникшее в процессе удаления.</param>
        /// <returns>True, если процедура удаления прошла успешно, иначе False.</returns>
        public bool TryUninstall(out PrintingException e) => TryUninstall(null, out e);

        /// <summary>
        /// Удалает компонента устройства печати на локальной машине.
        /// </summary>
        /// <returns>True, если процедура удаления прошла успешно, иначе False.</returns>
        public bool TryUninstall() => TryUninstall(out PrintingException e);
    }
}