using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;

// Этот класс является расширением для AutoCAD
public class CSVReaderCommands
{
    // Команда для открытия CSV файла и чтения данных
    [CommandMethod("OpenCSV")]
    public void OpenCSVCommand()
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = doc.Editor;

        // Настройка диалога для выбора файла с использованием AutoCAD API (форма Select CSV file)
        PromptOpenFileOptions pfo = new PromptOpenFileOptions("Select CSV file")
        {
            Filter = "CSV files (*.csv)|*.csv",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        PromptFileNameResult pfr = ed.GetFileNameForOpen(pfo);

        if (pfr.Status != PromptStatus.OK)
        {
            ed.WriteMessage("\nNo file selected.");
            return;
        }

        string filePath = pfr.StringResult;
        List<string[]> csvData = ReadCSVFile(filePath);

        foreach (string[] row in csvData)
        {
            // Выводим данные в командную строку AutoCAD
            ed.WriteMessage("\n" + string.Join(", ", row));
        }
    }

    // Метод для чтения данных из файла CSV
    private List<string[]> ReadCSVFile(string filePath)
    {
        List<string[]> csvData = new List<string[]>();
        using (StreamReader sr = new StreamReader(filePath))
        {
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(';');
                if (line.Length > 0 && line[line.Length - 1] == "")
                {
                    Array.Resize(ref line, line.Length - 1);
                }
                csvData.Add(line);
            }
        }
        return csvData;
    }
}