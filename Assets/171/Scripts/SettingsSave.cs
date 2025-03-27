using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class SettingsSave : MonoBehaviour
{
    private static GameObject instance;

    public Transform player;

    public List<NPCBehavior> npcs = new();
    private int npcCount;

    private InventoryState inventoryState;

    private DoorController[] doors;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if(PlayerPrefs.HasKey("Language"))
        {
            language = PlayerPrefs.GetString("Language");
        }
        else
        {
            language = "English";
            PlayerPrefs.SetString("Language", language);
        }
        if(PlayerPrefs.HasKey("Contrast"))
        {
            contrast = PlayerPrefs.GetString("Contrast");
        }
        else
        {
            contrast = "Normal";
            PlayerPrefs.SetString("Contrast", contrast);
        }
    }

    public void gameLoad()
    {
        if (player != null)
        {
            float x = PlayerPrefs.GetFloat("PlayerX", player.transform.position.x);
            float y = PlayerPrefs.GetFloat("PlayerY", player.transform.position.y);
            player.transform.position = new Vector3(x, y, player.transform.position.z);

            inventoryState = player.GetComponent<InventoryState>();
            string inventoryStr = PlayerPrefs.GetString("Inventory", "");
            if (inventoryStr != "")
            {
                inventoryState.LoadInventory(inventoryStr);
            }

            doors = FindObjectsOfType<DoorController>();

            Debug.Assert(doors != null);
            string doorsStr = PlayerPrefs.GetString("Doors", "");
            if (doorsStr != "")
            {
                string[] doorStrs = doorsStr.Split(",");
                int[] doorIds = new int[doorStrs.Length];
                for (int i = 0; i < doorStrs.Length; i++)
                {
                    doorIds[i] = int.Parse(doorStrs[i]);
                }

                foreach (DoorController door in doors)
                {
                    if (doorIds.Contains(door.id))
                    {
                        door.OpenQuiet();
                    }
                }
            }
        }

        // Try to retrieve last saved file from dictionary for each NPC
        npcCount = npcs.Count;
        Debug.Log("NpcCount: " + npcCount);
        for (int i = 0; i < npcCount; i++)
        {
            int fileId = PlayerPrefs.GetInt("Npc" + i);
            Debug.Log(fileId);
            //if (fileId != 0) npcs[i].dialogueFile = npcs[i].NPCFileLookup(fileId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetString("Language", language);
        //PlayerPrefs.SetString("Contrast", contrast);

        //if (player != null)
        //{
        //    PlayerPrefs.SetFloat("PlayerX", player.position.x);
        //    PlayerPrefs.SetFloat("PlayerY", player.position.y);
        //    PlayerPrefs.SetString("Inventory", inventoryState.SaveInventory());
        //    // Store instance ID for dialog file in PlayerPrefs
        //    for (int i = 0; i < npcCount; i++)
        //    {
        //        PlayerPrefs.SetInt("Npc" + i, npcs[i].dialogueFile.GetInstanceID());
        //    }

        //    string doorsStr = "";
        //    for (int i = 0; i < doors.Length; i++)
        //    {
        //        if (doors[i].isOpen())
        //        {
        //            if (doorsStr.Length > 0)
        //            {
        //                doorsStr += ",";
        //            }
        //            doorsStr += doors[i].id.ToString();
        //        }
        //    }
        //    if(doorsStr != "")
        //    {
        //        PlayerPrefs.SetString("Doors", doorsStr);
        //    }
        //}
    }

    public void Save()
    {
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.SetString("Contrast", contrast);

        if (player != null)
        {
            PlayerPrefs.SetFloat("PlayerX", player.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.position.y);
            PlayerPrefs.SetString("Inventory", inventoryState.SaveInventory());
            // Store instance ID for dialog file in PlayerPrefs
            for (int i = 0; i < npcCount; i++)
            {
                PlayerPrefs.SetInt("Npc" + i, npcs[i].dialogueFile.GetInstanceID());
            }

            string doorsStr = "";
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].isOpen())
                {
                    if (doorsStr.Length > 0)
                    {
                        doorsStr += ",";
                    }
                    doorsStr += doors[i].id.ToString();
                }
            }
            if (doorsStr != "")
            {
                PlayerPrefs.SetString("Doors", doorsStr);
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    
    public string language { get; set; } = "English";
    public string contrast { get; set; } = "Normal";
    
    //delete all save data but not the settings
    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("Inventory");
        PlayerPrefs.DeleteKey("Doors");
        // Delete key from PlayerPrefs
        for (int i = 0; i < npcCount; i++)
        {
            PlayerPrefs.DeleteKey("Npc" + i);
        }
    }
    
    //delete all accessibility data
    public void DeleteAccessibilityData()
    {
        PlayerPrefs.DeleteKey("Contrast");
        PlayerPrefs.DeleteKey("Language");
    }
    
    //delete all data
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        
        PlayerPrefs.SetString("Language", "English");
        language = PlayerPrefs.GetString("Language");
        PlayerPrefs.SetString("Contrast", "Normal");
        contrast = PlayerPrefs.GetString("Contrast");
    }
}
