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
        // Connection string pointing to your downloaded database file inside the bin/Debug folder
        private string dbConnectionString = "Data Source=dictionary.db;Version=3;";

        public Form1()
        {
            InitializeComponent();

            // Automatically select the first option (Exact Word) when the form loads
            if (cmbSearchOptions.Items.Count > 0)
            {
                cmbSearchOptions.SelectedIndex = 0;
            }
        }

        // Browse button logic (To pick a CSV or Text file)
        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        // Load button logic (To display the file content inside the RichTextBox)
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

        // Fetch Synonyms from the pre-made SQLite DB
        private List<string> GetSynonymsFromDb(string word)
        {
            HashSet<string> collectedSynonyms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string searchWord = word.Trim().ToLower();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "en_thesaurus.jsonl");

            // Always include the original search word
            collectedSynonyms.Add(searchWord);

            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("The file 'en_thesaurus.jsonl' was not found in the bin/Debug folder!", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return collectedSynonyms.ToList();
                }

                // =======================================================
                // PASS 1: Pull immediate core cluster matches
                // =======================================================
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(line))
                        {
                            System.Text.Json.JsonElement root = doc.RootElement;
                            string mainWord = root.GetProperty("word").GetString().ToLower().Trim();
                            System.Text.Json.JsonElement synonymsArray = root.GetProperty("synonyms");

                            bool directHit = (mainWord == searchWord);
                            List<string> tempRow = new List<string>();

                            foreach (System.Text.Json.JsonElement elem in synonymsArray.EnumerateArray())
                            {
                                string syn = elem.GetString().ToLower().Trim();
                                if (!syn.Contains(" ") && !syn.Contains("-") && !string.IsNullOrEmpty(syn))
                                {
                                    tempRow.Add(syn);
                                    if (syn == searchWord) directHit = true;
                                }
                            }

                            if (directHit)
                            {
                                if (!mainWord.Contains(" ") && !mainWord.Contains("-")) collectedSynonyms.Add(mainWord);
                                foreach (string tr in tempRow) collectedSynonyms.Add(tr);
                            }
                        }
                    }
                }

                // =======================================================
                // PASS 2: Tight Multi-Word Intersection check
                // =======================================================
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(line))
                        {
                            System.Text.Json.JsonElement root = doc.RootElement;
                            string mainWord = root.GetProperty("word").GetString().ToLower().Trim();
                            System.Text.Json.JsonElement synonymsArray = root.GetProperty("synonyms");

                            List<string> tempRow = new List<string>();
                            int overlapCount = collectedSynonyms.Contains(mainWord) ? 1 : 0;

                            foreach (System.Text.Json.JsonElement elem in synonymsArray.EnumerateArray())
                            {
                                string syn = elem.GetString().ToLower().Trim();
                                if (!syn.Contains(" ") && !syn.Contains("-") && !string.IsNullOrEmpty(syn))
                                {
                                    tempRow.Add(syn);
                                    if (collectedSynonyms.Contains(syn))
                                    {
                                        overlapCount++;
                                    }
                                }
                            }

                            // CRITICAL FILTER RULE:
                            // If the keyword itself is a direct match, or if this row strongly overlaps (shares 2+ related words)
                            // with our found terms, we safely pull the entire synonyms tree without drifting away.
                            if ((mainWord == searchWord) || (overlapCount >= 2))
                            {
                                if (!mainWord.Contains(" ") && !mainWord.Contains("-")) collectedSynonyms.Add(mainWord);
                                foreach (string tr in tempRow) collectedSynonyms.Add(tr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing synonyms dictionary tree: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return collectedSynonyms.ToList();
        }

        // Converts a word into its standard 4-character Soundex phonetic code
        private string GetSoundexCode(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return "0000";

            string val = word.Trim().ToUpper();
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            result.Append(val[0]); // Keep the first letter

            string previousCode = GetSoundexDigit(val[0]);

            for (int i = 1; i < val.Length; i++)
            {
                string currentCode = GetSoundexDigit(val[i]);

                if (currentCode != "0" && currentCode != previousCode)
                {
                    result.Append(currentCode);
                }

                if (currentCode != "0")
                {
                    previousCode = currentCode;
                }

                if (result.Length == 4) break;
            }

            while (result.Length < 4)
            {
                result.Append('0');
            }

            return result.ToString();
        }

        private string GetSoundexDigit(char c)
        {
            if ("BFPV".Contains(c.ToString())) return "1";
            if ("CGJKQSXZ".Contains(c.ToString())) return "2";
            if ("DT".Contains(c.ToString())) return "3";
            if ("L".Contains(c.ToString())) return "4";
            if ("MN".Contains(c.ToString())) return "5";
            if ("R".Contains(c.ToString())) return "6";
            return "0";
        }

        // Fetch Homonyms using local C# phonetic evaluation
        private List<string> GetHomonymsFromDb(string word)
        {
            List<string> homonyms = new List<string>();
            string searchWord = word.Trim().ToLower();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "homophones.txt");

            // Safety fallback: Always keep the user's primary word in the result array
            homonyms.Add(searchWord);

            try
            {
                if (File.Exists(filePath))
                {
                    // Read dataset line by line
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Split by spaces or commas to match formatting
                        List<string> wordGroup = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(w => w.Trim().ToLower())
                                                     .ToList();

                        // If our search word is part of this homophone line group, load them all
                        if (wordGroup.Contains(searchWord))
                        {
                            foreach (string groupWord in wordGroup)
                            {
                                if (!homonyms.Contains(groupWord))
                                {
                                    homonyms.Add(groupWord);
                                }
                            }
                            // Keep scanning other lines in case the word belongs to multiple phonetic groupings
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

        // Main Search button logic
        // Main Search button logic
        // 5. Main Search button logic
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();
            string fullText = richTextBoxInput.Text;

            if (cmbSearchOptions.SelectedItem == null)
            {
                MessageBox.Show("Please select a search option first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // [PASTED HERE]: Normalize selection string to absolute lowercase to prevent comparison failures
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

            // Initialize empty regular expression pattern string
            string searchPattern = "";

            // [PASTED HERE]: Conditional checks to assign correct regex pattern
            if (selectedOption == "exact word" || selectedOption.Contains("exact"))
            {
                searchPattern = @"\b" + Regex.Escape(keyword) + @"\b";
            }
            else if (selectedOption == "contains")
            {
                searchPattern = Regex.Escape(keyword);
            }
            else if (selectedOption == "synonyms" || selectedOption.Contains("synonym"))
            {
                List<string> synonymList = GetSynonymsFromDb(keyword);
                if (!synonymList.Contains(keyword, StringComparer.OrdinalIgnoreCase)) synonymList.Add(keyword);

                string combinedWords = string.Join("|", synonymList.Where(s => !string.IsNullOrEmpty(s)).Select(Regex.Escape));
                searchPattern = !string.IsNullOrEmpty(combinedWords) ? @"\b(" + combinedWords + @")\b" : @"\b" + Regex.Escape(keyword) + @"\b";
            }
            else if (selectedOption == "homonyms" || selectedOption.Contains("homonym"))
            {
                List<string> homonymList = GetHomonymsFromDb(keyword);
                if (!homonymList.Contains(keyword, StringComparer.OrdinalIgnoreCase)) homonymList.Add(keyword);

                string combinedWords = string.Join("|", homonymList.Where(h => !string.IsNullOrEmpty(h)).Select(Regex.Escape));
                searchPattern = !string.IsNullOrEmpty(combinedWords) ? @"\b(" + combinedWords + @")\b" : @"\b" + Regex.Escape(keyword) + @"\b";
            }

            // Global safety protection (Fallback if everything fails)
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == @"\b()\b")
            {
                searchPattern = @"\b" + Regex.Escape(keyword) + @"\b";
            }

            // 6. Grid Population Loop
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

            // 7. Stable Highlighting Loop
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

            // Remove structural focus overlays from text ranges
            richTextBoxInput.DeselectAll();

            // Status message banner updates
            lblStatus.Text = matchCount > 0
                ? $"Search Status: Completed! Found in {matchCount} lines."
                : "Search Status: No matches found.";
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

