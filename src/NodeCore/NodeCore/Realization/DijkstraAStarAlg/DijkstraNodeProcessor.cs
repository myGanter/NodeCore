using NodeCore.Base;
using NodeCore.Realization.Services;
using NodeCore.Realization.Universal;
using System.Collections.Generic;
using System.Linq;

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

        public override void Dispose()
        { }
        #endregion

        #region core
        protected override List<INode<T>> SearchProcess(INode<T> Start, INode<T> Finish)
        {
            var queueForContains = new HashSet<INode<T>>();
            var queueForDequeue = new Queue<INode<T>>();

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

                    if (checkedNodes.ContainsKey(chNode))
                    {
                        if (checkedNodes[chNode].Item1 > newDistance)
                        {
                            if (!queueForContains.Contains(chNode))
                            {
                                queueForContains.Add(chNode);
                                queueForDequeue.Enqueue(chNode);
                            }

                            checkedNodes[chNode] = TupleStructure.Create(newDistance, activeNode);
                        }
                    }
                    else
                    {
                        queueForContains.Add(chNode);
                        queueForDequeue.Enqueue(chNode);
                        checkedNodes.Add(chNode, TupleStructure.Create(newDistance, activeNode));

                        //var newPointDistance = Helper.CalculateDistance(chNode.Point, Finish.Point);
                        //if (nearNode.Item1 > newPointDistance)
                        //    nearNode = Tuple.Create(newPointDistance, chNode);
                    }
                }

                if (queueForContains.Count == 0)
                    break;

                var minNode = queueForDequeue.Dequeue();
                queueForContains.Remove(minNode);
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
    }
}
