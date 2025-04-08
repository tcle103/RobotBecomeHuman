/* 
 * Last modified by: Tien Le
 * Last modified on: 4/7/25
 *
 * NPCInteract.cs contains NPC behavior that occurs on 
 * interact with the player.
 * This should include stopping idle/walking state anim and 
 * behavior, as well as initializing dialogue.
 *
 * Created by: Tien Le
 * Created on: 3/28/25
 * Contributors: Tien Le
 */


using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class NPCInteract : MonoBehaviour
{
    // [3/28/25 Tien]
    // NPCConfig should contain the data which determines
    // how NPCs respond to the current player state
    // i.e. how to pick the appropriate script to use
    // in a dialogue interaction
    [SerializeField] private TextAsset NPCConfig;
    // [3/28/25 Tien]
    // firstTime is used to see if player has interacted with
    // NPC before and modify behavior as a result
    private bool firstTime = true;
    // [3/28/25 Tien]
    // dialogueScripts stores all (non-random) dialogue scripts used
    // by that NPC
    [SerializeField] private List<TextAsset> dialogueScripts;
    // [4/5/25 Tien]
    // randomScripts stores dialogue for the NPC to randomly cycle 
    // through (for example, for more lively "default" case interactions)
    [SerializeField] private List<TextAsset> randomScripts;
    // [3/29/25 Tien]
    // scriptChoices is a dictionary of conditions to indexes of dialogue
    // stored in dialogueScript
    private Dictionary<string, int> scriptChoices;
    // [3/29/25 Tien]
    // player
    private PlayerData playerData;
    // [4/5/25 Tien]
    // trackedObjects stores a list of GameObjects with properties that
    // the NPC needs to track
    // used in: dialogue switching
    [SerializeField] private List<GameObject> trackedObjects;
    // [4/7/25 Tien]
    // actions is a dict of events the NPC may cause during an interaction
    // e.g. saving
    [SerializedDictionary("Label", "Action")]
    public SerializedDictionary<string, UnityEvent> actions;

    // Start is called before the first frame update
    void Start()
    {
        // [3/29/25 Tien] populate scriptChoices with
        // conditions/indexes parsed from NPCConfig TextAsset
        // with NPCConfigParse()
        scriptChoices = NPCConfigParse();
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
    }

    /*
     * [3/29/25 Tien]
     * onInteract()
     * Runs NPC functionality for interaction with player.
     * Includes stopping the previous state (idle or walking)
     * as well as initiating dialogue.
     */
    public void onInteract()
    {
        int dialogueIndex = scriptSelect();
        // if dialogueIndex is positive, get that script from dialogueScripts
        // and initiate dialogue
        // else, choose a random script in randomScripts
        if (dialogueIndex >= 0)
        {
            Debug.Log(dialogueScripts[scriptSelect()].text);
        }
        else
        {
            Debug.Log(randomScripts[UnityEngine.Random.Range(0, randomScripts.Count)].text);
        }
        firstTime = false;
    }


    /*
     * [3/28/25 Tien]
     * scriptSelect()
     * Returns an integer corresponding to a dialogue script
     * in the instance's dialogueScript list determined by
     * parsing the condition keys in scriptChoices
     * 
     * A positive integer (and 0) means a specific dialogue script,
     * while a negative indicates random choice from 
     */
    private int scriptSelect()
    {
        string choice = "default";

        // [3/29/25 Tien] grab the player's current inventory
        Dictionary<string, int> currentInventory = playerData.inventoryGet();

        // [3/29/25 Tien] go through every choice in scriptChoices and check if conditions match
        foreach ((string key, int index) in scriptChoices)
        {
            bool chosen = true;
            // [3/29/25 Tien] breaks each conditions into individual conditions
            List<string> conditions = new List<string>(key.Split(';'));
            foreach (string condition in conditions)
            {
                // [4/4/25 Tien] "firstTime" keyword - check if this is first time
                // player has interacted with
                if (condition.Contains("firstTime"))
                {
                    if (!firstTime)
                    {
                        chosen = false;
                        break;
                    }
                }
                /*
                 *  [3/29/25 Tien] "has" keyword - check if player has a certain amount
                 *  of an item in their inventory
                 *  
                 *  inside the config file, it should look like
                 *  "has,itemLabel,amount"
                 *  so in the parameters list, parameters[1] will be the item label
                 *  and parameters[2] will be the amount
                 */
                if (condition.Contains("has"))
                {
                    List<string> parameters = new List<string>(condition.Split(','));
                    if (currentInventory.ContainsKey(parameters[1]))
                    {
                        if (currentInventory[parameters[1]] != int.Parse(parameters[2]))
                        {
                            chosen = false;
                            break;
                        }
                    }
                    else
                    {
                        chosen = false;
                        break;
                    }

                }
                /* 
                 * [4/5/25 Tien] "get" keyword - fetch property of a specified component
                 * from the list of tracked GameObjects
                 *
                 * inside the config file, it should look like
                 * "get,indexNum,component,property,value"
                 * it's up to the config file writer to know what that should be
                 * does not check if the string of value you write will match the
                 * ToString()equivalent of the fetched property value

                 * uses reflection; might be slow but i can't think of another way to do
                 */
                if (condition.Contains("get"))
                {
                    List<string> parameters = new List<string>(condition.Split(','));
                    Component component = trackedObjects[int.Parse(parameters[1])].GetComponent(parameters[2]);
                    if (component != null)
                    {
                        PropertyInfo fetchedProperty = component.GetType().GetProperty(parameters[3]);
                        if (fetchedProperty != null)
                        {
                            string value = fetchedProperty.GetValue(component, null).ToString();
                            if (value != parameters[4])
                            {
                                chosen = false;
                                break;
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"ERROR: {component} doesn't contain variable {parameters[3]}", this);
                            chosen = false;
                            break;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"ERROR: component {parameters[2]} not found", this);
                        chosen = false;
                        break;
                    }
                }
            }
            // [3/29/25 Tien] if all the conditions in a key are true,
            // immediately break and use that index
            // this means that order of dialogue choices in the config script
            // are important, and should be entered in order of priority
            if (chosen)
            {
                choice = key;
                break;
            }
        }
        return scriptChoices[choice];
    }


    /* 
     * [3/28/25 Tien]
     * NPCConfigParse()
     * Returns a dictionary from the NPCConfig TextAsset
     * containing string, int pairs representing
     * the conditions (string) that should determine
     * which dialogue script (int) will be played on
     * interaction with the NPC
     */
    private Dictionary<string, int> NPCConfigParse()
    {
        Dictionary<string, int> configScriptDict = new Dictionary<string, int>();

        // [3/28/25 Tien]
        // grab all the contents of the config file and tokenize 
        // by line into a list
        List<string> configLines = new List<string>(NPCConfig.text.Split('\n'));
        for (int i = 0; i < configLines.Count; i++)
        {
            //Debug.Log(configLines[i]);
            // [3/28/25 Tien] go line by line and parse

            if (string.IsNullOrEmpty(configLines[i]))
            {
                continue;
            }

            // [3/28/25 Tien] if line starts with {,
            // add as conditional key to configScriptDict
            // the next line will contain value
            // which is equivalent to an index in dialogueScripts
            if (configLines[i][0] == '{')
            {
                string key = configLines[i].Substring(1, configLines[i].Length - 3).Trim();

                // [3/28/25 Tien] isolate dialogueScripts index contained in
                // next line, if present
                if (!string.IsNullOrEmpty(configLines[i + 1]))
                {
                    int value;
                    bool success = int.TryParse(configLines[i + 1], out value);
                    if (!success)
                    {
                        Debug.LogWarning("ERROR: Unable to parse dialogueScripts index into an int - are you sure it contains a number?", this);
                        break;
                    }
                    // [3/28/25 Tien] if all is well, make an entry in configScriptDict
                    configScriptDict.Add(key, value);
                }
                else
                {
                    Debug.LogWarning("ERROR: Missing corresponding dialogueScripts index for conditional " + key, this);
                    continue;
                }
            }
            else
            {
                continue;
            }
        }

        return configScriptDict;
    }
}
