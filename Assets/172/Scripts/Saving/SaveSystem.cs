using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    private static GameObject instance;

    public Transform player;

    public List<NPCInteract> npcs = new();
    private int npcCount;

    public Dictionary<string, List<bool>> npcData = new();

    public PlayerData playerData;

    private DoorController[] doors;
    
    public List<PuzzleZone> puzzleZones = new();
    public Dictionary<string, bool> puzzleStates = new();
    private string savePath => Path.Combine(Application.persistentDataPath, "saveData.json");
    private string settingsPath => Path.Combine(Application.persistentDataPath, "settings.json");
    
    public bool sceneLoaded = false;
    
    public AnalyticsManager analyticsManager;
    
    [Serializable]
    private class SaveData
    {
        public float playerX;
        public float playerY;
        public string inventory;
        public List<int> openDoorIds = new();
        public Dictionary<string, List<bool>> npcFirstActive = new();
        public Dictionary<string, bool> puzzleCompletion = new();
    }

    [Serializable]
    private class SettingsData
    {
        public string language = "English";
        public string contrast = "Normal";
        public int colorblindFilter = 0;
    }
    
    public string language { get; set; } = "English";
    public string contrast { get; set; } = "Normal";
    
    public int colorblindFilter { get; set; } = 0; // 0 = none, 1 = protanopia, 2 = deuteranopia, 3 = tritanopia
    
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
        if (SceneManager.GetActiveScene().name == "e+w" && !sceneLoaded)
        {
            sceneLoaded = true;
            gameLoad();
        }
    }

    public void SaveSettings()
    {
        SettingsData settings = new SettingsData { language = this.language, contrast = this.contrast, colorblindFilter = this.colorblindFilter };
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
            SetColorblindType(settings.colorblindFilter);
        }
        else
        {
            language = "English";
            contrast = "Normal";
            SetColorblindType(0); // Default to no filter
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
        colorblindFilter = 0; // Reset to no filter
        SaveSettings();
    }

    public void gameLoad()
    {
        
        analyticsManager = GetComponentInParent(typeof(AnalyticsManager)) as AnalyticsManager;
        
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
        player = GameObject.FindWithTag("Player").transform;
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
        
        //debug doors
        Debug.Log("Doors loaded: " + doors.Length);
        Debug.Log("Open doors: " + string.Join(", ", data.openDoorIds));
        
        
        npcs = GameObject.FindGameObjectsWithTag("NPC").Select(go => go.GetComponent<NPCInteract>()).ToList();
        
        
        //find all inactive game objects with the tag "NPC" and get their components
        NPCInteract[] allNpcComponents = UnityEngine.Object.FindObjectsByType<NPCInteract>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        List<NPCInteract> inactiveNpcs = allNpcComponents
            .Where(npc => npc.CompareTag("NPC") && !npc.gameObject.activeInHierarchy)
            .ToList();
        //add inactive npcs to the list
        npcs.AddRange(inactiveNpcs);
        Debug.Log("inactive npcs found: " + inactiveNpcs.Count);
        
        
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

            /*if (!npcData.ContainsKey(npcName))
            {
                npcData.Add(npcName, new List<bool>(new bool[npcCount]));
            }

            for (int i = 0; i < Math.Min(bools.Count, npcCount); i++)
            {
                npcData[npcName][i] = bools[i];
            }*/
            
            if (!npcData.ContainsKey(npcName))
            {
                npcData.Add(npcName, new List<bool>(bools));
            }
        }

        for (int i = 0; i < npcCount; i++)
        {
            string npcName = npcs[i].name;

            if (npcData.ContainsKey(npcName))
            {
                Debug.Log("Loading NPC: " + npcName);
                //debug npc data of npcname
                Debug.Log("npc data count: " + npcData[npcName].Count);
                
                Debug.Log("npc data 0: " + npcData[npcName][0]);
                Debug.Log("npc data 1: " + npcData[npcName][1]);
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
        
        // Find all PuzzleZones in the scene (including inactive)
        PuzzleZone[] allPuzzleZones = UnityEngine.Object.FindObjectsByType<PuzzleZone>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        puzzleZones = allPuzzleZones.ToList();
        
        puzzleZones.RemoveAll(p => p == null);
        puzzleZones.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
        
        Debug.Log("PuzzleZones loaded: " + string.Join(", ", puzzleZones.Select(p => p.name)));

        puzzleStates.Clear();
        
        if (data.puzzleCompletion != null)
        {
            foreach (var kvp in data.puzzleCompletion)
            {
                puzzleStates[kvp.Key] = kvp.Value;
            }
        }
        
        foreach (var puzzle in puzzleZones)
        {
            if(puzzleStates.ContainsKey(puzzle.name))
            {
                if (puzzleStates[puzzle.name])
                {
                    puzzle.setComplete();
                }
                Debug.Log($"Puzzle {puzzle.name}: completed = {puzzleStates[puzzle.name]}");
            }
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
        Debug.Log("puzzleZones: " + puzzleZones.Count);

        SaveData data = new SaveData
        {
            playerX = player.position.x,
            playerY = player.position.y,
            inventory = playerData.SaveInventory(),
            openDoorIds = doors.Where(d => d.isOpen()).Select(d => d.id).ToList(),
            npcFirstActive = npcs.ToDictionary(npc => npc.name,
                npc => new List<bool> { npc.firstTime, npc.gameObject.activeInHierarchy }),
            puzzleCompletion = puzzleZones.ToDictionary(puzzle => puzzle.name, 
                puzzle => puzzle.completed)
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
    
    public void SetColorblindType(int type)
    {
        var feature = Resources.FindObjectsOfTypeAll<ColorblindFeature>().FirstOrDefault();
        if (feature != null)
        {
            feature.settings.Type = type;
            colorblindFilter = type;
        }
    }
}