using System;

namespace NodeCore.Realization
{
    public class NodeCoreEx : Exception 
    {
        public NodeCoreEx(string ExStr) : base(ExStr) { }

        public NodeCoreEx(string ExStr, Exception InnerException) : base(ExStr, InnerException) { }
    }

    public class GraphEx : NodeCoreEx
    {
        public GraphEx(string ExStr) : base(ExStr) { }

        public GraphEx(string ExStr, Exception InnerException) : base(ExStr, InnerException) { }
    }

    public class ProcessorEx : NodeCoreEx
    {
        public ProcessorEx(string ExStr) : base(ExStr) { }

        public ProcessorEx(string ExStr, Exception InnerException) : base(ExStr, InnerException) { }
    }

    public class GraphSerializationEx : NodeCoreEx 
    {
        public GraphSerializationEx(string ExStr) : base(ExStr) { }

        public GraphSerializationEx(string ExStr, Exception InnerException) : base(ExStr, InnerException) { }
    }
}
