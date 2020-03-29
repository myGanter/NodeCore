using System;
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
        private Bitmap GrassTexture { get; set; }
        private Bitmap StartTexture { get; set; }
        private Bitmap FinishTexture { get; set; }

        private ObjType[,] Map { get; set; }

        public void BindBrush(IMatrixFrameBrush MatrixFrameBrush) 
        {
            this.MatrixFrameBrush = MatrixFrameBrush;
        }

        public void Init(uint MatrixSize) 
        {
            this.MatrixSize = MatrixSize;
            InitTextures();
            InitMap();
            DrawFrame();
        }

        public void DrawObj(ObjType Obj, Point P) 
        {
            var mSizeValue = (int)MatrixSize.Value;
            var canvasSize = MatrixFrameBrush.GetCanvasSize();
            var cellW = canvasSize.Width / mSizeValue;
            var cellH = canvasSize.Height / mSizeValue;

            if (cellW == 0 || cellH == 0)
                return;

            var cx = P.X / cellW;
            var cy = P.Y / cellH;

            if (cx > (MatrixSize - 1) || cy > (MatrixSize - 1) || cx < 0 || cy < 0)
                return;

            var newGrassT = new Bitmap(GrassTexture, cellW, cellH);
            var newWallT = new Bitmap(WallTexture, cellW, cellH);            

            Map[cy, cx] = Obj;

            cx *= cellW;
            cy *= cellH;

            switch (Obj)
            {
                case ObjType.Grass:
                    MatrixFrameBrush.DrawImage(newGrassT, cx, cy);
                    break;
                case ObjType.Wall:
                    MatrixFrameBrush.DrawImage(newWallT, cx, cy);
                    break;
            }

            newGrassT.Dispose();
            newWallT.Dispose();

            MatrixFrameBrush.DrawBoof();
        }

        public void DrawFrame() 
        {
            var mSizeValue = (int)MatrixSize.Value;
            var canvasSize = MatrixFrameBrush.GetCanvasSize();
            var cellW = canvasSize.Width / mSizeValue;
            var cellH = canvasSize.Height / mSizeValue;

            if (cellW == 0 || cellH == 0)
                return;

            var newGrassT = new Bitmap(GrassTexture, cellW, cellH);
            var newWallT = new Bitmap(WallTexture, cellW, cellH);

            MatrixFrameBrush.Clear(Color.FromArgb(255, 62, 62, 64));

            for (var y = 0; y < mSizeValue; ++y) 
            {
                var cy = y * cellH;

                for (var x = 0; x < mSizeValue; ++x) 
                {
                    var cx = x * cellW;                                      

                    switch (Map[y, x]) 
                    {
                        case ObjType.Grass:
                            MatrixFrameBrush.DrawImage(newGrassT, cx, cy);
                            break;
                        case ObjType.Wall:
                            MatrixFrameBrush.DrawImage(newWallT, cx, cy);
                            break;
                    }
                }
            }

            newGrassT.Dispose();
            newWallT.Dispose();

            MatrixFrameBrush.DrawBoof();
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

            StartTexture = new Bitmap(50, 50);
            var g = Graphics.FromImage(StartTexture);
            g.Clear(Color.Red);
            g.Dispose();

            FinishTexture = new Bitmap(50, 50);
            g = Graphics.FromImage(FinishTexture);
            g.Clear(Color.Black);
            g.Dispose();
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
