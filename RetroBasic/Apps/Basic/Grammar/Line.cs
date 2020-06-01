using System;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Line
    {
        public readonly Number Number;
        public readonly Statement Statement;
        private readonly string rawUserInput;

        private Line(Number number, Statement statement, string rawUserInput)
        {
            Number = number;
            Statement = statement;
            this.rawUserInput = rawUserInput;
        }

        public static Line Parse(Input input)
        {
            return NewMethod(input);
        }

        private static Line NewMethod(Input input)
        {
           
                var number = Number.Parse(input);
                var statement = Statement.Parse(input);
                if (statement == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("statement"));
                    return null;
                }
                var remainder = input.Pop(length: Int32.MaxValue).Trim();

                if (remainder.Length != 0)
                {
                    Console.WriteLine(MessageFormatter.Expected($"eol (line was \"{input.Body}\")"));
                    return null;
                }
                return new Line(number, statement, input.Body);
            
        }

        public override string ToString()
        {
            return rawUserInput;
        }
    }
}