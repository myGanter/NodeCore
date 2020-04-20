using NodeCore.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NodeCore.Realization.Serialization
{
    public abstract class BaseXmlSerializer<T, O> : BaseSerializer<T, O>
    {
        private static readonly string GraphTag = "graph";
        private static readonly string NodeCollectionTag = "nodes";
        private static readonly string NodeTag = "node";
        private static readonly string EdgeCollectionTag = "edges";
        private static readonly string EdgeTag = "edge";
        private static readonly string NameAttr = "name";
        private static readonly string NodeCountAttr = "nodeCount";
        private static readonly string NodeIndexAttr = "nodeIndex";
        private static readonly string PointXAttr = "x";
        private static readonly string PointYAttr = "y";
        private static readonly string PointZAttr = "z";
        private static readonly string DistanceAttr = "distance";
        private static readonly string ChildNodeIndexAttr = "childNode";
        private static readonly string ParentNodeIndexAttr = "parentNode";
        private static readonly string DependenceAttr = "dependence";

        protected static XmlWriterSettings GetDefaultXmlWriterSettings() 
        {
            var settings = new XmlWriterSettings
            {
                Indent = true
            };
            return settings;
        }

        private readonly bool SerializeTType;

        private readonly XmlSerializer TTypeSerializer;

        private readonly XmlSerializerNamespaces TTypeSerializerNamespaces;

        private XmlWriter XW;

        private XmlReader XR;

        private Dictionary<INode<T>, int> CacheS;

        private Dictionary<int, INode<T>> CacheD;

        public BaseXmlSerializer(IGraph<T> Graph, O SerializationObj, bool SerializeTType, XmlSerializerNamespaces TTypeSerializerNamespaces = null) : base(Graph, SerializationObj)
        {
            this.SerializeTType = SerializeTType;
            OnFinishSerialize += BaseXmlSerializer_OnFinishSerialize;
            OnFinishDeserialize += BaseXmlSerializer_OnFinishDeserialize;

            if (SerializeTType) 
            {
                TTypeSerializer = new XmlSerializer(typeof(T));
                this.TTypeSerializerNamespaces = TTypeSerializerNamespaces;
            }
        }

        public override void Dispose()
        {
            GC.Collect();
        }

        protected abstract XmlWriter CreateXmlWriter();

        protected abstract XmlReader CreateXmlReader();

        protected override void DoDeserialize()
        {
            CacheD = new Dictionary<int, INode<T>>();

            using (XR = CreateXmlReaderOrExeption()) 
            {
                var doc = new XmlDocument();
                doc.Load(XR);

                var graphTag = doc.SelectSingleNode(GraphTag);
                if (graphTag == null)
                    throw new GraphSerializationEx("Invalid xml structure!");

                InitGraph(graphTag);
                if (ReadNodes(graphTag))                 
                    ReadConnections(graphTag);                
            }            
        }

        protected override void DoSerialize()
        {
            CacheS = new Dictionary<INode<T>, int>();

            using (XW = CreateXmlWriterOrExeption()) 
            {
                WriteHeaderInfoBegin();
                WriteNodes();
                WriteConnections();
                WriteHeaderInfoEnd();
            }
        }

        private XmlWriter CreateXmlWriterOrExeption()
        {
            var xw = CreateXmlWriter();
            if (xw == null)
                throw new GraphSerializationEx("CreateXmlWriter returned null!");

            return xw;
        }

        private XmlReader CreateXmlReaderOrExeption()
        {
            var xr = CreateXmlReader();
            if (xr == null)
                throw new GraphSerializationEx("CreateXmlWriter returned null!");

            return xr;
        }

        #region XmlWrite
        private void WriteHeaderInfoBegin() 
        {
            var graphName = Graph.Name ?? string.Empty;
            var graphNodeL = Graph.NodeLength;

            XW.WriteStartDocument();
            XW.WriteStartElement(GraphTag);

            XW.WriteAttributeString(NameAttr, graphName);
            XW.WriteAttributeString(NodeCountAttr, graphNodeL.ToString());
        }

        private void WriteHeaderInfoEnd() 
        {
            XW.WriteEndElement();
        }

        private void WriteNodes() 
        {
            XW.WriteStartElement(NodeCollectionTag);

            var index = 0;

            foreach (var n in Graph)
            {
                ++index;
                var nodeP = n.Point;
                var nodeName = n.Name ?? string.Empty;

                XW.WriteStartElement(NodeTag);

                XW.WriteAttributeString(NodeIndexAttr, index.ToString());
                XW.WriteAttributeString(NameAttr, nodeName);
                XW.WriteAttributeString(PointXAttr, nodeP.X.ToString());
                XW.WriteAttributeString(PointYAttr, nodeP.Y.ToString());
                XW.WriteAttributeString(PointZAttr, nodeP.Z.ToString());

                if (SerializeTType)
                    if (TTypeSerializerNamespaces == null)
                        TTypeSerializer.Serialize(XW, n.Object);
                    else
                        TTypeSerializer.Serialize(XW, n.Object, TTypeSerializerNamespaces);

                XW.WriteEndElement();

                CacheS.Add(n, index);
            }

            XW.WriteEndElement();
        }

        private void WriteConnections() 
        {
            XW.WriteStartElement(EdgeCollectionTag);

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

                    XW.WriteStartElement(EdgeTag);

                    //родительский индекс
                    XW.WriteAttributeString(ParentNodeIndexAttr, index.ToString());
                    //дочерний индекс
                    XW.WriteAttributeString(ChildNodeIndexAttr, chIndex.ToString());
                    //дистанция
                    XW.WriteAttributeString(DistanceAttr, distance.ToString());
                    //тип подключения
                    XW.WriteAttributeString(DependenceAttr, dependence.ToString());

                    XW.WriteEndElement();
                }
            }

            XW.WriteEndElement();
        }
        #endregion

        #region XmlRead
        private void InitGraph(XmlNode MainTag) 
        {
            var attr = MainTag.Attributes[NameAttr];
            var newGraphName = (attr?.Value) ?? string.Empty;
            Graph.Clear(newGraphName);
        }

        private bool ReadNodes(XmlNode MainTag) 
        {
            var nodes = MainTag.SelectSingleNode(NodeCollectionTag);
            if (nodes == null)
                return false;

            foreach (XmlNode n in nodes.SelectNodes(NodeTag)) 
            {
                var nodeName = (n.Attributes[NameAttr]?.Value) ?? string.Empty;

                if (!int.TryParse(n.Attributes[NodeIndexAttr]?.Value, out int nodeIndex))
                    throw new GraphSerializationEx($"The node [{nodeName}] does not have an index in the xml document!");

                int.TryParse(n.Attributes[PointXAttr]?.Value, out int x);
                int.TryParse(n.Attributes[PointYAttr]?.Value, out int y);
                int.TryParse(n.Attributes[PointZAttr]?.Value, out int z);

                var node = Graph.AddNode(nodeName, new Point3D(x, y, z));

                if (SerializeTType) 
                {
                    using (var strReader = new StringReader(n.InnerXml)) 
                    {
                        node.Object = (T)TTypeSerializer.Deserialize(strReader);
                    }
                }

                CacheD.Add(nodeIndex, node);
            }

            return CacheD.Count > 0;
        }

        private void ReadConnections(XmlNode MainTag) 
        {
            var edges = MainTag.SelectSingleNode(EdgeCollectionTag);
            if (edges == null)
                return;

            foreach (XmlNode e in edges.SelectNodes(EdgeTag))
            {
                if (!Enum.TryParse<Dependence>(e.Attributes[DependenceAttr]?.Value, out Dependence dependence))
                    throw new GraphSerializationEx($"The edge does not have an dependence in the xml document!");

                if (!int.TryParse(e.Attributes[ParentNodeIndexAttr]?.Value, out int parentNodeInd))
                    throw new GraphSerializationEx($"The edge does not have an parent node index in the xml document!");

                if (!int.TryParse(e.Attributes[ChildNodeIndexAttr]?.Value, out int childNodeInd))
                    throw new GraphSerializationEx($"The edge does not have an child node index in the xml document!");

                if (!int.TryParse(e.Attributes[DistanceAttr]?.Value, out int distance))
                    throw new GraphSerializationEx($"The edge does not have an distance in the xml document!");

                var pn = CacheD[parentNodeInd];
                var cn = CacheD[childNodeInd];
                pn.AddNode(distance, dependence, (g, n) => cn);
            }
        }
        #endregion

        private void BaseXmlSerializer_OnFinishSerialize(object obj)
        {
            CacheS = null;
        }

        private void BaseXmlSerializer_OnFinishDeserialize(object obj)
        {
            CacheD = null;
        }
    }
}
