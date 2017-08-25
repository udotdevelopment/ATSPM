namespace FTPFromOneController
{
    partial class Form1
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
            this.uxStartButton = new System.Windows.Forms.Button();
            this.uxUnameText = new System.Windows.Forms.TextBox();
            this.uxsignalIdText = new System.Windows.Forms.TextBox();
            this.uxIPaddrText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uxPasswordText = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.uxRemDirText = new System.Windows.Forms.TextBox();
            this.uxLocalDirtxt = new System.Windows.Forms.TextBox();
            this.uxActiveModeCombo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // uxStartButton
            // 
            this.uxStartButton.Location = new System.Drawing.Point(181, 226);
            this.uxStartButton.Name = "uxStartButton";
            this.uxStartButton.Size = new System.Drawing.Size(75, 23);
            this.uxStartButton.TabIndex = 8;
            this.uxStartButton.Text = "GO";
            this.uxStartButton.UseVisualStyleBackColor = true;
            this.uxStartButton.Click += new System.EventHandler(this.uxStartButton_Click);
            // 
            // uxUnameText
            // 
            this.uxUnameText.Location = new System.Drawing.Point(12, 112);
            this.uxUnameText.Name = "uxUnameText";
            this.uxUnameText.Size = new System.Drawing.Size(100, 20);
            this.uxUnameText.TabIndex = 3;
            this.uxUnameText.Text = "econolite";
            // 
            // uxsignalIdText
            // 
            this.uxsignalIdText.Location = new System.Drawing.Point(12, 47);
            this.uxsignalIdText.Name = "uxsignalIdText";
            this.uxsignalIdText.Size = new System.Drawing.Size(100, 20);
            this.uxsignalIdText.TabIndex = 1;
            this.uxsignalIdText.Text = "7628";
            this.uxsignalIdText.TextChanged += new System.EventHandler(this.uxsignalIdText_TextChanged);
            // 
            // uxIPaddrText
            // 
            this.uxIPaddrText.Location = new System.Drawing.Point(156, 47);
            this.uxIPaddrText.Name = "uxIPaddrText";
            this.uxIPaddrText.Size = new System.Drawing.Size(100, 20);
            this.uxIPaddrText.TabIndex = 2;
            this.uxIPaddrText.Text = "10.210.5.219";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Signal ID";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(153, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password";
            // 
            // uxPasswordText
            // 
            this.uxPasswordText.Location = new System.Drawing.Point(156, 112);
            this.uxPasswordText.Name = "uxPasswordText";
            this.uxPasswordText.Size = new System.Drawing.Size(100, 20);
            this.uxPasswordText.TabIndex = 4;
            this.uxPasswordText.Text = "ecpi2ecpi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(153, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "RemoteDir";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 145);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "LocalDir";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // uxRemDirText
            // 
            this.uxRemDirText.Location = new System.Drawing.Point(156, 161);
            this.uxRemDirText.Name = "uxRemDirText";
            this.uxRemDirText.Size = new System.Drawing.Size(100, 20);
            this.uxRemDirText.TabIndex = 6;
            this.uxRemDirText.Text = "\\set1";
            this.uxRemDirText.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // uxLocalDirtxt
            // 
            this.uxLocalDirtxt.Location = new System.Drawing.Point(12, 161);
            this.uxLocalDirtxt.Name = "uxLocalDirtxt";
            this.uxLocalDirtxt.Size = new System.Drawing.Size(100, 20);
            this.uxLocalDirtxt.TabIndex = 5;
            this.uxLocalDirtxt.Text = "c:\\temp\\asc3logs\\7628\\";
            this.uxLocalDirtxt.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // uxActiveModeCombo
            // 
            this.uxActiveModeCombo.AutoSize = true;
            this.uxActiveModeCombo.Checked = true;
            this.uxActiveModeCombo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uxActiveModeCombo.Location = new System.Drawing.Point(12, 202);
            this.uxActiveModeCombo.Name = "uxActiveModeCombo";
            this.uxActiveModeCombo.Size = new System.Drawing.Size(83, 17);
            this.uxActiveModeCombo.TabIndex = 7;
            this.uxActiveModeCombo.Text = "ActiveMode";
            this.uxActiveModeCombo.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 287);
            this.Controls.Add(this.uxActiveModeCombo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.uxRemDirText);
            this.Controls.Add(this.uxLocalDirtxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.uxPasswordText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uxIPaddrText);
            this.Controls.Add(this.uxsignalIdText);
            this.Controls.Add(this.uxUnameText);
            this.Controls.Add(this.uxStartButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxStartButton;
        private System.Windows.Forms.TextBox uxUnameText;
        private System.Windows.Forms.TextBox uxsignalIdText;
        private System.Windows.Forms.TextBox uxIPaddrText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox uxPasswordText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox uxRemDirText;
        private System.Windows.Forms.TextBox uxLocalDirtxt;
        private System.Windows.Forms.CheckBox uxActiveModeCombo;
    }
}

