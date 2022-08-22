namespace Ilyfairy.Tools.WebTranslate
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyExit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1228, 794);
            this.panel.TabIndex = 1;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Translate";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyStartup,
            this.notifyExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(161, 68);
            // 
            // notifyStartup
            // 
            this.notifyStartup.Name = "notifyStartup";
            this.notifyStartup.Size = new System.Drawing.Size(160, 32);
            this.notifyStartup.Text = "开机自启";
            this.notifyStartup.Click += new System.EventHandler(this.notifyStartup_Click);
            // 
            // notifyExit
            // 
            this.notifyExit.Name = "notifyExit";
            this.notifyExit.Size = new System.Drawing.Size(160, 32);
            this.notifyExit.Text = "退出";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 794);
            this.Controls.Add(this.panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Translate";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem notifyExit;
        private ToolStripMenuItem notifyStartup;
    }
}