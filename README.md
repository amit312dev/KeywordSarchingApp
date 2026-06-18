# Keyword Searching Application

A high-performance, offline **Windows Forms (C#)** desktop utility designed to scan local text/CSV files for keywords. It features semantic and phonetic analysis using industry-standard datasets to execute advanced search modes smoothly without any UI flickering.

---

## 🌟 Key Features

*   **Four Advanced Search Modes**:
    *   `Exact Word`: Matches whole words only using regex boundaries (`\b`).
    *   `Contains`: Finds substring pattern matches anywhere inside the line text.
    *   `Synonyms`: Dynamically matches relational concept trees powered by **Princeton WordNet**.
    *   `Homonyms`: Maps similar-sounding words via custom datasets (`homophones.txt`).
*   **Scoped Multi-Tier UI Resets**: 
    *   `Clear Text`: Clears only the paragraph viewer to prevent data loss.
    *   `Reset Search`: Resets search parameters and keywords back to default configurations.
    *   `Clear All`: Fully flushes all fields, data arrays, and status banners.
*   **Excel-Friendly Exporter**: Saves search results directly into clean, comma-separated `.csv` sheets with embedded quote protections.
*   **Zero UI Flickering**: Implements a native Win32 `WM_SETREDRAW` control override mechanism to freeze paint frames during heavy keyword highlighting loops.

---

## 🛠️ Built With

*   **Language**: C# (.NET Framework / .NET Core Windows Forms)
*   **Database Management Engine**: System.Data.SQLite (Direct Relational Engine)
*   **Lexical Databases**: 
    *   [Princeton University WordNet Relational Dataset](https://princeton.edu) (436 MB Offline SQLite Matrix)
    *   Dynamic Homophones Word Mapping Assets (`homophones.txt`)

---

## 🚀 How to Setup and Run

1.  **Clone the Repository**:
    ```bash
    git clone https://github.com
    ```
2.  **Add Database and Configuration Files**:
    *   Extract your downloaded Princeton WordNet SQLite database file, rename it to exactly `wordnet.db`, and drop it directly into your application output folder: `bin/Debug/`.
    *   Place your formatted `homophones.txt` file into the same `bin/Debug/` directory.
3.  **Open and Rebuild**:
    *   Launch the solution file (`.sln`) inside Visual Studio.
    *   Press `Ctrl + Shift + B` to restore package dependencies (`System.Data.SQLite`) and compile the software workspace safely.
4.  **Launch the App**:
    *   Click the **Start/Debug** play button to open the dashboard workspace interface!

---

## 📊 Application Visual Architecture

*   **Offline First**: Zero web-service or API dependencies. Fully functional on remote systems or air-gapped computers.
*   **Hybrid Memory Management**: Swaps heavy runtime JSON line looping for microsecond indexed database `JOIN` queries.
