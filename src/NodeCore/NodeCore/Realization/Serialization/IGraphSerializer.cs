using NodeCore.Base;
using System;
using System.Threading.Tasks;

namespace NodeCore.Realization.Serialization
{
    public interface IGraphSerializer<T, out O>: IDisposable
    {
        IGraph<T> Graph { get; }

        O SerializationObj { get; }

        event Action<object> OnStartSerialize;

        event Action<object> OnFinishSerialize;

        event Action<object> OnStartDeserialize;

        event Action<object> OnFinishDeserialize;

        void Serialize();

        Task SerializeAsync();

        void Deserialize();

        Task DeserializeAsync();
    }
}
