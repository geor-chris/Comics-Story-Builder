namespace Comics_Story_Builder
{
    partial class Canvas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.superheroesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bubblesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lAyoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Black;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.superheroesToolStripMenuItem,
            this.bubblesToolStripMenuItem,
            this.backgroundsToolStripMenuItem,
            this.lAyoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip1.Size = new System.Drawing.Size(1856, 49);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // superheroesToolStripMenuItem
            // 
            this.superheroesToolStripMenuItem.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superheroesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.superheroesToolStripMenuItem.Name = "superheroesToolStripMenuItem";
            this.superheroesToolStripMenuItem.Size = new System.Drawing.Size(174, 45);
            this.superheroesToolStripMenuItem.Text = "Superheroes";
            this.superheroesToolStripMenuItem.Click += new System.EventHandler(this.superheroesToolStripMenuItem_Click);
            // 
            // bubblesToolStripMenuItem
            // 
            this.bubblesToolStripMenuItem.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bubblesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.bubblesToolStripMenuItem.Name = "bubblesToolStripMenuItem";
            this.bubblesToolStripMenuItem.Size = new System.Drawing.Size(120, 45);
            this.bubblesToolStripMenuItem.Text = "Bubbles";
            this.bubblesToolStripMenuItem.Click += new System.EventHandler(this.bubblesToolStripMenuItem_Click);
            // 
            // backgroundsToolStripMenuItem
            // 
            this.backgroundsToolStripMenuItem.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backgroundsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.backgroundsToolStripMenuItem.Name = "backgroundsToolStripMenuItem";
            this.backgroundsToolStripMenuItem.Size = new System.Drawing.Size(177, 45);
            this.backgroundsToolStripMenuItem.Text = "Backgrounds";
            this.backgroundsToolStripMenuItem.Click += new System.EventHandler(this.backgroundsToolStripMenuItem_Click);
            // 
            // lAyoutToolStripMenuItem
            // 
            this.lAyoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boxToolStripMenuItem,
            this.bToolStripMenuItem,
            this.boxesToolStripMenuItem});
            this.lAyoutToolStripMenuItem.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAyoutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.lAyoutToolStripMenuItem.Name = "lAyoutToolStripMenuItem";
            this.lAyoutToolStripMenuItem.Size = new System.Drawing.Size(101, 45);
            this.lAyoutToolStripMenuItem.Text = "Layout";
            // 
            // boxToolStripMenuItem
            // 
            this.boxToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boxToolStripMenuItem.Name = "boxToolStripMenuItem";
            this.boxToolStripMenuItem.Size = new System.Drawing.Size(199, 38);
            this.boxToolStripMenuItem.Text = "1 Panel";
            this.boxToolStripMenuItem.Click += new System.EventHandler(this.boxToolStripMenuItem_Click);
            // 
            // bToolStripMenuItem
            // 
            this.bToolStripMenuItem.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.bToolStripMenuItem.Name = "bToolStripMenuItem";
            this.bToolStripMenuItem.Size = new System.Drawing.Size(199, 38);
            this.bToolStripMenuItem.Text = "2 Panels";
            this.bToolStripMenuItem.Click += new System.EventHandler(this.bToolStripMenuItem_Click);
            // 
            // boxesToolStripMenuItem
            // 
            this.boxesToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.boxesToolStripMenuItem.Name = "boxesToolStripMenuItem";
            this.boxesToolStripMenuItem.Size = new System.Drawing.Size(199, 38);
            this.boxesToolStripMenuItem.Text = "4 Panels";
            this.boxesToolStripMenuItem.Click += new System.EventHandler(this.boxesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(73, 45);
            this.toolStripMenuItem1.Text = "🏠";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.ClientSize = new System.Drawing.Size(1856, 779);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Canvas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Canvas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Canvas_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem superheroesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bubblesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lAyoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boxesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}