# 🔍 Keyword Searching Application

A fast, offline **C# Windows Forms** desktop app to scan text and CSV files for keywords. It features advanced semantic matching, phonetic filters, and zero screen flickering.

---

## 🌟 Key Features

*   **Four Search Modes**:
    *   `Exact Word`: Matches whole words only using boundary lines (`\b`).
    *   `Contains`: Finds substring word combinations anywhere in a sentence.
    *   `Synonyms`: Uses **Princeton WordNet** to find matching definitions.
    *   `Homonyms`: Uses a local text list to match sounds-like words (e.g., right/write).
*   **Smart Clear Buttons (No Accidental Data Loss)**:
    *   `Clear Text`: Wipes only the paragraph viewer box.
    *   `Reset Search`: Resets search choices and inputs back to default.
    *   `Clear All`: Fully flushes all fields, data arrays, and status text.
*   **Save Results**: Exports search hits into clean Excel-friendly `.csv` sheets.
*   **Anti-Flicker Layout**: Stays smooth and steady during heavy color highlights.

---

## ⚙️ How to Setup and Run

Follow these exact steps to run the application on your computer:

### Step 1: Clone the Project
```bash
git clone https://github.com
```

### Step 2: Download the Offline WordNet Database
1. Click this link to start the download instantly: [Direct Download Link for sqlite-31.db.zip](https://sourceforge.net)
2. Save the compressed file named **`sqlite-31.db.zip`** (approx. 15MB).
3. Unzip the file on your computer to get the **`sqlite-31.db`** file (approx. 436MB).
4. Copy `sqlite-31.db`, paste it inside your project's output folder at **`bin/Debug/`**, and rename it to exactly: **`wordnet.db`**.

### Step 3: Verify Asset Configurations
*   Make sure your **`homophones.txt`** asset file is also sitting inside that same **`bin/Debug/`** output folder.

### Step 4: Open and Launch
1. Open the main solution file (`.sln`) inside **Visual Studio**.
2. Press **`Ctrl + Shift + B`** on your keyboard to restore dependencies (`System.Data.SQLite`) and compile the software.
3. Click the green **Start / Debug** play button to open your application!

---

## 🛡️ Git Protection Note
The massive 436MB `wordnet.db` file is safely blocked inside the `.gitignore` file. It will never upload to GitHub, protecting your cloud synchronization bounds from size limits.
