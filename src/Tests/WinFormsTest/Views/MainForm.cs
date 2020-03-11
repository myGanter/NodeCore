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
    public partial class MainForm : Form, ImainForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public new void Show() 
        {
            Application.Run(this);
        }
    }
}
