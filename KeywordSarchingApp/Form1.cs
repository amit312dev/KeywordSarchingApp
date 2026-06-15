using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;


namespace KeywordSarchingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Automatically select the first option (Exact Word) when the form loads
            if (cmbSearchOptions.Items.Count > 0)
            {
                cmbSearchOptions.SelectedIndex = 0;
            }
        }

        // 1. Browse button logic (To pick a CSV or Text file)
        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        // 2. Load button logic (To display the file content inside the RichTextBox)
        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid CSV or Text file first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Read all text from the file and load it into the RichTextBox
            richTextBoxInput.Text = File.ReadAllText(txtFilePath.Text);
            lblStatus.Text = "Search Status: File successfully loaded.";
        }

        // 3. Main Search button logic
        // 3. Main Search button logic (Fixed for CSV Delimiters using Regex)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();
            string fullText = richTextBoxInput.Text;

            if (cmbSearchOptions.SelectedItem == null)
            {
                MessageBox.Show("Please select a search option first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedOption = cmbSearchOptions.SelectedItem.ToString();

            // Clear previous highlights and reset the results grid
            richTextBoxInput.SelectAll();
            richTextBoxInput.SelectionBackColor = richTextBoxInput.BackColor;
            dgvResults.Rows.Clear();

            if (string.IsNullOrEmpty(keyword))
            {
                lblStatus.Text = "Search Status: Please enter a keyword to search.";
                return;
            }

            // Standardize line breaks to handle unique CSV and notepad carriage returns safely
            string[] lines = fullText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int matchCount = 0;

            // Prepare Regex pattern for Exact Word boundary matching (\b handles spaces, commas, quotes, etc.)
            string exactWordPattern = @"\b" + Regex.Escape(keyword) + @"\b";

            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i].Trim();

                // Skip empty lines
                if (string.IsNullOrEmpty(currentLine)) continue;

                bool isMatch = false;

                // Adjust search logic based on the drop-down selection
                if (selectedOption == "Exact Word")
                {
                    // Matches exact word even if surrounded by commas, quotes, or spaces
                    isMatch = Regex.IsMatch(currentLine, exactWordPattern, RegexOptions.IgnoreCase);
                }
                else if (selectedOption == "Contains")
                {
                    // Matches if the keyword is part of any word or section
                    isMatch = currentLine.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                else if (selectedOption == "Synonyms" || selectedOption == "synonyms")
                {
                    isMatch = currentLine.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                else if (selectedOption == "Homonyms" || selectedOption == "homonyms")
                {
                    isMatch = currentLine.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                // If a matching row is found, append it to your DataGridView
                if (isMatch)
                {
                    matchCount++;
                    dgvResults.Rows.Add(i + 1, currentLine);
                }
            }

            // Visual highlight pass: Apply yellow background to matching keywords
            int index = 0;
            while ((index = fullText.IndexOf(keyword, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                richTextBoxInput.Select(index, keyword.Length);
                richTextBoxInput.SelectionBackColor = Color.Yellow;
                index += keyword.Length;
            }

            // Remove blue selection overlay
            richTextBoxInput.DeselectAll();

            // Update status banner
            if (matchCount > 0)
            {
                lblStatus.Text = $"Search Status: Search completed! Found in {matchCount} lines.";
            }
            else
            {
                lblStatus.Text = "Search Status: No matches found.";
            }
        }


    }
}
