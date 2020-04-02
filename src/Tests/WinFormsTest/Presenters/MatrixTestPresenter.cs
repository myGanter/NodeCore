using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WinFormsTest.Core.Attributes;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Models;
using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Services;
using System.Drawing;
using NodeCore.Base;

namespace WinFormsTest.Presenters
{
    [FrameElement(0, 191, 255, "Matrix test")]
    public class MatrixTestPresenter : BasePresenter<IMatrixForm, FrameElementArg>
    {
        private event Action<string> ExecLog;

        private IMatrixFrameBrush MatrixFrameBrush { get; set; }

        private readonly MatrixPaintService _MatrixPaintService;

        private readonly GraphService<string> _GraphService;

        public MatrixTestPresenter(IAppFactory Controller, IMatrixForm View, MatrixPaintService MatrixPaintService, GraphService<string> GraphService) : base(Controller, View)
        {
            _MatrixPaintService = MatrixPaintService;
            _GraphService = GraphService;

            View.OnClose += View_OnClose;
            View.OnStartBtn += View_Start;
            View.OnReDraw += View_ReDraw;
            View.OnPostShow += View_OnPostShow;
            View.OnCanvasClick += View_OnCanvasClick;
            View.OnFill += View_OnFill;
            View.OnSearch += View_OnSearch;
        }

        public override void Run(FrameElementArg Arg)
        {
            var tType = GetType();
            ExecLog += (str) => Arg.LogInvoke(tType, str);

            ExecLog("Init form");

            MatrixFrameBrush = View.GetMatrixFrameDrawing();
            _MatrixPaintService.BindBrush(MatrixFrameBrush);

            var graphNames = _GraphService.GetNames();
            View.BuildGrapbBox(graphNames);

            View.Show();
        }

        private void View_OnSearch(string MethodName)
        {
            //TODO логи

            var map = _MatrixPaintService.Map;
            var graph = _GraphService.CreateGraph(MethodName);
            var mapSize = _MatrixPaintService.MatrixSize.Value;

            for (var y = 0; y < mapSize; ++y) 
            {
                for (var x = 0; x < mapSize; ++x) 
                {
                    var p = new Point3D(x, y);
                    var node = graph.AddNode(p.ToString(), p);
                    var pastX = x - 1;

                    if (x > 0 && map[y, x] != ObjType.Wall && map[y, pastX] != ObjType.Wall) 
                    {
                        var pastP = new Point3D(pastX, y);
                        node.AddNodeDD((g, n) => g[pastP]);
                    }

                    var pastY = y - 1;
                    if (y > 0 && map[y, x] != ObjType.Wall && map[pastY, x] != ObjType.Wall) 
                    {
                        var pastP = new Point3D(x, pastY);
                        node.AddNodeDD((g, n) => g[pastP]);
                    }
                }
            }


            var nodeProc = graph.CreateNodeProcessor();
            var startN = graph[new Point3D(_MatrixPaintService.StartIndex)];
            var finishN = graph[new Point3D(_MatrixPaintService.FinishIndex)];

            var puth = nodeProc.SearchPath(startN, finishN);

            var pointPuth = puth.Select(x => new Point(x.Point.X, x.Point.Y)).ToList();
            _MatrixPaintService.SetPuth(pointPuth);
        }

        private void View_OnFill(ObjTypeV Obj)
        {
            switch (Obj)
            {
                case ObjTypeV.Grass:
                case ObjTypeV.Wall:
                    _MatrixPaintService.Fill((ObjType)Obj);
                    break;
            }
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
