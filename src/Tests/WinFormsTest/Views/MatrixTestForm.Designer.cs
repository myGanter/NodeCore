namespace WinFormsTest.Views
{
    partial class MatrixTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SizePicker = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ToolPanel = new System.Windows.Forms.Panel();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.FillBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ObjBox = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GraphBox = new System.Windows.Forms.ComboBox();
            this.Canvas = new RazorGDIControlWF.RazorPainterWFCtl();
            ((System.ComponentModel.ISupportInitialize)(this.SizePicker)).BeginInit();
            this.panel1.SuspendLayout();
            this.ToolPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SizePicker
            // 
            this.SizePicker.Font = new System.Drawing.Font("Consolas", 9F);
            this.SizePicker.Location = new System.Drawing.Point(2, 17);
            this.SizePicker.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.SizePicker.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.SizePicker.Name = "SizePicker";
            this.SizePicker.Size = new System.Drawing.Size(69, 22);
            this.SizePicker.TabIndex = 0;
            this.SizePicker.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9F);
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "X | Y syze";
            // 
            // StartBtn
            // 
            this.StartBtn.Font = new System.Drawing.Font("Consolas", 9F);
            this.StartBtn.Location = new System.Drawing.Point(75, 19);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(60, 20);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ToolPanel);
            this.panel1.Controls.Add(this.StartBtn);
            this.panel1.Controls.Add(this.SizePicker);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(11, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 500);
            this.panel1.TabIndex = 0;
            // 
            // ToolPanel
            // 
            this.ToolPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ToolPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolPanel.Controls.Add(this.GraphBox);
            this.ToolPanel.Controls.Add(this.SearchBtn);
            this.ToolPanel.Controls.Add(this.FillBtn);
            this.ToolPanel.Controls.Add(this.label2);
            this.ToolPanel.Controls.Add(this.ObjBox);
            this.ToolPanel.Location = new System.Drawing.Point(2, 45);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.Size = new System.Drawing.Size(133, 451);
            this.ToolPanel.TabIndex = 3;
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(3, 97);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(125, 23);
            this.SearchBtn.TabIndex = 5;
            this.SearchBtn.Text = "Search path";
            this.SearchBtn.UseVisualStyleBackColor = true;
            // 
            // FillBtn
            // 
            this.FillBtn.Location = new System.Drawing.Point(2, 30);
            this.FillBtn.Name = "FillBtn";
            this.FillBtn.Size = new System.Drawing.Size(126, 23);
            this.FillBtn.TabIndex = 4;
            this.FillBtn.Text = "Fill";
            this.FillBtn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9F);
            this.label2.Location = new System.Drawing.Point(4, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Object";
            // 
            // ObjBox
            // 
            this.ObjBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ObjBox.FormattingEnabled = true;
            this.ObjBox.Location = new System.Drawing.Point(59, 3);
            this.ObjBox.Name = "ObjBox";
            this.ObjBox.Size = new System.Drawing.Size(69, 21);
            this.ObjBox.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Canvas);
            this.panel2.Font = new System.Drawing.Font("Consolas", 9F);
            this.panel2.Location = new System.Drawing.Point(157, 10);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(4);
            this.panel2.Size = new System.Drawing.Size(500, 500);
            this.panel2.TabIndex = 1;
            // 
            // GraphBox
            // 
            this.GraphBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GraphBox.FormattingEnabled = true;
            this.GraphBox.Location = new System.Drawing.Point(3, 70);
            this.GraphBox.Name = "GraphBox";
            this.GraphBox.Size = new System.Drawing.Size(125, 21);
            this.GraphBox.TabIndex = 6;
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(4, 4);
            this.Canvas.MinimumSize = new System.Drawing.Size(1, 1);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(490, 490);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            // 
            // MatrixTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 519);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MatrixTestForm";
            this.Text = "Matrix test";
            ((System.ComponentModel.ISupportInitialize)(this.SizePicker)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ToolPanel.ResumeLayout(false);
            this.ToolPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown SizePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private RazorGDIControlWF.RazorPainterWFCtl Canvas;
        private System.Windows.Forms.ComboBox ObjBox;
        private System.Windows.Forms.Panel ToolPanel;
        private System.Windows.Forms.Button FillBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.ComboBox GraphBox;
    }
}