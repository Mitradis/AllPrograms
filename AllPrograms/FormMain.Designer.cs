namespace AllPrograms
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Minimize;
        private System.Windows.Forms.Button button_Widget;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_Minimize = new System.Windows.Forms.Button();
            this.button_Widget = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(522, 127);
            this.label1.TabIndex = 1;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // button_Close
            // 
            this.button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Close.BackgroundImage = global::AllPrograms.Properties.Resources.buttonClose;
            this.button_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Close.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button_Close.FlatAppearance.BorderSize = 0;
            this.button_Close.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlText;
            this.button_Close.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlText;
            this.button_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Close.Location = new System.Drawing.Point(485, 12);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(25, 25);
            this.button_Close.TabIndex = 0;
            this.button_Close.UseVisualStyleBackColor = false;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            this.button_Close.MouseEnter += new System.EventHandler(this.button_Close_MouseEnter);
            this.button_Close.MouseLeave += new System.EventHandler(this.button_Close_MouseLeave);
            // 
            // button_Minimize
            // 
            this.button_Minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Minimize.BackgroundImage = global::AllPrograms.Properties.Resources.buttonMinimize;
            this.button_Minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Minimize.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button_Minimize.FlatAppearance.BorderSize = 0;
            this.button_Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlText;
            this.button_Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlText;
            this.button_Minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Minimize.Location = new System.Drawing.Point(454, 12);
            this.button_Minimize.Name = "button_Minimize";
            this.button_Minimize.Size = new System.Drawing.Size(25, 25);
            this.button_Minimize.TabIndex = 1;
            this.button_Minimize.UseVisualStyleBackColor = false;
            this.button_Minimize.Click += new System.EventHandler(this.button_Minimize_Click);
            this.button_Minimize.MouseEnter += new System.EventHandler(this.button_Minimize_MouseEnter);
            this.button_Minimize.MouseLeave += new System.EventHandler(this.button_Minimize_MouseLeave);
            // 
            // button_Widget
            // 
            this.button_Widget.BackgroundImage = global::AllPrograms.Properties.Resources.buttonWidget;
            this.button_Widget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Widget.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button_Widget.FlatAppearance.BorderSize = 0;
            this.button_Widget.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlText;
            this.button_Widget.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlText;
            this.button_Widget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Widget.Location = new System.Drawing.Point(12, 12);
            this.button_Widget.Name = "button_Widget";
            this.button_Widget.Size = new System.Drawing.Size(25, 25);
            this.button_Widget.TabIndex = 2;
            this.button_Widget.UseVisualStyleBackColor = false;
            this.button_Widget.Click += new System.EventHandler(this.button_Widget_Click);
            this.button_Widget.MouseEnter += new System.EventHandler(this.button_Widget_MouseEnter);
            this.button_Widget.MouseLeave += new System.EventHandler(this.button_Widget_MouseLeave);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdd.Location = new System.Drawing.Point(43, 12);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(25, 25);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "+";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Visible = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDown1.Location = new System.Drawing.Point(74, 14);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(38, 21);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Выберите папку которую хотели бы видеть в качестве ярлыка.";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(93, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Shortcut Manager";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.label2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AllPrograms.Properties.Resources.FormBackgroundNone;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(522, 127);
            this.ControlBox = false;
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.button_Widget);
            this.Controls.Add(this.button_Minimize);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shortcut Manager";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

