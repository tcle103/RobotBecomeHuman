using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogContainerNode : DialogNode
{
    public DialogEntryNode entry { get; set; }

    public DialogContainerNode(DialogEntryNode entry)
    {
        this.entry = entry;
    }
}
