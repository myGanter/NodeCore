using System;
using System.Collections.Generic;

namespace NodeCore.Base
{
    /// <summary>
    /// Интерфейс для работы с графом
    /// Реализует IEnumerable<INode<T>>, IDisposable
    /// </summary>
    /// <typeparam name="T">Тип дополнительного объекта у ноды</typeparam>
    public interface IGraph<T> : IEnumerable<INode<T>>, IDisposable
    {
        /// <summary>
        /// Возвращает имя графа
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Возвращает колличество нод в графе
        /// </summary>
        int NodeLength { get; }

        /// <summary>
        /// Возвращает ноду по индексу
        /// </summary>
        /// <param name="Index">Индекс</param>
        /// <returns></returns>
        INode<T> this[int Index] { get; }

        /// <summary>
        /// Возвращает ноду по имени
        /// </summary>
        /// <param name="NodeName">Имя</param>
        /// <returns></returns>
        INode<T> this[string NodeName] { get; }

        /// <summary>
        /// Возвращает ноду по точке
        /// </summary>
        /// <param name="NodePoint">Point(3d)</param>
        /// <returns></returns>
        INode<T> this[Point3D NodePoint] { get; }

        /// <summary>
        /// Возвращает true, если переданный экземпляр ноды существует в графе
        /// </summary>
        /// <param name="Node">Нода</param>
        /// <returns></returns>
        bool NodeExist(INode<T> Node);

        /// <summary>
        /// Возвращает true, если нода с переданным именем существует в графе
        /// </summary>
        /// <param name="NodeName">Имя</param>
        /// <returns></returns>
        bool NodeExist(string NodeName);

        /// <summary>
        /// Возвращает true, если нода с переданной точкой существует в графе
        /// </summary>
        /// <param name="NodePoint">Point(3d)</param>
        /// <returns></returns>
        bool NodeExist(Point3D NodePoint);

        /// <summary>
        /// Добавляет новую ноду в граф
        /// </summary>
        /// <param name="NodeName">Имя</param>
        /// <param name="Point">Point(3d)</param>
        /// <returns></returns>
        INode<T> AddNode(string NodeName, Point3D Point = new Point3D());

        /// <summary>
        /// Удаляет ноду из графа по имени
        /// </summary>
        /// <param name="NodeName">Имя</param>
        void DeleteNode(string NodeName);

        /// <summary>
        /// Удаляет ноду из графа по точке
        /// </summary>
        /// <param name="Point">Point(3d)</param>
        void DeleteNode(Point3D Point);

        /// <summary>
        /// Удаляет переданный экземпляр ноды из графа
        /// </summary>
        /// <param name="Node">Нода</param>
        void DeleteNode(INode<T> Node);

        /// <summary>
        /// Возвращает экземпляр INodeProcessor<T>
        /// </summary>
        /// <returns>Интерфейс с реализацией основных алгоритмов работы с графом</returns>
        INodeProcessor<T> CreateNodeProcessor();

        /// <summary>
        /// Очищает граф
        /// </summary>
        void Clear();

        /// <summary>
        /// Очищает граф и устанавливает новое имя
        /// </summary>
        /// <param name="NewGraphName">Новое имя</param>
        void Clear(string NewGraphName);
    }
}
