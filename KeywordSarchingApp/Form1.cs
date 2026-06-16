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
        // CRITICAL FIX: Keeping a global connection active keeps the In-Memory DB from vanishing!
        private string inMemoryDbConnection = "Data Source=:memory:;Version=3;New=True;Internal Connection=True;";
        private SQLiteConnection memConn;

        public Form1()
        {
            InitializeComponent();

            if (cmbSearchOptions.Items.Count > 0)
            {
                cmbSearchOptions.SelectedIndex = 0;
            }

            // Fire up and permanently lock the in-memory graph database channel
            InitializeInMemoryThesaurus();
        }

        private void InitializeInMemoryThesaurus()
        {
            try
            {
                // Instantiate the global master connection object
                memConn = new SQLiteConnection(inMemoryDbConnection);
                memConn.Open();

                string createTableQuery = @"
                    CREATE TABLE synonym_map (
                        keyword TEXT NOT NULL,
                        synonym TEXT NOT NULL
                    );
                    CREATE INDEX idx_keyword ON synonym_map(keyword);
                    CREATE INDEX idx_synonym ON synonym_map(synonym);";

                using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, memConn))
                {
                    cmd.ExecuteNonQuery();
                }

                string jsonlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "en_thesaurus.jsonl");
                if (File.Exists(jsonlPath))
                {
                    // Use the global live connection within a high-speed database transaction block
                    using (var transaction = memConn.BeginTransaction())
                    {
                        string line;
                        string insertSql = "INSERT INTO synonym_map (keyword, synonym) VALUES (@kw, @syn)";

                        using (SQLiteCommand insCmd = new SQLiteCommand(insertSql, memConn))
                        {
                            insCmd.Parameters.Add("@kw", System.Data.DbType.String);
                            insCmd.Parameters.Add("@syn", System.Data.DbType.String);

                            using (StreamReader sr = new StreamReader(jsonlPath))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (string.IsNullOrWhiteSpace(line)) continue;

                                    using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(line))
                                    {
                                        System.Text.Json.JsonElement root = doc.RootElement;
                                        string mainWord = root.GetProperty("word").GetString().ToLower().Trim();
                                        System.Text.Json.JsonElement synonymsArray = root.GetProperty("synonyms");

                                        foreach (System.Text.Json.JsonElement elem in synonymsArray.EnumerateArray())
                                        {
                                            string synWord = elem.GetString().ToLower().Trim();

                                            if (!string.IsNullOrEmpty(synWord) && !synWord.Contains(" ") && !synWord.Contains("-") && !mainWord.Contains(" ") && !mainWord.Contains("-"))
                                            {
                                                insCmd.Parameters["@kw"].Value = mainWord;
                                                insCmd.Parameters["@syn"].Value = synWord;
                                                insCmd.ExecuteNonQuery();

                                                insCmd.Parameters["@kw"].Value = synWord;
                                                insCmd.Parameters["@syn"].Value = mainWord;
                                                insCmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to index synonym structures: " + ex.Message, "Index Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

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

        private List<string> GetSynonymsFromDb(string word)
        {
            List<string> synonyms = new List<string> { word.Trim().ToLower() };

            if (memConn == null || memConn.State != System.Data.ConnectionState.Open)
                return synonyms;

            // CRITICAL FIX: Query using the active global memory instance directly
            string query = "SELECT DISTINCT synonym FROM synonym_map WHERE keyword = @word";

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(query, memConn))
                {
                    cmd.Parameters.AddWithValue("@word", word.Trim().ToLower());
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string syn = reader["synonym"].ToString().Trim().ToLower();
                            if (!synonyms.Contains(syn)) synonyms.Add(syn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read Error: " + ex.Message);
            }

            return synonyms;
        }

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();
            string fullText = richTextBoxInput.Text;

            if (cmbSearchOptions.SelectedItem == null)
            {
                MessageBox.Show("Please select a search option first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedOption = cmbSearchOptions.SelectedItem.ToString().Trim().ToLower();

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
                // 🚨 SEARCH LOGGER POPUP: See exactly what the live in-memory table sends back!
                string testLog = string.Join(", ", synonymList);
                MessageBox.Show($"Search Term: '{keyword}'\nWords fetched from persistent RAM table: [{testLog}]",
                                "Live Engine Log", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string combinedWords = string.Join("|", synonymList.Where(s => !string.IsNullOrEmpty(s)).Select(Regex.Escape));
                searchPattern = !string.IsNullOrEmpty(combinedWords) ? @"\b(" + combinedWords + @")\b" : @"\b" + Regex.Escape(keyword) + @"\b";
            }
            else if (selectedOption.Contains("homonym"))
            {
                // Fixed: Restored <string> type marker to prevent compile crashes
                List<string> homonymList = GetHomonymsFromDb(keyword);
                string combinedWords = string.Join("|", homonymList.Where(h => !string.IsNullOrEmpty(h)).Select(Regex.Escape));
                searchPattern = !string.IsNullOrEmpty(combinedWords) ? @"\b(" + combinedWords + @")\b" : @"\b" + Regex.Escape(keyword) + @"\b";
            }

            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == @"\b()\b")
            {
                searchPattern = @"\b" + Regex.Escape(keyword) + @"\b";
            }

            // Grid Generation Loop
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

            // Stable Paint Highlighting Loop
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Close down the persistent database channel safely when closing the UI window
            if (memConn != null)
            {
                if (memConn.State == System.Data.ConnectionState.Open) memConn.Close();
                memConn.Dispose();
            }
            base.OnFormClosing(e);
        }
    }

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



