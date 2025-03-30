/*
 * Last modified by: Tien Le
 * Last modified on: 3/29/25
 * 
 * ItemData.cs defines the ItemData class inheriting from
 * ScriptableObject used to instantiate all items into the project,
 * as well as the ItemDict class used to maintain the master list of
 * all items.
 * 
 * Created by: Tien Le
 * Created on: 3/29/25
 * Contributers: Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

// [3/29/25 Tien] This allows you to create an
// "ItemData" instance as an asset :D
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    // [3/29/25 Tien]
    // making a scriptable object so can instance items
    // in the project rather than per scene!
    // idea from this tutorial here: 
    // https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/#items 

    public string itemName;
    public Sprite icon;

    // [3/29/25 Tien] apparently TextArea is an attribute
    // you can use to make a string editable in the inspector
    // in a flexible field. doesn't seem to do anything 
    // to the actual data functioning of it as a string.
    [TextAreaAttribute]
    public string description;
}

// [3/29/25 Tien] This masterlist should hold
// string-ItemData pairs that represent
// a label corresponding to the data of an item
// fancy serialized dictionary class from ayellowpaper here:
// https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052
[CreateAssetMenu]
public class ItemDict : ScriptableObject
{
    [SerializedDictionary("Label", "ItemData")]
    public SerializedDictionary<string, ItemData> List;
}