using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static GameObject instance;

    public Transform player;

    public List<NPCBehavior> npcs = new();
    private int npcCount;

    private InventoryState inventoryState;

    private DoorController[] doors;
    
    private string savePath => Path.Combine(Application.persistentDataPath, "saveData.json");
    private string settingsPath => Path.Combine(Application.persistentDataPath, "settings.json");
    
    [Serializable]
    private class SaveData
    {
        public float playerX;
        public float playerY;
        public string inventory;
        public List<int> openDoorIds = new();
        public List<int> npcFileIds = new();
    }

    [Serializable]
    private class SettingsData
    {
        public string language = "English";
        public string contrast = "Normal";
    }
    
    public string language { get; set; } = "English";
    public string contrast { get; set; } = "Normal";
    
    // Start is called before the first frame update
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        Debug.Log("Save Data Path: " + savePath);
        Debug.Log("Settings Path: " + settingsPath);
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
    }

    public void SaveSettings()
    {
        SettingsData settings = new SettingsData { language = this.language, contrast = this.contrast };
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(settingsPath, json);
    }
    private void LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            SettingsData settings = JsonUtility.FromJson<SettingsData>(json);
            language = settings.language;
            contrast = settings.contrast;
        }
        else
        {
            language = "English";
            contrast = "Normal";
            SaveSettings();
        }
    }
    
    public void DeleteSaveData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

    public void DeleteAccessibilityData()
    {
        if (File.Exists(settingsPath))
        {
            SettingsData settings = new SettingsData { language = language }; // Keep language or reset as needed
            File.WriteAllText(settingsPath, JsonUtility.ToJson(settings));
        }
    }
    
    public void DeleteData()
    {
        DeleteSaveData();
        if (File.Exists(settingsPath))
        {
            File.Delete(settingsPath);
        }
        language = "English";
        contrast = "Normal";
        SaveSettings();
    }

    public void gameLoad()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (player != null)
        {
            player.transform.position = new Vector3(data.playerX, data.playerY, player.transform.position.z);

            inventoryState = player.GetComponent<InventoryState>();
            if (!string.IsNullOrEmpty(data.inventory))
            {
                inventoryState.LoadInventory(data.inventory);
            }

            doors = FindObjectsOfType<DoorController>();
            Debug.Assert(doors != null);

            foreach (DoorController door in doors)
            {
                if (data.openDoorIds.Contains(door.id))
                {
                    door.OpenQuiet();
                }
            }
        }

        npcCount = npcs.Count;
        Debug.Log("NpcCount: " + npcCount);
        for (int i = 0; i < npcCount && i < data.npcFileIds.Count; i++)
        {
            int fileId = data.npcFileIds[i];
            Debug.Log(fileId);
        }
    }

    public void Save()
    {
        SaveSettings();

        if (player == null) return;

        Debug.Log("playerX: " + player.position.x);
        Debug.Log("playerY: " + player.position.y);
        Debug.Log("inventory: " + player.GetComponent<InventoryState>().SaveInventory());
        Debug.Log("npcs: " + npcs.Count);
        doors = FindObjectsOfType<DoorController>();
        Debug.Log("doors null: " + (doors == null));
        Debug.Log("doors: " + doors.Length);
        
        SaveData data = new SaveData
        {
            playerX = player.position.x,
            playerY = player.position.y,
            inventory = player.GetComponent<InventoryState>().SaveInventory(),
            npcFileIds = npcs.Select(npc => npc.dialogueFile.GetInstanceID()).ToList(),
            openDoorIds = doors.Where(d => d.isOpen()).Select(d => d.id).ToList()
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }
}