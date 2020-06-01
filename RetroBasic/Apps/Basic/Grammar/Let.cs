using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Let : ICommand
    {
        private readonly Var var;
        private readonly Expr expr;

        private Let(Var var, Expr expr)
        {
            this.var = var;
            this.expr = expr;
        }

        public const string Keyword = "let";

        public static Let Parse(Input input)
        {
            return NewMethod(input);
        }

        private static Let NewMethod(Input input)
        {
            
                if (input.Pop(new List<string> { Keyword }) == null)
                {
                    return null;
                }
                var var = Var.Parse(input);
                if (var == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("var"));
                    return null;
                }
                if (input.Pop(new List<string> { "=" }) == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("="));
                    return null;
                }
                var expr = Expr.Parse(input);
                if (expr == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("expression"));
                    return null;
                }
                return new Let(var, expr);
            
        }

        public void ExecuteIn(Vm vm)
        {
            vm[var.Name] = expr.EvalInt(vm);
        }
    }
}