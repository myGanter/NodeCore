using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NodeCore.Base;

namespace NodeCore.Realization.RecursiveAlg
{
    class RecNodeProcessor<T> : INodeProcessor<T>
    {
        public event Action<object, List<INode<T>>> AsyncSearchComplete;

        private List<Guid> UnicGuids { get; }

        public IGraph<T> Graph { get; }

        private RecGraph<T> RecGraph { get; }

        public RecNodeProcessor(RecGraph<T> Graph)
        {
            UnicGuids = new List<Guid>();
            RecGraph = Graph;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Search
        private Guid UnicGuid;
        private RecNode<T> Finish;
        private double? DistanceToFinish;        

        public List<INode<T>> SearchPath(INode<T> Startt, INode<T> Finishh)
        {
            RecNode<T> Start = (RecNode<T>)Startt;
            RecNode<T> Finish = (RecNode<T>)Finishh;

            if (Start.Graph != Finish.Graph)
                throw new Exception("Nodes are in different graphs");

            if (this.Finish != null || DistanceToFinish != null)
                throw new Exception("The search has already begun");

            UnicGuid = Guid.NewGuid();
            UnicGuids.Add(UnicGuid);
            this.Finish = Finish;

            Start.BondForProcess.Add(UnicGuid, new NodeBond<T>(null, 0));
            SearchPath(Start);

            this.Finish = null;
            DistanceToFinish = null;

            var res = new List<INode<T>>();
            res.Add(Finish);

            var f = Finish.BondForProcess[UnicGuid].Parent;
            while (f != null)
            {
                res.Add(f);
                f = f.BondForProcess[UnicGuid].Parent;
            }

            res.Reverse();

            return res;
        }

        public async Task<List<INode<T>>> SearchPathAsync(INode<T> Start, INode<T> Finish)
        {
            var result = await Task.Run(() => SearchPath(Start, Finish));

            AsyncSearchComplete?.Invoke(this, result);

            return result;
        }

        private void SearchPath(RecNode<T> S)
        {
            foreach (var connection in S)
            {
                var child = (RecNode<T>)connection.ChildNode;
                var newNodeBond = new NodeBond<T>(S.BondForProcess[UnicGuid], connection);
                if (DistanceToFinish < newNodeBond.Distance)
                    continue;

                if (!child.BondForProcess.ContainsKey(UnicGuid) || child.BondForProcess[UnicGuid].Distance > newNodeBond.Distance)
                {
                    child.BondForProcess[UnicGuid] = newNodeBond;
                    if (child == Finish)
                        DistanceToFinish = newNodeBond.Distance;

                    SearchPath(child);
                }
            }
        }
        #endregion
    }
}
