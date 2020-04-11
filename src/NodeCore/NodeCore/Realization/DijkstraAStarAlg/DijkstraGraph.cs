using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCore.Base;

namespace NodeCore.Realization.DijkstraAStarAlg
{
    class DijkstraGraph<T> : IGraph<T>
    {
        #region interface
        public string Name { get; private set; }

        private Dictionary<string, Tuple<DijkstraNode<T>, List<Connection<T>>>> StrValDict;
        private Dictionary<Point3D, string> PointStrDict;
        private readonly Func<DijkstraGraph<T>, INodeProcessor<T>> ProcessorBuilder;

        public int NodeLength => StrValDict.Count;

        public DijkstraGraph(string Name, Func<DijkstraGraph<T>, INodeProcessor<T>> ProcessorBuilder)
        {
            this.Name = Name;
            this.ProcessorBuilder = ProcessorBuilder;

            InitDicts();
        }

        public INode<T> this[int Index] => StrValDict.ElementAt(Index).Value.Item1;

        public INode<T> this[string NodeName] => StrValDict[NodeName].Item1;

        public INode<T> this[Point3D NodePoint] => StrValDict[PointStrDict[NodePoint]].Item1;

        public INode<T> AddNode(string NodeName, Point3D Point = default)
        {
            var node = new DijkstraNode<T>(NodeName, this, Point);

            if (NodeExist(node))
                throw new GraphEx($"Node [Name: {NodeName}, Point: {Point}] exists");

            AddNode(node);

            return node;
        }

        public void DeleteNode(string NodeName) => RemoveNodeFromGraph((DijkstraNode<T>)this[NodeName]);

        public void DeleteNode(Point3D NodePoint) => RemoveNodeFromGraph((DijkstraNode<T>)this[NodePoint]);

        public void DeleteNode(INode<T> Node) 
        {
            if (Node == null)
                throw new GraphEx($"Node is null");

            var delNode = this[Node.Name];
            if (delNode != Node)
                throw new GraphEx($"Node [Name: {Node.Name}, Point: {Node.Point}] was not found in the current graph");

            RemoveNodeFromGraph((DijkstraNode<T>)delNode);
        }

        public void Clear()
        {
            InitDicts();
        }

        public void Clear(string NewGraphName) 
        {
            Name = NewGraphName;
            Clear();
        }

        public INodeProcessor<T> CreateNodeProcessor() => ProcessorBuilder(this);

        public bool NodeExist(INode<T> Node) => NodeExist(Node.Name) || NodeExist(Node.Point);

        public bool NodeExist(string NodeName) => StrValDict.ContainsKey(NodeName);

        public bool NodeExist(Point3D NodePoint) => PointStrDict.ContainsKey(NodePoint);

        public IEnumerator<INode<T>> GetEnumerator() => StrValDict.Select(x => x.Value.Item1).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
        }
        #endregion

        #region core
        internal List<Connection<T>> GetConnectionsForName(string NodeName) => StrValDict[NodeName].Item2;

        internal List<Connection<T>> GetConnectionsForPoint(Point3D NodePont) => StrValDict[PointStrDict[NodePont]].Item2;

        internal void AddNode(DijkstraNode<T> Node)
        {
            StrValDict.Add(Node.Name, new Tuple<DijkstraNode<T>, List<Connection<T>>>(Node, new List<Connection<T>>()));
            PointStrDict.Add(Node.Point, Node.Name);
        }

        internal void AddConnection(DijkstraNode<T> ParentNode, Connection<T> Connection)
        {
            StrValDict[ParentNode.Name].Item2.Add(Connection);
        }

        private void InitDicts()
        {
            StrValDict = new Dictionary<string, Tuple<DijkstraNode<T>, List<Connection<T>>>>();
            PointStrDict = new Dictionary<Point3D, string>();
        }

        private void RemoveNodeFromGraph(DijkstraNode<T> Node) 
        {
            StrValDict.Remove(Node.Name);
            Node.InvokeOnDeleteNode();
        }
        #endregion
    }
}
