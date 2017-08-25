namespace InrixConfigurationTool
{
    partial class EditSegment
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
            this.uxUpdateNewSegmentButton = new System.Windows.Forms.Button();
            this.uxNewSegmentDescriptionText = new System.Windows.Forms.TextBox();
            this.uxNewSegmentNameText = new System.Windows.Forms.TextBox();
            this.uxCloseButton = new System.Windows.Forms.Button();
            this.SegDescLabel = new System.Windows.Forms.Label();
            this.SegNameLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxUpdateNewSegmentButton
            // 
            this.uxUpdateNewSegmentButton.Location = new System.Drawing.Point(12, 222);
            this.uxUpdateNewSegmentButton.Name = "uxUpdateNewSegmentButton";
            this.uxUpdateNewSegmentButton.Size = new System.Drawing.Size(75, 31);
            this.uxUpdateNewSegmentButton.TabIndex = 23;
            this.uxUpdateNewSegmentButton.Text = "OK";
            this.uxUpdateNewSegmentButton.UseVisualStyleBackColor = true;
            this.uxUpdateNewSegmentButton.Click += new System.EventHandler(this.uxUpdateNewSegmentButton_Click);
            // 
            // uxNewSegmentDescriptionText
            // 
            this.uxNewSegmentDescriptionText.Location = new System.Drawing.Point(12, 87);
            this.uxNewSegmentDescriptionText.Multiline = true;
            this.uxNewSegmentDescriptionText.Name = "uxNewSegmentDescriptionText";
            this.uxNewSegmentDescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxNewSegmentDescriptionText.Size = new System.Drawing.Size(268, 118);
            this.uxNewSegmentDescriptionText.TabIndex = 22;
            // 
            // uxNewSegmentNameText
            // 
            this.uxNewSegmentNameText.Location = new System.Drawing.Point(12, 36);
            this.uxNewSegmentNameText.Name = "uxNewSegmentNameText";
            this.uxNewSegmentNameText.Size = new System.Drawing.Size(214, 22);
            this.uxNewSegmentNameText.TabIndex = 21;
            // 
            // uxCloseButton
            // 
            this.uxCloseButton.Location = new System.Drawing.Point(205, 222);
            this.uxCloseButton.Name = "uxCloseButton";
            this.uxCloseButton.Size = new System.Drawing.Size(75, 31);
            this.uxCloseButton.TabIndex = 18;
            this.uxCloseButton.Text = "Close";
            this.uxCloseButton.UseVisualStyleBackColor = true;
            this.uxCloseButton.Click += new System.EventHandler(this.uxCloseButton_Click);
            // 
            // SegDescLabel
            // 
            this.SegDescLabel.AutoSize = true;
            this.SegDescLabel.Location = new System.Drawing.Point(12, 67);
            this.SegDescLabel.Name = "SegDescLabel";
            this.SegDescLabel.Size = new System.Drawing.Size(110, 17);
            this.SegDescLabel.TabIndex = 20;
            this.SegDescLabel.Text = "New Description";
            // 
            // SegNameLable
            // 
            this.SegNameLable.AutoSize = true;
            this.SegNameLable.Location = new System.Drawing.Point(12, 16);
            this.SegNameLable.Name = "SegNameLable";
            this.SegNameLable.Size = new System.Drawing.Size(136, 17);
            this.SegNameLable.TabIndex = 19;
            this.SegNameLable.Text = "New Segment Name";
            // 
            // EditSegment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.uxUpdateNewSegmentButton);
            this.Controls.Add(this.uxNewSegmentDescriptionText);
            this.Controls.Add(this.uxNewSegmentNameText);
            this.Controls.Add(this.uxCloseButton);
            this.Controls.Add(this.SegDescLabel);
            this.Controls.Add(this.SegNameLable);
            this.Name = "EditSegment";
            this.Text = "EditSegment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxUpdateNewSegmentButton;
        private System.Windows.Forms.TextBox uxNewSegmentDescriptionText;
        private System.Windows.Forms.TextBox uxNewSegmentNameText;
        private System.Windows.Forms.Button uxCloseButton;
        private System.Windows.Forms.Label SegDescLabel;
        private System.Windows.Forms.Label SegNameLable;
    }
}