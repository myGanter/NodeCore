using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTest.Core.Interfaces.IViews
{
    public interface ImainForm : IView
    {
        void BuildTests(List<Tuple<Type, string>> Data);

        event Func<Type, Task> Start;
    }
}
