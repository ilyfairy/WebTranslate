namespace Ilyfairy.Tools.WebTranslate
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
            this.versionLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            hotkeyLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hotkeyLabel
            // 
            hotkeyLabel.AutoSize = true;
            hotkeyLabel.Location = new System.Drawing.Point(12, 19);
            hotkeyLabel.Name = "hotkeyLabel";
            hotkeyLabel.Size = new System.Drawing.Size(73, 25);
            hotkeyLabel.TabIndex = 1;
            hotkeyLabel.Text = "快捷键:";
            // 
            // hotkeyTextBox
            // 
            this.hotkeyTextBox.Location = new System.Drawing.Point(88, 16);
            this.hotkeyTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.hotkeyTextBox.Name = "hotkeyTextBox";
            this.hotkeyTextBox.ReadOnly = true;
            this.hotkeyTextBox.Size = new System.Drawing.Size(238, 31);
            this.hotkeyTextBox.TabIndex = 1;
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(545, 9);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(121, 35);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "0.0.0.0";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // saveButton
            // 
            this.saveButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.saveButton.Location = new System.Drawing.Point(545, 379);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(121, 53);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "保存";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.Save_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 444);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(hotkeyLabel);
            this.Controls.Add(this.hotkeyTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox hotkeyTextBox;
        private Label versionLabel;
        private Button saveButton;
    }
}