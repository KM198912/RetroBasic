using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class ExprCont
    {
        public readonly string Op;
        public readonly Term Term;

        private ExprCont(string op, Term term)
        {
            this.Op = op;
            this.Term = term;
        }

        public static ExprCont Parse(Input input)
        {
            return NewMethod(input);
        }

        private static ExprCont NewMethod(Input input)
        {
            
                var op = input.Pop(new List<string> { "+", "-" });
                if (op == null)
                {
                    return null;
                }
                var term = Term.Parse(input);
                return term == null ? null : new ExprCont(op, term);
            
        }
    }
}