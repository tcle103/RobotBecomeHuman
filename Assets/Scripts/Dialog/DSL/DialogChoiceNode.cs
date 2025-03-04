using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

public class DialogChoiceNode : DialogNode
{
    public string? name;
    public string? text;
    public List<DialogOptionNode> options;

    public DialogChoiceNode()
    {
        options = new List<DialogOptionNode>();
    }
}
