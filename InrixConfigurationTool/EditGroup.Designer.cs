namespace InrixConfigurationTool
{
    partial class EditGroup
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
            this.uxUpdateGroupButton = new System.Windows.Forms.Button();
            this.uxNewGroupDescriptionText = new System.Windows.Forms.TextBox();
            this.uxNewGroupNameText = new System.Windows.Forms.TextBox();
            this.uxCloseButton = new System.Windows.Forms.Button();
            this.RouteDescLabel = new System.Windows.Forms.Label();
            this.RouteNameLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxUpdateGroupButton
            // 
            this.uxUpdateGroupButton.Location = new System.Drawing.Point(12, 222);
            this.uxUpdateGroupButton.Name = "uxUpdateGroupButton";
            this.uxUpdateGroupButton.Size = new System.Drawing.Size(75, 31);
            this.uxUpdateGroupButton.TabIndex = 23;
            this.uxUpdateGroupButton.Text = "OK";
            this.uxUpdateGroupButton.UseVisualStyleBackColor = true;
            this.uxUpdateGroupButton.Click += new System.EventHandler(this.uxUpdateGroupButton_Click);
            // 
            // uxNewGroupDescriptionText
            // 
            this.uxNewGroupDescriptionText.Location = new System.Drawing.Point(12, 87);
            this.uxNewGroupDescriptionText.Multiline = true;
            this.uxNewGroupDescriptionText.Name = "uxNewGroupDescriptionText";
            this.uxNewGroupDescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxNewGroupDescriptionText.Size = new System.Drawing.Size(268, 118);
            this.uxNewGroupDescriptionText.TabIndex = 22;
            // 
            // uxNewGroupNameText
            // 
            this.uxNewGroupNameText.Location = new System.Drawing.Point(12, 36);
            this.uxNewGroupNameText.Name = "uxNewGroupNameText";
            this.uxNewGroupNameText.Size = new System.Drawing.Size(214, 22);
            this.uxNewGroupNameText.TabIndex = 21;
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
            // RouteDescLabel
            // 
            this.RouteDescLabel.AutoSize = true;
            this.RouteDescLabel.Location = new System.Drawing.Point(12, 67);
            this.RouteDescLabel.Name = "RouteDescLabel";
            this.RouteDescLabel.Size = new System.Drawing.Size(110, 17);
            this.RouteDescLabel.TabIndex = 20;
            this.RouteDescLabel.Text = "New Description";
            // 
            // RouteNameLable
            // 
            this.RouteNameLable.AutoSize = true;
            this.RouteNameLable.Location = new System.Drawing.Point(12, 16);
            this.RouteNameLable.Name = "RouteNameLable";
            this.RouteNameLable.Size = new System.Drawing.Size(120, 17);
            this.RouteNameLable.TabIndex = 19;
            this.RouteNameLable.Text = "New Group Name";
            // 
            // EditGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.uxUpdateGroupButton);
            this.Controls.Add(this.uxNewGroupDescriptionText);
            this.Controls.Add(this.uxNewGroupNameText);
            this.Controls.Add(this.uxCloseButton);
            this.Controls.Add(this.RouteDescLabel);
            this.Controls.Add(this.RouteNameLable);
            this.Name = "EditGroup";
            this.Text = "EditGroup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxUpdateGroupButton;
        private System.Windows.Forms.TextBox uxNewGroupDescriptionText;
        private System.Windows.Forms.TextBox uxNewGroupNameText;
        private System.Windows.Forms.Button uxCloseButton;
        private System.Windows.Forms.Label RouteDescLabel;
        private System.Windows.Forms.Label RouteNameLable;
    }
}