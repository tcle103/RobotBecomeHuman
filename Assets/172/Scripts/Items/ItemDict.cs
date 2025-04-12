/*
 * Last modified by: Tien Le
 * Last modified on: 4/12/25
 * 
 * ItemDict.cs defines the ItemDict class used to 
 * maintain the master list of all items.
 * 
 * Created by: Tien Le
 * Created on: 4/12/25
 * Contributers: Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

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