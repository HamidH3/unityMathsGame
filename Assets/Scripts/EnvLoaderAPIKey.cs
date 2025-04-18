using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnvLoaderAPIKey : MonoBehaviour
{
    private static Dictionary<string, string> envVar;

    public static void LoadEnvironment()
    {
        envVar = new Dictionary<string, string>();
        string path = Path.Combine(Application.dataPath, "../.env");

        if (File.Exists(path))
        {
            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    envVar[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }


    }
    public static string GetEnv(string key)
    {
        if (envVar == null)
        {
            LoadEnvironment();
        }
        if (envVar.ContainsKey(key))
        {
            return envVar[key];
        }
        return null;
    }
}
  