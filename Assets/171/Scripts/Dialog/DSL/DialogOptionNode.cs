using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogOptionNode : DialogNode
{
    public string text;

    public DialogOptionNode(string text)
    {
        this.text = text;
    }
}