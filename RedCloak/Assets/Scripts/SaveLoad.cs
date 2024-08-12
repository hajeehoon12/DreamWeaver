using System.IO;
using UnityEngine;

public class SaveLoad
{
    public static void Save<T>(string fileName, T data)
    {
        string path = $"{Application.persistentDataPath}/{fileName}.json";
        string json = AEScrypt.Encrypt(JsonUtility.ToJson(data, true));
        
        File.WriteAllText(path, json);
    }
    
    public static T Load<T>(string fileName)
    {
        string path = $"{Application.persistentDataPath}/{fileName}.json";

        T result;
        
        if (File.Exists(path))
        {
            result = JsonUtility.FromJson<T>(AEScrypt.Decrypt(File.ReadAllText(path)));
        }
        else
        {
            TextAsset file = Resources.Load<TextAsset>($"JSONs/{fileName}");
            result = JsonUtility.FromJson<T>("{\"data\":" + file.text + "}");
        }

        return result;
    }
}
