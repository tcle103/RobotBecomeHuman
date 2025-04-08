/*
 * Last modified by: Tien Le
 * Last modified on: 4/7/25
 * 
 * Based on the work done by Ben Hess in the 171 version of
 * this project for our dialogue system. 
 * DSLParser.cs goes through a dialogue script text file
 * formatted in the DSL and constructs a dictionary representing
 * a dialogue tree w/ string, node pairs corresponding to nodes
 * 
 * Created by: Tien Le
 * Created on: 4/7/25
 * Contributors: Ben Hess, Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSLParser
{
    // [4/7/25 Tien] reference to the dialogue script TextAsset
    public TextAsset Input {  get; private set; }
    // [4/7/25 Tien] dictionary actually holding the tree
    public Dictionary<string, DialogueNode> dialogueTree;
    
    public DSLParser(TextAsset input)
    {
        Input = input;
        dialogueTree = new Dictionary<string, DialogueNode>();
    }

    public Dictionary<string, DialogueNode> parse()
    {
        // [4/7/25 Tien] go line by line
        List<string> inputLines = new List<string>(Input.text.Split('\n'));
        bool nextNode = false;
        DialogueNode currNode = new DialogueNode();
        foreach (string line in inputLines) 
        {
            if (nextNode)
            {
                currNode = new DialogueNode();
                nextNode = false;
            }

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            // [4/7/25 Tien] if line ends with ':' 
            // is a label
            else if (line[^2..].Equals(':'))
            {
                currNode.Label = line[0..^2];
                continue;
            }
            // [4/7/25 Tien] if line starts with '[' (speaker tag) or '"'
            // it's to be displayed as dialogue text
            // add to list of text (holds order it will be displayed)
            else if (line[0].Equals('[') || line[0].Equals('"'))
            {
                currNode.text.Add(line);
            }
            // [4/7/25 Tien] if line ends with '-'
            // now is either signifying end of a node or
            // a choice
            // check for both
            else if (line[0].Equals('-'))
            {
                // [4/7/25 Tien] check for end of node "--"
                if (line[1].Equals('-')) 
                {
                    nextNode = true;
                    continue;
                }
            }
        }
        return dialogueTree;
    }
}
