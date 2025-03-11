using System;
using System.Collections.Generic;

#nullable enable

public class DialogParser
{
    class Context
    {
        public DialogNode? firstNode;
        public List<DialogNode> endNodes;
        public List<string> labelNames;

        public Context()
        {
            endNodes = new List<DialogNode>();
            labelNames = new List<string>();
        }
    }

    private DialogScanner scanner;
    private bool readNext;
    private DialogToken _currentToken = null!;

    private Dictionary<string, DialogNode?> labels;
    private Dictionary<DialogNode, string> gotos;

    private Stack<Context> contexts;
    private Context? context;

    private DialogParser(string input)
    {
        readNext = true;
        scanner = new DialogScanner(input);
        labels = new Dictionary<string, DialogNode?>();
        gotos = new Dictionary<DialogNode, string>();
        contexts = new Stack<Context>();
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
        BeginContext();
        EmitNode(node);
        ParseSequence(true);
        EndContext();
        return node;
    }

    private void ParseChoice(bool inOption)
    {
        DialogChoiceNode node = new DialogChoiceNode();
        EmitNode(node);
        if (IsNext(DialogTokenType.Name))
        {
            node.name = SkipToken().value;
        }
        if(IsNext(DialogTokenType.String))
        {
            node.text = SkipToken().value;
            if (inOption)
            {
                return;
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
    }

    private void EmitNode(DialogNode node)
    {
        foreach (DialogNode endNode in context!.endNodes)
        {
            endNode.next = node;
        }
        context.endNodes.Clear();
        context.endNodes.Add(node);
        if (context.firstNode == null)
        {
            context.firstNode = node;
        }
        foreach (string labelName in context.labelNames)
        {
            labels.Add(labelName, node);
        }
        context.labelNames.Clear();
    }

    private void BeginContext()
    {
        if (context != null)
        {
            contexts.Push(context);
        }
        context = new Context();
    }

    private DialogNode? EndContext()
    {
        DialogNode? result = context!.firstNode;
        if (contexts.Count > 0)
        {
            Context nextContext = contexts.Pop();
            nextContext.endNodes.AddRange(context.endNodes);
            nextContext.labelNames.AddRange(context.labelNames);
            context = nextContext;
        } else
        {
            context = null;
        }
        return result;
    }

    private void EmitExit()
    {
        foreach (string labelName in context!.labelNames)
        {
            labels.Add(labelName, null);
        }
        context.labelNames.Clear();
        context.endNodes.Clear();
    }

    private void EmitLabel(string labelName)
    {
        context!.labelNames.Add(labelName);
    }

    private void EmitGoto(string labelName)
    {
        foreach (DialogNode endNode in context!.endNodes)
        {
            gotos.Add(endNode, labelName);
        }
        EmitNode(new DialogGoToNode(labelName));
    }

    private void ParseSequence(bool inline)
    {
        List<string> labelNames = new List<string>();
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
            else
            {
                ParseNode(false);
            }
        }
    }

    private void ParseNode(bool inOption)
    {
        if(IsNext(DialogTokenType.NewLine))
        {
            SkipToken();
        }
        else if (IsNext(DialogTokenType.Name) || IsNext(DialogTokenType.String)
            || IsNext(DialogTokenType.Option))
        {
            ParseChoice(inOption);
        }
        else if (IsNext(DialogTokenType.Goto))
        {
            SkipToken();
            EmitGoto(Expect(DialogTokenType.Id).value);
        }
        else if (IsNext(DialogTokenType.Id))
        {
            EmitLabel(SkipToken().value);
            Expect(DialogTokenType.Label);
            while (IsNext(DialogTokenType.NewLine))
            {
                SkipToken();
            }
        }
        else if (IsNext(DialogTokenType.BeginGroup))
        {
            SkipToken();
            ParseSequence(false);
            SkipToken();
        }
        else if(IsNext(DialogTokenType.Exit))
        {
            SkipToken();
            EmitExit();
        }
        else
        {
            Unexpected();
        }
    }

    private void LinkGotos()
    {
        foreach (KeyValuePair<string, DialogNode?> label in labels)
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

    public static DialogNode? Parse(string input)
    {
        DialogParser parser = new DialogParser(input);
        parser.BeginContext();
        parser.ParseSequence(false);
        DialogNode? result = parser.EndContext();
        parser.LinkGotos();
        return result;
    }
}