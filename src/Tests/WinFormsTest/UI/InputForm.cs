using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinFormsTest.UI
{
    public static class InputForm
    {
        public static InputForm<T> CreateForm<T>(string WinName, T Model) where T: class => new InputForm<T>(WinName, Model);
    }

    public partial class InputForm<T> : Form where T: class
    {
        public readonly T Model;

        private readonly List<Action> ModelSetter;

        public InputForm(string WinName, T Model)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(WinName))
                Text = WinName;

            ModelSetter = new List<Action>();
            this.Model = Model ?? throw new Exception("Model is null!");

            SetStyle();
            InitFields();
            CreateOkBtn();
        }

        private void SetStyle() 
        {
            Font = new Font("Consolas", 8);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            AutoSize = true;
            Width = 0;
            Height = 0;
        }

        private void CreateOkBtn() 
        {
            var okBtn = new Button();
            okBtn.Text = "Ok";

            var y = ModelSetter.Count * 40 + 30;
            var x = 182 - okBtn.Width / 2;

            okBtn.Location = new Point(x, y);
            okBtn.Visible = true;
            okBtn.Parent = this;

            okBtn.Click += OkBtn_Click;           
        }

        private void InitFields() 
        {
            var startPoint = new Point(10, 10);
            var tType = Model.GetType();
            var props = tType.GetProperties();
            var fields = tType.GetFields();

            var listStrType = typeof(List<string>);
            var boolType = typeof(bool);
            var strType = typeof(string);

            for (var i = 0; i < props.Length; ++i) 
            {
                var prop = props[i];
                var propName = prop.Name;

                if (!TypeConverter.ContainsKey(prop.PropertyType))
                    continue;

                var label = new Label()
                {
                    Text = propName,
                    Location = startPoint + new Size(0, i * 40),
                    Parent = this,
                    Visible = true, 
                    Width = 170
                };

                if (prop.PropertyType == boolType)
                {
                    var chBox = new CheckBox()
                    {
                        Location = startPoint + new Size(label.Width + 10, i * 40),
                        Parent = this,
                        Visible = true
                    };

                    ModelSetter.Add(() => prop.SetValue(Model, chBox.Checked));
                }
                else
                {
                    var fieldName = prop.Name + "s";
                    var listField = fields.FirstOrDefault(x => x.Name == fieldName && x.FieldType == listStrType);

                    if (listField == null || prop.PropertyType != strType)
                    {
                        var textb = new TextBox()
                        {
                            Text = prop.GetValue(Model)?.ToString(),
                            Location = startPoint + new Size(label.Width + 10, i * 40),
                            Parent = this,
                            Visible = true,
                            Width = 175
                        };

                        ModelSetter.Add(() => prop.SetValue(Model, TypeConverter[prop.PropertyType](textb.Text)));
                    }
                    else
                    {
                        var listBox = new ComboBox()
                        {
                            Location = startPoint + new Size(label.Width + 10, i * 40),
                            Parent = this,
                            Visible = true,
                            Width = 175,
                            DropDownStyle = ComboBoxStyle.DropDownList
                        };

                        var vals = (List<string>)listField.GetValue(Model);
                        for (var j = 0; j < vals.Count; ++j)
                        {
                            listBox.Items.Add(vals[j]);
                        }

                        if (listBox.Items.Count > 0)
                            listBox.SelectedIndex = 0;

                        ModelSetter.Add(() => prop.SetValue(Model, listBox.SelectedItem));
                    }
                }
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            ModelSetter.ForEach(x => x());
            Close();
            DialogResult = DialogResult.OK;
        }

        private static Dictionary<Type, Func<string, object>> TypeConverter = new Dictionary<Type, Func<string, object>>
        {
            { typeof(string), (str) => str },
            { typeof(int), (str) => int.TryParse(str, out int val) ? val : 0 },
            { typeof(double), (str) => double.TryParse(str, out double val) ? val : 0 },
            { typeof(float), (str) => float.TryParse(str, out float val) ? val : 0 },

            { typeof(bool), null }
        };
    }
}
