/*
 * Last modified by: Tien Le
 * Last modified on: 4/7/25
 * 
 * Based on the work done by Ben Hess in the 171 version of
 * this project for our dialogue system. 
 * DialogueNode.cs represents a node in a dialogue
 * tree, holding relevant data for traversing the tree.
 * 
 * Created by: Tien Le
 * Created on: 4/7/25
 * Contributors: Ben Hess, Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class DialogueNode
{
    public string Label { get; set; }
    public List<string> text;
    // [4/7/25 Tien] this is essentially a list of edges
    public List<DialogueChoice> choices;
    // [4/7/25 Tien] stores a bunch of strings containing
    // action keywords and their parameters
    public List<string> actions;

    public void addChoice(DialogueChoice choice)
    {
        choices.Add(choice);
    }
}
