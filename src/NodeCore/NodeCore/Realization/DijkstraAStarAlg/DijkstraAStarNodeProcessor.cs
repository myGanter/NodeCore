using System;
using System.Collections.Generic;
using System.Linq;
using NodeCore.Base;
using NodeCore.Realization.Services;
using NodeCore.Realization.Universal;

namespace NodeCore.Realization.DijkstraAStarAlg
{
    class DijkstraAStarNodeProcessor<T> : UniversalBaseNodeProcessor<T>
    {
        #region interface
        private DijkstraGraph<T> DGraph { get; }

        public DijkstraAStarNodeProcessor(DijkstraGraph<T> Graph) : base(Graph)
        {
            DGraph = Graph;
        }

        public override List<INode<T>> SearchPath(INode<T> Start, INode<T> Finish)
        {
            if (Start == null || Finish == null)
                throw new ProcessorEx("Start or Finish is null");

            if (Start.Graph != Finish.Graph || Start.Graph != Graph)
                throw new ProcessorEx("Nodes are in different graphs");

            if (Start == Finish || Start.ConnectionLength == 0)
                return new List<INode<T>>() { Start };

            var queue = new List<Tuple<INode<T>, double>>();
            //var queueForContains = new HashSet<INode<T>>();

            var checkedNodes = new Dictionary<INode<T>, TupleStructure<double, INode<T>>>();
            var activeNode = Start;
            //var nearNode = Tuple.Create(Helper.CalculateDistance(Start.Point, Finish.Point), Start);

            checkedNodes.Add(activeNode, new TupleStructure<double, INode<T>>(0, null));

            do
            {
                foreach (var n in activeNode)
                {
                    var chNode = n.ChildNode;
                    var distance = n.Distance;
                    var newDistance = distance + checkedNodes[activeNode].Item1;
                    var priority = newDistance + Helper.CalculateManhattanDistance(chNode.Point, Finish.Point);

                    if (checkedNodes.ContainsKey(chNode))
                    {
                        if (checkedNodes[chNode].Item1 > newDistance)
                        {
                            if (!ContainsNodeForList(queue, chNode)) 
                            {
                                queue.Add(Tuple.Create(chNode, priority));
                            }

                            //if (!queueForContains.Contains(chNode))// для оптимизации поиска можно использовать HashSet
                            //{
                            //    queueForContains.Add(chNode);
                            //    queue.Add(Tuple.Create(chNode, priority));
                            //}                                 

                            checkedNodes[chNode] = TupleStructure.Create(newDistance, activeNode);
                        }
                    }
                    else
                    {
                        queue.Add(Tuple.Create(chNode, priority));
                        //queueForContains.Add(chNode);
                        checkedNodes.Add(chNode, TupleStructure.Create(newDistance, activeNode));

                        //var newPointDistance = Helper.CalculateDistance(chNode.Point, Finish.Point);
                        //if (nearNode.Item1 > newPointDistance)
                        //    nearNode = Tuple.Create(newPointDistance, chNode);
                    }
                }

                if (queue.Count == 0 || activeNode == Finish)
                    break;

                var minNode = DequeueNodeForList(queue);
                //queueForContains.Remove(minNode);// для оптимизации удаления можно использовать HashSet
                activeNode = minNode;
            } while (true);

            if (checkedNodes.ContainsKey(Finish))
            {
                return RecoverPuth(checkedNodes, Start, Finish);
            }
            else
            {
                var newFinish = Helper.NodeFlightBirdSearch(checkedNodes.Select(x => x.Key), Finish.Point);

                return RecoverPuth(checkedNodes, Start, newFinish /*nearNode.Item2*/);
            }
        }
        #endregion

        #region core
        private INode<T> DequeueNodeForList(List<Tuple<INode<T>, double>> Collection)
        {
            int bestIndex = 0;

            for (int i = 1; i < Collection.Count; ++i)
            {
                if (Collection[i].Item2 < Collection[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            var bestItem = Collection[bestIndex].Item1;
            Collection.RemoveAt(bestIndex);
            return bestItem;
        }

        private bool ContainsNodeForList(List<Tuple<INode<T>, double>> Collection, INode<T> Node) 
        {
            for (var i = 0; i < Collection.Count; ++i)
            {
                if (Collection[i].Item1 == Node)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
