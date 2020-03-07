using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NodeCore.Base
{
    /// <summary>
    /// Интерфейс с реализацией основных алгоритмов работы с графом
    /// </summary>
    /// <typeparam name="T">Тип дополнительного объекта у ноды</typeparam>
    public interface INodeProcessor<T> : IDisposable
    {
        /// <summary>
        /// Вызывается после завершения асинхронной операции поиска пути
        /// </summary>
        event Action<object, List<INode<T>>> AsyncSearchComplete;

        /// <summary>
        /// Возвращает экземпляр графа, в котором был создан данный процессор
        /// </summary>
        IGraph<T> Graph { get; }

        /// <summary>
        /// Ищет путь от ноды Start до ноды Finish
        /// </summary>
        /// <param name="Start">Нода начала</param>
        /// <param name="Finish">Нода конца</param>
        /// <returns>Список нод, в первой ячейке хранится начальная нода, в последующих ячейках, ноды, через которые можно достигнуть конечную ноду</returns>
        List<INode<T>> SearchPath(INode<T> Start, INode<T> Finish);

        /// <summary>
        /// Асинхронная версия метода SearchPath
        /// </summary>
        /// <param name="Start">Нода начала</param>
        /// <param name="Finish">Нода конца</param>
        /// <returns></returns>
        Task<List<INode<T>>> SearchPathAsync(INode<T> Start, INode<T> Finish);
    }
}
