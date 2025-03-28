/* Last modified by: Tien Le
 * Last modified on: 3/28/25
 *
 * NPCInteract.cs contains NPC behavior that occurs on 
 * interact with the player.
 * This should include stopping idle/walking state anim and 
 * behavior, as well as initializing dialogue.
 *
 * Created by: Tien Le
 * Contributors: Tien Le
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    // NPCConfig should contain the data which determines
    // how NPCs respond to the current player state
    // i.e. how to pick the appropriate script to use
    // in a dialogue interaction
    [SerializeField] private TextAsset NPCConfig;
    // Used to see if player has interacted with NPC before
    private bool firstTime = true;
    // stores all dialogue scripts used by that NPC
    [SerializeField] private List<TextAsset> dialogueScripts;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void onInteract()
    {
        
    }
}
