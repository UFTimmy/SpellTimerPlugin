namespace SpellTimerPlugin
{
    partial class SpellTimerForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.EnableCastbar = new System.Windows.Forms.CheckBox();
            this.ShowCountdown = new System.Windows.Forms.CheckBox();
            this.ShowSpinners = new System.Windows.Forms.CheckBox();
            this.ShowReady = new System.Windows.Forms.CheckBox();
            this.ShowBar = new System.Windows.Forms.CheckBox();
            this.CastbarGroupBox = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Ready = new System.Windows.Forms.TextBox();
            this.Quarter = new System.Windows.Forms.TextBox();
            this.Half = new System.Windows.Forms.TextBox();
            this.Triquarter = new System.Windows.Forms.TextBox();
            this.CastbarGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(242, 42);
            this.label5.TabIndex = 14;
            this.label5.Text = "Here you can set colors to change to for each stage of casting progress. You can " +
    "also set these through command, type /castbar for more info. \r\n";
            // 
            // EnableCastbar
            // 
            this.EnableCastbar.AutoSize = true;
            this.EnableCastbar.Location = new System.Drawing.Point(11, 11);
            this.EnableCastbar.Name = "EnableCastbar";
            this.EnableCastbar.Size = new System.Drawing.Size(99, 17);
            this.EnableCastbar.TabIndex = 6;
            this.EnableCastbar.Text = "Enable CastBar";
            this.EnableCastbar.UseVisualStyleBackColor = true;
            this.EnableCastbar.CheckedChanged += new System.EventHandler(this.EnableCastbar_CheckedChanged);
            // 
            // ShowCountdown
            // 
            this.ShowCountdown.AutoSize = true;
            this.ShowCountdown.Location = new System.Drawing.Point(6, 19);
            this.ShowCountdown.Name = "ShowCountdown";
            this.ShowCountdown.Size = new System.Drawing.Size(110, 17);
            this.ShowCountdown.TabIndex = 1;
            this.ShowCountdown.Text = "Show Countdown";
            this.ShowCountdown.UseVisualStyleBackColor = true;
            this.ShowCountdown.CheckedChanged += new System.EventHandler(this.ShowCountdown_CheckedChanged);
            // 
            // ShowSpinners
            // 
            this.ShowSpinners.AutoSize = true;
            this.ShowSpinners.Location = new System.Drawing.Point(122, 42);
            this.ShowSpinners.Name = "ShowSpinners";
            this.ShowSpinners.Size = new System.Drawing.Size(97, 17);
            this.ShowSpinners.TabIndex = 4;
            this.ShowSpinners.Text = "Show Spinners";
            this.ShowSpinners.UseVisualStyleBackColor = true;
            this.ShowSpinners.CheckedChanged += new System.EventHandler(this.ShowSpinners_CheckedChanged);
            // 
            // ShowReady
            // 
            this.ShowReady.AutoSize = true;
            this.ShowReady.Location = new System.Drawing.Point(6, 42);
            this.ShowReady.Name = "ShowReady";
            this.ShowReady.Size = new System.Drawing.Size(87, 17);
            this.ShowReady.TabIndex = 2;
            this.ShowReady.Text = "Show Ready";
            this.ShowReady.UseVisualStyleBackColor = true;
            this.ShowReady.CheckedChanged += new System.EventHandler(this.ShowReady_CheckedChanged);
            // 
            // ShowBar
            // 
            this.ShowBar.AutoSize = true;
            this.ShowBar.Location = new System.Drawing.Point(122, 19);
            this.ShowBar.Name = "ShowBar";
            this.ShowBar.Size = new System.Drawing.Size(116, 17);
            this.ShowBar.TabIndex = 3;
            this.ShowBar.Text = "Show Progress Bar";
            this.ShowBar.UseVisualStyleBackColor = true;
            this.ShowBar.CheckedChanged += new System.EventHandler(this.ShowBar_CheckedChanged);
            // 
            // CastbarGroupBox
            // 
            this.CastbarGroupBox.Controls.Add(this.label6);
            this.CastbarGroupBox.Controls.Add(this.label5);
            this.CastbarGroupBox.Controls.Add(this.label4);
            this.CastbarGroupBox.Controls.Add(this.label3);
            this.CastbarGroupBox.Controls.Add(this.label2);
            this.CastbarGroupBox.Controls.Add(this.label1);
            this.CastbarGroupBox.Controls.Add(this.Ready);
            this.CastbarGroupBox.Controls.Add(this.Quarter);
            this.CastbarGroupBox.Controls.Add(this.Half);
            this.CastbarGroupBox.Controls.Add(this.Triquarter);
            this.CastbarGroupBox.Controls.Add(this.ShowCountdown);
            this.CastbarGroupBox.Controls.Add(this.ShowSpinners);
            this.CastbarGroupBox.Controls.Add(this.ShowReady);
            this.CastbarGroupBox.Controls.Add(this.ShowBar);
            this.CastbarGroupBox.Location = new System.Drawing.Point(11, 34);
            this.CastbarGroupBox.Name = "CastbarGroupBox";
            this.CastbarGroupBox.Size = new System.Drawing.Size(251, 215);
            this.CastbarGroupBox.TabIndex = 7;
            this.CastbarGroupBox.TabStop = false;
            this.CastbarGroupBox.Text = "CastBar Settings";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(242, 41);
            this.label6.TabIndex = 15;
            this.label6.Text = "Colors can be any named color recognized by Genie (Lime, Cyan) or any RGB Color w" +
    "ith included Hex (#FF0055).";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(121, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Ready";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "25%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "50%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "75%";
            // 
            // Ready
            // 
            this.Ready.Location = new System.Drawing.Point(165, 188);
            this.Ready.Name = "Ready";
            this.Ready.Size = new System.Drawing.Size(80, 20);
            this.Ready.TabIndex = 8;
            this.Ready.TextChanged += new System.EventHandler(this.Ready_TextChanged);
            // 
            // Quarter
            // 
            this.Quarter.Location = new System.Drawing.Point(36, 188);
            this.Quarter.Name = "Quarter";
            this.Quarter.Size = new System.Drawing.Size(80, 20);
            this.Quarter.TabIndex = 7;
            this.Quarter.TextChanged += new System.EventHandler(this.Quarter_TextChanged);
            // 
            // Half
            // 
            this.Half.Location = new System.Drawing.Point(165, 165);
            this.Half.Name = "Half";
            this.Half.Size = new System.Drawing.Size(80, 20);
            this.Half.TabIndex = 6;
            this.Half.TextChanged += new System.EventHandler(this.Half_TextChanged);
            // 
            // Triquarter
            // 
            this.Triquarter.Location = new System.Drawing.Point(36, 165);
            this.Triquarter.Name = "Triquarter";
            this.Triquarter.Size = new System.Drawing.Size(80, 20);
            this.Triquarter.TabIndex = 5;
            this.Triquarter.TextChanged += new System.EventHandler(this.Triquarter_TextChanged);
            // 
            // SpellTimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 261);
            this.Controls.Add(this.EnableCastbar);
            this.Controls.Add(this.CastbarGroupBox);
            this.Name = "SpellTimerForm";
            this.Text = "SpellTimer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpellTimerForm_FormClosing);
            this.Enter += new System.EventHandler(this.SpellTimerForm_Enter);
            this.CastbarGroupBox.ResumeLayout(false);
            this.CastbarGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox EnableCastbar;
        private System.Windows.Forms.CheckBox ShowCountdown;
        private System.Windows.Forms.CheckBox ShowSpinners;
        private System.Windows.Forms.CheckBox ShowReady;
        private System.Windows.Forms.CheckBox ShowBar;
        private System.Windows.Forms.GroupBox CastbarGroupBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Ready;
        private System.Windows.Forms.TextBox Quarter;
        private System.Windows.Forms.TextBox Half;
        private System.Windows.Forms.TextBox Triquarter;
        private System.Windows.Forms.Label label6;
    }
}