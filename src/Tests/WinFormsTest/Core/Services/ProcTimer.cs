using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WinFormsTest.Core.Services
{
    public class ProcTimer
    {
        public event Action<string> Log;

        public void ProcTest(Func<string> PrevText, Action Clbk)
        {
            var sw = new Stopwatch();
            sw.Start();

            Clbk();

            sw.Stop();

            Log?.Invoke((string.IsNullOrEmpty(PrevText()) ? "" : PrevText() + ": ") + sw.ElapsedMilliseconds + " ms.");
        }

        public void ProcTest(string PrevText, Action Clbk) 
        {
            ProcTest(() => PrevText, Clbk);
        }

        public void ProcTest(Func<string> PrevText, Action<Stopwatch> Clbk)
        {
            var sw = new Stopwatch();
            sw.Start();

            Clbk(sw);

            sw.Stop();

            Log?.Invoke((string.IsNullOrEmpty(PrevText()) ? "" : PrevText() + ": ") + sw.ElapsedMilliseconds + " ms.");
        }

        public void ProcTest(string PrevText, Action<Stopwatch> Clbk) 
        {
            ProcTest(() => PrevText, Clbk);
        }
    }
}
