namespace StudentManager
{
    partial class FrmAttendance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAttendance));
            this.txtStuCardNo = new System.Windows.Forms.TextBox();
            this.lblWeek = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblTime = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStuClass = new System.Windows.Forms.Label();
            this.lblStuId = new System.Windows.Forms.Label();
            this.lblStuName = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtStuCardNo
            // 
            this.txtStuCardNo.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStuCardNo.Location = new System.Drawing.Point(89, 217);
            this.txtStuCardNo.Name = "txtStuCardNo";
            this.txtStuCardNo.Size = new System.Drawing.Size(161, 35);
            this.txtStuCardNo.TabIndex = 68;
            this.txtStuCardNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStuCardNo_KeyDown);
            // 
            // lblWeek
            // 
            this.lblWeek.BackColor = System.Drawing.Color.Black;
            this.lblWeek.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWeek.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeek.ForeColor = System.Drawing.Color.Red;
            this.lblWeek.Location = new System.Drawing.Point(512, 26);
            this.lblWeek.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.lblWeek.Name = "lblWeek";
            this.lblWeek.Size = new System.Drawing.Size(45, 29);
            this.lblWeek.TabIndex = 53;
            this.lblWeek.Text = "07";
            this.lblWeek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDay
            // 
            this.lblDay.BackColor = System.Drawing.Color.Black;
            this.lblDay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDay.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.ForeColor = System.Drawing.Color.Red;
            this.lblDay.Location = new System.Drawing.Point(222, 26);
            this.lblDay.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(45, 29);
            this.lblDay.TabIndex = 55;
            this.lblDay.Text = "07";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMonth
            // 
            this.lblMonth.BackColor = System.Drawing.Color.Black;
            this.lblMonth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMonth.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.ForeColor = System.Drawing.Color.Red;
            this.lblMonth.Location = new System.Drawing.Point(138, 26);
            this.lblMonth.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(45, 29);
            this.lblMonth.TabIndex = 54;
            this.lblMonth.Text = "07";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Black;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.Red;
            this.lblTime.Location = new System.Drawing.Point(306, 26);
            this.lblTime.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(134, 29);
            this.lblTime.TabIndex = 52;
            this.lblTime.Text = "12 : 25  45";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYear
            // 
            this.lblYear.BackColor = System.Drawing.Color.Black;
            this.lblYear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblYear.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.Red;
            this.lblYear.Location = new System.Drawing.Point(26, 26);
            this.lblYear.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(73, 29);
            this.lblYear.TabIndex = 51;
            this.lblYear.Text = "2013";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(440, 21);
            this.label9.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 39);
            this.label9.TabIndex = 48;
            this.label9.Text = "星期";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(267, 21);
            this.label6.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 39);
            this.label6.TabIndex = 47;
            this.label6.Text = "日";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(183, 21);
            this.label5.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 39);
            this.label5.TabIndex = 50;
            this.label5.Text = "月";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(99, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 39);
            this.label2.TabIndex = 49;
            this.label2.Text = "年";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(593, 213);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 39);
            this.btnClose.TabIndex = 69;
            this.btnClose.Text = "结束打卡";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(371, 84);
            this.label7.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 39);
            this.label7.TabIndex = 64;
            this.label7.Text = "下午时间";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label17.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(6, 217);
            this.label17.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 39);
            this.label17.TabIndex = 63;
            this.label17.Text = "卡号";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label23.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(491, 146);
            this.label23.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(67, 39);
            this.label23.TabIndex = 62;
            this.label23.Text = "班级";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label21.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(252, 148);
            this.label21.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(67, 39);
            this.label21.TabIndex = 67;
            this.label21.Text = "学号";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label19.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(21, 150);
            this.label19.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(67, 39);
            this.label19.TabIndex = 66;
            this.label19.Text = "姓名";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(494, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(226, 40);
            this.label3.TabIndex = 59;
            this.label3.Text = "13:30--17:00";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStuClass
            // 
            this.lblStuClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblStuClass.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStuClass.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStuClass.ForeColor = System.Drawing.Color.Black;
            this.lblStuClass.Location = new System.Drawing.Point(559, 146);
            this.lblStuClass.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblStuClass.Name = "lblStuClass";
            this.lblStuClass.Size = new System.Drawing.Size(161, 39);
            this.lblStuClass.TabIndex = 60;
            this.lblStuClass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStuId
            // 
            this.lblStuId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblStuId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStuId.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStuId.ForeColor = System.Drawing.Color.Black;
            this.lblStuId.Location = new System.Drawing.Point(320, 148);
            this.lblStuId.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblStuId.Name = "lblStuId";
            this.lblStuId.Size = new System.Drawing.Size(161, 39);
            this.lblStuId.TabIndex = 61;
            this.lblStuId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStuName
            // 
            this.lblStuName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblStuName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStuName.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStuName.ForeColor = System.Drawing.Color.Black;
            this.lblStuName.Location = new System.Drawing.Point(89, 150);
            this.lblStuName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblStuName.Name = "lblStuName";
            this.lblStuName.Size = new System.Drawing.Size(161, 39);
            this.lblStuName.TabIndex = 57;
            this.lblStuName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblInfo.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.Blue;
            this.lblInfo.Location = new System.Drawing.Point(315, 213);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(166, 39);
            this.lblInfo.TabIndex = 63;
            this.lblInfo.Text = "打卡信息";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.White;
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.Location = new System.Drawing.Point(135, 84);
            this.lblName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(226, 39);
            this.lblName.TabIndex = 58;
            this.lblName.Text = "8:00--11:30";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 84);
            this.label4.Margin = new System.Windows.Forms.Padding(17, 0, 17, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 39);
            this.label4.TabIndex = 65;
            this.label4.Text = "上午时间";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmAttendance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 271);
            this.Controls.Add(this.txtStuCardNo);
            this.Controls.Add(this.lblWeek);
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.lblMonth);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblStuClass);
            this.Controls.Add(this.lblStuId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblStuName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmAttendance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[考勤打卡]";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStuCardNo;
        private System.Windows.Forms.Label lblWeek;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStuClass;
        private System.Windows.Forms.Label lblStuId;
        private System.Windows.Forms.Label lblStuName;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label4;
    }
}