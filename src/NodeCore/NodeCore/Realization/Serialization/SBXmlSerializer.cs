using NodeCore.Base;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NodeCore.Realization.Serialization
{
    public class SBXmlSerializer<T> : BaseXmlSerializer<T, StringBuilder>
    {
        private StringReader StrReader;

        public SBXmlSerializer(IGraph<T> Graph, StringBuilder SerializationObj) : 
            this(Graph, SerializationObj, false)
        { }

        public SBXmlSerializer(IGraph<T> Graph, StringBuilder SerializationObj, bool SerializeTType, XmlSerializerNamespaces TTypeSerializerNamespaces = null) : 
            base(Graph, SerializationObj, SerializeTType, TTypeSerializerNamespaces)
        {
            OnFinishDeserialize += SBXmlSerializer_OnFinishDeserialize;
        }

        protected override XmlWriter CreateXmlWriter()
        {
            var settings = GetDefaultXmlWriterSettings();
            var xw = XmlWriter.Create(SerializationObj, settings);

            return xw;
        }

        protected override XmlReader CreateXmlReader()
        {
            StrReader = new StringReader(SerializationObj.ToString());            

            var xr = XmlReader.Create(StrReader);

            return xr;
        }

        private void SBXmlSerializer_OnFinishDeserialize(object obj)
        {
            StrReader.Dispose();
        }
    }
}
