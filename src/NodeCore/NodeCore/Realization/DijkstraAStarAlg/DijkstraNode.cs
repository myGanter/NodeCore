using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCore.Base;

namespace NodeCore.Realization.DijkstraAStarAlg
{
    class DijkstraNode<T> : INode<T>
    {
        #region interface
        public T Object { get; set; }

        public string Name { get; }

        public Point3D Point { get; }

        public IGraph<T> Graph { get => DGraph; }

        private DijkstraGraph<T> DGraph { get; }

        public int ConnectionLength => DGraph.GetConnectionsForName(Name).Count;

        internal DijkstraNode(string Name, DijkstraGraph<T> Graph, Point3D Point = default)
        {
            if (string.IsNullOrEmpty(Name))
                Name = Point.ToString();

            DGraph = Graph;
            this.Point = Point;
            this.Name = Name;
        }

        public Connection<T> this[int Index] => DGraph.GetConnectionsForName(Name)[Index];

        public Connection<T> this[string NodeName] => DGraph.GetConnectionsForName(Name).First(x => x.ChildNode.Name == NodeName);

        public Connection<T> this[Point3D NodePoint] => DGraph.GetConnectionsForName(Name).First(x => x.ChildNode.Point == NodePoint);

        public INode<T> AddNode(string Name, double Distance, Dependence Dependence, Point3D Point = default)
        {
            var node = new DijkstraNode<T>(Name, DGraph, Point);

            if (DGraph.NodeExist(node.Name) || DGraph.NodeExist(node.Point))
                throw new Exception($"Node [Name: {node.Name}, Point: {node.Point}] exists");

            var cnct = new Connection<T>(Distance, node, this, Dependence);

            DGraph.AddNode(node);
            DGraph.AddConnection(this, cnct);

            if (Dependence == Dependence.Doubly)
            {
                var cnctD = new Connection<T>(Distance, this, node, Dependence);
                DGraph.AddConnection(node, cnctD);

                node.OnDeleteNode += OnDeleteConnectionsForNode;
                OnDeleteNode += node.OnDeleteConnectionsForNode;
            }

            return this;
        }

        public INode<T> AddNodeDS(string Name, double Distance = 1, Point3D Point = default) => AddNode(Name, Distance, Dependence.Simply, Point);

        public INode<T> AddNodeDD(string Name, double Distance = 1, Point3D Point = default) => AddNode(Name, Distance, Dependence.Doubly, Point);

        public INode<T> AddNode(double Distance, Dependence Dependence, Func<IGraph<T>, INode<T>, INode<T>> ClBk)
        {
            var node = ClBk(DGraph, this);

            if (node == null)
                throw new Exception($"Returned node is null");

            if (node.Graph != Graph || !DGraph.NodeExist(node))
                throw new Exception($"Returned node was not found in the current graph");

            if (NodeExist(node) || node.NodeExist(this))
                throw new Exception($"The node [Name: {Name}, Point: {Point}] to node [Name: {node.Name}, Point: {node.Point}] relationship already exists");

            var cnct = new Connection<T>(Distance, node, this, Dependence);
            DGraph.AddConnection(this, cnct);
            var dijkNode = (DijkstraNode<T>)node;

            if (Dependence == Dependence.Doubly)
            {
                var cnctD = new Connection<T>(Distance, this, node, Dependence);                
                DGraph.AddConnection(dijkNode, cnctD);

                dijkNode.OnDeleteNode += OnDeleteConnectionsForNode;
                OnDeleteNode += dijkNode.OnDeleteConnectionsForNode;
            }

            return this;
        }

        public INode<T> AddNodeDS(Func<IGraph<T>, INode<T>, INode<T>> ClBk, double Distance = 1) => AddNode(Distance, Dependence.Simply, ClBk);

        public INode<T> AddNodeDD(Func<IGraph<T>, INode<T>, INode<T>> ClBk, double Distance = 1) => AddNode(Distance, Dependence.Doubly, ClBk);

        public bool NodeExist(INode<T> Node) => DGraph.GetConnectionsForName(Name).Any(x => x.ChildNode == Node);

        public bool NodeExist(string NodeName) => DGraph.GetConnectionsForName(Name).Any(x => x.ChildNode.Name == NodeName);

        public bool NodeExist(Point3D NodePoint) => DGraph.GetConnectionsForName(Name).Any(x => x.ChildNode.Point == NodePoint);

        public IEnumerator<Connection<T>> GetEnumerator() => DGraph.GetConnectionsForName(Name).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region core

            #region events
        private event Action<DijkstraNode<T>> OnDeleteNode;

        private void OnDeleteConnectionsForNode(DijkstraNode<T> Node)
        {
            var connections = DGraph.GetConnectionsForName(Name);

            for (var i = 0; i < connections.Count; ++i) 
            {
                if (connections[i].ChildNode == Node) 
                {
                    connections.RemoveAt(i);
                    break;
                }
            }

            Node.OnDeleteNode -= OnDeleteConnectionsForNode;
            OnDeleteNode -= Node.OnDeleteConnectionsForNode;
        }
            #endregion

        internal void InvokeOnDeleteNode() 
        {
            OnDeleteNode?.Invoke(this);
        }          
        #endregion
    }
}
