using Cosmos.Debug.Kernel;
using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Goto : ICommand
    {
        private readonly Expr expr;

        private Goto(Expr expr)
        {
            this.expr = expr;
        }

        public const string Keyword = "goto";

        public static Goto Parse(Input input)
        {
            return NewMethod(input);
        }

        private static Goto NewMethod(Input input)
        {
            var cmd = input.Pop(new List<string> { Keyword });
            if (cmd == null)
            {
                return null;
            }
            var expr = Expr.Parse(input);
            if (expr == null)
            {
                Console.WriteLine(MessageFormatter.Expected("expression"));
                return null;
            }
            return new Goto(expr);
            
        }

        public void ExecuteIn(Vm vm)
        {
            var lineNumber = expr.EvalInt(vm);
            vm.Goto(lineNumber);
        }
    }
}