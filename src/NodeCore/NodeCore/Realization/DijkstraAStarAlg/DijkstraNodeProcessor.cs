using NodeCore.Realization.Universal;

namespace NodeCore.Realization.DijkstraAStarAlg
{
    class DijkstraNodeProcessor<T> : UniversalBaseNodeProcessor<T>
    {
        #region interface
        private DijkstraGraph<T> DGraph { get; }

        public DijkstraNodeProcessor(DijkstraGraph<T> Graph) : base(Graph)
        {
            DGraph = Graph;
        }
        #endregion

        #region core

        #endregion
    }
}
