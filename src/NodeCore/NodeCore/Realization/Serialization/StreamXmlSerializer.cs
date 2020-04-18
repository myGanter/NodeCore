using NodeCore.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NodeCore.Realization.Serialization
{
    public class StreamXmlSerializer<T> : BaseXmlSerializer<T, Stream>
    {
        public StreamXmlSerializer(IGraph<T> Graph, Stream SerializationObj) :
            base(Graph, SerializationObj, false)
        { }

        public StreamXmlSerializer(IGraph<T> Graph, Stream SerializationObj, bool SerializeTType, XmlSerializerNamespaces TTypeSerializerNamespaces = null) :
            base(Graph, SerializationObj, SerializeTType, TTypeSerializerNamespaces)
        { }

        protected override XmlWriter CreateXmlWriter()
        {
            var settings = GetDefaultXmlWriterSettings();
            var xw = XmlWriter.Create(SerializationObj, settings);

            return xw;
        }
    }
}
