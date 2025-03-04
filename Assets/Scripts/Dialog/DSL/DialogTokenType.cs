using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DialogTokenType
{
    Id,
    String,
    Name,
    BeginGroup,
    EndGroup,
    Label,
    Option,
    Goto,
    Exit,
    NewLine,
    EOF
}