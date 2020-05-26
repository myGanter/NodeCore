using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodeCore.Base;

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
                throw new ProcessorEx("Start or Finish is null");

            if (Start.Graph != Finish.Graph || Start.Graph != Graph)
                throw new ProcessorEx("Nodes are in different graphs");

            if (Start == Finish || Start.ConnectionLength == 0)
                return new List<INode<T>>() { Start };

            return SearchProcess(Start, Finish);
        }

        public virtual async Task<List<INode<T>>> SearchPathAsync(INode<T> Start, INode<T> Finish)
        {
            var result = await Task.Run(() => SearchPath(Start, Finish));

            AsyncSearchComplete?.Invoke(this, result);

            return result;
        }

        public abstract void Dispose();
        #endregion

        #region core
        protected abstract List<INode<T>> SearchProcess(INode<T> Start, INode<T> Finish);

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

                result.Reverse();
            }

            return result;
        }        
        #endregion
    }
}

