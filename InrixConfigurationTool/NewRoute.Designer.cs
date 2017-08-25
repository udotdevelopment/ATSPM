namespace InrixConfigurationTool
{
    partial class NewRoute
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
            this.RouteNameLable = new System.Windows.Forms.Label();
            this.RouteDescLabel = new System.Windows.Forms.Label();
            this.uxCloseButton = new System.Windows.Forms.Button();
            this.uxNewRouteDescriptionText = new System.Windows.Forms.TextBox();
            this.uxNewRouteNameText = new System.Windows.Forms.TextBox();
            this.uxSaveNewRouteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RouteNameLable
            // 
            this.RouteNameLable.AutoSize = true;
            this.RouteNameLable.Location = new System.Drawing.Point(12, 19);
            this.RouteNameLable.Name = "RouteNameLable";
            this.RouteNameLable.Size = new System.Drawing.Size(87, 17);
            this.RouteNameLable.TabIndex = 4;
            this.RouteNameLable.Text = "Route Name";
            this.RouteNameLable.Click += new System.EventHandler(this.RouteNameLable_Click);
            // 
            // RouteDescLabel
            // 
            this.RouteDescLabel.AutoSize = true;
            this.RouteDescLabel.Location = new System.Drawing.Point(12, 70);
            this.RouteDescLabel.Name = "RouteDescLabel";
            this.RouteDescLabel.Size = new System.Drawing.Size(79, 17);
            this.RouteDescLabel.TabIndex = 5;
            this.RouteDescLabel.Text = "Description";
            this.RouteDescLabel.Click += new System.EventHandler(this.RouteDescLabel_Click);
            // 
            // uxCloseButton
            // 
            this.uxCloseButton.Location = new System.Drawing.Point(205, 225);
            this.uxCloseButton.Name = "uxCloseButton";
            this.uxCloseButton.Size = new System.Drawing.Size(75, 31);
            this.uxCloseButton.TabIndex = 4;
            this.uxCloseButton.Text = "Close";
            this.uxCloseButton.UseVisualStyleBackColor = true;
            this.uxCloseButton.Click += new System.EventHandler(this.uxCloseButton_Click);
            // 
            // uxNewRouteDescriptionText
            // 
            this.uxNewRouteDescriptionText.Location = new System.Drawing.Point(12, 90);
            this.uxNewRouteDescriptionText.Multiline = true;
            this.uxNewRouteDescriptionText.Name = "uxNewRouteDescriptionText";
            this.uxNewRouteDescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxNewRouteDescriptionText.Size = new System.Drawing.Size(268, 118);
            this.uxNewRouteDescriptionText.TabIndex = 10;
            this.uxNewRouteDescriptionText.TextChanged += new System.EventHandler(this.uxNewRouteDescriptionText_TextChanged);
            // 
            // uxNewRouteNameText
            // 
            this.uxNewRouteNameText.Location = new System.Drawing.Point(12, 39);
            this.uxNewRouteNameText.Name = "uxNewRouteNameText";
            this.uxNewRouteNameText.Size = new System.Drawing.Size(214, 22);
            this.uxNewRouteNameText.TabIndex = 9;
            this.uxNewRouteNameText.TextChanged += new System.EventHandler(this.uxNewRouteNameText_TextChanged);
            // 
            // uxSaveNewRouteButton
            // 
            this.uxSaveNewRouteButton.Location = new System.Drawing.Point(12, 225);
            this.uxSaveNewRouteButton.Name = "uxSaveNewRouteButton";
            this.uxSaveNewRouteButton.Size = new System.Drawing.Size(75, 31);
            this.uxSaveNewRouteButton.TabIndex = 11;
            this.uxSaveNewRouteButton.Text = "OK";
            this.uxSaveNewRouteButton.UseVisualStyleBackColor = true;
            this.uxSaveNewRouteButton.Click += new System.EventHandler(this.uxSaveNewRouteButton_Click);
            // 
            // NewRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.uxSaveNewRouteButton);
            this.Controls.Add(this.uxNewRouteDescriptionText);
            this.Controls.Add(this.uxNewRouteNameText);
            this.Controls.Add(this.uxCloseButton);
            this.Controls.Add(this.RouteDescLabel);
            this.Controls.Add(this.RouteNameLable);
            this.Name = "NewRoute";
            this.Text = "New Route";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox uxRouteNameText;
        private System.Windows.Forms.TextBox uxRouteDescriptionText;
        private System.Windows.Forms.Label RouteNameLable;
        private System.Windows.Forms.Label RouteDescLabel;
        private System.Windows.Forms.Button uxCloseButton;
        private System.Windows.Forms.TextBox uxNewRouteDescriptionText;
        private System.Windows.Forms.TextBox uxNewRouteNameText;
        private System.Windows.Forms.Button uxSaveNewRouteButton;
    }
}