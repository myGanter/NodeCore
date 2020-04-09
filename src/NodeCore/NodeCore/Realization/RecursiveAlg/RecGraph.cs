using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NodeCore.Base;
using System.Linq;

namespace NodeCore.Realization.RecursiveAlg
{
    class RecGraph<T> : IGraph<T>
    {
        public string Name { get; }

        public int NodeLength => Nodes.Count;

        private protected Dictionary<string, RecNode<T>> Nodes;

        public RecGraph(string Name)
        {
            this.Name = Name;
            Nodes = new Dictionary<string, RecNode<T>>();
        }

        public INode<T> this[int Index] => Nodes.ElementAt(Index).Value;

        public INode<T> this[string NodeName] => Nodes[NodeName];

        public INode<T> this[Point3D NodePoint] => Nodes[NodePoint.ToString()];

        public INodeProcessor<T> CreateNodeProcessor() => new RecNodeProcessor<T>(this);

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool NodeExist(INode<T> Node) => NodeExist(Node.Name);

        public bool NodeExist(string NodeName) => Nodes.ContainsKey(NodeName);

        public bool NodeExist(Point3D NodePoint) => Nodes.ContainsKey(NodePoint.ToString());

        public INode<T> AddNode(string NodeName, Point3D Point = default)
        {
            var node = new RecNode<T>(NodeName, this, Point);
            return node;
        }

        public void DeleteNode(string NodeName) => DeleteNode(this[NodeName]);

        public void DeleteNode(Point3D NodePoint) => DeleteNode(this[NodePoint]);

        public void DeleteNode(INode<T> Node)
        {
            throw new GraphEx("");
        }

        public void Clear()
        {
            Nodes = new Dictionary<string, RecNode<T>>();
        }

        internal void AddNode(string Name, RecNode<T> Node) => Nodes.Add(Name, Node);

        public IEnumerator<INode<T>> GetEnumerator() => Nodes.Select(x => x.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
