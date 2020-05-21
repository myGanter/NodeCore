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
using WinFormsTest.Models;

namespace WinFormsTest.Views
{
    public partial class MatrixTestForm : Form, IMatrixForm
    {
        public event Action OnClose;
        public event Action<uint> OnStartBtn;
        public event Action OnReDraw;
        public event Action OnPostShow;
        public event Action<ObjTypeV, Point> OnCanvasClick;
        public event Action<ObjTypeV> OnFill;
        public event Action<string> OnSearch;

        private readonly RazorMatrixBrush _PicBoxPainter;

        public MatrixTestForm(RazorMatrixBrush PicBoxPainter)
        {
            InitializeComponent();
            InitObjBoxValues();
            ToolPanel.Visible = false;

            Shown += MatrixTestForm_Shown;
            FormClosing += MatrixTestForm_FormClosing;
            SearchBtn.Click += SearchBtn_Click; FillBtn.Click += FillBtn_Click;

            _PicBoxPainter = PicBoxPainter;
            _PicBoxPainter.BindPicBox(Canvas);

            Canvas.Resize += Canvas_Resize;
            Canvas.MouseDown += Canvas_MouseDown;
            Canvas.MouseMove += Canvas_MouseMove;
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

        public void ShowToolPanel() 
        {
            ToolPanel.Visible = true;
        }

        public void BuildGrapbBox(List<string> Objs) 
        {
            GraphBox.DataSource = Objs;
        }

        private void InitObjBoxValues()
        {
            ObjBox.Items.Add(ObjTypeV.Grass);
            ObjBox.Items.Add(ObjTypeV.Wall);
            ObjBox.Items.Add(ObjTypeV.Water);
            ObjBox.Items.Add(ObjTypeV.Start);
            ObjBox.Items.Add(ObjTypeV.Finish);

            ObjBox.SelectedIndex = 1;
        }

        private bool GetObjType(out ObjTypeV ObjType) 
        {
            ObjType = default;

            if (ObjBox.SelectedItem == null)
                return false;

            ObjType = (ObjTypeV)ObjBox.SelectedItem;
            return true;
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (GetObjType(out ObjTypeV selectedObj)) 
            {
                OnCanvasClick?.Invoke(selectedObj, e.Location);
            }            
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                Canvas_MouseDown(sender, e);
            }
        }

        private void MatrixTestForm_Shown(object sender, EventArgs e)
        {
            OnPostShow?.Invoke();
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

        private void FillBtn_Click(object sender, EventArgs e)
        {
            if (GetObjType(out ObjTypeV selectedObj))
            {
                OnFill?.Invoke(selectedObj);
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            if (GraphBox.SelectedItem == null)
                return;

            var methodName = (string)GraphBox.SelectedItem;

            OnSearch?.Invoke(methodName);
        }
    }
}
