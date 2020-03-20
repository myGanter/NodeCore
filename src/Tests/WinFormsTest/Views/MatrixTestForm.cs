using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsTest.Core.Interfaces.IViews;

namespace WinFormsTest.Views
{
    public partial class MatrixTestForm : Form, IMatrixForm
    {
        public MatrixTestForm()
        {
            InitializeComponent();

            FormClosing += MatrixTestForm_FormClosing;
        }

        private void MatrixTestForm_FormClosing(object sender, FormClosingEventArgs e) => OnClose?.Invoke();

        public event Action OnClose;

        private void StartBtn_Click(object sender, EventArgs e)
        {
            
        }

        public new void Close() 
        {
            OnClose?.Invoke();
            base.Close();
        }
    }
}
