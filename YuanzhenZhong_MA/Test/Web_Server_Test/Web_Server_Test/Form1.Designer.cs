namespace Web_Server_Test
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
            this.content_send = new System.Windows.Forms.TextBox();
            this.Connect = new System.Windows.Forms.Button();
            this.ID_send = new System.Windows.Forms.Button();
            this.IP = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.IP_label = new System.Windows.Forms.Label();
            this.Port_lable = new System.Windows.Forms.Label();
            this.FDA_S = new System.Windows.Forms.Button();
            this.FDA_M = new System.Windows.Forms.Button();
            this.TD = new System.Windows.Forms.Button();
            this.Comb = new System.Windows.Forms.Button();
            this.FDA_S_times = new System.Windows.Forms.TextBox();
            this.FDA_M_times = new System.Windows.Forms.TextBox();
            this.TD_times = new System.Windows.Forms.TextBox();
            this.Comb_times = new System.Windows.Forms.TextBox();
            this.DC = new System.Windows.Forms.Button();
            this.DC_times = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // content_send
            // 
            this.content_send.Location = new System.Drawing.Point(66, 134);
            this.content_send.Multiline = true;
            this.content_send.Name = "content_send";
            this.content_send.Size = new System.Drawing.Size(100, 23);
            this.content_send.TabIndex = 0;
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(66, 102);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 23);
            this.Connect.TabIndex = 1;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // ID_send
            // 
            this.ID_send.Location = new System.Drawing.Point(173, 134);
            this.ID_send.Name = "ID_send";
            this.ID_send.Size = new System.Drawing.Size(75, 23);
            this.ID_send.TabIndex = 2;
            this.ID_send.Text = "ID Send";
            this.ID_send.UseVisualStyleBackColor = true;
            this.ID_send.Click += new System.EventHandler(this.button1_Click);
            // 
            // IP
            // 
            this.IP.Location = new System.Drawing.Point(66, 49);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(140, 21);
            this.IP.TabIndex = 3;
            this.IP.Text = "http://localhost";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(66, 76);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(140, 21);
            this.port.TabIndex = 4;
            this.port.Text = "8080";
            // 
            // IP_label
            // 
            this.IP_label.AutoSize = true;
            this.IP_label.Location = new System.Drawing.Point(19, 52);
            this.IP_label.Name = "IP_label";
            this.IP_label.Size = new System.Drawing.Size(17, 12);
            this.IP_label.TabIndex = 6;
            this.IP_label.Text = "IP";
            // 
            // Port_lable
            // 
            this.Port_lable.AutoSize = true;
            this.Port_lable.Location = new System.Drawing.Point(19, 79);
            this.Port_lable.Name = "Port_lable";
            this.Port_lable.Size = new System.Drawing.Size(29, 12);
            this.Port_lable.TabIndex = 7;
            this.Port_lable.Text = "Port";
            // 
            // FDA_S
            // 
            this.FDA_S.Location = new System.Drawing.Point(66, 194);
            this.FDA_S.Name = "FDA_S";
            this.FDA_S.Size = new System.Drawing.Size(75, 23);
            this.FDA_S.TabIndex = 8;
            this.FDA_S.Text = "FDA_S";
            this.FDA_S.UseVisualStyleBackColor = true;
            this.FDA_S.Click += new System.EventHandler(this.FDA_S_Click);
            // 
            // FDA_M
            // 
            this.FDA_M.Location = new System.Drawing.Point(66, 223);
            this.FDA_M.Name = "FDA_M";
            this.FDA_M.Size = new System.Drawing.Size(75, 23);
            this.FDA_M.TabIndex = 9;
            this.FDA_M.Text = "FDA_M";
            this.FDA_M.UseVisualStyleBackColor = true;
            this.FDA_M.Click += new System.EventHandler(this.FDA_M_Click);
            // 
            // TD
            // 
            this.TD.Location = new System.Drawing.Point(66, 252);
            this.TD.Name = "TD";
            this.TD.Size = new System.Drawing.Size(75, 23);
            this.TD.TabIndex = 10;
            this.TD.Text = "TD";
            this.TD.UseVisualStyleBackColor = true;
            this.TD.Click += new System.EventHandler(this.TD_Click);
            // 
            // Comb
            // 
            this.Comb.Location = new System.Drawing.Point(66, 281);
            this.Comb.Name = "Comb";
            this.Comb.Size = new System.Drawing.Size(75, 23);
            this.Comb.TabIndex = 11;
            this.Comb.Text = "Comb";
            this.Comb.UseVisualStyleBackColor = true;
            this.Comb.Click += new System.EventHandler(this.Comb_Click);
            // 
            // FDA_S_times
            // 
            this.FDA_S_times.Location = new System.Drawing.Point(173, 194);
            this.FDA_S_times.Multiline = true;
            this.FDA_S_times.Name = "FDA_S_times";
            this.FDA_S_times.Size = new System.Drawing.Size(75, 23);
            this.FDA_S_times.TabIndex = 12;
            // 
            // FDA_M_times
            // 
            this.FDA_M_times.Location = new System.Drawing.Point(173, 223);
            this.FDA_M_times.Multiline = true;
            this.FDA_M_times.Name = "FDA_M_times";
            this.FDA_M_times.Size = new System.Drawing.Size(75, 23);
            this.FDA_M_times.TabIndex = 13;
            // 
            // TD_times
            // 
            this.TD_times.Location = new System.Drawing.Point(173, 252);
            this.TD_times.Multiline = true;
            this.TD_times.Name = "TD_times";
            this.TD_times.Size = new System.Drawing.Size(75, 23);
            this.TD_times.TabIndex = 15;
            // 
            // Comb_times
            // 
            this.Comb_times.Location = new System.Drawing.Point(173, 281);
            this.Comb_times.Multiline = true;
            this.Comb_times.Name = "Comb_times";
            this.Comb_times.Size = new System.Drawing.Size(75, 23);
            this.Comb_times.TabIndex = 16;
            // 
            // DC
            // 
            this.DC.Location = new System.Drawing.Point(66, 310);
            this.DC.Name = "DC";
            this.DC.Size = new System.Drawing.Size(75, 23);
            this.DC.TabIndex = 17;
            this.DC.Text = "DC";
            this.DC.UseVisualStyleBackColor = true;
            this.DC.Click += new System.EventHandler(this.DC_Click);
            // 
            // DC_times
            // 
            this.DC_times.Location = new System.Drawing.Point(173, 310);
            this.DC_times.Multiline = true;
            this.DC_times.Name = "DC_times";
            this.DC_times.Size = new System.Drawing.Size(75, 23);
            this.DC_times.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(192, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Times";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 350);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DC_times);
            this.Controls.Add(this.DC);
            this.Controls.Add(this.Comb_times);
            this.Controls.Add(this.TD_times);
            this.Controls.Add(this.FDA_M_times);
            this.Controls.Add(this.FDA_S_times);
            this.Controls.Add(this.Comb);
            this.Controls.Add(this.TD);
            this.Controls.Add(this.FDA_M);
            this.Controls.Add(this.FDA_S);
            this.Controls.Add(this.Port_lable);
            this.Controls.Add(this.IP_label);
            this.Controls.Add(this.port);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.ID_send);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.content_send);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox content_send;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Button ID_send;
        private System.Windows.Forms.TextBox IP;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label IP_label;
        private System.Windows.Forms.Label Port_lable;
        private System.Windows.Forms.Button FDA_S;
        private System.Windows.Forms.Button FDA_M;
        private System.Windows.Forms.Button TD;
        private System.Windows.Forms.Button Comb;
        private System.Windows.Forms.TextBox FDA_S_times;
        private System.Windows.Forms.TextBox FDA_M_times;
        private System.Windows.Forms.TextBox TD_times;
        private System.Windows.Forms.TextBox Comb_times;
        private System.Windows.Forms.Button DC;
        private System.Windows.Forms.TextBox DC_times;
        private System.Windows.Forms.Label label1;
    }
}

