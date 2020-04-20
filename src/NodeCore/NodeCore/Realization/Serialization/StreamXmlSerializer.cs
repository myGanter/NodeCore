using NodeCore.Base;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NodeCore.Realization.Serialization
{
    public class StreamXmlSerializer<T> : BaseXmlSerializer<T, Stream>
    {
        public StreamXmlSerializer(IGraph<T> Graph, Stream SerializationObj) :
            this(Graph, SerializationObj, false)
        { }

        public StreamXmlSerializer(IGraph<T> Graph, Stream SerializationObj, bool SerializeTType, XmlSerializerNamespaces TTypeSerializerNamespaces = null) :
            base(Graph, SerializationObj, SerializeTType, TTypeSerializerNamespaces)
        {
            OnFinishDeserialize += StreamXmlSerializer_OnFinishDeserialize;
        }

        public Encoding Encoding { get; set; }

        private StreamReader SR;

        protected override XmlWriter CreateXmlWriter()
        {
            var settings = GetDefaultXmlWriterSettings();
            if (Encoding != null)
                settings.Encoding = Encoding;

            var xw = XmlWriter.Create(SerializationObj, settings);

            return xw;
        }

        protected override XmlReader CreateXmlReader()
        {
            XmlReader xr;
            if (Encoding != null) 
            {
                SR = new StreamReader(SerializationObj, Encoding);
                xr = XmlReader.Create(SR);
            }
            else
                xr = XmlReader.Create(SerializationObj);

            return xr;
        }

        private void StreamXmlSerializer_OnFinishDeserialize(object obj)
        {
            SR?.Dispose();
            SR = null;
        }
    }
}
