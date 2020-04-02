using RazorGDIControlWF;
using RazorGDIPainter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Interfaces.IViews;

namespace WinFormsTest.Core.Services
{
    public class RazorMatrixBrush : IMatrixFrameBrush
    {
        private RazorPainterWFCtl PB;

        public void BindPicBox(RazorPainterWFCtl PB) 
        {
            this.PB = PB;
        }
        public Size GetCanvasSize()
        {
            return PB.Size;
        }

        public void DrawImage(Image Image, int X, int Y, int Width, int Height)
        {
            PB.RazorGFX.DrawImage(Image, X, Y, Width, Height);
        }

        public void DrawImage(Image Image, int X, int Y)
        {
            PB.RazorGFX.DrawImage(Image, X, Y);
        }

        public void FillRectangle(Color Col, int X, int Y, int W, int H) 
        {
            PB.RazorGFX.FillRectangle(new SolidBrush(Col), X, Y, W, H);
        }

        public void Clear(Color Col)
        {
            PB.RazorGFX.Clear(Col);
        }

        public void DrawBoof()
        {
            PB.RazorPaint();
        }
    }
}
