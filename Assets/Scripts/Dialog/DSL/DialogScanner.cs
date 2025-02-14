using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogScanner
{
    public string input { get; private set; }
    public int index { get; private set; }
    public int line { get; private set; }

    private bool lineHasCode = false;

    public DialogScanner(string input)
    {
        this.input = input;
        index = 0;
        line = 0;
    }

    private bool EndOfFile()
    {
        return index >= input.Length;
    }

    private char NextCh()
    {
        if (EndOfFile()) throw new Exception("Unexpected end of file");
        return input[index];
    }

    private bool IsNext(char ch)
    {
        if (EndOfFile()) return false;
        return NextCh() == ch;
    }

    private char SkipCh()
    {
        char ch = NextCh();
        index++;
        if (ch == '\n')
        {
            line++;
            lineHasCode = false;
        }
        return ch;
    }

    private void SkipNewLine()
    {
        if (NextCh() == '\n' || NextCh() == '\r')
        {
            char ch = SkipCh();
            if (ch == '\n')
            {
                if (NextCh() == '\r')
                {
                    SkipCh();
                }
            }
            else if (ch == '\n')
            {
                SkipCh();
            }
        }
    }

    private void SkipWhiteSpace()
    {
        while (true)
        {
            while(!EndOfFile() && char.IsWhiteSpace(NextCh()) && NextCh() != '\n')
            {
                SkipCh();
            }
            if (EndOfFile())
            {
                break;
            }
            else if(NextCh() == '#')
            {
                while (!EndOfFile() && NextCh() != '\n')
                {
                    SkipCh();
                }
                if(!EndOfFile() && !lineHasCode)
                {
                    SkipNewLine();
                }
            } else
            {
                break;
            }
        }
    }

    private DialogToken ScanString()
    {
        string value = "";
        int startLine = line;
        SkipCh();
        while (!EndOfFile() && NextCh() != '"')
        {
            if (NextCh() == '\\')
            {
                SkipCh();
            }
            value += SkipCh();
        }
        SkipCh();
        return new DialogToken(startLine, DialogTokenType.String, value);
    }

    private DialogToken ScanId()
    {
        string value = "";
        while (!EndOfFile() && (char.IsLetterOrDigit(NextCh()) || NextCh() == '_'))
        {
            value += SkipCh();
        }
        return new DialogToken(line, DialogTokenType.Id, value);
    }

    public DialogToken NextToken()
    {
        SkipWhiteSpace();

        if (EndOfFile())
        {
            return new DialogToken(line, DialogTokenType.EOF, "EOF");
        }

        if(NextCh() == '\n' || NextCh() == '\r')
        {
            SkipNewLine();
            return new DialogToken(line - 1, DialogTokenType.NewLine, "");
        }

        lineHasCode = true;

        char ch = NextCh();
        if (ch == '(')
        {
            SkipCh();
            return new DialogToken(line, DialogTokenType.BeginGroup, "(");
        }
        else if (ch == ')')
        {
            SkipCh();
            return new DialogToken(line, DialogTokenType.EndGroup, ")");
        }
        else if (ch == ':')
        {
            SkipCh();
            return new DialogToken(line, DialogTokenType.Label, ":");
        }
        else if(ch == '-')
        {
            SkipCh();
            if(IsNext('>'))
            {
                SkipCh();
                return new DialogToken(line, DialogTokenType.Goto, "->");
            }
            else if(IsNext('-'))
            {
                SkipCh();
                return new DialogToken(line, DialogTokenType.Exit, "--");
            }
            else
            {
                return new DialogToken(line, DialogTokenType.Option, "-");
            }
        }
        else if (ch == '@')
        {
            SkipCh();
            return new DialogToken(line, DialogTokenType.Container, "@");
        }
        else if (ch == '"')
        {
            return ScanString();
        }
        else if (char.IsLetterOrDigit(ch) || ch == '_')
        {
            return ScanId();
        }
        else
        {
            throw new Exception("Unexpected character '" + ch + "' (" + (int)ch + ") on line " + line);
        }
    }
}
