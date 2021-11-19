using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public static class FileListener 
{
    public static bool fileOpen = false;
    public static string creatFile(string filePath, string extension)
    {
        List<int> files = getFileName(filePath, extension);
        files.Sort();
        if(files.Count > 0)
        {
            filePath = filePath + (files[files.Count-1]+1).ToString() + "." + extension;
        }
        else
        {
            filePath = filePath +  "1." + extension;
        }

        if (!File.Exists(filePath))
        {
            StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine("Comfort Level");
            writer.Flush();
            writer.Close();
        }

        return filePath;
    }


    public static void updateFile(string filePath, ref int result)
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown("[" + i.ToString() + "]"))
            {
                result = result * 10 + i;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && fileOpen)
        {
            Debug.Log("Result: " + result);
            File.AppendAllText(filePath, result.ToString() + Environment.NewLine);
            result = 0;
            fileOpen = false;
        }
    }

    public static List<int> getFileName(string path, string extension)
    {
        List<int> fileNames = new List<int>();
        DirectoryInfo dir = new DirectoryInfo(path);
        string fileType = "*." + extension; 
        FileInfo[] files = dir.GetFiles(fileType);
        foreach(FileInfo fi in files)
        {
            int fiNumber = Convert.ToInt32(Path.GetFileNameWithoutExtension(fi.Name));
            fileNames.Add(fiNumber);
        }
        return fileNames;
    }

}
