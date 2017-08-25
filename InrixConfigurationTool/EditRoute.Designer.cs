namespace InrixConfigurationTool
{
    partial class EditRoute
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
            this.uxUpdateNewRouteButton = new System.Windows.Forms.Button();
            this.uxNewRouteDescriptionText = new System.Windows.Forms.TextBox();
            this.uxNewRouteNameText = new System.Windows.Forms.TextBox();
            this.uxCloseButton = new System.Windows.Forms.Button();
            this.RouteDescLabel = new System.Windows.Forms.Label();
            this.RouteNameLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxUpdateNewRouteButton
            // 
            this.uxUpdateNewRouteButton.Location = new System.Drawing.Point(12, 222);
            this.uxUpdateNewRouteButton.Name = "uxUpdateNewRouteButton";
            this.uxUpdateNewRouteButton.Size = new System.Drawing.Size(75, 31);
            this.uxUpdateNewRouteButton.TabIndex = 17;
            this.uxUpdateNewRouteButton.Text = "OK";
            this.uxUpdateNewRouteButton.UseVisualStyleBackColor = true;
            this.uxUpdateNewRouteButton.Click += new System.EventHandler(this.uxUpdateNewRouteButton_Click);
            // 
            // uxNewRouteDescriptionText
            // 
            this.uxNewRouteDescriptionText.Location = new System.Drawing.Point(12, 87);
            this.uxNewRouteDescriptionText.Multiline = true;
            this.uxNewRouteDescriptionText.Name = "uxNewRouteDescriptionText";
            this.uxNewRouteDescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxNewRouteDescriptionText.Size = new System.Drawing.Size(268, 118);
            this.uxNewRouteDescriptionText.TabIndex = 16;
            // 
            // uxNewRouteNameText
            // 
            this.uxNewRouteNameText.Location = new System.Drawing.Point(12, 36);
            this.uxNewRouteNameText.Name = "uxNewRouteNameText";
            this.uxNewRouteNameText.Size = new System.Drawing.Size(214, 22);
            this.uxNewRouteNameText.TabIndex = 15;
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
            this.RouteDescLabel.Size = new System.Drawing.Size(110, 17);
            this.RouteDescLabel.TabIndex = 14;
            this.RouteDescLabel.Text = "New Description";
            // 
            // RouteNameLable
            // 
            this.RouteNameLable.AutoSize = true;
            this.RouteNameLable.Location = new System.Drawing.Point(12, 16);
            this.RouteNameLable.Name = "RouteNameLable";
            this.RouteNameLable.Size = new System.Drawing.Size(118, 17);
            this.RouteNameLable.TabIndex = 13;
            this.RouteNameLable.Text = "New Route Name";
            // 
            // EditRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.uxUpdateNewRouteButton);
            this.Controls.Add(this.uxNewRouteDescriptionText);
            this.Controls.Add(this.uxNewRouteNameText);
            this.Controls.Add(this.uxCloseButton);
            this.Controls.Add(this.RouteDescLabel);
            this.Controls.Add(this.RouteNameLable);
            this.Name = "EditRoute";
            this.Text = "EditRoute";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxUpdateNewRouteButton;
        private System.Windows.Forms.TextBox uxNewRouteDescriptionText;
        private System.Windows.Forms.TextBox uxNewRouteNameText;
        private System.Windows.Forms.Button uxCloseButton;
        private System.Windows.Forms.Label RouteDescLabel;
        private System.Windows.Forms.Label RouteNameLable;
    }
}