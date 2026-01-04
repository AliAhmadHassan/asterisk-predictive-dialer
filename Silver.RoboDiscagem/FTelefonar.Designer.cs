namespace Silver.RoboDiscagem
{
    partial class FTelefonar
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_discar = new System.Windows.Forms.Button();
            this.txt_ddd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_telefone = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddl_campanha = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbl_msg = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ddl_campanha);
            this.groupBox2.Controls.Add(this.btn_discar);
            this.groupBox2.Controls.Add(this.txt_ddd);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txt_telefone);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 127);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Discar Número";
            // 
            // btn_discar
            // 
            this.btn_discar.Location = new System.Drawing.Point(270, 44);
            this.btn_discar.Name = "btn_discar";
            this.btn_discar.Size = new System.Drawing.Size(52, 26);
            this.btn_discar.TabIndex = 2;
            this.btn_discar.Text = "Discar";
            this.btn_discar.UseVisualStyleBackColor = true;
            this.btn_discar.Click += new System.EventHandler(this.btn_discar_Click);
            // 
            // txt_ddd
            // 
            this.txt_ddd.Location = new System.Drawing.Point(20, 46);
            this.txt_ddd.MaxLength = 2;
            this.txt_ddd.Name = "txt_ddd";
            this.txt_ddd.Size = new System.Drawing.Size(58, 20);
            this.txt_ddd.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "DDD";
            // 
            // txt_telefone
            // 
            this.txt_telefone.Location = new System.Drawing.Point(84, 46);
            this.txt_telefone.MaxLength = 9;
            this.txt_telefone.Name = "txt_telefone";
            this.txt_telefone.Size = new System.Drawing.Size(180, 20);
            this.txt_telefone.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Telefone";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Campanha";
            // 
            // ddl_campanha
            // 
            this.ddl_campanha.FormattingEnabled = true;
            this.ddl_campanha.Location = new System.Drawing.Point(20, 94);
            this.ddl_campanha.Name = "ddl_campanha";
            this.ddl_campanha.Size = new System.Drawing.Size(302, 21);
            this.ddl_campanha.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_msg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 149);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(369, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbl_msg
            // 
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(46, 17);
            this.lbl_msg.Text = "Pronto!";
            // 
            // FTelefonar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 171);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FTelefonar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Discar um telefone";
            this.Load += new System.EventHandler(this.FTelefonar_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_discar;
        private System.Windows.Forms.TextBox txt_ddd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_telefone;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ddl_campanha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_msg;
    }
}