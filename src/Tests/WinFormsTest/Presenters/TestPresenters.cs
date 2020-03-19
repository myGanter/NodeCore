using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using WinFormsTest.Models;
using WinFormsTest.Core.Attributes;
using WinFormsTest.Core.Interfaces.IPresenters;

namespace WinFormsTest.Presenters
{
    [FrameElement(168, 136, 56, "Test frame")]
    public class TestPresenter : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            var t = GetType();

            for (var i = 0; i < 100; ++i)
                Arg.LogInvoke(t, new string('w', 196) );
        }
    }

    [FrameElement("Test 2")]
    public class TestPresenter2 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            var t = GetType();

            for (var i = 0; i < 100; ++i)
                Arg.LogInvoke(t, i.ToString());
        }
    }

    [FrameElement("Test 3")]
    public class TestPresenter3 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            var t = GetType();

            for (var i = 0; i < 100; ++i)
                Arg.LogInvoke(t, i.ToString());
        }
    }

    [FrameElement("Test 4")]
    public class TestPresenter4 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            var t = GetType();

            for (var i = 0; i < 100; ++i)
                Arg.LogInvoke(t, i.ToString());
        }
    }

    [FrameElement("Test 5")]
    public class TestPresenter5 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            var t = GetType();

            for (var i = 0; i < 100; ++i)
                Arg.LogInvoke(t, i.ToString());
        }
    }
}
