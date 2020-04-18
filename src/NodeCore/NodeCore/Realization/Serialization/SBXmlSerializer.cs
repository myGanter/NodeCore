using NodeCore.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NodeCore.Realization.Serialization
{
    public class SBXmlSerializer<T> : BaseXmlSerializer<T, StringBuilder>
    {
        public SBXmlSerializer(IGraph<T> Graph, StringBuilder SerializationObj) : 
            base(Graph, SerializationObj, false)
        { }

        public SBXmlSerializer(IGraph<T> Graph, StringBuilder SerializationObj, bool SerializeTType, XmlSerializerNamespaces TTypeSerializerNamespaces = null) : 
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
