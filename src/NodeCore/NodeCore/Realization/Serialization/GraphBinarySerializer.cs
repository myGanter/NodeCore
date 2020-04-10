using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NodeCore.Base;
using NodeCore.Realization;

namespace NodeCore.Realization.Serialization
{
    class GraphBinarySerializer<T>: IDisposable
    {
        public IGraph<T> Graph { get; private set; }

        public Stream SerializationStream { get; private set; }

        private BinaryWriter BW;

        private BinaryReader BR;

        private Dictionary<INode<T>, int> CacheS;

        private Dictionary<int, INode<T>> CacheD;

        public GraphBinarySerializer(IGraph<T> Graph, Stream SerializationStream) 
        {
            if (Graph == null)
                throw new GraphSerializationEx("Graph cannot be null!");

            if (SerializationStream == null)
                throw new GraphSerializationEx("The serialization thread cannot be null!");

            this.Graph = Graph;
            this.SerializationStream = SerializationStream;
        }

        public void Dispose() 
        {            
            GC.Collect();
        }

        public void Serialize() 
        {
            CacheS = new Dictionary<INode<T>, int>();

            try
            {
                BW = new BinaryWriter(SerializationStream);
                WriteHeaderInfo();
                WriteNodes();
                WriteConnections();
            }
            catch (Exception e)
            {
                throw new GraphSerializationEx("Serialization error!", e);
            }
            finally 
            {
                BW.Dispose();
                CacheS = null;
            }            
        }

        public void Deserialize() 
        {
            Graph.Clear();
            CacheD = new Dictionary<int, INode<T>>();

            try
            {
                BR = new BinaryReader(SerializationStream);
                var headInfo = ReadHeaderInfo();
                ReadNodes(headInfo.Item2);
                ReadConnections();
            }
            catch (Exception e)
            {
                throw new GraphSerializationEx("Deserialization error!", e);
            }
            finally 
            {
                BR.Dispose();
                CacheD = null;
            }            
        }

        private void WriteHeaderInfo() 
        {
            var graphName = Graph.Name ?? string.Empty;
            var graphNodeL = Graph.NodeLength;

            BW.Write(graphName);
            BW.Write(graphNodeL);
        }

        private TupleStructure<string, int> ReadHeaderInfo() 
        {
            var graphName = BR.ReadString();
            var graphNodeL = BR.ReadInt32();

            return TupleStructure.Create(graphName, graphNodeL);
        }

        private void WriteNodes() 
        {
            var index = 0;

            foreach (var n in Graph) 
            {
                ++index;
                BW.Write(index);

                var nodeP = n.Point;
                BW.Write(nodeP.X);
                BW.Write(nodeP.Y);
                BW.Write(nodeP.Z);

                var nodeName = n.Name ?? string.Empty;
                BW.Write(nodeName);

                CacheS.Add(n, index);
            }
        }

        private void ReadNodes(int NodeL) 
        {
            for (var i = 0; i < NodeL; ++i) 
            {
                var nIndex = BR.ReadInt32();
                var x = BR.ReadInt32();
                var y = BR.ReadInt32();
                var z = BR.ReadInt32();
                var nName = BR.ReadString();

                var n = Graph.AddNode(nName, new Point3D(x, y, z));
                CacheD.Add(nIndex, n);
            }
        }

        private void WriteConnections() 
        {
            var dependenceChecker = new HashSet<long>();

            foreach (var i in CacheS)
            {
                var index = i.Value;
                var n = i.Key;

                foreach (var c in n) 
                {
                    var chNode = c.ChildNode;
                    var dependence = c.Dependence;
                    var chIndex = CacheS[chNode];

                    if (dependence == Dependence.Doubly) 
                    {
                        var heshIndexs = index * chIndex;

                        if (dependenceChecker.Contains(heshIndexs))
                            continue;
                        else
                            dependenceChecker.Add(heshIndexs);
                    }

                    var distance = c.Distance;

                    //родительский индекс
                    BW.Write(index);
                    //дочерний индекс
                    BW.Write(chIndex);
                    //дистанция
                    BW.Write(distance);
                    //тип подключения
                    BW.Write((int)dependence);
                }
            }
        }

        private void ReadConnections() 
        {
            while (BR.BaseStream.Position < BR.BaseStream.Length) 
            {
                var parentIndex = BR.ReadInt32();
                var chIndex = BR.ReadInt32();
                var distance = BR.ReadDouble();
                var dependence = (Dependence)BR.ReadInt32();

                var pn = CacheD[parentIndex];
                var cn = CacheD[chIndex];
                pn.AddNode(distance, dependence, (g, n) => cn);
            }
        }
    }
}
