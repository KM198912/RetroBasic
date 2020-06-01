using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class TermCont
    {
        public readonly string Op;
        public readonly Factor Factor;

        private TermCont(string op, Factor factor)
        {
            Op = op;
            Factor = factor;
        }

        public static TermCont Parse(Input input)
        {
            return NewMethod(input);
        }

        private static TermCont NewMethod(Input input)
        {
            
                var op = input.Pop(new List<string> { "*", "/" });
                if (op == null)
                {
                    return null;
                }
                var factor = Factor.Parse(input);
                return factor == null ? null : new TermCont(op, factor);
            
        }
    }
}