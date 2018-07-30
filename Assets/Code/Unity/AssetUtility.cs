using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetUtility
{
    public static string[] GetFilesAtPath(string path, string[] extensions)
    {
        Debug.Log("GetFiles => " + Application.dataPath + "/" + path);

        ArrayList arraylist = new ArrayList();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path, "*", SearchOption.AllDirectories);

        foreach (string fileName in fileEntries)
        {
            Debug.Log("filename => " + fileName);
            string ext = Path.GetExtension(fileName).ToLower();
            Debug.Log("ext => " + ext);

            foreach (string filter in extensions)
            {
                if (ext.Contains(filter.ToLower()))
                {
                    string localPath = fileName.Substring(fileName.IndexOf("Assets"));

                    Debug.Log("localPath => " + localPath);
                    arraylist.Add(localPath);
                }
            }
        }

        string[] result = new string[arraylist.Count];
        for (int i = 0; i < arraylist.Count; i++)
            result[i] = (string)arraylist[i];

        return result;
    }
}