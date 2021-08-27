
namespace Inforus.WavesNft
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
            this.gbMyAccount = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tbAccountBalance = new System.Windows.Forms.TextBox();
            this.tbAdress = new System.Windows.Forms.TextBox();
            this.rbSeed = new System.Windows.Forms.RadioButton();
            this.rbPrivateKey = new System.Windows.Forms.RadioButton();
            this.tbAccountKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbNodeChainId = new System.Windows.Forms.ComboBox();
            this.btnAccountCreate = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbNftDesciption = new System.Windows.Forms.TextBox();
            this.tbNftName = new System.Windows.Forms.TextBox();
            this.tbNftId = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvNft = new System.Windows.Forms.DataGridView();
            this.gbNft = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnNewNft = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbTransferAccount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTransferNft = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbMyAccount.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNft)).BeginInit();
            this.gbNft.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMyAccount
            // 
            this.gbMyAccount.Controls.Add(this.panel5);
            this.gbMyAccount.Controls.Add(this.panel6);
            this.gbMyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMyAccount.Location = new System.Drawing.Point(0, 0);
            this.gbMyAccount.Name = "gbMyAccount";
            this.gbMyAccount.Size = new System.Drawing.Size(1259, 254);
            this.gbMyAccount.TabIndex = 0;
            this.gbMyAccount.TabStop = false;
            this.gbMyAccount.Text = "Мой Waves кошелёк";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.linkLabel1);
            this.panel5.Controls.Add(this.tbAccountBalance);
            this.panel5.Controls.Add(this.tbAdress);
            this.panel5.Controls.Add(this.rbSeed);
            this.panel5.Controls.Add(this.rbPrivateKey);
            this.panel5.Controls.Add(this.tbAccountKey);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(226, 22);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1030, 229);
            this.panel5.TabIndex = 8;
            // 
            // tbAccountBalance
            // 
            this.tbAccountBalance.Location = new System.Drawing.Point(13, 189);
            this.tbAccountBalance.Name = "tbAccountBalance";
            this.tbAccountBalance.ReadOnly = true;
            this.tbAccountBalance.Size = new System.Drawing.Size(160, 26);
            this.tbAccountBalance.TabIndex = 8;
            // 
            // tbAdress
            // 
            this.tbAdress.Location = new System.Drawing.Point(13, 109);
            this.tbAdress.Name = "tbAdress";
            this.tbAdress.ReadOnly = true;
            this.tbAdress.Size = new System.Drawing.Size(883, 26);
            this.tbAdress.TabIndex = 7;
            // 
            // rbSeed
            // 
            this.rbSeed.AutoSize = true;
            this.rbSeed.Checked = true;
            this.rbSeed.Location = new System.Drawing.Point(13, 17);
            this.rbSeed.Name = "rbSeed";
            this.rbSeed.Size = new System.Drawing.Size(152, 24);
            this.rbSeed.TabIndex = 0;
            this.rbSeed.TabStop = true;
            this.rbSeed.Text = "кодовая фраза";
            this.rbSeed.UseVisualStyleBackColor = true;
            // 
            // rbPrivateKey
            // 
            this.rbPrivateKey.AutoSize = true;
            this.rbPrivateKey.Location = new System.Drawing.Point(171, 17);
            this.rbPrivateKey.Name = "rbPrivateKey";
            this.rbPrivateKey.Size = new System.Drawing.Size(160, 24);
            this.rbPrivateKey.TabIndex = 1;
            this.rbPrivateKey.TabStop = true;
            this.rbPrivateKey.Text = "приватный ключ";
            this.rbPrivateKey.UseVisualStyleBackColor = true;
            // 
            // tbAccountKey
            // 
            this.tbAccountKey.Location = new System.Drawing.Point(13, 48);
            this.tbAccountKey.Name = "tbAccountKey";
            this.tbAccountKey.Size = new System.Drawing.Size(883, 26);
            this.tbAccountKey.TabIndex = 2;
            this.tbAccountKey.Text = "seed4Alice";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Баланс кошелька";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Адрес кошелька";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.cmbNodeChainId);
            this.panel6.Controls.Add(this.btnAccountCreate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(3, 22);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(223, 229);
            this.panel6.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Блокчейн";
            // 
            // cmbNodeChainId
            // 
            this.cmbNodeChainId.FormattingEnabled = true;
            this.cmbNodeChainId.Items.AddRange(new object[] {
            "TestNet",
            "MainNet"});
            this.cmbNodeChainId.Location = new System.Drawing.Point(10, 48);
            this.cmbNodeChainId.Name = "cmbNodeChainId";
            this.cmbNodeChainId.Size = new System.Drawing.Size(188, 28);
            this.cmbNodeChainId.TabIndex = 3;
            // 
            // btnAccountCreate
            // 
            this.btnAccountCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAccountCreate.Location = new System.Drawing.Point(9, 104);
            this.btnAccountCreate.Name = "btnAccountCreate";
            this.btnAccountCreate.Size = new System.Drawing.Size(188, 37);
            this.btnAccountCreate.TabIndex = 7;
            this.btnAccountCreate.Text = "Открыть кошелёк";
            this.btnAccountCreate.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(293, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "Получить список NFT кошелька";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbMyAccount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1259, 254);
            this.panel1.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(3, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(593, 434);
            this.panel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnNewNft);
            this.groupBox1.Controls.Add(this.tbNftDesciption);
            this.groupBox1.Controls.Add(this.tbNftName);
            this.groupBox1.Controls.Add(this.tbNftId);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 249);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поля NFT";
            // 
            // tbNftDesciption
            // 
            this.tbNftDesciption.Location = new System.Drawing.Point(99, 145);
            this.tbNftDesciption.Name = "tbNftDesciption";
            this.tbNftDesciption.Size = new System.Drawing.Size(463, 26);
            this.tbNftDesciption.TabIndex = 2;
            // 
            // tbNftName
            // 
            this.tbNftName.Location = new System.Drawing.Point(99, 95);
            this.tbNftName.Name = "tbNftName";
            this.tbNftName.Size = new System.Drawing.Size(463, 26);
            this.tbNftName.TabIndex = 1;
            // 
            // tbNftId
            // 
            this.tbNftId.Location = new System.Drawing.Point(99, 45);
            this.tbNftId.Name = "tbNftId";
            this.tbNftId.ReadOnly = true;
            this.tbNftId.Size = new System.Drawing.Size(463, 26);
            this.tbNftId.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvNft);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 81);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(660, 353);
            this.panel4.TabIndex = 1;
            // 
            // dgvNft
            // 
            this.dgvNft.AllowUserToAddRows = false;
            this.dgvNft.AllowUserToDeleteRows = false;
            this.dgvNft.AllowUserToOrderColumns = true;
            this.dgvNft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNft.Location = new System.Drawing.Point(0, 0);
            this.dgvNft.Name = "dgvNft";
            this.dgvNft.ReadOnly = true;
            this.dgvNft.RowHeadersWidth = 62;
            this.dgvNft.RowTemplate.Height = 28;
            this.dgvNft.Size = new System.Drawing.Size(660, 353);
            this.dgvNft.TabIndex = 4;
            this.dgvNft.SelectionChanged += new System.EventHandler(this.DgvNft_SelectionChanged);
            // 
            // gbNft
            // 
            this.gbNft.Controls.Add(this.panel7);
            this.gbNft.Controls.Add(this.panel3);
            this.gbNft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbNft.Location = new System.Drawing.Point(0, 254);
            this.gbNft.Name = "gbNft";
            this.gbNft.Size = new System.Drawing.Size(1259, 459);
            this.gbNft.TabIndex = 7;
            this.gbNft.TabStop = false;
            this.gbNft.Text = "NFT";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(660, 81);
            this.panel2.TabIndex = 2;
            // 
            // btnNewNft
            // 
            this.btnNewNft.Location = new System.Drawing.Point(10, 191);
            this.btnNewNft.Name = "btnNewNft";
            this.btnNewNft.Size = new System.Drawing.Size(303, 37);
            this.btnNewNft.TabIndex = 3;
            this.btnNewNft.Text = "Выпустить NFT";
            this.btnNewNft.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnTransferNft);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbTransferAccount);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 249);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(593, 185);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Передача  NFT";
            // 
            // tbTransferAccount
            // 
            this.tbTransferAccount.Location = new System.Drawing.Point(9, 65);
            this.tbTransferAccount.Name = "tbTransferAccount";
            this.tbTransferAccount.Size = new System.Drawing.Size(553, 26);
            this.tbTransferAccount.TabIndex = 9;
            this.tbTransferAccount.Text = "3N1atv1SuhTC3sQwqrR6BkK2PNwrC8LKLqp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Адрес кошелька";
            // 
            // btnTransferNft
            // 
            this.btnTransferNft.Location = new System.Drawing.Point(10, 111);
            this.btnTransferNft.Name = "btnTransferNft";
            this.btnTransferNft.Size = new System.Drawing.Size(303, 37);
            this.btnTransferNft.TabIndex = 10;
            this.btnTransferNft.Text = "Передать NFT";
            this.btnTransferNft.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel4);
            this.panel7.Controls.Add(this.panel2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(596, 22);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(660, 434);
            this.panel7.TabIndex = 5;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(17, 142);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(80, 20);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Id";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Desciption";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 713);
            this.Controls.Add(this.gbNft);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Waves NFT";
            this.gbMyAccount.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNft)).EndInit();
            this.gbNft.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMyAccount;
        private System.Windows.Forms.RadioButton rbPrivateKey;
        private System.Windows.Forms.RadioButton rbSeed;
        private System.Windows.Forms.TextBox tbAccountKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbNodeChainId;
        private System.Windows.Forms.Button btnAccountCreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvNft;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox tbAdress;
        private System.Windows.Forms.GroupBox gbNft;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbNftDesciption;
        private System.Windows.Forms.TextBox tbNftName;
        private System.Windows.Forms.TextBox tbNftId;
        private System.Windows.Forms.TextBox tbAccountBalance;
        private System.Windows.Forms.Button btnNewNft;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTransferNft;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbTransferAccount;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}

