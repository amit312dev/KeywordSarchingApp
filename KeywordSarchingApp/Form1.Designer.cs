namespace KeywordSarchingApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnClearText = new Button();
            label6 = new Label();
            panel4 = new Panel();
            label5 = new Label();
            btnBrowse = new Button();
            btnLoad = new Button();
            txtFilePath = new TextBox();
            label4 = new Label();
            richTextBoxInput = new RichTextBox();
            label1 = new Label();
            panel2 = new Panel();
            btnClear = new Button();
            groupBox2 = new GroupBox();
            lblStatus = new Label();
            groupBox1 = new GroupBox();
            cmbSearchOptions = new ComboBox();
            btnSearch = new Button();
            groupBoxSearchKeyword = new GroupBox();
            txtSearchKeyword = new TextBox();
            label2 = new Label();
            panel3 = new Panel();
            btnClearAll = new Button();
            btnExport = new Button();
            groupBox3 = new GroupBox();
            dgvResults = new DataGridView();
            colLine = new DataGridViewTextBoxColumn();
            colPreview = new DataGridViewTextBoxColumn();
            label3 = new Label();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBoxSearchKeyword.SuspendLayout();
            panel3.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnClearText);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(richTextBoxInput);
            panel1.Controls.Add(label1);
            panel1.ForeColor = Color.DarkGray;
            panel1.Location = new Point(15, 15);
            panel1.Name = "panel1";
            panel1.Size = new Size(674, 683);
            panel1.TabIndex = 0;
            // 
            // btnClearText
            // 
            btnClearText.BackColor = Color.Gainsboro;
            btnClearText.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearText.ForeColor = Color.Black;
            btnClearText.Location = new Point(571, 512);
            btnClearText.Name = "btnClearText";
            btnClearText.Size = new Size(94, 47);
            btnClearText.TabIndex = 10;
            btnClearText.Text = "Clear Text";
            btnClearText.UseVisualStyleBackColor = false;
            btnClearText.Click += btnClearText_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(15, 51);
            label6.Name = "label6";
            label6.Size = new Size(168, 25);
            label6.TabIndex = 8;
            label6.Text = "Type or Paste Text";
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label5);
            panel4.Controls.Add(btnBrowse);
            panel4.Controls.Add(btnLoad);
            panel4.Controls.Add(txtFilePath);
            panel4.Controls.Add(label4);
            panel4.Location = new Point(3, 562);
            panel4.Name = "panel4";
            panel4.Size = new Size(666, 113);
            panel4.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(154, 25);
            label5.TabIndex = 11;
            label5.Text = "Import CSV Data";
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.LightGray;
            btnBrowse.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBrowse.ForeColor = Color.Black;
            btnBrowse.Location = new Point(453, 43);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(96, 48);
            btnBrowse.TabIndex = 10;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += btnBrowse_Click_1;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.LightSkyBlue;
            btnLoad.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoad.ForeColor = Color.Black;
            btnLoad.Location = new Point(555, 43);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(94, 48);
            btnLoad.TabIndex = 9;
            btnLoad.Text = "Load Text";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click_1;
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(111, 43);
            txtFilePath.Multiline = true;
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(336, 48);
            txtFilePath.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(-2, 55);
            label4.Name = "label4";
            label4.Size = new Size(114, 23);
            label4.TabIndex = 7;
            label4.Text = "CSV File Path:";
            // 
            // richTextBoxInput
            // 
            richTextBoxInput.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxInput.Location = new Point(3, 77);
            richTextBoxInput.Name = "richTextBoxInput";
            richTextBoxInput.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            richTextBoxInput.Size = new Size(666, 434);
            richTextBoxInput.TabIndex = 1;
            richTextBoxInput.Text = "";
            // 
            // label1
            // 
            label1.BackColor = Color.SteelBlue;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(672, 39);
            label1.TabIndex = 0;
            label1.Text = "1. Input Text Source";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(btnClear);
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(groupBox1);
            panel2.Controls.Add(btnSearch);
            panel2.Controls.Add(groupBoxSearchKeyword);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(695, 16);
            panel2.Name = "panel2";
            panel2.Size = new Size(672, 200);
            panel2.TabIndex = 1;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.LightYellow;
            btnClear.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClear.ForeColor = Color.Black;
            btnClear.Location = new Point(569, 71);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(94, 51);
            btnClear.TabIndex = 5;
            btnClear.Text = "Reset";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblStatus);
            groupBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(3, 136);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(660, 59);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Search Status";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.Location = new Point(14, 33);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(156, 23);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Waiting for search...";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cmbSearchOptions);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(270, 50);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(170, 80);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Search Options";
            // 
            // cmbSearchOptions
            // 
            cmbSearchOptions.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSearchOptions.FormattingEnabled = true;
            cmbSearchOptions.Items.AddRange(new object[] { "Exact Word", "Contains", "Synonyms ", "Homonyms " });
            cmbSearchOptions.Location = new Point(6, 34);
            cmbSearchOptions.Name = "cmbSearchOptions";
            cmbSearchOptions.Size = new Size(158, 33);
            cmbSearchOptions.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.Navy;
            btnSearch.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(460, 70);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 51);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // groupBoxSearchKeyword
            // 
            groupBoxSearchKeyword.Controls.Add(txtSearchKeyword);
            groupBoxSearchKeyword.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxSearchKeyword.Location = new Point(3, 50);
            groupBoxSearchKeyword.Name = "groupBoxSearchKeyword";
            groupBoxSearchKeyword.Size = new Size(250, 80);
            groupBoxSearchKeyword.TabIndex = 1;
            groupBoxSearchKeyword.TabStop = false;
            groupBoxSearchKeyword.Text = "Enter Keyword to Search";
            // 
            // txtSearchKeyword
            // 
            txtSearchKeyword.Location = new Point(14, 34);
            txtSearchKeyword.Name = "txtSearchKeyword";
            txtSearchKeyword.Size = new Size(230, 31);
            txtSearchKeyword.TabIndex = 0;
            // 
            // label2
            // 
            label2.BackColor = Color.SteelBlue;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(670, 39);
            label2.TabIndex = 0;
            label2.Text = "2. Search Keyword";
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(btnClearAll);
            panel3.Controls.Add(btnExport);
            panel3.Controls.Add(groupBox3);
            panel3.Controls.Add(label3);
            panel3.Location = new Point(695, 230);
            panel3.Name = "panel3";
            panel3.Size = new Size(671, 468);
            panel3.TabIndex = 2;
            // 
            // btnClearAll
            // 
            btnClearAll.BackColor = Color.MistyRose;
            btnClearAll.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearAll.ForeColor = Color.Black;
            btnClearAll.Location = new Point(367, 426);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.Size = new Size(118, 41);
            btnClearAll.TabIndex = 7;
            btnClearAll.Text = "Clear All";
            btnClearAll.UseVisualStyleBackColor = false;
            btnClearAll.Click += btnClearAll_Click;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.LightGreen;
            btnExport.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExport.ForeColor = Color.Black;
            btnExport.Location = new Point(231, 426);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(118, 41);
            btnExport.TabIndex = 6;
            btnExport.Text = "Save to CSV";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dgvResults);
            groupBox3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(0, 42);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(666, 382);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Matching Lines (Preview)";
            // 
            // dgvResults
            // 
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Columns.AddRange(new DataGridViewColumn[] { colLine, colPreview });
            dgvResults.Location = new Point(6, 29);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersVisible = false;
            dgvResults.RowHeadersWidth = 51;
            dgvResults.Size = new Size(660, 380);
            dgvResults.TabIndex = 0;
            // 
            // colLine
            // 
            colLine.HeaderText = "Line #";
            colLine.MinimumWidth = 6;
            colLine.Name = "colLine";
            colLine.ReadOnly = true;
            // 
            // colPreview
            // 
            colPreview.HeaderText = "Content Preview";
            colPreview.MinimumWidth = 6;
            colPreview.Name = "colPreview";
            colPreview.ReadOnly = true;
            // 
            // label3
            // 
            label3.BackColor = Color.SteelBlue;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(669, 39);
            label3.TabIndex = 0;
            label3.Text = "3. Search Results & CSV Preview";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 704);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBoxSearchKeyword.ResumeLayout(false);
            groupBoxSearchKeyword.PerformLayout();
            panel3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Label label2;
        private Panel panel3;
        private Label label3;
        private RichTextBox richTextBoxInput;
        private Panel panel4;
        private Label label5;
        private Button btnBrowse;
        private Button btnLoad;
        private TextBox txtFilePath;
        private Label label4;
        private Label label6;
        private GroupBox groupBoxSearchKeyword;
        private Button btnSearch;
        private TextBox txtSearchKeyword;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private Label lblStatus;
        private ComboBox cmbSearchOptions;
        private GroupBox groupBox3;
        private DataGridView dgvResults;
        private DataGridViewTextBoxColumn colLine;
        private DataGridViewTextBoxColumn colPreview;
        private Button btnClear;
        private Button btnExport;
        private Button btnClearText;
        private Button btnClearAll;
    }
}
