namespace Ilyfairy.Tools.WinFormTranslate
{
    partial class SettingForm
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
            System.Windows.Forms.Label hotkeyLabel;
            this.hotkeyTextBox = new System.Windows.Forms.TextBox();
            hotkeyLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hotkeyTextBox
            // 
            this.hotkeyTextBox.Location = new System.Drawing.Point(126, 35);
            this.hotkeyTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.hotkeyTextBox.Name = "hotkeyTextBox";
            this.hotkeyTextBox.ReadOnly = true;
            this.hotkeyTextBox.Size = new System.Drawing.Size(150, 31);
            this.hotkeyTextBox.TabIndex = 0;
            // 
            // hotkeyLabel
            // 
            hotkeyLabel.AutoSize = true;
            hotkeyLabel.Location = new System.Drawing.Point(31, 38);
            hotkeyLabel.Name = "hotkeyLabel";
            hotkeyLabel.Size = new System.Drawing.Size(73, 25);
            hotkeyLabel.TabIndex = 1;
            hotkeyLabel.Text = "快捷键:";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 430);
            this.Controls.Add(hotkeyLabel);
            this.Controls.Add(this.hotkeyTextBox);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox hotkeyTextBox;
    }
}