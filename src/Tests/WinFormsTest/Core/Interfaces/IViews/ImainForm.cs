﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace WinFormsTest.Core.Interfaces.IViews
{
    public interface ImainForm : IView
    {
        void BuildTests(List<Tuple<Type, string>> Data);

        void Log(string LogText, Color HColor);

        event Action<Type> Start;

        object Invoke(Delegate Clbk);
    }
}
