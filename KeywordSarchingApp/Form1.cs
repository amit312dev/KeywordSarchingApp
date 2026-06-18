using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace KeywordSarchingApp
{
    public partial class Form1 : Form
    {
        // 1. Direct connection link pointing to your 436 MB offline relational WordNet database
        private string wordNetDbConnection = "Data Source=wordnet.db;Version=3;";

        public Form1()
        {
            InitializeComponent();

            // Automatically select the first option (Exact Word) when the form loads
            if (cmbSearchOptions.Items.Count > 0)
            {
                cmbSearchOptions.SelectedIndex = 0;
            }
        }

        // 2. Browse button logic (To pick a CSV or Text file)
        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        // 3. Load button logic (To display the file content inside the RichTextBox)
        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid CSV or Text file first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            richTextBoxInput.Text = File.ReadAllText(txtFilePath.Text);
            lblStatus.Text = "Search Status: File successfully loaded.";
        }

        // 4. Highly optimized relational database query for Princeton WordNet
        private List<string> GetSynonymsFromDb(string word)
        {
            List<string> synonyms = new List<string> { word.Trim().ToLower() };

            // Maps user input through WordNet's linked concept directory tables
            string query = @"
                SELECT DISTINCT w2.lemma 
                FROM words w1
                JOIN senses s1 ON w1.wordid = s1.wordid
                JOIN senses s2 ON s1.synsetid = s2.synsetid
                JOIN words w2 ON s2.wordid = w2.wordid
                WHERE w1.lemma = @word AND w2.lemma != @word;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(wordNetDbConnection))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@word", word.Trim().ToLower());

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string syn = reader["lemma"].ToString().Trim().ToLower();

                                // WordNet stores phrases using underscores (e.g., clever_boy). 
                                // strip underscores/hyphens to ensure only single standalone words are matched.
                                if (!string.IsNullOrEmpty(syn) && !syn.Contains(" ") && !syn.Contains("_") && !syn.Contains("-"))
                                {
                                    if (!synonyms.Contains(syn)) synonyms.Add(syn);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("WordNet Synset Query Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return synonyms;
        }

        // 5. Fetch Homonyms using your structured local text file mapping
        private List<string> GetHomonymsFromDb(string word)
        {
            List<string> homonyms = new List<string> { word.Trim().ToLower() };
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "homophones.txt");

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    string searchWord = word.Trim().ToLower();

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        List<string> wordGroup = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(w => w.Trim().ToLower())
                                                     .ToList();

                        if (wordGroup.Contains(searchWord))
                        {
                            foreach (string groupWord in wordGroup)
                            {
                                if (!homonyms.Contains(groupWord)) homonyms.Add(groupWord);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading homophones file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return homonyms;
        }

        // 6. Main Search button logic processing execution blocks cleanly
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();
            string fullText = richTextBoxInput.Text;

            if (cmbSearchOptions.SelectedItem == null)
            {
                MessageBox.Show("Please select a search option first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Normalise selection string to absolute lowercase to prevent comparison failures
            string selectedOption = cmbSearchOptions.SelectedItem.ToString().Trim().ToLower();

            // Clear previous user background selections and clean the results DataGridView
            richTextBoxInput.SelectAll();
            richTextBoxInput.SelectionBackColor = richTextBoxInput.BackColor;
            dgvResults.Rows.Clear();

            if (string.IsNullOrEmpty(keyword))
            {
                lblStatus.Text = "Search Status: Please enter a keyword to search.";
                return;
            }

            string[] lines = fullText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int matchCount = 0;
            string searchPattern = "";

            if (selectedOption.Contains("exact"))
            {
                searchPattern = @"\b" + Regex.Escape(keyword) + @"\b";
            }
            else if (selectedOption.Contains("contain"))
            {
                searchPattern = Regex.Escape(keyword);
            }
            else if (selectedOption.Contains("synonym"))
            {
                List<string> synonymList = GetSynonymsFromDb(keyword);

                // 🚨 SEARCH LOGGER POPUP: Displays what the WordNet database pulled
                string testLog = string.Join(", ", synonymList);
                MessageBox.Show($"Search Term: '{keyword}'\nWords fetched from offline WordNet: [{testLog}]",
                                "WordNet Log", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string combinedWords = string.Join("|", synonymList.Where(s => !string.IsNullOrEmpty(s)).Select(Regex.Escape));
                searchPattern = !string.IsNullOrEmpty(combinedWords) ? @"\b(" + combinedWords + @")\b" : @"\b" + Regex.Escape(keyword) + @"\b";
            }
            else if (selectedOption.Contains("homonym"))
            {
                List<string> homonymList = GetHomonymsFromDb(keyword);
                string combinedWords = string.Join("|", homonymList.Where(h => !string.IsNullOrEmpty(h)).Select(Regex.Escape));
                searchPattern = !string.IsNullOrEmpty(combinedWords) ? @"\b(" + combinedWords + @")\b" : @"\b" + Regex.Escape(keyword) + @"\b";
            }

            // Fallback safety layer to handle empty bracket errors safely
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == @"\b()\b")
            {
                searchPattern = @"\b" + Regex.Escape(keyword) + @"\b";
            }

            // 7. Grid Generation Loop
            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i].Trim();
                if (string.IsNullOrEmpty(currentLine)) continue;

                if (Regex.IsMatch(currentLine, searchPattern, RegexOptions.IgnoreCase))
                {
                    matchCount++;
                    dgvResults.Rows.Add(i + 1, currentLine);
                }
            }

            // 8. Stable Paint Highlighting Loop
            if (matchCount > 0)
            {
                MatchCollection matches = Regex.Matches(fullText, searchPattern, RegexOptions.IgnoreCase);

                richTextBoxInput.BeginControlUpdate();
                foreach (Match match in matches)
                {
                    richTextBoxInput.Select(match.Index, match.Length);
                    richTextBoxInput.SelectionBackColor = Color.Yellow;
                }
                richTextBoxInput.EndControlUpdate();
            }

            richTextBoxInput.DeselectAll();

            lblStatus.Text = matchCount > 0
                ? $"Search Status: Completed! Found in {matchCount} lines."
                : "Search Status: No matches found.";
        }

        // 9. Clear button logic to completely reset the search keyword and drop down
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchKeyword.Clear();

            if (cmbSearchOptions.Items.Count > 0)
            {
                cmbSearchOptions.SelectedIndex = 0;
            }

            lblStatus.Text = "Search Status: Search parameters reset.";
        }


        // 10. Export button logic to save the filtered DataGridView rows to a CSV file
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvResults.Rows.Count == 0)
            {
                MessageBox.Show("There are no search results to export!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt";
            saveFileDialog.Title = "Export Search Results";
            saveFileDialog.FileName = "Search_Results.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Set standard CSV tabular header metrics
                        sw.WriteLine("Line Number,Matched Sentence");

                        foreach (DataGridViewRow row in dgvResults.Rows)
                        {
                            if (row.IsNewRow) continue;

                            // Explicitly map cell coordinates to grab keys flawlessly
                            string lineNum = row.Cells[0].Value?.ToString() ?? "";
                            string sentence = row.Cells[1].Value?.ToString() ?? "";

                            // Fixed: Correctly escape literal double quotes for standard compilation rules
                            if (sentence.Contains(","))
                            {
                                sentence = "\"" + sentence.Replace("\"", "\"\"") + "\"";
                            }

                            sw.WriteLine($"{lineNum},{sentence}");
                        }
                    }

                    MessageBox.Show("Results successfully exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to export files: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClearText_Click(object sender, EventArgs e)
        {
            richTextBoxInput.Clear();
            lblStatus.Text = "Search Status: Text area cleared.";
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            
            richTextBoxInput.Clear();
            txtSearchKeyword.Clear();
            txtFilePath.Clear();

            if (cmbSearchOptions.Items.Count > 0)
            {
                cmbSearchOptions.SelectedIndex = 0;
            }

            dgvResults.Rows.Clear();

            lblStatus.Text = "Search Status: Idle. Application fully reset.";

            MessageBox.Show("All fields and results have been successfully cleared!", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // Win32 Message Handle wrapper library extensions to freeze visual control flickering
    public static class RichTextBoxExtensions
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public static void BeginControlUpdate(this Control control)
        {
            SendMessage(control.Handle, 0x000B, IntPtr.Zero, IntPtr.Zero);
        }

        public static void EndControlUpdate(this Control control)
        {
            SendMessage(control.Handle, 0x000B, new IntPtr(1), IntPtr.Zero);
            control.Invalidate();
        }
    }
}

