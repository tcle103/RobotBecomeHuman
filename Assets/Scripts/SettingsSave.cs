using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsSave : MonoBehaviour
{
    private static GameObject instance;

    public Transform player;

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

        language = "English";
        contrast = "Normal";
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.SetString("Contrast", contrast);
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
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.SetString("Contrast", contrast);

        if (player != null)
        {
            PlayerPrefs.SetFloat("PlayerX", player.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.position.y);
            PlayerPrefs.SetString("Inventory", inventoryState.SaveInventory());

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
            if(doorsStr != "")
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
}
