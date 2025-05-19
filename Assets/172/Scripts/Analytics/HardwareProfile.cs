using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class HardwareInfo
{
    public string userName;
    public string deviceModel;
    public string operatingSystem;
    public string processorType;
    public int    processorCount;
    public int    systemMemoryMB;
    public string graphicsDeviceName;
    public string graphicsDeviceVersion;
    public int    graphicsMemoryMB;
    public int    screenWidth;
    public int    screenHeight;
    public float  screenDPI;
}

public class HardwareProfile : MonoBehaviour
{
    [Tooltip("Base filename (without extension) under Application.persistentDataPath)")]
    public string baseFileName = "hardware_profile";

    void Start()
    {
        var profile = new HardwareInfo
        {
            userName              = Environment.UserName,
            deviceModel           = SystemInfo.deviceModel,
            operatingSystem       = SystemInfo.operatingSystem,
            processorType         = SystemInfo.processorType,
            processorCount        = SystemInfo.processorCount,
            systemMemoryMB        = SystemInfo.systemMemorySize,
            graphicsDeviceName    = SystemInfo.graphicsDeviceName,
            graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion,
            graphicsMemoryMB      = SystemInfo.graphicsMemorySize,
            screenWidth           = Screen.width,
            screenHeight          = Screen.height,
            screenDPI             = Screen.dpi,
        };

        WriteJsonToFile(JsonUtility.ToJson(profile, true));
    }

    private void WriteJsonToFile(string json)
    {
        var path = Path.Combine(Application.persistentDataPath, baseFileName + ".json");
        
        try {
            File.WriteAllText(path, json);
            Debug.Log($"Hardware profile written to: {path}");
        } catch (Exception e) { 
            Debug.LogError($"Probably read-only folder or no write permissions?");
        }
    }
}
