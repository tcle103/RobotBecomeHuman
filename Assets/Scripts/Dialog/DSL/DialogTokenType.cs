using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DialogTokenType
{
    Id,
    String,
    BeginGroup,
    EndGroup,
    Label,
    Option,
    Goto,
    Container,
    Exit,
    NewLine,
    EOF
}