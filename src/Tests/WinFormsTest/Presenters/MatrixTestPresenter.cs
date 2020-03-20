using System;
using System.Collections.Generic;
using System.Text;
using WinFormsTest.Core.Attributes;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Models;
using WinFormsTest.Core.Interfaces;

namespace WinFormsTest.Presenters
{
    [FrameElement(0, 191, 255, "Matrix test")]
    public class MatrixTestPresenter : BasePresenter<IMatrixForm, FrameElementArg>
    {
        private event Action<string> ExecLog;

        public MatrixTestPresenter(IAppFactory Controller, IMatrixForm View) : base(Controller, View)
        {
            View.OnClose += View_OnClose;
        }

        private void View_OnClose()
        {
            ExecLog("Close form");
        }

        public override void Run(FrameElementArg Arg)
        {
            var tType = GetType();
            ExecLog += (str) => Arg.LogInvoke(tType, str);

            ExecLog("Init form");

            View.Show();
        }
    }
}
