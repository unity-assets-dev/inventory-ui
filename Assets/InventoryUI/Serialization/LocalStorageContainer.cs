using System.IO;
using UnityEngine;

public class LocalStorageContainer : ISerializationContainer {
    
    private string LocalPath => Path.Combine(Application.persistentDataPath, "Inventory");
    public TResult LoadSaved<TResult>(string path, string defaults) {
        var content = defaults;
        path = Path.Combine(LocalPath, path + ".json");
        
        if (File.Exists(path)) {
            content = File.ReadAllText(path);
        }
        
        return JsonUtility.FromJson<TResult>(content);
    }

    public void Save<TResult>(TResult result, string path) {
        path = Path.Combine(LocalPath, path + ".json");

        if (!Directory.Exists(LocalPath)) {
            Directory.CreateDirectory(LocalPath);
        }
        
        var content = JsonUtility.ToJson(result);
        
        File.WriteAllText(path, content);
    }
}