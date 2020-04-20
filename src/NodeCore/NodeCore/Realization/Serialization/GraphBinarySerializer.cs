using System;
using System.Collections.Generic;
using System.IO;
using NodeCore.Base;

namespace NodeCore.Realization.Serialization
{
    public static class GraphBinarySerializer
    {
        private static readonly Dictionary<Type, Tuple<Delegate, Delegate>> TypeSerializeDeserializeCache;

        static GraphBinarySerializer() 
        {
            TypeSerializeDeserializeCache = new Dictionary<Type, Tuple<Delegate, Delegate>>();
        }

        public static void ConfigureBaseTypes() 
        {
            SetCustomSerializeForT<bool>((n, w) => w.Write(n), r => r.ReadBoolean());

            SetCustomSerializeForT<byte>((n, w) => w.Write(n), r => r.ReadByte());
            SetCustomSerializeForT<sbyte>((n, w) => w.Write(n), r => r.ReadSByte());

            SetCustomSerializeForT<short>((n, w) => w.Write(n), r => r.ReadInt16());
            SetCustomSerializeForT<ushort>((n, w) => w.Write(n), r => r.ReadUInt16());

            SetCustomSerializeForT<uint>((n, w) => w.Write(n), r => r.ReadUInt32());
            SetCustomSerializeForT<int>((n, w) => w.Write(n), r => r.ReadInt32());

            SetCustomSerializeForT<long>((n, w) => w.Write(n), r => r.ReadInt64());
            SetCustomSerializeForT<ulong>((n, w) => w.Write(n), r => r.ReadUInt64());

            SetCustomSerializeForT<double>((n, w) => w.Write(n), r => r.ReadDouble());

            SetCustomSerializeForT<decimal>((n, w) => w.Write(n), r => r.ReadDecimal());

            SetCustomSerializeForT<char>((n, w) => w.Write(n), r => r.ReadChar());

            SetCustomSerializeForT<string>((n, w) => w.Write(n ?? string.Empty), r => r.ReadString());
        }

        public static void SetCustomSerializeForT<T>(Action<T, BinaryWriter> Serializer, Func<BinaryReader, T> Deserializer)
        {
            if (Serializer == null || Deserializer == null)
                throw new GraphSerializationEx("Arguments cannot be null!");

            var t = typeof(T);

            if (TypeSerializeDeserializeCache.ContainsKey(t))
                throw new GraphSerializationEx("Custom serialization for this type is already installed!");

            TypeSerializeDeserializeCache.Add(t, new Tuple<Delegate, Delegate>(Serializer, Deserializer));
        }

        public static bool TypeContains(Type T) 
        {
            if (T == null)
                return false;

            return TypeSerializeDeserializeCache.ContainsKey(T);
        }

        public static void RemType(Type T) 
        {
            if (T == null)
                return;

            TypeSerializeDeserializeCache.Remove(T);
        }

        public static Tuple<Delegate, Delegate> GetSerializer(Type T) 
        {
            if (T == null)
                return null;

            return TypeSerializeDeserializeCache[T];
        }

        public static bool TryGetSerializer(Type T, out Tuple<Delegate, Delegate> Value) 
        {
            return TypeSerializeDeserializeCache.TryGetValue(T, out Value);
        }
    }

    public class GraphBinarySerializer<T>: BaseSerializer<T, Stream>
    {
        private BinaryWriter BW;

        private BinaryReader BR;

        private Dictionary<INode<T>, int> CacheS;

        private Dictionary<int, INode<T>> CacheD;

        private readonly Action<T, BinaryWriter> CustomTypeSerializer;
        private readonly Func<BinaryReader, T> CustomTypeDeserializer;

        private readonly bool UseCustomTypeSerializer;

        public GraphBinarySerializer(IGraph<T> Graph, Stream SerializationStream, bool UseCustomTypeSerializer): base(Graph, SerializationStream)
        {
            var t = typeof(T);

            if (UseCustomTypeSerializer && GraphBinarySerializer.TryGetSerializer(t, out Tuple<Delegate, Delegate> customSerializer)) 
            {
                this.UseCustomTypeSerializer = true;
                CustomTypeSerializer = (Action<T, BinaryWriter>)customSerializer.Item1;
                CustomTypeDeserializer = (Func<BinaryReader, T>)customSerializer.Item2;
            }

            OnFinishSerialize += GraphBinarySerializer_OnFinishSerialize;
            OnFinishDeserialize += GraphBinarySerializer_OnFinishDeserialize;
        }

        public GraphBinarySerializer(IGraph<T> Graph, Stream SerializationStream, Action<T, BinaryWriter> CustomTSerializer, Func<BinaryReader, T> CustomTDeserializer) 
            : this(Graph, SerializationStream, false)
        {
            if (CustomTSerializer == null || CustomTDeserializer == null)
                throw new GraphSerializationEx("Arguments cannot be null!");

            UseCustomTypeSerializer = true;
            CustomTypeSerializer = CustomTSerializer;
            CustomTypeDeserializer = CustomTDeserializer;
        }

        public override void Dispose() 
        {            
            GC.Collect();
        }

        protected override void DoSerialize()
        {
            CacheS = new Dictionary<INode<T>, int>();

            using (BW = new BinaryWriter(SerializationObj))
            {                
                WriteHeaderInfo();
                WriteNodes();
                WriteConnections();
            }          
        }

        protected override void DoDeserialize()
        {
            CacheD = new Dictionary<int, INode<T>>();

            using (BR = new BinaryReader(SerializationObj))
            {                
                var headInfo = ReadHeaderInfo();
                Graph.Clear(headInfo.Item1);
                ReadNodes(headInfo.Item2);
                ReadConnections();
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

                if (UseCustomTypeSerializer)
                    CustomTypeSerializer(n.Object, BW);

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

                if (UseCustomTypeSerializer)
                    n.Object = CustomTypeDeserializer(BR);

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

        private void GraphBinarySerializer_OnFinishDeserialize(object obj)
        {
            CacheD = null;
        }

        private void GraphBinarySerializer_OnFinishSerialize(object obj)
        {
            CacheS = null;
        }
    }
}
