using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WinFormsTest.Models;

namespace WinFormsTest.Core.Interfaces.IViews
{
    public interface IMatrixForm : IView
    {
        event Action OnClose;

        event Action<uint> OnStartBtn;

        event Action OnReDraw;

        event Action OnPostShow;

        event Action<ObjTypeV, Point> OnCanvasClick;

        event Action<ObjTypeV> OnFill;

        event Action<string> OnSearch;

        IMatrixFrameBrush GetMatrixFrameDrawing();

        void BuildGrapbBox(List<string> Objs);

        void ShowToolPanel();
    }
}
