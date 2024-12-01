namespace Rhythm_Plus___Splamei_Client
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
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.Close = new System.Windows.Forms.ToolStripMenuItem();
            this.About = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutOption = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.GetHelp = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.Black;
            this.webView21.Location = new System.Drawing.Point(0, 23);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(1194, 643);
            this.webView21.TabIndex = 0;
            this.webView21.ZoomFactor = 1D;
            this.webView21.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webView21_NavigationCompleted);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.About});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1194, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // File
            // 
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Close});
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(37, 20);
            this.File.Text = "File";
            // 
            // Close
            // 
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(155, 22);
            this.Close.Text = "Close the client";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // About
            // 
            this.About.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutOption,
            this.toolStripSeparator1,
            this.GetHelp});
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(44, 20);
            this.About.Text = "Help";
            // 
            // AboutOption
            // 
            this.AboutOption.Name = "AboutOption";
            this.AboutOption.Size = new System.Drawing.Size(120, 22);
            this.AboutOption.Text = "About";
            this.AboutOption.Click += new System.EventHandler(this.AboutOption_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(117, 6);
            // 
            // GetHelp
            // 
            this.GetHelp.Name = "GetHelp";
            this.GetHelp.Size = new System.Drawing.Size(120, 22);
            this.GetHelp.Text = "Get Help";
            this.GetHelp.Click += new System.EventHandler(this.GetHelp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 666);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.webView21);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rhythm Plus - Splamei Client";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem About;
        private System.Windows.Forms.ToolStripMenuItem AboutOption;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem GetHelp;
        private System.Windows.Forms.ToolStripMenuItem Close;
    }
}

