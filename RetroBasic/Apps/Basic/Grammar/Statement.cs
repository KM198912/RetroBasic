using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    abstract class CommandParser
    {
        public abstract ICommand Parse(Input input);
    }
    
    class PrintParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Print.Parse(input);
        }
    }

    class IfParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return If.Parse(input);
        }
    }

    class GotoParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Goto.Parse(input);
        }
    }

    class InputCmdParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return InputCmd.Parse(input);
        }
    }
    class LetParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Let.Parse(input);
        }
    }
    class GosubParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Gosub.Parse(input);
        }
    }
    class ReturnParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Return.Parse(input);
        }
    }
    class ClearParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Clear.Parse(input);
        }
    }
    class ListCmdParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return ListCmd.Parse(input);
        }
    }
    class RunParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Run.Parse(input);
        }
    }
    class EndParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return End.Parse(input);
        }
    }
    class SaveParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Save.Parse(input);
        }
    }
    class LoadParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Load.Parse(input);
        }
    }
    class RemParse : CommandParser
    {
        public override ICommand Parse(Input input)
        {
            return Rem.Parse(input);
        }
    }
    public class Statement
    {
        public readonly ICommand Command;

        private static readonly List<CommandParser> Parsers = new List<CommandParser>
        {
            new GotoParse(), new ClearParse(), new EndParse(), new GosubParse(), new IfParse(), new InputCmdParse(), new LetParse(), new ListCmdParse(), new LoadParse(), new PrintParse(), new ReturnParse(), new RunParse(), new SaveParse(), new RemParse() //Important rem MUST be last
        };

        private Statement(ICommand command)
        {
            Command = command;
        }

        public static Statement Parse(Input input)
        {
            foreach (var commandParser in Parsers)
            {
                var command = commandParser.Parse(input);
                if (command != null)
                {
                    return new Statement(command);
                }
            }
            Console.WriteLine("Input: " + input.Body + " " + input.Index);
            Console.WriteLine(MessageFormatter.Expected(new List<string> { Print.Keyword, If.KeywordIf, Goto.Keyword, InputCmd.Keyword, Let.Keyword, Gosub.Keyword,
                Return.Keyword, Clear.Keyword, ListCmd.Keyword, Run.Keyword, End.Keyword, Save.Keyword, Load.Keyword, Rem.Keyword }));
            return null;
        }
    }
}