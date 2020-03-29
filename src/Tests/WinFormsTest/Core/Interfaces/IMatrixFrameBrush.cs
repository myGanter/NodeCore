using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinFormsTest.Core.Interfaces
{
    public interface IMatrixFrameBrush
    {
        Size GetCanvasSize();

        void DrawImage(Image Image, int X, int Y);

        void DrawImage(Image Image, int X, int Y, int Width, int Height);

        void Clear(Color Col);

        void DrawBoof();
    }
}
