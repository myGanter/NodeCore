using NodeCore.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeCore.Realization.Serialization
{
    public abstract class BaseSerializer<T, O> : IGraphSerializer<T, O>
    {
        public IGraph<T> Graph { get; private set; }

        public O SerializationObj { get; private set; }

        public event Action<object> OnStartSerialize;
        public event Action<object> OnFinishSerialize;
        public event Action<object> OnStartDeserialize;
        public event Action<object> OnFinishDeserialize;

        protected readonly object Locker;
        protected bool IsWork;

        public BaseSerializer(IGraph<T> Graph, O SerializationObj) 
        {
            if (Graph == null)
                throw new GraphSerializationEx("Graph cannot be null!");

            if (SerializationObj == null)
                throw new GraphSerializationEx("The serialization object cannot be null!");

            this.Graph = Graph;
            this.SerializationObj = SerializationObj;

            Locker = new object();
        }

        public virtual void Deserialize()
        {
            SetWork();
            OnStartDeserialize?.Invoke(this);

            try
            {
                DoDeserialize();
            }
            catch (Exception e)
            {
                throw new GraphSerializationEx("Deserialization error!", e);
            }
            finally
            {
                SetUnwork();
                OnFinishDeserialize?.Invoke(this);
            }
        }

        public async virtual Task DeserializeAsync()
        {
            await Task.Run(new Action(Deserialize));
        }

        public virtual void Serialize()
        {
            SetWork();
            OnStartSerialize?.Invoke(this);

            try
            {
                DoSerialize();
            }
            catch (Exception e)
            {
                throw new GraphSerializationEx("Serialization error!", e);
            }
            finally
            {
                SetUnwork();
                OnFinishSerialize?.Invoke(this);
            }
        }

        public async virtual Task SerializeAsync()
        {
            await Task.Run(new Action(Serialize));
        }

        public abstract void Dispose();

        protected abstract void DoSerialize();

        protected abstract void DoDeserialize();

        protected void SetWork() 
        {
            lock (Locker)
            {
                if (IsWork)
                    throw new GraphSerializationEx("This serializer is already working!");

                IsWork = true;
            }
        }

        protected void SetUnwork()
        {
            lock (Locker)
            {
                IsWork = false;
            }
        }
    }
}
