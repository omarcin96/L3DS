namespace L3DS
{
    partial class L3DS
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ustawieniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eEPROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inicjalizacyjneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kalibracjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dystorsjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polaczToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rozlaczToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pomocToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oProgramieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zamknijToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.scanButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openScan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.newButton = new System.Windows.Forms.Button();
            this.repauseButton = new System.Windows.Forms.Button();
            this.savePointCloudWorker = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ustawieniaToolStripMenuItem,
            this.kalibracjaToolStripMenuItem,
            this.widokToolStripMenuItem,
            this.pomocToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1067, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ustawieniaToolStripMenuItem
            // 
            this.ustawieniaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eEPROMToolStripMenuItem,
            this.inicjalizacyjneToolStripMenuItem});
            this.ustawieniaToolStripMenuItem.Name = "ustawieniaToolStripMenuItem";
            this.ustawieniaToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.ustawieniaToolStripMenuItem.Text = "Ustawienia";
            // 
            // eEPROMToolStripMenuItem
            // 
            this.eEPROMToolStripMenuItem.Enabled = false;
            this.eEPROMToolStripMenuItem.Name = "eEPROMToolStripMenuItem";
            this.eEPROMToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.eEPROMToolStripMenuItem.Text = "EEPROM";
            this.eEPROMToolStripMenuItem.Click += new System.EventHandler(this.eEPROMToolStripMenuItem_Click);
            // 
            // inicjalizacyjneToolStripMenuItem
            // 
            this.inicjalizacyjneToolStripMenuItem.Name = "inicjalizacyjneToolStripMenuItem";
            this.inicjalizacyjneToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.inicjalizacyjneToolStripMenuItem.Text = "Inicjalizacyjne";
            this.inicjalizacyjneToolStripMenuItem.Click += new System.EventHandler(this.inicjalizacyjneToolStripMenuItem_Click);
            // 
            // kalibracjaToolStripMenuItem
            // 
            this.kalibracjaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dystorsjaToolStripMenuItem,
            this.laserToolStripMenuItem});
            this.kalibracjaToolStripMenuItem.Name = "kalibracjaToolStripMenuItem";
            this.kalibracjaToolStripMenuItem.Size = new System.Drawing.Size(87, 24);
            this.kalibracjaToolStripMenuItem.Text = "Kalibracja";
            // 
            // dystorsjaToolStripMenuItem
            // 
            this.dystorsjaToolStripMenuItem.Name = "dystorsjaToolStripMenuItem";
            this.dystorsjaToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.dystorsjaToolStripMenuItem.Text = "Dystorsja";
            this.dystorsjaToolStripMenuItem.Click += new System.EventHandler(this.dystorsjaToolStripMenuItem_Click);
            // 
            // laserToolStripMenuItem
            // 
            this.laserToolStripMenuItem.Name = "laserToolStripMenuItem";
            this.laserToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.laserToolStripMenuItem.Text = "Laser";
            this.laserToolStripMenuItem.Click += new System.EventHandler(this.laserToolStripMenuItem_Click);
            // 
            // widokToolStripMenuItem
            // 
            this.widokToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.polaczToolStripMenuItem,
            this.rozlaczToolStripMenuItem});
            this.widokToolStripMenuItem.Name = "widokToolStripMenuItem";
            this.widokToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.widokToolStripMenuItem.Text = "Widok";
            // 
            // polaczToolStripMenuItem
            // 
            this.polaczToolStripMenuItem.Name = "polaczToolStripMenuItem";
            this.polaczToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.polaczToolStripMenuItem.Text = "Połącz";
            this.polaczToolStripMenuItem.Click += new System.EventHandler(this.polaczToolStripMenuItem_Click);
            // 
            // rozlaczToolStripMenuItem
            // 
            this.rozlaczToolStripMenuItem.Enabled = false;
            this.rozlaczToolStripMenuItem.Name = "rozlaczToolStripMenuItem";
            this.rozlaczToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.rozlaczToolStripMenuItem.Text = "Rozłącz";
            this.rozlaczToolStripMenuItem.Click += new System.EventHandler(this.rozlaczToolStripMenuItem_Click);
            // 
            // pomocToolStripMenuItem
            // 
            this.pomocToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oProgramieToolStripMenuItem,
            this.zamknijToolStripMenuItem});
            this.pomocToolStripMenuItem.Name = "pomocToolStripMenuItem";
            this.pomocToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.pomocToolStripMenuItem.Text = "Pomoc";
            // 
            // oProgramieToolStripMenuItem
            // 
            this.oProgramieToolStripMenuItem.Name = "oProgramieToolStripMenuItem";
            this.oProgramieToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.oProgramieToolStripMenuItem.Text = "O Programie";
            this.oProgramieToolStripMenuItem.Click += new System.EventHandler(this.oProgramieToolStripMenuItem_Click_1);
            // 
            // zamknijToolStripMenuItem
            // 
            this.zamknijToolStripMenuItem.Name = "zamknijToolStripMenuItem";
            this.zamknijToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.zamknijToolStripMenuItem.Text = "Zamknij";
            this.zamknijToolStripMenuItem.Click += new System.EventHandler(this.zamknijToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 1;
            // 
            // scanButton
            // 
            this.scanButton.Enabled = false;
            this.scanButton.Location = new System.Drawing.Point(16, 511);
            this.scanButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(100, 28);
            this.scanButton.TabIndex = 2;
            this.scanButton.Text = "Skanuj";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(20, 341);
            this.stopButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(100, 28);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Zatrzymaj";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(20, 377);
            this.pauseButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(100, 28);
            this.pauseButton.TabIndex = 4;
            this.pauseButton.Text = "Pauzuj";
            this.pauseButton.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.openScan);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.newButton);
            this.splitContainer1.Panel1.Controls.Add(this.repauseButton);
            this.splitContainer1.Panel1.Controls.Add(this.pauseButton);
            this.splitContainer1.Panel1.Controls.Add(this.scanButton);
            this.splitContainer1.Panel1.Controls.Add(this.stopButton);
            this.splitContainer1.Size = new System.Drawing.Size(1067, 554);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 5;
            // 
            // openScan
            // 
            this.openScan.Location = new System.Drawing.Point(20, 439);
            this.openScan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openScan.Name = "openScan";
            this.openScan.Size = new System.Drawing.Size(100, 28);
            this.openScan.TabIndex = 8;
            this.openScan.Text = "Otwórz skan";
            this.openScan.UseVisualStyleBackColor = true;
            this.openScan.Click += new System.EventHandler(this.openScan_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 102);
            this.label2.TabIndex = 0;
            this.label2.Text = "INSTRUKCJA\r\n1. Widok -> Połącz\r\n2. Kalibracja -> Dystorsja\r\n3. Kalibracja -> Lase" +
    "r\r\n4. Skanuj\r\n5. Zapisz do pliku";
            // 
            // newButton
            // 
            this.newButton.Enabled = false;
            this.newButton.Location = new System.Drawing.Point(16, 475);
            this.newButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(100, 28);
            this.newButton.TabIndex = 7;
            this.newButton.Text = "Nowy skan";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // repauseButton
            // 
            this.repauseButton.Enabled = false;
            this.repauseButton.Location = new System.Drawing.Point(20, 305);
            this.repauseButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.repauseButton.Name = "repauseButton";
            this.repauseButton.Size = new System.Drawing.Size(100, 28);
            this.repauseButton.TabIndex = 5;
            this.repauseButton.Text = "Wznów";
            this.repauseButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // L3DS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "L3DS";
            this.Text = "L3DS Application";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ustawieniaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eEPROMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inicjalizacyjneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kalibracjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dystorsjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem widokToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polaczToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rozlaczToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem pomocToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oProgramieToolStripMenuItem;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button repauseButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.ToolStripMenuItem zamknijToolStripMenuItem;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker savePointCloudWorker;
        private System.Windows.Forms.Button openScan;
        private System.Windows.Forms.Button button1;
    }
}

