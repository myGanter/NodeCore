using NodeCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeCore.Realization.RecursiveAlg
{
    struct NodeBond<T>
    {
        public RecNode<T> Parent { get; set; }

        public double Distance { get; set; }

        public NodeBond(RecNode<T> Parent, double Distance)
        {
            this.Parent = Parent;
            this.Distance = Distance;
        }

        public NodeBond(NodeBond<T> ParentBond, Connection<T> ParentConnection)
        {
            Parent = (RecNode<T>)ParentConnection.ParentNode;
            Distance = ParentBond.Distance + ParentConnection.Distance;
        }
    }
}
