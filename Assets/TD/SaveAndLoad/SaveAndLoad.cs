using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveAndLoad<T>
{
    private const int MaxSaveDataSize = 1024 * 1024; // Maximum allowed size for saving data using File.WriteAllBytes

    public static void Save(T data, string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        string jsonData = JsonUtility.ToJson(data, true);
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            File.WriteAllBytes(dataPath, byteData);
            Debug.Log("SAVEANDLOAD: Save data to: " + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to save data to: " + dataPath);
            Debug.LogError("SAVEANDLOAD: Error " + e.Message);
        }
    }

    public static T Load(string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("SAVEANDLOAD: File or path does not exist! " + dataPath);
            return default(T);
        }

        if (!File.Exists(dataPath))
        {
            Debug.LogWarning("SAVEANDLOAD: File does not exist! " + dataPath);
            return default(T);
        }

        try
        {
            long fileSize = new FileInfo(dataPath).Length;
            if (fileSize > MaxSaveDataSize)
            {
                Debug.LogError("SAVEANDLOAD: File size exceeds the maximum allowed size!");
                return default(T);
            }

            byte[] jsonDataAsBytes = File.ReadAllBytes(dataPath);
            string jsonData = Encoding.UTF8.GetString(jsonDataAsBytes);
            T returnedData = JsonUtility.FromJson<T>(jsonData);
            Debug.Log("SAVEANDLOAD: Loaded all data from: " + dataPath);
            return returnedData;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SAVEANDLOAD: Failed to load data from: " + dataPath);
            Debug.LogWarning("SAVEANDLOAD: Error: " + e.Message);
            return default(T);
        }
    }
    public static void SaveWithJsonConvert(object data, string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(dataPath, jsonData);
            Debug.Log("SAVEANDLOAD: Save data to: " + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to save data to: " + dataPath);
            Debug.LogError("SAVEANDLOAD: Error " + e.Message);
        }
    }

    public static T LoadWithJsonConvert(string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("SAVEANDLOAD: File or path does not exist! " + dataPath);
            return default(T);
        }

        if (!File.Exists(dataPath))
        {
            Debug.LogWarning("SAVEANDLOAD: File does not exist! " + dataPath);
            return default(T);
        }

        try
        {
            long fileSize = new FileInfo(dataPath).Length;
            if (fileSize > MaxSaveDataSize)
            {
                Debug.LogError("SAVEANDLOAD: File size exceeds the maximum allowed size!");
                return default(T);
            }

            string jsonData = File.ReadAllText(dataPath);
            T returnedData = JsonConvert.DeserializeObject<T>(jsonData);
            Debug.Log("SAVEANDLOAD: Loaded all data from: " + dataPath);
            return returnedData;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SAVEANDLOAD: Failed to load data from: " + dataPath);
            Debug.LogWarning("SAVEANDLOAD: Error: " + e.Message);
            return default(T);
        }
    }

    public static string LoadLargeText(string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("SAVEANDLOAD: File or path does not exist! " + dataPath);
            return null;
        }

        if (!File.Exists(dataPath))
        {
            Debug.LogWarning("SAVEANDLOAD: File does not exist! " + dataPath);
            return null;
        }

        try
        {
            long fileSize = new FileInfo(dataPath).Length;
            if (fileSize > MaxSaveDataSize)
            {
                Debug.LogError("SAVEANDLOAD: File size exceeds the maximum allowed size!");
                return null;
            }

            string fileContents = File.ReadAllText(dataPath);
            Debug.Log("SAVEANDLOAD: Loaded large text from: " + dataPath);
            return fileContents;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SAVEANDLOAD: Failed to load data from: " + dataPath);
            Debug.LogWarning("SAVEANDLOAD: Error: " + e.Message);
            return null;
        }
    }


    public static void SaveLargeText(string data, string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            using (StreamWriter writer = new StreamWriter(dataPath))
            {
                writer.Write(data);
            }
            Debug.Log("SAVEANDLOAD: Save large text to: " + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to save large text to: " + dataPath);
            Debug.LogError("SAVEANDLOAD: Error " + e.Message);
        }
    }
    public static void SaveFileAsJson(string json, string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            File.WriteAllText(dataPath, json);
            Debug.Log("SAVEANDLOAD: Save file as JSON to: " + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to save file as JSON to: " + dataPath);
            Debug.LogError("SAVEANDLOAD: Error " + e.Message);
        }
    }

    public static string LoadFileAsJson(string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("SAVEANDLOAD: File or path does not exist! " + dataPath);
            return null;
        }

        if (!File.Exists(dataPath))
        {
            Debug.LogWarning("SAVEANDLOAD: File does not exist! " + dataPath);
            return null;
        }

        try
        {
            string json = File.ReadAllText(dataPath);
            Debug.Log("SAVEANDLOAD: Loaded file as JSON from: " + dataPath);
            return json;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SAVEANDLOAD: Failed to load file as JSON from: " + dataPath);
            Debug.LogWarning("SAVEANDLOAD: Error: " + e.Message);
            return null;
        }
    }

    public static void DeleteFolder(string folder)
    {
        string folderPath = GetFolderPath(folder);
        try
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
                Debug.Log("SAVEANDLOAD: Deleted folder: " + folderPath);
            }
            else
            {
                Debug.LogWarning("SAVEANDLOAD: Folder does not exist: " + folderPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to delete folder: " + folderPath);
            Debug.LogError("SAVEANDLOAD: Error: " + e.Message);
        }
    }

    public static void DeleteFilesInFolder(string folder)
    {
        string folderPath = GetFolderPath(folder);
        try
        {
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                Debug.Log("SAVEANDLOAD: Deleted files in folder: " + folderPath);
            }
            else
            {
                Debug.LogWarning("SAVEANDLOAD: Folder does not exist: " + folderPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to delete files in folder: " + folderPath);
            Debug.LogError("SAVEANDLOAD: Error: " + e.Message);
        }
    }
    /// <summary>
    /// Create file path for where a file is stored on the specific platform given a folder name and file name
    /// </summary>
    /// <param name="FolderName"></param>
    /// <param name="FileName"></param>
    /// <returns></returns>
    private static string GetFolderPath(string FolderName)
    {
        string folderPath = "";

        // Combine the folder path based on the current platform
#if UNITY_EDITOR
        folderPath = Path.Combine(Application.dataPath, "StreamingAssets", "data", FolderName);
#else
        // Handle other platforms
        if (Application.isEditor)
        {
            folderPath = Path.Combine(Application.dataPath, "StreamingAssets", "data", FolderName);
        }
        else
        {
#if UNITY_ANDROID
            folderPath = Path.Combine(Application.persistentDataPath, "data", FolderName);
#elif UNITY_IOS
        folderPath = Path.Combine(Application.persistentDataPath, "data", FolderName);
#else
        // For other platforms
        folderPath = Path.Combine(Application.persistentDataPath, "data", FolderName);
#endif
        }
#endif

        return folderPath;
    }
    private static string GetFilePath(string FolderName, string FileName = "")
    {
        string filePath = "";

#if UNITY_EDITOR
    filePath = Path.Combine(Application.dataPath, "StreamingAssets", "data", FolderName);

    if (FileName != "")
        filePath = Path.Combine(filePath, FileName + ".txt");

#else
        // Handle other platforms as before
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // mac
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (FileName != "")
            filePath = Path.Combine(filePath, FileName + ".txt");

#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);
        if (FileName != "")
            filePath = Path.Combine(filePath, FileName + ".txt");

#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (FileName != "")
            filePath = Path.Combine(filePath, FileName + ".txt");

#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (!string.IsNullOrEmpty(FileName))
            filePath = Path.Combine(filePath, FileName + ".txt");
#else
        //webGL or others
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (!string.IsNullOrEmpty(FileName))
            filePath = Path.Combine(filePath, FileName + ".txt");
#endif

#endif

        return filePath;
    }

}