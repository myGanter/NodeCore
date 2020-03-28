using System;
using System.Collections.Generic;
using System.Text;
using WinFormsTest.Core.Interfaces;

namespace WinFormsTest.Core.Services
{
    public class MatrixPaintService
    {
        public uint? MatrixSize { get; private set; }

        public bool SizeInit { get => MatrixSize != null; }

        private IMatrixFrameBrush MatrixFrameBrush { get; set; }

        public void BindBrush(IMatrixFrameBrush MatrixFrameBrush) 
        {
            this.MatrixFrameBrush = MatrixFrameBrush;
        }

        public void Init(uint MatrixSize) 
        {
            this.MatrixSize = MatrixSize;
        }
    }
}
