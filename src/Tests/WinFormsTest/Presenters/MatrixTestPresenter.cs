using System;
using System.Collections.Generic;
using System.Text;
using WinFormsTest.Core.Attributes;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Models;
using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Services;
using System.Drawing;

namespace WinFormsTest.Presenters
{
    [FrameElement(0, 191, 255, "Matrix test")]
    public class MatrixTestPresenter : BasePresenter<IMatrixForm, FrameElementArg>
    {
        private event Action<string> ExecLog;

        private IMatrixFrameBrush MatrixFrameBrush { get; set; }

        private readonly MatrixPaintService _MatrixPaintService;

        public MatrixTestPresenter(IAppFactory Controller, IMatrixForm View, MatrixPaintService _MatrixPaintService) : base(Controller, View)
        {
            this._MatrixPaintService = _MatrixPaintService;

            View.OnClose += View_OnClose;
            View.OnStartBtn += View_Start;
            View.OnReDraw += View_ReDraw;
            View.OnPostShow += View_OnPostShow;
            View.OnCanvasClick += View_OnCanvasClick;
        }

        public override void Run(FrameElementArg Arg)
        {
            var tType = GetType();
            ExecLog += (str) => Arg.LogInvoke(tType, str);

            ExecLog("Init form");

            MatrixFrameBrush = View.GetMatrixFrameDrawing();
            _MatrixPaintService.BindBrush(MatrixFrameBrush);

            View.Show();
        }

        private void View_OnCanvasClick(ObjTypeV Obj, Point P)
        {
            if (!_MatrixPaintService.SizeInit)
                return;

            switch (Obj)
            {
                case ObjTypeV.Grass:
                case ObjTypeV.Wall:
                    _MatrixPaintService.DrawObj((ObjType)Obj, P);
                    break;
                case ObjTypeV.Start:
                    _MatrixPaintService.DrawStart(P);
                    break;
                case ObjTypeV.Finish:
                    _MatrixPaintService.DrawFinish(P);
                    break;
            }            
        }

        private void View_OnClose()
        {
            ExecLog("Close form");
        }

        private void View_OnPostShow()
        {
            MatrixFrameBrush.Clear(System.Drawing.Color.FromArgb(255, 62, 62, 64));
            MatrixFrameBrush.DrawBoof();
        }

        private void View_ReDraw()
        {
            if (_MatrixPaintService.SizeInit)
            {
                _MatrixPaintService.DrawFrame();
            }
            else
            {
                View_OnPostShow();
            }
        }

        private void View_Start(uint Size)
        {
            ExecLog($"Init canvas... Matrix size: {Size}");

            _MatrixPaintService.Init(Size);
        }
    }
}
