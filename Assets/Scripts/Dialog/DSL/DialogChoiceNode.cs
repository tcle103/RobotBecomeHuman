using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

public class DialogChoiceNode : DialogNode
{
    public string? text;
    public List<DialogOptionNode> options;

    public DialogChoiceNode(string? text)
    {
        this.text = text;
        options = new List<DialogOptionNode>();
    }
}
