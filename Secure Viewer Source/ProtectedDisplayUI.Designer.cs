namespace ProtectedDisplayHost
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.panel = new System.Windows.Forms.Panel();
			this.lblKeyStatus = new System.Windows.Forms.Label();
			this.lblGetPicStatus = new System.Windows.Forms.Label();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel.BackColor = System.Drawing.Color.Transparent;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Location = new System.Drawing.Point(16, 15);
			this.panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(1279, 886);
			this.panel.TabIndex = 8;
			// 
			// lblKeyStatus
			// 
			this.lblKeyStatus.AutoSize = true;
			this.lblKeyStatus.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.lblKeyStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(66)))), ((int)(((byte)(129)))));
			this.lblKeyStatus.Location = new System.Drawing.Point(-105, 160);
			this.lblKeyStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKeyStatus.Name = "lblKeyStatus";
			this.lblKeyStatus.Size = new System.Drawing.Size(0, 20);
			this.lblKeyStatus.TabIndex = 19;
			// 
			// lblGetPicStatus
			// 
			this.lblGetPicStatus.AutoSize = true;
			this.lblGetPicStatus.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.lblGetPicStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(66)))), ((int)(((byte)(129)))));
			this.lblGetPicStatus.Location = new System.Drawing.Point(-105, 245);
			this.lblGetPicStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGetPicStatus.Name = "lblGetPicStatus";
			this.lblGetPicStatus.Size = new System.Drawing.Size(0, 20);
			this.lblGetPicStatus.TabIndex = 20;
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Location = new System.Drawing.Point(1304, 11);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 886);
			this.vScrollBar1.TabIndex = 23;
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.ClientSize = new System.Drawing.Size(1339, 924);
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.lblGetPicStatus);
			this.Controls.Add(this.lblKeyStatus);
			this.Controls.Add(this.panel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Secure Viewer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lblKeyStatus;
        private System.Windows.Forms.Label lblGetPicStatus;
		private System.Windows.Forms.VScrollBar vScrollBar1;
	}
}

