namespace NodeCore.Base
{
    /// <summary>
    /// Контейнер, связывающий две ноды
    /// </summary>
    /// <typeparam name="T">Тип дополнительного объекта у ноды</typeparam>
    public struct Connection<T>
    {
        /// <summary>
        /// Расстояние между нодами
        /// </summary>
        public double Distance { get; }

        /// <summary>
        /// Дочерняя нода
        /// </summary>
        public INode<T> ChildNode { get; }

        /// <summary>
        /// Родительская нода
        /// </summary>
        public INode<T> ParentNode { get; }

        /// <summary>
        /// Тип связи между нодами
        /// </summary>
        public Dependence Dependence { get; }

        public Connection(double Distance, INode<T> ChildNode, INode<T> ParentNode, Dependence Dependence)
        {
            this.Distance = Distance;
            this.ChildNode = ChildNode;
            this.ParentNode = ParentNode;
            this.Dependence = Dependence;
        }
    }
}
