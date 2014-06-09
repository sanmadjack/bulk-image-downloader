namespace JuiceBoxGalleryDownloader {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.urlsTxt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.goBtn = new System.Windows.Forms.Button();
            this.outputTxt = new System.Windows.Forms.TextBox();
            this.filesProg = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dirTxt = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.urlsProg = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoShimmie = new System.Windows.Forms.RadioButton();
            this.rdoJuiceBox = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // urlsTxt
            // 
            this.urlsTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.urlsTxt.Location = new System.Drawing.Point(3, 16);
            this.urlsTxt.Multiline = true;
            this.urlsTxt.Name = "urlsTxt";
            this.urlsTxt.Size = new System.Drawing.Size(459, 69);
            this.urlsTxt.TabIndex = 0;
            this.urlsTxt.TextChanged += new System.EventHandler(this.urlsTxt_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.urlsTxt);
            this.groupBox1.Location = new System.Drawing.Point(105, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 88);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gallery URL(s)";
            // 
            // goBtn
            // 
            this.goBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.goBtn.Enabled = false;
            this.goBtn.Location = new System.Drawing.Point(12, 164);
            this.goBtn.Name = "goBtn";
            this.goBtn.Size = new System.Drawing.Size(558, 23);
            this.goBtn.TabIndex = 2;
            this.goBtn.Text = "Go!";
            this.goBtn.UseVisualStyleBackColor = true;
            this.goBtn.Click += new System.EventHandler(this.goBtn_Click);
            // 
            // outputTxt
            // 
            this.outputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTxt.Location = new System.Drawing.Point(12, 193);
            this.outputTxt.Multiline = true;
            this.outputTxt.Name = "outputTxt";
            this.outputTxt.ReadOnly = true;
            this.outputTxt.Size = new System.Drawing.Size(558, 124);
            this.outputTxt.TabIndex = 3;
            // 
            // filesProg
            // 
            this.filesProg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesProg.Location = new System.Drawing.Point(12, 352);
            this.filesProg.Name = "filesProg";
            this.filesProg.Size = new System.Drawing.Size(558, 23);
            this.filesProg.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.dirTxt);
            this.groupBox2.Location = new System.Drawing.Point(12, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(558, 51);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Download Folder";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(477, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dirTxt
            // 
            this.dirTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dirTxt.Location = new System.Drawing.Point(7, 20);
            this.dirTxt.Name = "dirTxt";
            this.dirTxt.ReadOnly = true;
            this.dirTxt.Size = new System.Drawing.Size(464, 20);
            this.dirTxt.TabIndex = 0;
            this.dirTxt.TextChanged += new System.EventHandler(this.dirTxt_TextChanged);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select Donwload Folder";
            // 
            // urlsProg
            // 
            this.urlsProg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlsProg.Location = new System.Drawing.Point(12, 323);
            this.urlsProg.Name = "urlsProg";
            this.urlsProg.Size = new System.Drawing.Size(558, 23);
            this.urlsProg.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoShimmie);
            this.groupBox3.Controls.Add(this.rdoJuiceBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(87, 88);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type";
            // 
            // rdoShimmie
            // 
            this.rdoShimmie.AutoSize = true;
            this.rdoShimmie.Checked = true;
            this.rdoShimmie.Location = new System.Drawing.Point(7, 19);
            this.rdoShimmie.Name = "rdoShimmie";
            this.rdoShimmie.Size = new System.Drawing.Size(64, 17);
            this.rdoShimmie.TabIndex = 1;
            this.rdoShimmie.TabStop = true;
            this.rdoShimmie.Text = "Shimmie";
            this.rdoShimmie.UseVisualStyleBackColor = true;
            // 
            // rdoJuiceBox
            // 
            this.rdoJuiceBox.AutoSize = true;
            this.rdoJuiceBox.Location = new System.Drawing.Point(7, 42);
            this.rdoJuiceBox.Name = "rdoJuiceBox";
            this.rdoJuiceBox.Size = new System.Drawing.Size(71, 17);
            this.rdoJuiceBox.TabIndex = 0;
            this.rdoJuiceBox.Text = "Juice Box";
            this.rdoJuiceBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 387);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.urlsProg);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.filesProg);
            this.Controls.Add(this.outputTxt);
            this.Controls.Add(this.goBtn);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "Form1";
            this.Text = "Gallery Downloader by Matthew Barbour";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox urlsTxt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button goBtn;
        private System.Windows.Forms.TextBox outputTxt;
        private System.Windows.Forms.ProgressBar filesProg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox dirTxt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ProgressBar urlsProg;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoShimmie;
        private System.Windows.Forms.RadioButton rdoJuiceBox;
    }
}

