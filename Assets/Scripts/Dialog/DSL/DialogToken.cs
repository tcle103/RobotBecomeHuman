using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogToken
{
    public int line;
    public DialogTokenType type;
    public string value;

    public DialogToken(int line, DialogTokenType type, string value)
    {
        this.line = line;
        this.type = type;
        this.value = value;
    }
}