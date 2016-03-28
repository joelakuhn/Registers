namespace Registers
{
    partial class RegistersUI
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.howTo_tab = new System.Windows.Forms.TabPage();
            this.settings_tab = new System.Windows.Forms.TabPage();
            this.instructions_rtf = new System.Windows.Forms.RichTextBox();
            this.tabs.SuspendLayout();
            this.howTo_tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.howTo_tab);
            this.tabs.Controls.Add(this.settings_tab);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(711, 422);
            this.tabs.TabIndex = 0;
            // 
            // howTo_tab
            // 
            this.howTo_tab.Controls.Add(this.instructions_rtf);
            this.howTo_tab.Location = new System.Drawing.Point(4, 22);
            this.howTo_tab.Name = "howTo_tab";
            this.howTo_tab.Padding = new System.Windows.Forms.Padding(3);
            this.howTo_tab.Size = new System.Drawing.Size(703, 396);
            this.howTo_tab.TabIndex = 0;
            this.howTo_tab.Text = "How To";
            this.howTo_tab.UseVisualStyleBackColor = true;
            // 
            // settings_tab
            // 
            this.settings_tab.Location = new System.Drawing.Point(4, 22);
            this.settings_tab.Name = "settings_tab";
            this.settings_tab.Padding = new System.Windows.Forms.Padding(3);
            this.settings_tab.Size = new System.Drawing.Size(703, 396);
            this.settings_tab.TabIndex = 1;
            this.settings_tab.Text = "Settings";
            this.settings_tab.UseVisualStyleBackColor = true;
            // 
            // instructions_rtf
            // 
            this.instructions_rtf.BackColor = System.Drawing.SystemColors.Window;
            this.instructions_rtf.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.instructions_rtf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instructions_rtf.Location = new System.Drawing.Point(3, 3);
            this.instructions_rtf.Name = "instructions_rtf";
            this.instructions_rtf.ReadOnly = true;
            this.instructions_rtf.Size = new System.Drawing.Size(697, 390);
            this.instructions_rtf.TabIndex = 0;
            this.instructions_rtf.Text = "";
            // 
            // RegistersUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 446);
            this.Controls.Add(this.tabs);
            this.Name = "RegistersUI";
            this.Text = "Registers Window";
            this.tabs.ResumeLayout(false);
            this.howTo_tab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage howTo_tab;
        private System.Windows.Forms.RichTextBox instructions_rtf;
        private System.Windows.Forms.TabPage settings_tab;

    }
}

