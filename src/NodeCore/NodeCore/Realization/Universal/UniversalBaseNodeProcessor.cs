using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodeCore.Base;
using NodeCore.Realization.Services;

namespace NodeCore.Realization.Universal
{
    abstract class UniversalBaseNodeProcessor<T> : INodeProcessor<T>
    {
        #region interface
        public event Action<object, List<INode<T>>> AsyncSearchComplete;

        public IGraph<T> Graph { get; }

        public UniversalBaseNodeProcessor(IGraph<T> Graph)
        {
            this.Graph = Graph;
        }

        public virtual List<INode<T>> SearchPath(INode<T> Start, INode<T> Finish)
        {
            if (Start == null || Finish == null)
                throw new Exception("Start or Finish is null");

            if (Start.Graph != Finish.Graph || Start.Graph != Graph)
                throw new Exception("Nodes are in different graphs");

            if (Start == Finish || Start.ConnectionLength == 0)
                return new List<INode<T>>() { Start };

            var queue = new List<INode<T>>();
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
                            if (!queue.Contains(chNode))// для оптимизации поиска можно использовать HashSet
                                queue.Add(chNode);

                            checkedNodes[chNode] = TupleStructure.Create(newDistance, activeNode);
                        }
                    }
                    else
                    {
                        queue.Add(chNode);
                        checkedNodes.Add(chNode, TupleStructure.Create(newDistance, activeNode));

                        //var newPointDistance = Helper.CalculateDistance(chNode.Point, Finish.Point);
                        //if (nearNode.Item1 > newPointDistance)
                        //    nearNode = Tuple.Create(newPointDistance, chNode);
                    }
                }

                if (queue.Count == 0)
                    break;

                var minNode = queue[0];//если будет использоваться HashSet нужно создать Queue и с помощью Dequeue извлекать первый элемент (HashSet.First() и HashSet.ElementAt(0) работают медленно)
                queue.Remove(minNode);// для оптимизации удаления можно использовать HashSet
                activeNode = minNode;
            } while (true);

            if (checkedNodes.ContainsKey(Finish))
            {
                return RecoverPuth(checkedNodes, Start, Finish);
            }
            else
            {
                var newFinish = NodeFlightBirdSearch(checkedNodes.Select(x => x.Key), Finish.Point);

                return RecoverPuth(checkedNodes, Start, newFinish /*nearNode.Item2*/);
            }
        }

        public virtual async Task<List<INode<T>>> SearchPathAsync(INode<T> Start, INode<T> Finish)
        {
            var result = await Task.Run(() => SearchPath(Start, Finish));

            AsyncSearchComplete?.Invoke(this, result);

            return result;
        }

        public virtual void Dispose()
        {           
        }
        #endregion

        #region core
        protected List<INode<T>> RecoverPuth(Dictionary<INode<T>, TupleStructure<double, INode<T>>> CheckedNodes, INode<T> Start, INode<T> Finish)
        {
            var activeNode = Finish;
            var result = new List<INode<T>>() { Finish };

            if (Start != Finish)
            {
                do
                {
                    activeNode = CheckedNodes[activeNode].Item2;
                    result.Add(activeNode);
                } while (activeNode != Start);

                result.Reverse();//list.Reverse работает на удивление быстро
            }

            return result;
        }

        protected INode<T> NodeFlightBirdSearch(IEnumerable<INode<T>> Collection, Point3D Finish)
        {
            var res = Collection.First();
            var minL = Helper.CalculateDistance(res.Point, Finish);

            foreach (var n in Collection.Skip(1))
            {
                var newL = Helper.CalculateDistance(n.Point, Finish);
                if (minL > newL)
                {
                    minL = newL;
                    res = n;
                }
            }

            return res;
        }
        #endregion
    }
}

