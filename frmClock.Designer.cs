
namespace ItsAClock
{
    partial class frmClock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClock));
            this.lblMainTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStropShowSeconds = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTimeZone = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMainTime
            // 
            this.lblMainTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainTime.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblMainTime.Location = new System.Drawing.Point(94, 40);
            this.lblMainTime.Name = "lblMainTime";
            this.lblMainTime.Size = new System.Drawing.Size(150, 32);
            this.lblMainTime.TabIndex = 1;
            this.lblMainTime.Text = "12:59:59 PM";
            this.lblMainTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMainTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseDown);
            this.lblMainTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseMove);
            this.lblMainTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseUp);
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblDate.Location = new System.Drawing.Point(0, 40);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(88, 32);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "12-31-21";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseDown);
            this.lblDate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseMove);
            this.lblDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(381, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clockToolStripMenuItem
            // 
            this.clockToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStropShowSeconds,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.clockToolStripMenuItem.Name = "clockToolStripMenuItem";
            this.clockToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.clockToolStripMenuItem.Text = "Clock";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "Select Timezones";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStropShowSeconds
            // 
            this.toolStropShowSeconds.Checked = true;
            this.toolStropShowSeconds.CheckOnClick = true;
            this.toolStropShowSeconds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStropShowSeconds.Name = "toolStropShowSeconds";
            this.toolStropShowSeconds.Size = new System.Drawing.Size(180, 22);
            this.toolStropShowSeconds.Text = "Show Seconds";
            this.toolStropShowSeconds.CheckStateChanged += new System.EventHandler(this.toolStropShowSeconds_CheckStateChanged);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // lblTimeZone
            // 
            this.lblTimeZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeZone.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblTimeZone.Location = new System.Drawing.Point(260, 40);
            this.lblTimeZone.Name = "lblTimeZone";
            this.lblTimeZone.Size = new System.Drawing.Size(114, 32);
            this.lblTimeZone.TabIndex = 6;
            this.lblTimeZone.Text = "Eastern";
            this.lblTimeZone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTimeZone.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseDown);
            this.lblTimeZone.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseMove);
            this.lblTimeZone.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseUp);
            // 
            // frmClock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(381, 94);
            this.Controls.Add(this.lblTimeZone);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblMainTime);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmClock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Its a Clock";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_added_MouseUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblMainTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblTimeZone;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStropShowSeconds;
    }
}

