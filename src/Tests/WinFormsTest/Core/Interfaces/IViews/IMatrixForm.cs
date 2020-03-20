using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsTest.Core.Interfaces.IViews
{
    public interface IMatrixForm : IView
    {
        event Action OnClose;
    }
}
