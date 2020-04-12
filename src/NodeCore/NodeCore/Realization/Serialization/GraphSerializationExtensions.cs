using System;
using NodeCore.Base;
using System.IO;
using System.Threading.Tasks;

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

        public static void SerializeToXml<T>(this IGraph<T> Graph, Stream SerializationStream)
        {

        }

        public static string SerializeToXml<T>(this IGraph<T> Graph)
        {
            throw new Exception();
        }
    }
}
