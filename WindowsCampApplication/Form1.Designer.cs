namespace WindowsCampApplication
{
    partial class webCampingWindows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(webCampingWindows));
            this.orderInforTextBox = new System.Windows.Forms.RichTextBox();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.orderLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.loadFileBtn = new System.Windows.Forms.Button();
            this.campBtn = new System.Windows.Forms.Button();
            this.headlessCheckbox = new System.Windows.Forms.CheckBox();
            this.tabBox = new System.Windows.Forms.TextBox();
            this.tabLb = new System.Windows.Forms.Label();
            this.stopBtn = new System.Windows.Forms.Button();
            this.clear_btn = new System.Windows.Forms.Button();
            this.timerLb = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // orderInforTextBox
            // 
            this.orderInforTextBox.Location = new System.Drawing.Point(56, 153);
            this.orderInforTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.orderInforTextBox.Name = "orderInforTextBox";
            this.orderInforTextBox.Size = new System.Drawing.Size(317, 432);
            this.orderInforTextBox.TabIndex = 0;
            this.orderInforTextBox.Text = "";
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(691, 153);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(317, 432);
            this.resultTextBox.TabIndex = 0;
            this.resultTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(267, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(470, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "WEB-CAMPING APPLICATON";
            // 
            // orderLabel
            // 
            this.orderLabel.AutoSize = true;
            this.orderLabel.BackColor = System.Drawing.Color.Black;
            this.orderLabel.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLabel.ForeColor = System.Drawing.Color.Transparent;
            this.orderLabel.Location = new System.Drawing.Point(53, 114);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.Size = new System.Drawing.Size(177, 25);
            this.orderLabel.TabIndex = 2;
            this.orderLabel.Text = "Order Information";
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.BackColor = System.Drawing.Color.Black;
            this.resultLabel.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.ForeColor = System.Drawing.Color.Transparent;
            this.resultLabel.Location = new System.Drawing.Point(687, 114);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(179, 25);
            this.resultLabel.TabIndex = 2;
            this.resultLabel.Text = "Result Information";
            // 
            // loadFileBtn
            // 
            this.loadFileBtn.BackColor = System.Drawing.Color.DimGray;
            this.loadFileBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadFileBtn.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.loadFileBtn.Location = new System.Drawing.Point(57, 606);
            this.loadFileBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadFileBtn.Name = "loadFileBtn";
            this.loadFileBtn.Size = new System.Drawing.Size(316, 38);
            this.loadFileBtn.TabIndex = 3;
            this.loadFileBtn.Text = "Load File";
            this.loadFileBtn.UseVisualStyleBackColor = false;
            this.loadFileBtn.Click += new System.EventHandler(this.loadFileBtn_Click);
            // 
            // campBtn
            // 
            this.campBtn.BackColor = System.Drawing.Color.DimGray;
            this.campBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campBtn.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.campBtn.Location = new System.Drawing.Point(433, 336);
            this.campBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.campBtn.Name = "campBtn";
            this.campBtn.Size = new System.Drawing.Size(196, 43);
            this.campBtn.TabIndex = 3;
            this.campBtn.Text = "Camping";
            this.campBtn.UseVisualStyleBackColor = false;
            this.campBtn.Click += new System.EventHandler(this.campBtn_Click);
            // 
            // headlessCheckbox
            // 
            this.headlessCheckbox.AutoSize = true;
            this.headlessCheckbox.BackColor = System.Drawing.Color.Transparent;
            this.headlessCheckbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headlessCheckbox.ForeColor = System.Drawing.Color.Transparent;
            this.headlessCheckbox.Location = new System.Drawing.Point(485, 399);
            this.headlessCheckbox.Name = "headlessCheckbox";
            this.headlessCheckbox.Size = new System.Drawing.Size(102, 27);
            this.headlessCheckbox.TabIndex = 4;
            this.headlessCheckbox.Text = "Headless";
            this.headlessCheckbox.UseVisualStyleBackColor = false;
            this.headlessCheckbox.CheckedChanged += new System.EventHandler(this.headlessCheckbox_CheckedChanged);
            // 
            // tabBox
            // 
            this.tabBox.BackColor = System.Drawing.Color.Black;
            this.tabBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabBox.ForeColor = System.Drawing.Color.Transparent;
            this.tabBox.Location = new System.Drawing.Point(581, 288);
            this.tabBox.Name = "tabBox";
            this.tabBox.Size = new System.Drawing.Size(48, 30);
            this.tabBox.TabIndex = 5;
            this.tabBox.Text = "3";
            this.tabBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabLb
            // 
            this.tabLb.AutoSize = true;
            this.tabLb.BackColor = System.Drawing.Color.Transparent;
            this.tabLb.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabLb.ForeColor = System.Drawing.Color.Transparent;
            this.tabLb.Location = new System.Drawing.Point(429, 291);
            this.tabLb.Name = "tabLb";
            this.tabLb.Size = new System.Drawing.Size(123, 23);
            this.tabLb.TabIndex = 6;
            this.tabLb.Text = "Launched TAB";
            // 
            // stopBtn
            // 
            this.stopBtn.BackColor = System.Drawing.Color.DimGray;
            this.stopBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopBtn.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.stopBtn.Location = new System.Drawing.Point(433, 446);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(196, 48);
            this.stopBtn.TabIndex = 7;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // clear_btn
            // 
            this.clear_btn.BackColor = System.Drawing.Color.DimGray;
            this.clear_btn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear_btn.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.clear_btn.Location = new System.Drawing.Point(691, 606);
            this.clear_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clear_btn.Name = "clear_btn";
            this.clear_btn.Size = new System.Drawing.Size(316, 38);
            this.clear_btn.TabIndex = 8;
            this.clear_btn.Text = "Clear";
            this.clear_btn.UseVisualStyleBackColor = false;
            this.clear_btn.Click += new System.EventHandler(this.clear_btn_Click);
            // 
            // timerLb
            // 
            this.timerLb.AutoSize = true;
            this.timerLb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.timerLb.Font = new System.Drawing.Font("Segoe UI", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerLb.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.timerLb.Location = new System.Drawing.Point(430, 207);
            this.timerLb.Name = "timerLb";
            this.timerLb.Size = new System.Drawing.Size(199, 59);
            this.timerLb.TabIndex = 10;
            this.timerLb.Text = "00:00:00";
            this.timerLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.button1.Location = new System.Drawing.Point(433, 559);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(196, 48);
            this.button1.TabIndex = 11;
            this.button1.Text = "Log Out";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // webCampingWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImage = global::WindowsCampApplication.Properties.Resources._78a45eb3fc20ad8c5116930a112ffdb1;
            this.ClientSize = new System.Drawing.Size(1061, 702);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.timerLb);
            this.Controls.Add(this.clear_btn);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.tabLb);
            this.Controls.Add(this.tabBox);
            this.Controls.Add(this.headlessCheckbox);
            this.Controls.Add(this.campBtn);
            this.Controls.Add(this.loadFileBtn);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.orderLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.orderInforTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "webCampingWindows";
            this.Text = "web-camping";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox orderInforTextBox;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label orderLabel;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Button loadFileBtn;
        private System.Windows.Forms.Button campBtn;
        private System.Windows.Forms.CheckBox headlessCheckbox;
        private System.Windows.Forms.TextBox tabBox;
        private System.Windows.Forms.Label tabLb;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button clear_btn;
        private System.Windows.Forms.Label timerLb;
        private System.Windows.Forms.Button button1;
    }
}

