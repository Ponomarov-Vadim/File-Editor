namespace TextEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFromBDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileToDBAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileToLoadInDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vievToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.vievToolStripMenuItem,
            this.autoTabToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(452, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openFromBDToolStripMenuItem,
            this.saveFileToolStripMenuItem,
            this.saveFileToDBAsToolStripMenuItem,
            this.selectFileToLoadInDBToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.openToolStripMenuItem.Text = "Open file...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // openFromBDToolStripMenuItem
            // 
            this.openFromBDToolStripMenuItem.Name = "openFromBDToolStripMenuItem";
            this.openFromBDToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.openFromBDToolStripMenuItem.Text = "Open file from DB...";
            this.openFromBDToolStripMenuItem.Click += new System.EventHandler(this.OpenFromBDToolStripMenuItem_Click);
            // 
            // saveFileToolStripMenuItem
            // 
            this.saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
            this.saveFileToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveFileToolStripMenuItem.Text = "Save file to DB";
            this.saveFileToolStripMenuItem.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // saveFileToDBAsToolStripMenuItem
            // 
            this.saveFileToDBAsToolStripMenuItem.Name = "saveFileToDBAsToolStripMenuItem";
            this.saveFileToDBAsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveFileToDBAsToolStripMenuItem.Text = "Save file to DB as...";
            this.saveFileToDBAsToolStripMenuItem.Click += new System.EventHandler(this.SaveFileToDBAsToolStripMenuItem_Click);
            // 
            // selectFileToLoadInDBToolStripMenuItem
            // 
            this.selectFileToLoadInDBToolStripMenuItem.Name = "selectFileToLoadInDBToolStripMenuItem";
            this.selectFileToLoadInDBToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.selectFileToLoadInDBToolStripMenuItem.Text = "Select file to load in DB...";
            this.selectFileToLoadInDBToolStripMenuItem.Click += new System.EventHandler(this.SelectFileToLoadInDBToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // vievToolStripMenuItem
            // 
            this.vievToolStripMenuItem.Name = "vievToolStripMenuItem";
            this.vievToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.vievToolStripMenuItem.Text = "Font";
            this.vievToolStripMenuItem.Click += new System.EventHandler(this.VievToolStripMenuItem_Click);
            // 
            // autoTabToolStripMenuItem
            // 
            this.autoTabToolStripMenuItem.Name = "autoTabToolStripMenuItem";
            this.autoTabToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.autoTabToolStripMenuItem.Text = "Auto-Tab";
            this.autoTabToolStripMenuItem.Click += new System.EventHandler(this.AutoTabToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox.Location = new System.Drawing.Point(0, 24);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(452, 460);
            this.textBox.TabIndex = 2;
            this.textBox.Text = "";
            this.textBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 484);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Text Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFromBDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileToDBAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vievToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem selectFileToLoadInDBToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.ToolStripMenuItem autoTabToolStripMenuItem;
    }
}

