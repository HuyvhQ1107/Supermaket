namespace qlsieuthi.src.main.qlkho
{
    partial class BangQlkho
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.cbbLoaiHang = new System.Windows.Forms.ComboBox();
            this.cbbMaNhaCC = new System.Windows.Forms.ComboBox();
            this.cbbTrangThai = new System.Windows.Forms.ComboBox();
            this.dgvLoadHH = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnBangHH = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadHH)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtSoLuong);
            this.panel2.Controls.Add(this.cbbLoaiHang);
            this.panel2.Controls.Add(this.cbbMaNhaCC);
            this.panel2.Controls.Add(this.cbbTrangThai);
            this.panel2.Controls.Add(this.dgvLoadHH);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(114, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(804, 583);
            this.panel2.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(519, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Mã loại hàng hóa";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(260, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Mã nhà cung cấp";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Trạng thái";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Tổng số lượng";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Enabled = false;
            this.txtSoLuong.Location = new System.Drawing.Point(358, 73);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(121, 20);
            this.txtSoLuong.TabIndex = 24;
            // 
            // cbbLoaiHang
            // 
            this.cbbLoaiHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLoaiHang.FormattingEnabled = true;
            this.cbbLoaiHang.Location = new System.Drawing.Point(614, 148);
            this.cbbLoaiHang.Name = "cbbLoaiHang";
            this.cbbLoaiHang.Size = new System.Drawing.Size(165, 21);
            this.cbbLoaiHang.TabIndex = 23;
            this.cbbLoaiHang.DropDownClosed += new System.EventHandler(this.cbbMaLoaiHang_DropDownClosed);
            // 
            // cbbMaNhaCC
            // 
            this.cbbMaNhaCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbMaNhaCC.FormattingEnabled = true;
            this.cbbMaNhaCC.Location = new System.Drawing.Point(357, 148);
            this.cbbMaNhaCC.Name = "cbbMaNhaCC";
            this.cbbMaNhaCC.Size = new System.Drawing.Size(147, 21);
            this.cbbMaNhaCC.TabIndex = 22;
            this.cbbMaNhaCC.DropDownClosed += new System.EventHandler(this.cbbNhaCC_DropDownClosed);
            // 
            // cbbTrangThai
            // 
            this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTrangThai.FormattingEnabled = true;
            this.cbbTrangThai.Location = new System.Drawing.Point(76, 148);
            this.cbbTrangThai.Name = "cbbTrangThai";
            this.cbbTrangThai.Size = new System.Drawing.Size(121, 21);
            this.cbbTrangThai.TabIndex = 21;
            this.cbbTrangThai.DropDownClosed += new System.EventHandler(this.cbbTrangThai_DropDownClosed);
            // 
            // dgvLoadHH
            // 
            this.dgvLoadHH.AllowUserToAddRows = false;
            this.dgvLoadHH.AllowUserToDeleteRows = false;
            this.dgvLoadHH.AllowUserToResizeColumns = false;
            this.dgvLoadHH.AllowUserToResizeRows = false;
            this.dgvLoadHH.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvLoadHH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoadHH.Location = new System.Drawing.Point(5, 190);
            this.dgvLoadHH.Name = "dgvLoadHH";
            this.dgvLoadHH.ReadOnly = true;
            this.dgvLoadHH.RowHeadersVisible = false;
            this.dgvLoadHH.Size = new System.Drawing.Size(788, 388);
            this.dgvLoadHH.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(5, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(788, 58);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(237, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 31);
            this.label1.TabIndex = 20;
            this.label1.Text = "BẢNG KHO HÀNG HÓA";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.btnBangHH);
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(105, 583);
            this.panel1.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(-1, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 39);
            this.button3.TabIndex = 5;
            this.button3.Text = "Thêm hàng hóa";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(-1, 109);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 39);
            this.button2.TabIndex = 4;
            this.button2.Text = "Sửa hàng hóa";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(-1, -1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 39);
            this.button4.TabIndex = 3;
            this.button4.Text = "Trang chủ";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnBangHH
            // 
            this.btnBangHH.Location = new System.Drawing.Point(-1, 65);
            this.btnBangHH.Name = "btnBangHH";
            this.btnBangHH.Size = new System.Drawing.Size(105, 51);
            this.btnBangHH.TabIndex = 0;
            this.btnBangHH.Text = "Bảng hàng hóa";
            this.btnBangHH.UseVisualStyleBackColor = true;
            this.btnBangHH.Click += new System.EventHandler(this.btnBangHH_Click);
            // 
            // BangQlkho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 586);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "BangQlkho";
            this.Text = "HÀNG HÓA TRONG KHO";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadHH)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnBangHH;
        private System.Windows.Forms.DataGridView dgvLoadHH;
        private System.Windows.Forms.ComboBox cbbMaNhaCC;
        private System.Windows.Forms.ComboBox cbbTrangThai;
        private System.Windows.Forms.ComboBox cbbLoaiHang;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
    }
}