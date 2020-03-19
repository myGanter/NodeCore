using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsTest.Models
{
    public class FrameElementArg
    {
        public event Action<Type, string> Log;

        public void LogInvoke(Type PType, string Str) => Log?.Invoke(PType, Str);
    }
}
