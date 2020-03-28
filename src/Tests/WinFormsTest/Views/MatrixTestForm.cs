using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Services;

namespace WinFormsTest.Views
{
    public partial class MatrixTestForm : Form, IMatrixForm
    {
        public event Action OnClose;
        public event Action<uint> OnStartBtn;
        public event Action OnReDraw;
        public event Action OnPostShow;

        private readonly RazorMatrixBrush _PicBoxPainter;

        public MatrixTestForm(RazorMatrixBrush PicBoxPainter)
        {
            InitializeComponent();

            Shown += MatrixTestForm_Shown;
            FormClosing += MatrixTestForm_FormClosing;

            _PicBoxPainter = PicBoxPainter;
            _PicBoxPainter.BindPicBox(Canvas);

            Canvas.Resize += Canvas_Resize;
        }

        private void MatrixTestForm_Shown(object sender, EventArgs e)
        {
            OnPostShow?.Invoke();
        }

        public new void Close() 
        {
            OnClose?.Invoke();
            base.Close();
        }

        public IMatrixFrameBrush GetMatrixFrameDrawing()
        {
            return _PicBoxPainter;
        }

        private void Canvas_Resize(object sender, EventArgs e)
        {
            OnReDraw?.Invoke();
        }
        private void MatrixTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClose?.Invoke();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            var value = (uint)SizePicker.Value;
            OnStartBtn?.Invoke(value);
        }
    }
}
