using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
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
            StartBtn.Click += StartBtn_Click;
        }

        public event Func<Type, Task> Start;

        public void BuildTests(List<Tuple<Type, string>> Data)
        {
            TestsCombo.DataSource = Data;
            TestsCombo.DisplayMember = "Item2";
            TestsCombo.ValueMember = "Item1";
        }

        public void Log(string LogText, Color HColor)
        {
            Invoke(new Action(
                () => 
                {
                    var headL = LogText.Split(Environment.NewLine)[0].Length;
                    var pastL = LogVue.Text.Length;

                    LogVue.AppendText(LogText);

                    LogVue.Select(pastL, headL);
                    LogVue.SelectionColor = HColor;
                    LogVue.SelectionFont = new Font(LogVue.SelectionFont.Name, 10f, LogVue.SelectionFont.Style);

                    LogVue.SelectionStart = LogVue.Text.Length;
                    LogVue.ScrollToCaret();
                }));            
        }

        public new void Show()
        {
            Application.Run(this);
        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            StartBtn.Enabled = false;
            var type = TestsCombo.SelectedValue;
            if (type != null)
            {
                await Start?.Invoke((Type)type);
                StartBtn.Enabled = true;
            }
        }
    }
}
