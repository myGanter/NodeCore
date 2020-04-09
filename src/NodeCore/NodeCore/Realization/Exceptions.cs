using System;
using System.Collections.Generic;
using System.Text;

namespace NodeCore.Realization
{
    public class NodeCoreEx : Exception 
    {
        public NodeCoreEx(string ExStr) : base(ExStr) { }
    }

    public class GraphEx : NodeCoreEx
    {
        public GraphEx(string ExStr) : base(ExStr) { }
    }

    public class ProcessorEx : NodeCoreEx
    {
        public ProcessorEx(string ExStr) : base(ExStr) { }
    }
}
