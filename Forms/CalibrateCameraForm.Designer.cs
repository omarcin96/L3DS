namespace L3DS.Forms
{
    partial class CalibrateCameraForm
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
            this.components = new System.ComponentModel.Container();
            this.videoImage = new Emgu.CV.UI.ImageBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.brightnessSlider = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.exposureSlider = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.saturationSlider = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.contrastSlider = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.videoImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saturationSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // videoImage
            // 
            this.videoImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.videoImage.Dock = System.Windows.Forms.DockStyle.Right;
            this.videoImage.Location = new System.Drawing.Point(157, 0);
            this.videoImage.Name = "videoImage";
            this.videoImage.Size = new System.Drawing.Size(640, 442);
            this.videoImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.videoImage.TabIndex = 2;
            this.videoImage.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.countLabel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.calibrateButton);
            this.groupBox1.Controls.Add(this.acceptButton);
            this.groupBox1.Controls.Add(this.cancelButton);
            this.groupBox1.Controls.Add(this.brightnessSlider);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.exposureSlider);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.saturationSlider);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.contrastSlider);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(120, 442);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ustawienia Kamery";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(78, 330);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 330);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Błąd [mm]:";
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(78, 307);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(13, 13);
            this.countLabel.TabIndex = 17;
            this.countLabel.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 307);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Liczba zdjęć:";
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(8, 277);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(104, 23);
            this.calibrateButton.TabIndex = 15;
            this.calibrateButton.Text = "Kalibruj kamerę";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(8, 382);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 14;
            this.acceptButton.Text = "Akceptuj";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(8, 411);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Anuluj";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // brightnessSlider
            // 
            this.brightnessSlider.LargeChange = 20;
            this.brightnessSlider.Location = new System.Drawing.Point(8, 226);
            this.brightnessSlider.Maximum = 255;
            this.brightnessSlider.Name = "brightnessSlider";
            this.brightnessSlider.Size = new System.Drawing.Size(104, 45);
            this.brightnessSlider.TabIndex = 12;
            this.brightnessSlider.TickFrequency = 10;
            this.brightnessSlider.ValueChanged += new System.EventHandler(this.brightness_changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Jasność kamery";
            // 
            // exposureSlider
            // 
            this.exposureSlider.Location = new System.Drawing.Point(8, 162);
            this.exposureSlider.Maximum = 1;
            this.exposureSlider.Minimum = -11;
            this.exposureSlider.Name = "exposureSlider";
            this.exposureSlider.Size = new System.Drawing.Size(104, 45);
            this.exposureSlider.TabIndex = 10;
            this.exposureSlider.ValueChanged += new System.EventHandler(this.exposure_changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Czułość Kamery";
            // 
            // saturationSlider
            // 
            this.saturationSlider.Location = new System.Drawing.Point(8, 98);
            this.saturationSlider.Maximum = 200;
            this.saturationSlider.Name = "saturationSlider";
            this.saturationSlider.Size = new System.Drawing.Size(104, 45);
            this.saturationSlider.TabIndex = 8;
            this.saturationSlider.TickFrequency = 10;
            this.saturationSlider.ValueChanged += new System.EventHandler(this.saturation_changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Saturacja kamery";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Kontrast Kamery";
            // 
            // contrastSlider
            // 
            this.contrastSlider.Location = new System.Drawing.Point(8, 34);
            this.contrastSlider.Name = "contrastSlider";
            this.contrastSlider.Size = new System.Drawing.Size(104, 45);
            this.contrastSlider.TabIndex = 5;
            this.contrastSlider.ValueChanged += new System.EventHandler(this.contrast_changed);
            // 
            // CalibrateCameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(797, 442);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.videoImage);
            this.Name = "CalibrateCameraForm";
            this.Text = "Kalibracja Kamery - Dystorsja";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.window_close);
            this.Load += new System.EventHandler(this.CalibrateCameraForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.videoImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saturationSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox videoImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar brightnessSlider;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar exposureSlider;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar saturationSlider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar contrastSlider;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}