using System;
using System.Collections.Generic;
using System.Text;
using NodeCore.Base;
using System.IO;

namespace NodeCore.Realization.Serialization
{
    public static class GraphSerializationExtensions
    {
        public static void SerializeToBinary<T>(this IGraph<T> Graph, Stream SerializationStream) 
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream);
            binSer.Serialize();
        }

        public static void BinaryDeserialize<T>(this IGraph<T> Graph, Stream SerializationStream)
        {
            var binSer = new GraphBinarySerializer<T>(Graph, SerializationStream);
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
