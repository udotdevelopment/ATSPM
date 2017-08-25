namespace InrixConfigurationTool
{
    partial class NewGroup
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
            this.uxCloseButton = new System.Windows.Forms.Button();
            this.GroupDescLabel = new System.Windows.Forms.Label();
            this.GroupNameLable = new System.Windows.Forms.Label();
            this.uxGroupDescriptionText = new System.Windows.Forms.TextBox();
            this.uxSaveNewGroupButton = new System.Windows.Forms.Button();
            this.uxGroupNameText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // uxCloseButton
            // 
            this.uxCloseButton.Location = new System.Drawing.Point(205, 222);
            this.uxCloseButton.Name = "uxCloseButton";
            this.uxCloseButton.Size = new System.Drawing.Size(75, 31);
            this.uxCloseButton.TabIndex = 9;
            this.uxCloseButton.Text = "Close";
            this.uxCloseButton.UseVisualStyleBackColor = true;
            this.uxCloseButton.Click += new System.EventHandler(this.uxCloseButton_Click);
            // 
            // GroupDescLabel
            // 
            this.GroupDescLabel.AutoSize = true;
            this.GroupDescLabel.Location = new System.Drawing.Point(12, 67);
            this.GroupDescLabel.Name = "GroupDescLabel";
            this.GroupDescLabel.Size = new System.Drawing.Size(79, 17);
            this.GroupDescLabel.TabIndex = 11;
            this.GroupDescLabel.Text = "Description";
            // 
            // GroupNameLable
            // 
            this.GroupNameLable.AutoSize = true;
            this.GroupNameLable.Location = new System.Drawing.Point(12, 16);
            this.GroupNameLable.Name = "GroupNameLable";
            this.GroupNameLable.Size = new System.Drawing.Size(89, 17);
            this.GroupNameLable.TabIndex = 10;
            this.GroupNameLable.Text = "Group Name";
            // 
            // uxGroupDescriptionText
            // 
            this.uxGroupDescriptionText.Location = new System.Drawing.Point(12, 87);
            this.uxGroupDescriptionText.Multiline = true;
            this.uxGroupDescriptionText.Name = "uxGroupDescriptionText";
            this.uxGroupDescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxGroupDescriptionText.Size = new System.Drawing.Size(268, 118);
            this.uxGroupDescriptionText.TabIndex = 7;
            // 
            // uxSaveNewGroupButton
            // 
            this.uxSaveNewGroupButton.Location = new System.Drawing.Point(12, 222);
            this.uxSaveNewGroupButton.Name = "uxSaveNewGroupButton";
            this.uxSaveNewGroupButton.Size = new System.Drawing.Size(75, 31);
            this.uxSaveNewGroupButton.TabIndex = 8;
            this.uxSaveNewGroupButton.Text = "OK";
            this.uxSaveNewGroupButton.UseVisualStyleBackColor = true;
            this.uxSaveNewGroupButton.Click += new System.EventHandler(this.uxSaveNewGroupButton_Click);
            // 
            // uxGroupNameText
            // 
            this.uxGroupNameText.Location = new System.Drawing.Point(12, 36);
            this.uxGroupNameText.Name = "uxGroupNameText";
            this.uxGroupNameText.Size = new System.Drawing.Size(214, 22);
            this.uxGroupNameText.TabIndex = 6;
            // 
            // NewGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.uxCloseButton);
            this.Controls.Add(this.GroupDescLabel);
            this.Controls.Add(this.GroupNameLable);
            this.Controls.Add(this.uxGroupDescriptionText);
            this.Controls.Add(this.uxSaveNewGroupButton);
            this.Controls.Add(this.uxGroupNameText);
            this.Name = "NewGroup";
            this.Text = "New Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxCloseButton;
        private System.Windows.Forms.Label GroupDescLabel;
        private System.Windows.Forms.Label GroupNameLable;
        private System.Windows.Forms.TextBox uxGroupDescriptionText;
        private System.Windows.Forms.Button uxSaveNewGroupButton;
        private System.Windows.Forms.TextBox uxGroupNameText;
    }
}