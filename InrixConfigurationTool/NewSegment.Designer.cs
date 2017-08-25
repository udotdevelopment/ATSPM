namespace InrixConfigurationTool
{
    partial class NewSegment
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
            this.uxSaveNewSegmentButton = new System.Windows.Forms.Button();
            this.uxNewSegmentDescriptionText = new System.Windows.Forms.TextBox();
            this.uxNewSegmentNameText = new System.Windows.Forms.TextBox();
            this.uxCloseButton = new System.Windows.Forms.Button();
            this.RouteDescLabel = new System.Windows.Forms.Label();
            this.RouteNameLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxSaveNewSegmentButton
            // 
            this.uxSaveNewSegmentButton.Location = new System.Drawing.Point(12, 222);
            this.uxSaveNewSegmentButton.Name = "uxSaveNewSegmentButton";
            this.uxSaveNewSegmentButton.Size = new System.Drawing.Size(75, 31);
            this.uxSaveNewSegmentButton.TabIndex = 17;
            this.uxSaveNewSegmentButton.Text = "OK";
            this.uxSaveNewSegmentButton.UseVisualStyleBackColor = true;
            this.uxSaveNewSegmentButton.Click += new System.EventHandler(this.uxSaveNewSegmentButton_Click);
            // 
            // uxNewSegmentDescriptionText
            // 
            this.uxNewSegmentDescriptionText.Location = new System.Drawing.Point(12, 87);
            this.uxNewSegmentDescriptionText.Multiline = true;
            this.uxNewSegmentDescriptionText.Name = "uxNewSegmentDescriptionText";
            this.uxNewSegmentDescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxNewSegmentDescriptionText.Size = new System.Drawing.Size(268, 118);
            this.uxNewSegmentDescriptionText.TabIndex = 16;
            // 
            // uxNewSegmentNameText
            // 
            this.uxNewSegmentNameText.Location = new System.Drawing.Point(12, 36);
            this.uxNewSegmentNameText.Name = "uxNewSegmentNameText";
            this.uxNewSegmentNameText.Size = new System.Drawing.Size(214, 22);
            this.uxNewSegmentNameText.TabIndex = 15;
            // 
            // uxCloseButton
            // 
            this.uxCloseButton.Location = new System.Drawing.Point(205, 222);
            this.uxCloseButton.Name = "uxCloseButton";
            this.uxCloseButton.Size = new System.Drawing.Size(75, 31);
            this.uxCloseButton.TabIndex = 12;
            this.uxCloseButton.Text = "Close";
            this.uxCloseButton.UseVisualStyleBackColor = true;
            this.uxCloseButton.Click += new System.EventHandler(this.uxCloseButton_Click);
            // 
            // RouteDescLabel
            // 
            this.RouteDescLabel.AutoSize = true;
            this.RouteDescLabel.Location = new System.Drawing.Point(12, 67);
            this.RouteDescLabel.Name = "RouteDescLabel";
            this.RouteDescLabel.Size = new System.Drawing.Size(79, 17);
            this.RouteDescLabel.TabIndex = 14;
            this.RouteDescLabel.Text = "Description";
            // 
            // RouteNameLable
            // 
            this.RouteNameLable.AutoSize = true;
            this.RouteNameLable.Location = new System.Drawing.Point(12, 16);
            this.RouteNameLable.Name = "RouteNameLable";
            this.RouteNameLable.Size = new System.Drawing.Size(105, 17);
            this.RouteNameLable.TabIndex = 13;
            this.RouteNameLable.Text = "Segment Name";
            // 
            // NewSegment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.uxSaveNewSegmentButton);
            this.Controls.Add(this.uxNewSegmentDescriptionText);
            this.Controls.Add(this.uxNewSegmentNameText);
            this.Controls.Add(this.uxCloseButton);
            this.Controls.Add(this.RouteDescLabel);
            this.Controls.Add(this.RouteNameLable);
            this.Name = "NewSegment";
            this.Text = "NewSegment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxSaveNewSegmentButton;
        private System.Windows.Forms.TextBox uxNewSegmentDescriptionText;
        private System.Windows.Forms.TextBox uxNewSegmentNameText;
        private System.Windows.Forms.Button uxCloseButton;
        private System.Windows.Forms.Label RouteDescLabel;
        private System.Windows.Forms.Label RouteNameLable;
    }
}