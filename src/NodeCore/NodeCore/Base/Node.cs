using System;
using System.Collections.Generic;

namespace NodeCore.Base
{
    /// <summary>
    /// Интерфейс для работы с нодой
    /// Реализует IEnumerable<Connection<T>>
    /// </summary>
    /// <typeparam name="T">Тип дополнительного объекта у ноды</typeparam>
    public interface INode<T> : IEnumerable<Connection<T>>
    {
        /// <summary>
        /// Возвращает или устанавливает дополнительный объект ноды
        /// </summary>
        T Object { get; set; }

        /// <summary>
        /// Возвращает имя ноды
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Возвращает точку ноды
        /// </summary>
        Point3D Point { get; }

        /// <summary>
        /// Возвращает экземпляр графа, в который входит данная нода
        /// </summary>
        IGraph<T> Graph { get; }

        /// <summary>
        /// Возвращает колличество дочерних связей данной ноды (куда можно попасть из данной ноды)
        /// </summary>
        int ConnectionLength { get; }

        /// <summary>
        /// Возвращает контейнер по индексу
        /// </summary>
        /// <param name="Index">Индекс</param>
        /// <returns></returns>
        Connection<T> this[int Index] { get; }

        /// <summary>
        /// Возвращает контейнер по имени дочерней ноды
        /// </summary>
        /// <param name="NodeName">Имя</param>
        /// <returns></returns>
        Connection<T> this[string NodeName] { get; }

        /// <summary>
        /// Возвращает контейнер по точке дочерней ноды
        /// </summary>
        /// <param name="NodePoint">Точка</param>
        /// <returns></returns>
        Connection<T> this[Point3D NodePoint] { get; }

        /// <summary>
        /// Возвращает true, если переданный экземпляр ноды существует как дочерняя для текущей ноды
        /// </summary>
        /// <param name="Node">Нода</param>
        /// <returns></returns>
        bool NodeExist(INode<T> Node);

        /// <summary>
        /// Возвращает true, если нода по имени существует как дочерняя для текущей ноды
        /// </summary>
        /// <param name="NodeName">Имя</param>
        /// <returns></returns>
        bool NodeExist(string NodeName);

        /// <summary>
        /// Возвращает true, если нода по точке существует как дочерняя для текущей ноды
        /// </summary>
        /// <param name="NodePoint">Точка</param>
        /// <returns></returns>
        bool NodeExist(Point3D NodePoint);

        /// <summary>
        /// Добавляет новую ноду в граф, устанавдивая её дочерней для текущей ноды
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Distance">Расстояние</param>
        /// <param name="Dependence">Связь</param>
        /// <param name="Point">Точка</param>
        /// <returns>Текущая нода</returns>
        INode<T> AddNode(string Name, double Distance, Dependence Dependence, Point3D Point = new Point3D());

        /// <summary>
        /// Добавляет новую ноду в граф, устанавдивая её дочерней для текущей ноды. Так-же устанавливает текущую ноду как дочернюю для новой
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Distance">Расстояние</param>
        /// <param name="Point">Точка</param>
        /// <returns>Текущая нода</returns>
        INode<T> AddNodeDD(string Name, double Distance = 1, Point3D Point = new Point3D());

        /// <summary>
        /// Добавляет новую ноду в граф, устанавдивая её дочерней для текущей ноды. Не устанавливает текущую ноду как дочернюю для новой
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Distance">Расстояние</param>
        /// <param name="Point">Точка</param>
        /// <returns>Текущая нода</returns>
        INode<T> AddNodeDS(string Name, double Distance = 1, Point3D Point = new Point3D());


        /// <summary>
        /// Добавляет связь между текущей нодой и нодой возвращённой с помощью ClBk
        /// </summary>
        /// <param name="Distance">Расстояние</param>
        /// <param name="Dependence">Связь</param>
        /// <param name="ClBk">Селектор (текущий граф, текущая нода) => будущая дочерняя нода</param>
        /// <returns>Текущая нода</returns>
        INode<T> AddNode(double Distance, Dependence Dependence, Func<IGraph<T>, INode<T>, INode<T>> ClBk);

        /// <summary>
        /// Добавляет связь между текущей нодой и нодой возвращённой с помощью ClBk. Так-же устанавливает текущую ноду как дочернюю для возвращённой
        /// </summary>
        /// <param name="ClBk">Селектор (текущий граф, текущая нода) => будущая дочерняя нода</param>
        /// <param name="Distance">Расстояние</param>
        /// <returns>Текущая нода</returns>
        INode<T> AddNodeDD(Func<IGraph<T>, INode<T>, INode<T>> ClBk, double Distance = 1);

        /// <summary>
        /// Добавляет связь между текущей нодой и нодой возвращённой с помощью ClBk. Не устанавливает текущую ноду как дочернюю для возвращённой
        /// </summary>
        /// <param name="ClBk">Селектор (текущий граф, текущая нода) => будущая дочерняя нода</param>
        /// <param name="Distance">Расстояние</param>
        /// <returns>Текущая нода</returns>
        INode<T> AddNodeDS(Func<IGraph<T>, INode<T>, INode<T>> ClBk, double Distance = 1);
    }
}
