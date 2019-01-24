namespace L3DS.Forms
{
    partial class PrescanSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.xmaxBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.xresBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.klxBox = new System.Windows.Forms.NumericUpDown();
            this.klyBox = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.klzBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.krxBox = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.kryBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.krzBox = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.xmaxBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xresBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.klxBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.klyBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.klzBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.krxBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.krzBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ustawienia główne procesu skanowania";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rozdzielczość[mm]:";
            // 
            // xmaxBox
            // 
            this.xmaxBox.DecimalPlaces = 1;
            this.xmaxBox.Location = new System.Drawing.Point(148, 31);
            this.xmaxBox.Maximum = new decimal(new int[] {
            220,
            0,
            0,
            0});
            this.xmaxBox.Name = "xmaxBox";
            this.xmaxBox.Size = new System.Drawing.Size(49, 20);
            this.xmaxBox.TabIndex = 2;
            this.xmaxBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.xmaxBox.ValueChanged += new System.EventHandler(this.xmax_changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Długość skanowania[mm]";
            // 
            // xresBox
            // 
            this.xresBox.DecimalPlaces = 2;
            this.xresBox.Location = new System.Drawing.Point(148, 57);
            this.xresBox.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.xresBox.Name = "xresBox";
            this.xresBox.Size = new System.Drawing.Size(49, 20);
            this.xresBox.TabIndex = 4;
            this.xresBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.xresBox.ValueChanged += new System.EventHandler(this.xres_changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(76, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Wsp. klx [1]:";
            // 
            // klxBox
            // 
            this.klxBox.DecimalPlaces = 2;
            this.klxBox.Location = new System.Drawing.Point(148, 83);
            this.klxBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.klxBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.klxBox.Name = "klxBox";
            this.klxBox.Size = new System.Drawing.Size(49, 20);
            this.klxBox.TabIndex = 6;
            this.klxBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.klxBox.ValueChanged += new System.EventHandler(this.klx_changed);
            // 
            // klyBox
            // 
            this.klyBox.DecimalPlaces = 2;
            this.klyBox.Location = new System.Drawing.Point(148, 109);
            this.klyBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.klyBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.klyBox.Name = "klyBox";
            this.klyBox.Size = new System.Drawing.Size(49, 20);
            this.klyBox.TabIndex = 8;
            this.klyBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.klyBox.ValueChanged += new System.EventHandler(this.kly_changed);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Wsp. kly [1]:";
            // 
            // klzBox
            // 
            this.klzBox.DecimalPlaces = 2;
            this.klzBox.Location = new System.Drawing.Point(148, 135);
            this.klzBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.klzBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.klzBox.Name = "klzBox";
            this.klzBox.Size = new System.Drawing.Size(49, 20);
            this.klzBox.TabIndex = 10;
            this.klzBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.klzBox.ValueChanged += new System.EventHandler(this.klz_changed);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Wsp. klz [1]:";
            // 
            // krxBox
            // 
            this.krxBox.DecimalPlaces = 2;
            this.krxBox.Location = new System.Drawing.Point(148, 162);
            this.krxBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.krxBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.krxBox.Name = "krxBox";
            this.krxBox.Size = new System.Drawing.Size(49, 20);
            this.krxBox.TabIndex = 12;
            this.krxBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.krxBox.ValueChanged += new System.EventHandler(this.krx_changed);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(76, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Wsp. krx [1]:";
            // 
            // kryBox
            // 
            this.kryBox.DecimalPlaces = 2;
            this.kryBox.Location = new System.Drawing.Point(148, 188);
            this.kryBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.kryBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.kryBox.Name = "kryBox";
            this.kryBox.Size = new System.Drawing.Size(49, 20);
            this.kryBox.TabIndex = 14;
            this.kryBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.kryBox.ValueChanged += new System.EventHandler(this.kry_changed);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(76, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Wsp. kry [1]:";
            // 
            // krzBox
            // 
            this.krzBox.DecimalPlaces = 2;
            this.krzBox.Location = new System.Drawing.Point(148, 214);
            this.krzBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.krzBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.krzBox.Name = "krzBox";
            this.krzBox.Size = new System.Drawing.Size(49, 20);
            this.krzBox.TabIndex = 16;
            this.krzBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.krzBox.ValueChanged += new System.EventHandler(this.krz_changed);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(75, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Wsp. krz [1]:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(93, 243);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Zapisz Ustawienia";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 272);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Skanuj";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // PrescanSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 306);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.krzBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.kryBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.krxBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.klzBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.klyBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.klxBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.xresBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.xmaxBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PrescanSettings";
            this.Text = "Ustawienia skanowania";
            this.Load += new System.EventHandler(this.PrescanSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xmaxBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xresBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.klxBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.klyBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.klzBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.krxBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.krzBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown xmaxBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown xresBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown klxBox;
        private System.Windows.Forms.NumericUpDown klyBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown klzBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown krxBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown kryBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown krzBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}