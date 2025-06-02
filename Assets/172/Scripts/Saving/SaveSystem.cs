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

    public List<NPCInteract> npcs = new();
    private int npcCount;
    private List<bool> npcFT = new();
    private List<bool> npcAC = new();

    public PlayerData playerData;

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
        public List<bool> npcFirstTimes = new();
        public List<bool> npcActive = new();
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
        npcs.Clear();
        npcFT.Clear();
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
            
        npcCount = npcs.Count;
        Debug.Log("NpcCount: " + npcCount);
        for (int i = 0; i < npcCount; i++)
        {
            Debug.Log("npcftcount" + npcFT.Count);
            Debug.Log("npcACcount" + npcAC.Count);
            Debug.Log("datanpcftcount" + data.npcFirstTimes.Count);
            Debug.Log("datanpcACcount" + data.npcActive.Count);
            
            //npc first time is a public boolean variable in npc interact=
            if (data.npcFirstTimes.Count > 0 && i < data.npcFirstTimes.Count)
            {
                if (npcFT.Count > i) { npcFT[i] = data.npcFirstTimes[i]; } else { npcFT.Add(data.npcFirstTimes[i]); }
                if (npcAC.Count > i) { npcAC[i] = data.npcActive[i]; } else { npcAC.Add(data.npcActive[i]); }
            }
            else
            {
                if (npcFT.Count > i) { npcFT[i] = true; } else { npcFT.Add(true); }
                if (npcAC.Count > i) { npcAC[i] = false; } else { npcAC.Add(false); }
            }
            
            Debug.Log("npcftcount" + npcFT.Count);
            Debug.Log("datanpcftcount" + data.npcFirstTimes.Count);
            Debug.Log("npcACcount" + npcAC.Count);
            npcs[i].firstTime = npcFT[i];
            npcs[i].gameObject.SetActive(npcAC[i]);
            Debug.Log(npcs[i].name + ": " + npcs[i].firstTime);
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
        Debug.Log("npc first times null: " + (npcFT == null));
        Debug.Log("npc active null: " + (npcAC == null));
        Debug.Log("npcs ft + npc list match length: " + (npcFT.Count == npcs.Count));
        Debug.Log("npcs ac + npc list match length: " + (npcAC.Count == npcs.Count));
        
        SaveData data = new SaveData
        {
            playerX = player.position.x,
            playerY = player.position.y,
            inventory = playerData.SaveInventory(),
            openDoorIds = doors.Where(d => d.isOpen()).Select(d => d.id).ToList(),
            npcFirstTimes = npcs.Select(npc => npc.firstTime).ToList(),
            npcActive = npcs.Select(npc => npc.gameObject.activeInHierarchy).ToList()
        };

        string json = JsonUtility.ToJson(data);
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