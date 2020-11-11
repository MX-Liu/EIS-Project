namespace Comm
{
    partial class NewProject
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
            this.tb_name = new System.Windows.Forms.TextBox();
            this.tb_save = new System.Windows.Forms.TextBox();
            this.tb_descrip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cal = new System.Windows.Forms.Button();
            this.btn_SaveFile = new System.Windows.Forms.Button();
            this.fdb = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(160, 56);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(147, 21);
            this.tb_name.TabIndex = 0;
            this.tb_name.Text = "NewProject1";
            // 
            // tb_save
            // 
            this.tb_save.Location = new System.Drawing.Point(160, 83);
            this.tb_save.Name = "tb_save";
            this.tb_save.Size = new System.Drawing.Size(147, 21);
            this.tb_save.TabIndex = 1;
            // 
            // tb_descrip
            // 
            this.tb_descrip.Location = new System.Drawing.Point(160, 110);
            this.tb_descrip.Multiline = true;
            this.tb_descrip.Name = "tb_descrip";
            this.tb_descrip.Size = new System.Drawing.Size(187, 97);
            this.tb_descrip.TabIndex = 2;
            this.tb_descrip.Text = "This is a New Project for ..";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "ProjectName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Save as";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Description";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(139, 228);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 6;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Cal
            // 
            this.btn_Cal.Location = new System.Drawing.Point(260, 228);
            this.btn_Cal.Name = "btn_Cal";
            this.btn_Cal.Size = new System.Drawing.Size(75, 23);
            this.btn_Cal.TabIndex = 7;
            this.btn_Cal.Text = "Cancel";
            this.btn_Cal.UseVisualStyleBackColor = true;
            this.btn_Cal.Click += new System.EventHandler(this.btn_Cal_Click);
            // 
            // btn_SaveFile
            // 
            this.btn_SaveFile.Location = new System.Drawing.Point(314, 82);
            this.btn_SaveFile.Name = "btn_SaveFile";
            this.btn_SaveFile.Size = new System.Drawing.Size(33, 23);
            this.btn_SaveFile.TabIndex = 8;
            this.btn_SaveFile.Text = "...";
            this.btn_SaveFile.UseVisualStyleBackColor = true;
            this.btn_SaveFile.Click += new System.EventHandler(this.btn_SaveFile_Click);
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 322);
            this.Controls.Add(this.btn_SaveFile);
            this.Controls.Add(this.btn_Cal);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_descrip);
            this.Controls.Add(this.tb_save);
            this.Controls.Add(this.tb_name);
            this.Name = "NewProject";
            this.Text = "NewProject";
            this.Load += new System.EventHandler(this.NewProject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.TextBox tb_save;
        private System.Windows.Forms.TextBox tb_descrip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cal;
        private System.Windows.Forms.Button btn_SaveFile;
        private System.Windows.Forms.FolderBrowserDialog fdb;
    }
}