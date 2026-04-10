using UnityEngine;

public class PlayerPrefsContainer : ISerializationContainer {
    
    public TResult LoadSaved<TResult>(string path, string defaults) {
        var content = PlayerPrefs.GetString(path, string.Empty);

        if (string.IsNullOrEmpty(content)) {
            content = defaults;
        }
        
        return JsonUtility.FromJson<TResult>(content);
    }

    public void Save<TResult>(TResult result, string path) {
        PlayerPrefs.SetString(path, JsonUtility.ToJson(result));
    }
}