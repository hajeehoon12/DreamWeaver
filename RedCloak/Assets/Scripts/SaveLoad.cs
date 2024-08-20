using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class SaveLoad
{
    public static void Save<T>(string fileName, T data)
    {
        string path = $"{Application.persistentDataPath}/{fileName}.json";
        
        Type type = data.GetType();
        FieldInfo fieldInfo = type.GetField("version", BindingFlags.Public | BindingFlags.Instance);
        
        if (fieldInfo != null)
            fieldInfo.SetValue(data, Application.version);
        
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
            
            Type type = result.GetType();
            FieldInfo fieldInfo = type.GetField("version", BindingFlags.Public | BindingFlags.Instance);
            
            if (fieldInfo != null)
            {
                object fieldValue = fieldInfo.GetValue(result);
                
                if (fieldValue.ToString() == Application.version)
                    return result;
            }
        }
        
        TextAsset file = Resources.Load<TextAsset>($"JSONs/{fileName}");
        result = JsonUtility.FromJson<T>("{\"data\":" + file.text + "}");
        
        return result;
    }
}
