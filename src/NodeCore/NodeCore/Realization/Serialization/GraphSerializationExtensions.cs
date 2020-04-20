using NodeCore.Base;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace NodeCore.Realization.Serialization
{
    public static class GraphSerializationExtensions
    {
        #region Binary
        public static void SerializeToBinary<T>(this IGraph<T> Graph, Stream SerializationStream) 
        {
            SerializeToBinary(Graph, SerializationStream, true);
        }

        public static void SerializeToBinary<T>(this IGraph<T> Graph, Stream SerializationStream, bool UseCustomTypeSerializer)
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream, UseCustomTypeSerializer);
            binSer.Serialize();
        }

        public async static Task SerializeToBinaryAsync<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            await SerializeToBinaryAsync(Graph, SerializationStream, true);
        }

        public async static Task SerializeToBinaryAsync<T>(this IGraph<T> Graph, Stream SerializationStream, bool UseCustomTypeSerializer)
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream, UseCustomTypeSerializer);
            await binSer.SerializeAsync();
        }

        public static void BinaryDeserialize<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            BinaryDeserialize(Graph, SerializationStream, true);
        }

        public static void BinaryDeserialize<T>(this IGraph<T> Graph, Stream SerializationStream, bool UseCustomTypeSerializer)
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream, UseCustomTypeSerializer);
            binSer.Deserialize();
        }

        public async static Task BinaryDeserializeAsync<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            await BinaryDeserializeAsync(Graph, SerializationStream, true);
        }

        public async static Task BinaryDeserializeAsync<T>(this IGraph<T> Graph, Stream SerializationStream, bool UseCustomTypeSerializer)
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream, UseCustomTypeSerializer);
            await binSer.DeserializeAsync();
        }
        #endregion

        #region XML
        public static void SerializeToXml<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            SerializeToXml(Graph, SerializationStream, false);
        }

        public static void SerializeToXml<T>(this IGraph<T> Graph, StringBuilder SerializationSB)
        {
            SerializeToXml(Graph, SerializationSB, false);
        }

        public static void SerializeToXml<T>(this IGraph<T> Graph, Stream SerializationStream, bool SerializeTType)
        {
            var streamSer = new StreamXmlSerializer<T>(Graph, SerializationStream, SerializeTType);
            streamSer.Serialize();
        }

        public static void SerializeToXml<T>(this IGraph<T> Graph, StringBuilder SerializationSB, bool SerializeTType)
        {
            var streamSer = new SBXmlSerializer<T>(Graph, SerializationSB, SerializeTType);
            streamSer.Serialize();
        }

        public async static Task SerializeToXmlAsync<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            await SerializeToXmlAsync(Graph, SerializationStream, false);
        }

        public async static Task SerializeToXmlAsync<T>(this IGraph<T> Graph, StringBuilder SerializationSB)
        {
            await SerializeToXmlAsync(Graph, SerializationSB, false);
        }

        public async static Task SerializeToXmlAsync<T>(this IGraph<T> Graph, Stream SerializationStream, bool SerializeTType)
        {
            var streamSer = new StreamXmlSerializer<T>(Graph, SerializationStream, SerializeTType);
            await streamSer.SerializeAsync();
        }

        public async static Task SerializeToXmlAsync<T>(this IGraph<T> Graph, StringBuilder SerializationSB, bool SerializeTType)
        {
            var streamSer = new SBXmlSerializer<T>(Graph, SerializationSB, SerializeTType);
            await streamSer.SerializeAsync();
        }

        public static void XmlDeserialize<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            XmlDeserialize(Graph, SerializationStream, false);
        }

        public static void XmlDeserialize<T>(this IGraph<T> Graph, StringBuilder SerializationSB)
        {
            XmlDeserialize(Graph, SerializationSB, false);
        }

        public static void XmlDeserialize<T>(this IGraph<T> Graph, Stream SerializationStream, bool SerializeTType)
        {
            var streamSer = new StreamXmlSerializer<T>(Graph, SerializationStream, SerializeTType);
            streamSer.Deserialize();
        }

        public static void XmlDeserialize<T>(this IGraph<T> Graph, StringBuilder SerializationSB, bool SerializeTType)
        {
            var streamSer = new SBXmlSerializer<T>(Graph, SerializationSB, SerializeTType);
            streamSer.Deserialize();
        }

        public async static Task XmlDeserializeAsync<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            await XmlDeserializeAsync(Graph, SerializationStream, false);
        }

        public async static Task XmlDeserializeAsync<T>(this IGraph<T> Graph, StringBuilder SerializationSB)
        {
            await XmlDeserializeAsync(Graph, SerializationSB, false);
        }

        public async static Task XmlDeserializeAsync<T>(this IGraph<T> Graph, Stream SerializationStream, bool SerializeTType)
        {
            var streamSer = new StreamXmlSerializer<T>(Graph, SerializationStream, SerializeTType);
            await streamSer.DeserializeAsync();
        }

        public async static Task XmlDeserializeAsync<T>(this IGraph<T> Graph, StringBuilder SerializationSB, bool SerializeTType)
        {
            var streamSer = new SBXmlSerializer<T>(Graph, SerializationSB, SerializeTType);
            await streamSer.DeserializeAsync();
        }
        #endregion
    }
}
