/* Last modified by: Tien Le
 * Last modified on: 3/28/25
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
using System.Collections.Generic;
using UnityEngine;

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
    // dialogueScripts stores all dialogue scripts used by that NPC
    [SerializeField] private List<TextAsset> dialogueScripts;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void onInteract()
    {
        scriptSelect();
    }

    private int scriptSelect()
    {
        Dictionary<string, int> choices = NPCConfigParse();
        return 0;
    }

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
                string key = configLines[i].Substring(2, configLines[i].Length - 4);
                int value = -1;

                // [3/28/25 Tien] isolate dialogueScripts index contained in
                // next line, if present
                if (!string.IsNullOrEmpty(configLines[i + 1]))
                {
                    for (int a = 0; a < configLines[i + 1].Length; a++)
                    {
                        if (Char.IsWhiteSpace(configLines[i + 1][a]))
                        {
                            continue;
                        }
                        else
                        {
                            bool success = int.TryParse(configLines[i + 1].Substring(a, configLines[i + 1].Length - a), out value);
                            if (!success)
                            {
                                Debug.Log("ERROR: Unable to parse dialogueScripts index into an int - are you sure it contains a number?");
                                break;
                            }
                        }
                    }
                    // [3/28/25 Tien] if all is well, make an entry in configScriptDict
                    configScriptDict.Add(key, value);
                }
                else
                {
                    Debug.Log("ERROR: Missing corresponding dialogueScripts index for conditional " + key);
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
