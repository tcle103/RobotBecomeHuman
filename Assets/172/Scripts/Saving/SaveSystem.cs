using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    private static GameObject instance;

    public Transform player;

    public List<NPCInteract> npcs = new();
    private int npcCount;

    public Dictionary<string, List<bool>> npcData = new();

    public PlayerData playerData;

    private DoorController[] doors;
    
    private string savePath => Path.Combine(Application.persistentDataPath, "saveData.json");
    private string settingsPath => Path.Combine(Application.persistentDataPath, "settings.json");
    
    public bool sceneLoaded = false;
    
    [Serializable]
    private class SaveData
    {
        public float playerX;
        public float playerY;
        public string inventory;
        public List<int> openDoorIds = new();
        public Dictionary<string, List<bool>> npcFirstActive = new();
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

    public void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "test1" && !sceneLoaded)
        {
            sceneLoaded = true;
            gameLoad();
        }
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
        npcs.Clear();
        npcData.Clear();
        language = "English";
        contrast = "Normal";
        SaveSettings();
    }

    public void gameLoad()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);

        if (player != null)
        {
            player.GetComponent<PlayerController>().MoveInterrupt(new Vector3(data.playerX, data.playerY, player.transform.position.z));

            playerData = player.GetComponent<PlayerData>();
            playerData.LoadInventory(data.inventory);

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
        
        
        npcs = GameObject.FindGameObjectsWithTag("NPC").Select(go => go.GetComponent<NPCInteract>()).ToList();
        //if any npcs are null, remove them from the list
        npcs.RemoveAll(npc => npc == null);
        npcs.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
        //list all names in log
        Debug.Log("NPCs loaded: " + string.Join(", ", npcs.Select(npc => npc.name)));
            
        npcCount = npcs.Count;
        Debug.Log("NpcCount: " + npcCount);

        npcData.Clear();

        foreach (var kvp in data.npcFirstActive)
        {
            string npcName = kvp.Key;
            List<bool> bools = kvp.Value;

            if (!npcData.ContainsKey(npcName))
            {
                npcData.Add(npcName, new List<bool>(new bool[npcCount]));
            }

            for (int i = 0; i < Math.Min(bools.Count, npcCount); i++)
            {
                npcData[npcName][i] = bools[i];
            }
        }

        for (int i = 0; i < npcCount; i++)
        {
            string npcName = npcs[i].name;

            if (npcData.ContainsKey(npcName))
            {
                npcs[i].firstTime = npcData[npcName][0];
                npcs[i].gameObject.SetActive(npcData[npcName][1]);
                Debug.Log($"{npcName}: firstTime = {npcs[i].firstTime}, active = {npcData[npcName][1]}");
            }
        }

        //debug log the npcData
        foreach (var kvp in npcData)
        {
            Debug.Log($"NPC: {kvp.Key}, FirstTime: {kvp.Value[0]}, Active: {kvp.Value[1]}");
        }

    }

    public void Save()
    {
        SaveSettings();

        if (player == null) return;

        Debug.Log("playerX: " + player.position.x);
        Debug.Log("playerY: " + player.position.y);
        Debug.Log("inventory: " + playerData.SaveInventory());
        Debug.Log("npcs: " + npcs.Count);
        doors = FindObjectsOfType<DoorController>();
        Debug.Log("doors null: " + (doors == null));
        Debug.Log("doors: " + doors.Length);
        npcs.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
        Debug.Log("npc data null: " + (npcData == null));
        Debug.Log("npcs ft + npc list match length: " + (npcData.Count == npcs.Count));

        SaveData data = new SaveData
        {
            playerX = player.position.x,
            playerY = player.position.y,
            inventory = playerData.SaveInventory(),
            openDoorIds = doors.Where(d => d.isOpen()).Select(d => d.id).ToList(),
            npcFirstActive = npcs.ToDictionary(npc => npc.name,
                npc => new List<bool> { npc.firstTime, npc.gameObject.activeInHierarchy })
        };

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    public void DeleteFiles()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        if (File.Exists(settingsPath))
        {
            File.Delete(settingsPath);
        }
    }
}