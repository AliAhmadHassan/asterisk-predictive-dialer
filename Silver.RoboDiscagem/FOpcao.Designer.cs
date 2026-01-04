namespace Silver.RoboDiscagem
{
    partial class FOpcao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FOpcao));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_abrir_discagem = new System.Windows.Forms.Button();
            this.btn_abrir_e = new System.Windows.Forms.Button();
            this.txt_discagem = new System.Windows.Forms.TextBox();
            this.btn_abrir_d = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_escuta = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_discador = new System.Windows.Forms.TextBox();
            this.btn_cancelar = new System.Windows.Forms.Button();
            this.btn_salvar = new System.Windows.Forms.Button();
            this.lbl_excluir = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_parametro = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 219);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(524, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_abrir_discagem);
            this.groupBox1.Controls.Add(this.btn_abrir_e);
            this.groupBox1.Controls.Add(this.txt_discagem);
            this.groupBox1.Controls.Add(this.btn_abrir_d);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_parametro);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txt_escuta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_discador);
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 162);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Path dos Executáveis:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path .exe aplicação do Discador";
            // 
            // btn_abrir_discagem
            // 
            this.btn_abrir_discagem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_abrir_discagem.Location = new System.Drawing.Point(449, 129);
            this.btn_abrir_discagem.Name = "btn_abrir_discagem";
            this.btn_abrir_discagem.Size = new System.Drawing.Size(39, 21);
            this.btn_abrir_discagem.TabIndex = 3;
            this.btn_abrir_discagem.Text = "Abrir";
            this.btn_abrir_discagem.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_abrir_discagem.UseVisualStyleBackColor = true;
            this.btn_abrir_discagem.Click += new System.EventHandler(this.btn_abrir_discagem_Click);
            // 
            // btn_abrir_e
            // 
            this.btn_abrir_e.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_abrir_e.Location = new System.Drawing.Point(449, 87);
            this.btn_abrir_e.Name = "btn_abrir_e";
            this.btn_abrir_e.Size = new System.Drawing.Size(39, 21);
            this.btn_abrir_e.TabIndex = 3;
            this.btn_abrir_e.Text = "Abrir";
            this.btn_abrir_e.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_abrir_e.UseVisualStyleBackColor = true;
            this.btn_abrir_e.Click += new System.EventHandler(this.btn_abrir_e_Click);
            // 
            // txt_discagem
            // 
            this.txt_discagem.Location = new System.Drawing.Point(29, 130);
            this.txt_discagem.Name = "txt_discagem";
            this.txt_discagem.ReadOnly = true;
            this.txt_discagem.Size = new System.Drawing.Size(414, 20);
            this.txt_discagem.TabIndex = 2;
            // 
            // btn_abrir_d
            // 
            this.btn_abrir_d.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_abrir_d.Location = new System.Drawing.Point(449, 45);
            this.btn_abrir_d.Name = "btn_abrir_d";
            this.btn_abrir_d.Size = new System.Drawing.Size(39, 21);
            this.btn_abrir_d.TabIndex = 1;
            this.btn_abrir_d.Text = "Abrir";
            this.btn_abrir_d.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_abrir_d.UseVisualStyleBackColor = true;
            this.btn_abrir_d.Click += new System.EventHandler(this.btn_abrir_d_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Path .exe aplicação de Discagem";
            // 
            // txt_escuta
            // 
            this.txt_escuta.Location = new System.Drawing.Point(29, 88);
            this.txt_escuta.Name = "txt_escuta";
            this.txt_escuta.ReadOnly = true;
            this.txt_escuta.Size = new System.Drawing.Size(292, 20);
            this.txt_escuta.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Path .exe aplicação de Escuta";
            // 
            // txt_discador
            // 
            this.txt_discador.Location = new System.Drawing.Point(29, 45);
            this.txt_discador.Name = "txt_discador";
            this.txt_discador.ReadOnly = true;
            this.txt_discador.Size = new System.Drawing.Size(414, 20);
            this.txt_discador.TabIndex = 0;
            // 
            // btn_cancelar
            // 
            this.btn_cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cancelar.Location = new System.Drawing.Point(441, 193);
            this.btn_cancelar.Name = "btn_cancelar";
            this.btn_cancelar.Size = new System.Drawing.Size(75, 23);
            this.btn_cancelar.TabIndex = 1;
            this.btn_cancelar.Text = "&Cancelar";
            this.btn_cancelar.UseVisualStyleBackColor = true;
            this.btn_cancelar.Click += new System.EventHandler(this.btn_cancelar_Click);
            // 
            // btn_salvar
            // 
            this.btn_salvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_salvar.Location = new System.Drawing.Point(360, 193);
            this.btn_salvar.Name = "btn_salvar";
            this.btn_salvar.Size = new System.Drawing.Size(75, 23);
            this.btn_salvar.TabIndex = 0;
            this.btn_salvar.Text = "&Salvar";
            this.btn_salvar.UseVisualStyleBackColor = true;
            this.btn_salvar.Click += new System.EventHandler(this.btn_salvar_Click);
            // 
            // lbl_excluir
            // 
            this.lbl_excluir.AutoSize = true;
            this.lbl_excluir.Location = new System.Drawing.Point(12, 203);
            this.lbl_excluir.Name = "lbl_excluir";
            this.lbl_excluir.Size = new System.Drawing.Size(77, 13);
            this.lbl_excluir.TabIndex = 2;
            this.lbl_excluir.TabStop = true;
            this.lbl_excluir.Text = "Excluir Arquivo";
            this.lbl_excluir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_excluir_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Parâmetro de entrada";
            // 
            // txt_parametro
            // 
            this.txt_parametro.Location = new System.Drawing.Point(327, 88);
            this.txt_parametro.Name = "txt_parametro";
            this.txt_parametro.Size = new System.Drawing.Size(116, 20);
            this.txt_parametro.TabIndex = 2;
            // 
            // FOpcao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(524, 241);
            this.Controls.Add(this.lbl_excluir);
            this.Controls.Add(this.btn_salvar);
            this.Controls.Add(this.btn_cancelar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FOpcao";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opções do Discador";
            this.Load += new System.EventHandler(this.FOpcao_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_cancelar;
        private System.Windows.Forms.Button btn_salvar;
        private System.Windows.Forms.Button btn_abrir_d;
        private System.Windows.Forms.TextBox txt_escuta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_discador;
        private System.Windows.Forms.Button btn_abrir_e;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lbl_excluir;
        private System.Windows.Forms.Button btn_abrir_discagem;
        private System.Windows.Forms.TextBox txt_discagem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_parametro;
        private System.Windows.Forms.Label label4;
    }
}