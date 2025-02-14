using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

#nullable enable

public class DialogParser
{
    private DialogScanner scanner;

    private bool readNext;
    private DialogToken _currentToken = null!;

    private Dictionary<string, DialogNode> labels;
    private Dictionary<DialogNode, string> gotos;

    private DialogParser(string input)
    {
        readNext = true;
        scanner = new DialogScanner(input);
        labels = new Dictionary<string, DialogNode>();
        gotos = new Dictionary<DialogNode, string>();
    }

    private DialogNode Unexpected(DialogTokenType? expected = null)
    {
        if (_currentToken?.type == DialogTokenType.EOF)
        {
            throw new Exception("Unexpected end of file");
        }
        else if (expected != null)
        {
            throw new Exception("Expected " + expected + " token but found "
                + _currentToken?.type + ": '" + _currentToken?.value + "'");
        } else
        {
            throw new Exception("Unexpected token " + _currentToken?.type + ": '" + _currentToken?.value + "'");
        }
    }

    private DialogToken NextToken()
    {
        if (readNext)
        {
            _currentToken = scanner.NextToken();
            readNext = false;
        }
        return _currentToken;
    }

    public DialogToken SkipToken()
    {
        DialogToken result = NextToken();
        readNext = true;
        return result;
    }
    

    private DialogToken Expect(DialogTokenType expectedType)
    {
        DialogToken result = SkipToken();
        if(result.type != expectedType)
        {
            Unexpected(expectedType);
        }
        return result;
    }

    private bool IsNext(DialogTokenType type)
    {
        return NextToken().type == type;
    }

    private DialogOptionNode ParseOption()
    {
        Expect(DialogTokenType.Option);
        DialogOptionNode node = new DialogOptionNode(Expect(DialogTokenType.String).value);
        ParseSequence(node, true);
        return node;
    }

    private DialogChoiceNode ParseChoice(bool inOption)
    {
        DialogChoiceNode node = new DialogChoiceNode(null);
        if(IsNext(DialogTokenType.String))
        {
            node.text = SkipToken().value;
            if (inOption)
            {
                return node;
            }
        }
        if(IsNext(DialogTokenType.NewLine))
        {
            SkipToken();
        }
        while(IsNext(DialogTokenType.Option))
        {
            node.options.Add(ParseOption());
            if (IsNext(DialogTokenType.NewLine))
            {
                SkipToken();
            }
        }
        return node;
    }

    private DialogNode ParseSequence(DialogNode lastNode, bool inline)
    {
        string? labelName = null;
        while (true)
        {
            if (IsNext(DialogTokenType.EOF))
            {
                break;
            }
            else if (inline && IsNext(DialogTokenType.NewLine))
            {
                break;
            }
            else if (!inline && IsNext(DialogTokenType.EndGroup))
            {
                break;
            }
            else if(IsNext(DialogTokenType.Id))
            {
                if (IsNext(DialogTokenType.Id))
                {
                    labelName = SkipToken().value;
                    Expect(DialogTokenType.Label);
                    while (IsNext(DialogTokenType.NewLine))
                    {
                        SkipToken();
                    }
                }
            }
            else
            {
                DialogNode nextNode = ParseNode(lastNode, false);
                lastNode.next = nextNode;
                lastNode = nextNode;
                if (labelName != null)
                {
                    labels.Add(labelName, nextNode);
                    labelName = null;
                }
            }
        }
        return lastNode;
    }

    private DialogNode ParseNode(DialogNode lastNode, bool inOption)
    {
        if(IsNext(DialogTokenType.NewLine))
        {
            SkipToken();
            return lastNode;
        }
        else if (IsNext(DialogTokenType.String) || IsNext(DialogTokenType.Option))
        {
            return ParseChoice(inOption);
        }
        else if (IsNext(DialogTokenType.Goto))
        {
            SkipToken();
            string name = Expect(DialogTokenType.Id).value;
            gotos.Add(lastNode, name);
            return new DialogGoToNode(name);
        }
        else if (IsNext(DialogTokenType.Container))
        {
            SkipToken();
            DialogEntryNode entryPoint = new DialogEntryNode();
            ParseSequence(entryPoint, true);
            return new DialogContainerNode(entryPoint);
        }
        else if (IsNext(DialogTokenType.BeginGroup))
        {
            SkipToken();
            DialogNode node = ParseSequence(lastNode, false);
            SkipToken();
            return node;
        }
        else
        {
            return Unexpected();
        }
    }

    private void LinkGotos()
    {
        foreach (KeyValuePair<string, DialogNode> label in labels)
        {
            foreach (KeyValuePair<DialogNode, string> gt in gotos)
            {
                if (label.Key == gt.Value)
                {
                    gt.Key.next = label.Value;
                }
            }
        }
    }

    public static DialogNode Parse(string input)
    {
        DialogParser parser = new DialogParser(input);
        DialogNode entryPoint = new DialogEntryNode();
        parser.ParseSequence(entryPoint, false);
        parser.LinkGotos();
        return entryPoint;
    }
}