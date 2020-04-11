using System;
using NodeCore.Base;
using System.IO;

namespace NodeCore.Realization.Serialization
{
    public static class GraphSerializationExtensions
    {
        public static void SerializeToBinary<T>(this IGraph<T> Graph, Stream SerializationStream) 
        {
            SerializeToBinary(Graph, SerializationStream, true);
        }

        public static void SerializeToBinary<T>(this IGraph<T> Graph, Stream SerializationStream, bool UseCustomTypeSerializer)
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream, UseCustomTypeSerializer);
            binSer.Serialize();
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

        public static void SerializeToXml<T>(this IGraph<T> Graph, Stream SerializationStream)
        {

        }

        public static string SerializeToXml<T>(this IGraph<T> Graph)
        {
            throw new Exception();
        }
    }
}
