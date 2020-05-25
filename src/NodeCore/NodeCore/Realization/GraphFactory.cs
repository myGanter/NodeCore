using NodeCore.Base;
using NodeCore.Realization.DijkstraAStarAlg;
using NodeCore.Realization.RecursiveAlg;

namespace NodeCore.Realization
{
    public class GraphFactory
    {
        public static IGraph<T> CreateDijkstraGraph<T>(string Name)
        {
            var graph = new DijkstraGraph<T>(Name, g => new DijkstraNodeProcessor<T>(g));
            return graph;
        }

        public static IGraph<T> CreateAStarGraph<T>(string Name)
        {
            var graph = new DijkstraGraph<T>(Name, g => new DijkstraAStarNodeProcessor<T>(g));
            return graph;
        }

        public static IGraph<T> CreateGreedyGraph<T>(string Name)
        {
            var graph = new DijkstraGraph<T>(Name, g => new DijkstraGreedyNodeProcessor<T>(g));
            return graph;
        }

        public static IGraph<T> CreateRecGraph<T>(string Name) 
        {
            var graph = new RecGraph<T>(Name);
            return graph;
        }
    }
}
