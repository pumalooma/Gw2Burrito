
using Newtonsoft.Json;
using System.IO;

public class Config {
    public int worldId;

    private static Config instance;
    public static Config Instance {
        get {
            if (instance != null)
                return instance;

            if (File.Exists("config.json")) {
                string jsonData = File.ReadAllText("config.json");
                instance = JsonConvert.DeserializeObject<Config>(jsonData);
            }
            else {
                instance = new Config() { worldId = 1016 };
                string jsonData = JsonConvert.SerializeObject(instance);
                File.WriteAllText("config.json", jsonData);
            }

            return instance;
        }
    }
}
