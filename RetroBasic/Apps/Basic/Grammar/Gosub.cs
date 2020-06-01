using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Gosub : ICommand
    {
        private readonly Expr expr;

        private Gosub(Expr expr)
        {
            this.expr = expr;
        }

        public const string Keyword = "gosub";

        public static Gosub Parse(Input input)
        {
            return NewMethod(input);
        }

        private static Gosub NewMethod(Input input)
        {
            
                if (input.Pop(new List<string> { Keyword }) == null)
                {
                    return null;
                }
                var expr = Expr.Parse(input);
                if (expr == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("expression"));
                    return null;
                }
                return new Gosub(expr);
            
        }

        public void ExecuteIn(Vm vm)
        {
            var lineNumber = expr.EvalInt(vm);
            vm.Gosub(lineNumber);
        }
    }
}