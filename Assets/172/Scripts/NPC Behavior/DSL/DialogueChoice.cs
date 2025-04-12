/*
 * Last modified by: Tien Le
 * Last modified on: 4/7/25
 * 
 * Based on the work done by Ben Hess in the 171 version of
 * this project for our dialogue system. 
 * DialogueChoice.cs represents an edge in a dialogue
 * tree, storing the text for display and the label
 * for the next node
 * 
 * Created by: Tien Le
 * Created on: 4/7/25
 * Contributors: Ben Hess, Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice
{
    public string Text {  get; set; }
    public string To {  get; set; }

    public DialogueChoice(string text, string to)
    {
        Text = text;
        To = to;
    }
}
