namespace WinTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRead = new System.Windows.Forms.Button();
            this.txtPdfFolderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.txtHelp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTxtFolder = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnChooseTxtFolder = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnAnaylse = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(502, 173);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(151, 81);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "一键完成";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // txtPdfFolderName
            // 
            this.txtPdfFolderName.Location = new System.Drawing.Point(98, 17);
            this.txtPdfFolderName.Name = "txtPdfFolderName";
            this.txtPdfFolderName.Size = new System.Drawing.Size(434, 21);
            this.txtPdfFolderName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "pdf文件目录";
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Location = new System.Drawing.Point(547, 15);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(106, 23);
            this.btnChooseFolder.TabIndex = 3;
            this.btnChooseFolder.Text = "选择pdf目录";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // txtHelp
            // 
            this.txtHelp.Location = new System.Drawing.Point(14, 158);
            this.txtHelp.Multiline = true;
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.ReadOnly = true;
            this.txtHelp.Size = new System.Drawing.Size(193, 131);
            this.txtHelp.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "txt文件存放目录";
            // 
            // txtTxtFolder
            // 
            this.txtTxtFolder.Location = new System.Drawing.Point(98, 61);
            this.txtTxtFolder.Name = "txtTxtFolder";
            this.txtTxtFolder.Size = new System.Drawing.Size(434, 21);
            this.txtTxtFolder.TabIndex = 6;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(117, 110);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(415, 21);
            this.txtResult.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "分析结果存放位置";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 352);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(641, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(419, 319);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(53, 12);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "提示信息";
            // 
            // btnChooseTxtFolder
            // 
            this.btnChooseTxtFolder.Location = new System.Drawing.Point(547, 64);
            this.btnChooseTxtFolder.Name = "btnChooseTxtFolder";
            this.btnChooseTxtFolder.Size = new System.Drawing.Size(106, 23);
            this.btnChooseTxtFolder.TabIndex = 3;
            this.btnChooseTxtFolder.Text = "选择txt目录";
            this.btnChooseTxtFolder.UseVisualStyleBackColor = true;
            this.btnChooseTxtFolder.Click += new System.EventHandler(this.btnChooseTxtFolder_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(240, 173);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(79, 50);
            this.btnConvert.TabIndex = 0;
            this.btnConvert.Text = "pdf转txt";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnAnaylse
            // 
            this.btnAnaylse.Location = new System.Drawing.Point(325, 173);
            this.btnAnaylse.Name = "btnAnaylse";
            this.btnAnaylse.Size = new System.Drawing.Size(79, 50);
            this.btnAnaylse.TabIndex = 0;
            this.btnAnaylse.Text = "分析txt";
            this.btnAnaylse.UseVisualStyleBackColor = true;
            this.btnAnaylse.Click += new System.EventHandler(this.btnAnayle_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(547, 110);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(106, 23);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpenResult_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 412);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtTxtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHelp);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnChooseTxtFolder);
            this.Controls.Add(this.btnChooseFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPdfFolderName);
            this.Controls.Add(this.btnAnaylse);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnRead);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(694, 450);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(694, 450);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "读取议案投票信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TextBox txtPdfFolderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.TextBox txtHelp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTxtFolder;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnChooseTxtFolder;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnAnaylse;
        private System.Windows.Forms.Button btnOpen;
    }
}

