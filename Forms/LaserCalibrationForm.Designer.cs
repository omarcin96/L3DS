namespace L3DS.Forms
{
    partial class LaserCalibrationForm
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
            this.videoMainImage = new Emgu.CV.UI.ImageBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.whiteBalance = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.exposureSlider = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.brightnessSlider = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.saturationSlider = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.contrastSlider = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.laserSwitch = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.delayLaser = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.laserAngleNumber = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.windowNumber = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.sigmaNumber = new System.Windows.Forms.NumericUpDown();
            this.thresholdNumber = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.heightNumber = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.laserLeftPointGraph = new ZedGraph.ZedGraphControl();
            this.workModeBox = new System.Windows.Forms.ComboBox();
            this.channelLeftImageBox = new Emgu.CV.UI.ImageBox();
            this.channelRightImageBox = new Emgu.CV.UI.ImageBox();
            this.laserRightPointGraph = new ZedGraph.ZedGraphControl();
            this.homeAxis = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.videoMainImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteBalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saturationSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastSlider)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayLaser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.laserAngleNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sigmaNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelLeftImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelRightImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // videoMainImage
            // 
            this.videoMainImage.Location = new System.Drawing.Point(221, 12);
            this.videoMainImage.MaximumSize = new System.Drawing.Size(640, 480);
            this.videoMainImage.MinimumSize = new System.Drawing.Size(640, 480);
            this.videoMainImage.Name = "videoMainImage";
            this.videoMainImage.Size = new System.Drawing.Size(640, 480);
            this.videoMainImage.TabIndex = 2;
            this.videoMainImage.TabStop = false;
            this.videoMainImage.Click += new System.EventHandler(this.videoImage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.whiteBalance);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.exposureSlider);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.brightnessSlider);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.saturationSlider);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.contrastSlider);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.MaximumSize = new System.Drawing.Size(200, 0);
            this.groupBox1.MinimumSize = new System.Drawing.Size(200, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 212);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ustawienia kamery";
            // 
            // whiteBalance
            // 
            this.whiteBalance.Location = new System.Drawing.Point(90, 148);
            this.whiteBalance.Maximum = 10000;
            this.whiteBalance.Minimum = 2000;
            this.whiteBalance.Name = "whiteBalance";
            this.whiteBalance.Size = new System.Drawing.Size(104, 45);
            this.whiteBalance.TabIndex = 9;
            this.whiteBalance.Value = 2000;
            this.whiteBalance.ValueChanged += new System.EventHandler(this.whiteBalance_changed);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Balans bieli";
            // 
            // exposureSlider
            // 
            this.exposureSlider.Location = new System.Drawing.Point(90, 116);
            this.exposureSlider.Maximum = 1;
            this.exposureSlider.Minimum = -11;
            this.exposureSlider.Name = "exposureSlider";
            this.exposureSlider.Size = new System.Drawing.Size(104, 45);
            this.exposureSlider.TabIndex = 7;
            this.exposureSlider.ValueChanged += new System.EventHandler(this.exposure_changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Czułość";
            // 
            // brightnessSlider
            // 
            this.brightnessSlider.Location = new System.Drawing.Point(90, 84);
            this.brightnessSlider.Maximum = 255;
            this.brightnessSlider.Name = "brightnessSlider";
            this.brightnessSlider.Size = new System.Drawing.Size(104, 45);
            this.brightnessSlider.TabIndex = 5;
            this.brightnessSlider.ValueChanged += new System.EventHandler(this.brightness_changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Jasność";
            // 
            // saturationSlider
            // 
            this.saturationSlider.Location = new System.Drawing.Point(90, 52);
            this.saturationSlider.Maximum = 200;
            this.saturationSlider.Name = "saturationSlider";
            this.saturationSlider.Size = new System.Drawing.Size(104, 45);
            this.saturationSlider.TabIndex = 3;
            this.saturationSlider.ValueChanged += new System.EventHandler(this.saturation_changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Saturacja";
            // 
            // contrastSlider
            // 
            this.contrastSlider.Location = new System.Drawing.Point(90, 19);
            this.contrastSlider.Name = "contrastSlider";
            this.contrastSlider.Size = new System.Drawing.Size(104, 45);
            this.contrastSlider.TabIndex = 1;
            this.contrastSlider.ValueChanged += new System.EventHandler(this.contrast_changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kontrast";
            // 
            // laserSwitch
            // 
            this.laserSwitch.AutoSize = true;
            this.laserSwitch.Location = new System.Drawing.Point(12, 393);
            this.laserSwitch.Name = "laserSwitch";
            this.laserSwitch.Size = new System.Drawing.Size(87, 17);
            this.laserSwitch.TabIndex = 14;
            this.laserSwitch.Text = "Włącz Laser";
            this.laserSwitch.UseVisualStyleBackColor = true;
            this.laserSwitch.CheckedChanged += new System.EventHandler(this.laser_changed);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.delayLaser);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.laserAngleNumber);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.windowNumber);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.sigmaNumber);
            this.groupBox2.Controls.Add(this.thresholdNumber);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.heightNumber);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 198);
            this.groupBox2.MaximumSize = new System.Drawing.Size(200, 0);
            this.groupBox2.MinimumSize = new System.Drawing.Size(200, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 189);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ustawienia lasera";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // delayLaser
            // 
            this.delayLaser.Location = new System.Drawing.Point(90, 150);
            this.delayLaser.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.delayLaser.Name = "delayLaser";
            this.delayLaser.Size = new System.Drawing.Size(101, 20);
            this.delayLaser.TabIndex = 20;
            this.delayLaser.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.delayLaser.ValueChanged += new System.EventHandler(this.delayLaser_change);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Czas [ms]";
            // 
            // laserAngleNumber
            // 
            this.laserAngleNumber.Location = new System.Drawing.Point(90, 124);
            this.laserAngleNumber.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.laserAngleNumber.Name = "laserAngleNumber";
            this.laserAngleNumber.Size = new System.Drawing.Size(101, 20);
            this.laserAngleNumber.TabIndex = 18;
            this.laserAngleNumber.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.laserAngleNumber.ValueChanged += new System.EventHandler(this.laserAngle_changed);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Kąt Lasera";
            // 
            // windowNumber
            // 
            this.windowNumber.Location = new System.Drawing.Point(90, 98);
            this.windowNumber.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.windowNumber.Name = "windowNumber";
            this.windowNumber.Size = new System.Drawing.Size(101, 20);
            this.windowNumber.TabIndex = 16;
            this.windowNumber.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.windowNumber.ValueChanged += new System.EventHandler(this.window_changed);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Odchylenie:";
            // 
            // sigmaNumber
            // 
            this.sigmaNumber.Location = new System.Drawing.Point(90, 72);
            this.sigmaNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sigmaNumber.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.sigmaNumber.Name = "sigmaNumber";
            this.sigmaNumber.Size = new System.Drawing.Size(101, 20);
            this.sigmaNumber.TabIndex = 14;
            this.sigmaNumber.ValueChanged += new System.EventHandler(this.sigma_changed);
            // 
            // thresholdNumber
            // 
            this.thresholdNumber.Location = new System.Drawing.Point(90, 17);
            this.thresholdNumber.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdNumber.Name = "thresholdNumber";
            this.thresholdNumber.Size = new System.Drawing.Size(101, 20);
            this.thresholdNumber.TabIndex = 12;
            this.thresholdNumber.ValueChanged += new System.EventHandler(this.threshold_changed);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Korekcja";
            // 
            // heightNumber
            // 
            this.heightNumber.Enabled = false;
            this.heightNumber.Location = new System.Drawing.Point(90, 44);
            this.heightNumber.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.heightNumber.Name = "heightNumber";
            this.heightNumber.Size = new System.Drawing.Size(101, 20);
            this.heightNumber.TabIndex = 11;
            this.heightNumber.ValueChanged += new System.EventHandler(this.height_changed);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Wysokość Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Progowanie";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 440);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(107, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Zapisz ustawienia";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(12, 469);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Zamknij";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // laserLeftPointGraph
            // 
            this.laserLeftPointGraph.Location = new System.Drawing.Point(221, 12);
            this.laserLeftPointGraph.Name = "laserLeftPointGraph";
            this.laserLeftPointGraph.ScrollGrace = 0D;
            this.laserLeftPointGraph.ScrollMaxX = 0D;
            this.laserLeftPointGraph.ScrollMaxY = 0D;
            this.laserLeftPointGraph.ScrollMaxY2 = 0D;
            this.laserLeftPointGraph.ScrollMinX = 0D;
            this.laserLeftPointGraph.ScrollMinY = 0D;
            this.laserLeftPointGraph.ScrollMinY2 = 0D;
            this.laserLeftPointGraph.Size = new System.Drawing.Size(640, 240);
            this.laserLeftPointGraph.TabIndex = 9;
            this.laserLeftPointGraph.UseExtendedPrintDialog = true;
            // 
            // workModeBox
            // 
            this.workModeBox.FormattingEnabled = true;
            this.workModeBox.Location = new System.Drawing.Point(737, 12);
            this.workModeBox.Name = "workModeBox";
            this.workModeBox.Size = new System.Drawing.Size(121, 21);
            this.workModeBox.TabIndex = 10;
            this.workModeBox.SelectedIndexChanged += new System.EventHandler(this.workModeBox_SelectedIndexChanged);
            this.workModeBox.SelectedValueChanged += new System.EventHandler(this.workmode_changed);
            // 
            // channelLeftImageBox
            // 
            this.channelLeftImageBox.Location = new System.Drawing.Point(221, 12);
            this.channelLeftImageBox.MaximumSize = new System.Drawing.Size(640, 240);
            this.channelLeftImageBox.MinimumSize = new System.Drawing.Size(640, 240);
            this.channelLeftImageBox.Name = "channelLeftImageBox";
            this.channelLeftImageBox.Size = new System.Drawing.Size(640, 240);
            this.channelLeftImageBox.TabIndex = 11;
            this.channelLeftImageBox.TabStop = false;
            // 
            // channelRightImageBox
            // 
            this.channelRightImageBox.Location = new System.Drawing.Point(221, 258);
            this.channelRightImageBox.MaximumSize = new System.Drawing.Size(640, 240);
            this.channelRightImageBox.MinimumSize = new System.Drawing.Size(640, 240);
            this.channelRightImageBox.Name = "channelRightImageBox";
            this.channelRightImageBox.Size = new System.Drawing.Size(640, 240);
            this.channelRightImageBox.TabIndex = 12;
            this.channelRightImageBox.TabStop = false;
            // 
            // laserRightPointGraph
            // 
            this.laserRightPointGraph.Location = new System.Drawing.Point(221, 258);
            this.laserRightPointGraph.Name = "laserRightPointGraph";
            this.laserRightPointGraph.ScrollGrace = 0D;
            this.laserRightPointGraph.ScrollMaxX = 0D;
            this.laserRightPointGraph.ScrollMaxY = 0D;
            this.laserRightPointGraph.ScrollMaxY2 = 0D;
            this.laserRightPointGraph.ScrollMinX = 0D;
            this.laserRightPointGraph.ScrollMinY = 0D;
            this.laserRightPointGraph.ScrollMinY2 = 0D;
            this.laserRightPointGraph.Size = new System.Drawing.Size(640, 240);
            this.laserRightPointGraph.TabIndex = 13;
            this.laserRightPointGraph.UseExtendedPrintDialog = true;
            // 
            // homeAxis
            // 
            this.homeAxis.Location = new System.Drawing.Point(12, 411);
            this.homeAxis.Name = "homeAxis";
            this.homeAxis.Size = new System.Drawing.Size(87, 23);
            this.homeAxis.TabIndex = 15;
            this.homeAxis.Text = "Zerowanie Osi";
            this.homeAxis.UseVisualStyleBackColor = true;
            this.homeAxis.Click += new System.EventHandler(this.homeAxis_Click);
            // 
            // LaserCalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 687);
            this.Controls.Add(this.homeAxis);
            this.Controls.Add(this.laserSwitch);
            this.Controls.Add(this.laserRightPointGraph);
            this.Controls.Add(this.workModeBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.videoMainImage);
            this.Controls.Add(this.channelLeftImageBox);
            this.Controls.Add(this.channelRightImageBox);
            this.Controls.Add(this.laserLeftPointGraph);
            this.Name = "LaserCalibrationForm";
            this.Text = "Kalibracja lasera";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.window_close);
            ((System.ComponentModel.ISupportInitialize)(this.videoMainImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteBalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saturationSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastSlider)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayLaser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.laserAngleNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sigmaNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelLeftImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelRightImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox videoMainImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar brightnessSlider;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar saturationSlider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar contrastSlider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar exposureSlider;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown thresholdNumber;
        private System.Windows.Forms.NumericUpDown heightNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown windowNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown sigmaNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.NumericUpDown laserAngleNumber;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown delayLaser;
        private System.Windows.Forms.Label label11;
        private ZedGraph.ZedGraphControl laserLeftPointGraph;
        private System.Windows.Forms.ComboBox workModeBox;
        private Emgu.CV.UI.ImageBox channelLeftImageBox;
        private Emgu.CV.UI.ImageBox channelRightImageBox;
        private ZedGraph.ZedGraphControl laserRightPointGraph;
        private System.Windows.Forms.CheckBox laserSwitch;
        private System.Windows.Forms.TrackBar whiteBalance;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button homeAxis;
    }
}