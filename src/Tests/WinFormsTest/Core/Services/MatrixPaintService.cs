﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using WinFormsTest.Core.Interfaces;

namespace WinFormsTest.Core.Services
{
    public class MatrixPaintService
    {
        public uint? MatrixSize { get; private set; }

        public bool SizeInit { get => MatrixSize != null; }

        private IMatrixFrameBrush MatrixFrameBrush { get; set; }

        private Bitmap WallTexture { get; set; }
        private Bitmap ConvertWallTexture { get; set; }

        private Bitmap GrassTexture { get; set; }
        private Bitmap ConvertGrassTexture { get; set; }

        private Bitmap StartTexture { get; set; }
        private Bitmap ConvertStartTexture { get; set; }

        private Bitmap FinishTexture { get; set; }
        private Bitmap ConvertFinishTexture { get; set; }

        private ObjType[,] Map { get; set; }

        private int CellH { get; set; }

        private int CellW { get; set; }

        private Point StartIndex { get; set; }

        private Point FinishIndex { get; set; }

        public void BindBrush(IMatrixFrameBrush MatrixFrameBrush) 
        {
            this.MatrixFrameBrush = MatrixFrameBrush;
        }

        public void Init(uint MatrixSize) 
        {
            this.MatrixSize = MatrixSize;

            StartIndex = new Point();
            var finishindex = (int)(MatrixSize - 1);
            FinishIndex = new Point(finishindex, finishindex);

            InitTextures();
            InitMap();
            DrawFrame();            
        }

        public void DrawObj(ObjType Obj, Point P) 
        {
            var cx = P.X / CellW;
            var cy = P.Y / CellH;

            if (cx > (MatrixSize - 1) || cy > (MatrixSize - 1) || cx < 0 || cy < 0)
                return;       

            Map[cy, cx] = Obj;

            var chIndex = new Point(cx, cy);
            if (chIndex == StartIndex || chIndex == FinishIndex)
            {
                RedrawMap();
            }
            else
            {
                cx *= CellW;
                cy *= CellH;

                DrawObj(Obj, cx, cy);

                MatrixFrameBrush.DrawBoof();
            }
        }

        public void DrawStart(Point P) 
        {
            var cx = P.X / CellW;
            var cy = P.Y / CellH;
            var si = new Point(cx, cy);

            if (cx > (MatrixSize - 1) || cy > (MatrixSize - 1) || cx < 0 || cy < 0 || si == FinishIndex)
                return;

            StartIndex = si;

            RedrawMap();
        }

        public void DrawFinish(Point P)
        {
            var cx = P.X / CellW;
            var cy = P.Y / CellH;
            var si = new Point(cx, cy);

            if (cx > (MatrixSize - 1) || cy > (MatrixSize - 1) || cx < 0 || cy < 0 || si == StartIndex)
                return;

            FinishIndex = si;

            RedrawMap();
        }

        public void DrawFrame() 
        {
            if (!InitConvertTextures())
                return;

            RedrawMap();            
        }

        private void RedrawMap() 
        {
            var mSizeValue = (int)MatrixSize.Value;

            MatrixFrameBrush.Clear(Color.FromArgb(255, 62, 62, 64));

            for (var y = 0; y < mSizeValue; ++y)
            {
                var cy = y * CellH;

                for (var x = 0; x < mSizeValue; ++x)
                {
                    var cx = x * CellW;

                    DrawObj(Map[y, x], cx, cy);
                }
            }

            var div4W = (int)Math.Ceiling((CellW - CellW / 1.2f) / 2);
            var div4H = (int)Math.Ceiling((CellH - CellH / 1.2f) / 2);

            var finishI = FinishIndex;
            MatrixFrameBrush.DrawImage(ConvertFinishTexture, finishI.X * CellW + div4W, finishI.Y * CellH + div4H);

            var startI = StartIndex;
            MatrixFrameBrush.DrawImage(ConvertStartTexture, startI.X * CellW + div4W, startI.Y * CellH + div4H);

            MatrixFrameBrush.DrawBoof();
        }

        private void DrawObj(ObjType Obj, int X, int Y) 
        {
            switch (Obj)
            {
                case ObjType.Grass:
                    MatrixFrameBrush.DrawImage(ConvertGrassTexture, X, Y);
                    break;
                case ObjType.Wall:
                    MatrixFrameBrush.DrawImage(ConvertWallTexture, X, Y);
                    break;
            }
        }

        private void InitTextures() 
        {
            if (WallTexture != null)
                WallTexture.Dispose();
            if (GrassTexture != null)
                GrassTexture.Dispose();
            if (StartTexture != null)
                StartTexture.Dispose();
            if (FinishTexture != null)
                FinishTexture.Dispose();

            WallTexture = new Bitmap(@"Textures\wall.jpg");
            GrassTexture = new Bitmap(@"Textures\gross.jpg");
            StartTexture = new Bitmap(@"Textures\start.jpg");
            FinishTexture = new Bitmap(@"Textures\finish.jpg");
        }

        private bool InitConvertTextures() 
        {
            if (ConvertWallTexture != null)
                ConvertWallTexture.Dispose();
            if (ConvertGrassTexture != null)
                ConvertGrassTexture.Dispose();
            if (ConvertStartTexture != null)
                ConvertStartTexture.Dispose();
            if (ConvertFinishTexture != null)
                ConvertFinishTexture.Dispose();

            var mSizeValue = (int)MatrixSize.Value;
            var canvasSize = MatrixFrameBrush.GetCanvasSize();
            CellW = canvasSize.Width / mSizeValue;
            CellH = canvasSize.Height / mSizeValue;

            if (CellW == 0 || CellH == 0)
                return false;

            ConvertGrassTexture = new Bitmap(GrassTexture, CellW, CellH);
            ConvertWallTexture = new Bitmap(WallTexture, CellW, CellH);

            var div2W = (int)Math.Ceiling(CellW / 1.2f);
            var div2H = (int)Math.Ceiling(CellH / 1.2f);
            ConvertStartTexture = new Bitmap(StartTexture, div2W, div2H);
            ConvertFinishTexture = new Bitmap(FinishTexture, div2W, div2H);

            return true;
        }

        private void InitMap() 
        {
            var l = (int)MatrixSize;
            Map = new ObjType[l, l];
        }
    }

    public enum ObjType
    {
        Grass,
        Wall
    }
}
