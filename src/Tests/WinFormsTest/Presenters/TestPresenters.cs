using System;
using System.Collections.Generic;
using System.Text;
using WinFormsTest.Models;
using WinFormsTest.Core.Attributes;
using WinFormsTest.Core.Interfaces.IPresenters;

namespace WinFormsTest.Presenters
{
    [FrameElement(FrameName = "Test frame")]
    public class TestPresenter : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            throw new NotImplementedException();
        }
    }

    [FrameElement(FrameName = "2")]
    public class TestPresenter2 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            throw new NotImplementedException();
        }
    }

    [FrameElement(FrameName = "3")]
    public class TestPresenter3 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            throw new NotImplementedException();
        }
    }

    [FrameElement(FrameName = "4")]
    public class TestPresenter4 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            throw new NotImplementedException();
        }
    }

    [FrameElement(FrameName = "5")]
    public class TestPresenter5 : IPresenter<FrameElementArg>
    {
        public void Run(FrameElementArg Arg)
        {
            throw new NotImplementedException();
        }
    }
}
