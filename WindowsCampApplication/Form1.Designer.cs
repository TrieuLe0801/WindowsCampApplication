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
            this.orderInforTextBox = new System.Windows.Forms.RichTextBox();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.orderLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.loadFileBtn = new System.Windows.Forms.Button();
            this.campBtn = new System.Windows.Forms.Button();
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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(267, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(495, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "WEB-CAMPING APPLICATON";
            // 
            // orderLabel
            // 
            this.orderLabel.AutoSize = true;
            this.orderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLabel.Location = new System.Drawing.Point(53, 114);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.Size = new System.Drawing.Size(173, 24);
            this.orderLabel.TabIndex = 2;
            this.orderLabel.Text = "Order Information";
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.Location = new System.Drawing.Point(687, 114);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(177, 24);
            this.resultLabel.TabIndex = 2;
            this.resultLabel.Text = "Result Information";
            // 
            // loadFileBtn
            // 
            this.loadFileBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadFileBtn.Location = new System.Drawing.Point(57, 606);
            this.loadFileBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadFileBtn.Name = "loadFileBtn";
            this.loadFileBtn.Size = new System.Drawing.Size(316, 38);
            this.loadFileBtn.TabIndex = 3;
            this.loadFileBtn.Text = "Load File";
            this.loadFileBtn.UseVisualStyleBackColor = true;
            this.loadFileBtn.Click += new System.EventHandler(this.loadFileBtn_Click);
            // 
            // campBtn
            // 
            this.campBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campBtn.Location = new System.Drawing.Point(433, 336);
            this.campBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.campBtn.Name = "campBtn";
            this.campBtn.Size = new System.Drawing.Size(196, 43);
            this.campBtn.TabIndex = 3;
            this.campBtn.Text = "Camping";
            this.campBtn.UseVisualStyleBackColor = true;
            this.campBtn.Click += new System.EventHandler(this.campBtn_Click);
            // 
            // webCampingWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 702);
            this.Controls.Add(this.campBtn);
            this.Controls.Add(this.loadFileBtn);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.orderLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.orderInforTextBox);
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
    }
}

