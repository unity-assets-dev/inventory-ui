public interface ISerializationContainer {
    TResult LoadSaved<TResult>(string path, string defaults);
    void Save<TResult>(TResult result, string path);
}