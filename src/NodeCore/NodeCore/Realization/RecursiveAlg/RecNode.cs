using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NodeCore.Base;

namespace NodeCore.Realization.RecursiveAlg
{
    class RecNode<T> : INode<T>
    {
        public T Object { get; set; }

        public Dictionary<Guid, NodeBond<T>> BondForProcess { get; }

        private protected Dictionary<string, Connection<T>> Connections;

        public IGraph<T> Graph => RecGraph;

        public RecGraph<T> RecGraph { get; }

        public Point3D Point { get; }

        public string Name { get; }

        public RecNode(string Name, RecGraph<T> Graph, Point3D Point = new Point3D())
        {
            if (Graph.NodeExist(Name))
                throw new Exception($"Node {Name} exist");

            BondForProcess = new Dictionary<Guid, NodeBond<T>>();
            Connections = new Dictionary<string, Connection<T>>();
            this.RecGraph = Graph;
            this.Point = Point;
            this.Name = Name;

            Graph.AddNode(Name, this);
        }

        public Connection<T> this[int Index] => Connections.ElementAt(Index).Value;

        public Connection<T> this[string NodeName] => Connections[NodeName];

        public Connection<T> this[Point3D NodePoint] => Connections[NodePoint.ToString()];             

        public int ConnectionLength => Connections.Count;

        public INode<T> AddNode(string Name, double Distance, Dependence Dependence, Point3D Point = default)
        {
            var node = new RecNode<T>(Name, RecGraph, Point);

            var cntc = new Connection<T>(Distance, node, this, Dependence);
            Connections.Add(Name, cntc);

            if (Dependence == Dependence.Doubly)
                node.AddNode(this, Distance, Dependence.Doubly);

            return this;
        }

        public INode<T> AddNode(double Distance, Dependence Dependence, Func<IGraph<T>, INode<T>, INode<T>> ClBk)
        {
            var node = ClBk(Graph, this);
            if (node == null)
                throw new Exception("Node is null");

            if (NodeExist(node) || node.NodeExist(this))
                throw new Exception($"Node exist in {Name}");

            var cntc = new Connection<T>(Distance, node, this, Dependence);
            Connections.Add(node.Name, cntc);

            if (Dependence == Dependence.Doubly)
                ((RecNode<T>)node).AddNode(this, Distance, Dependence.Doubly);

            return this;
        }

        public INode<T> AddNodeDD(string Name, double Distance = 1, Point3D Point = default) => AddNode(Name, Distance, Dependence.Doubly, Point);

        public INode<T> AddNodeDD(Func<IGraph<T>, INode<T>, INode<T>> ClBk, double Distance = 1) => AddNode(Distance, Dependence.Doubly, ClBk);

        public INode<T> AddNodeDS(string Name, double Distance, Point3D Point = default) => AddNode(Name, Distance, Dependence.Simply, Point);

        public INode<T> AddNodeDS(Func<IGraph<T>, INode<T>, INode<T>> ClBk, double Distance = 1) => AddNode(Distance, Dependence.Simply, ClBk);

        private protected void AddNode(RecNode<T> Node, double Distance, Dependence Dependence)
        {
            if (Node.Graph != Graph)
                throw new Exception("Nodes from different graphs");

            var cntc = new Connection<T>(Distance, Node, this, Dependence);
            Connections.Add(Node.Name, cntc);
        }

        public bool NodeExist(INode<T> Node) => NodeExist(Node.Name);

        public bool NodeExist(string NodeName) => Connections.ContainsKey(Name);

        public bool NodeExist(Point3D NodePoint) => NodeExist(NodePoint.ToString());

        public IEnumerator<Connection<T>> GetEnumerator() => Connections.Select(x => x.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
