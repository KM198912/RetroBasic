using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Rnd
    {
        private readonly Expr expr;

        private Rnd(Expr expr)
        {
            this.expr = expr;
        }

        private const string Keyword = "!rnd";

        public static Rnd Parse(Input input)
        {
            return NewMethod(input);
        }

        private static Rnd NewMethod(Input input)
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
                return new Rnd(expr);
            
        }

        public int EvalInt(Vm vm)
        {
            var ulim = expr.EvalInt(vm);
            return vm.Random(ulim);
        }
    }
}