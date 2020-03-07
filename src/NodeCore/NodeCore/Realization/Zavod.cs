using System;
using System.Collections.Generic;
using System.Text;
using NodeCore.Base;
using NodeCore.Realization.RecursiveAlg;
using NodeCore.Realization.DijkstraAStarAlg;

namespace NodeCore.Realization
{
    public class Zavod
    {
        public static IGraph<T> Get<T>(string N) => new RecGraph<T>(N);

        public static IGraph<T> Get2<T>(string N)
        {
            var graph = new DijkstraGraph<T>(N, g => new DijkstraNodeProcessor<T>(g));
            return graph;
        }

        public static IGraph<T> Get3<T>(string N)
        {
            var graph = new DijkstraGraph<T>(N, g => new DijkstraAStarNodeProcessor<T>(g));
            return graph;
        }
    }
}
