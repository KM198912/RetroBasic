using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Expr
    {
        private readonly string sign;
        private readonly Term term;
        private readonly List<ExprCont> cont;

        private Expr(string sign, Term term, List<ExprCont> cont)
        {
            this.sign = sign;
            this.term = term;
            this.cont = cont;
        }

        public static Expr Parse(Input input)
        {
            return input.RewindOnNull(NewMethod(input));
        }

        private static Expr NewMethod(Input input)
        {
            var sign = input.Pop(new List<string> { "+", "-" });
            var term = Term.Parse(input);
            var items = new List<ExprCont>();
            ExprCont item = null;
            while ((item = ExprCont.Parse(input)) != null)
            {
                items.Add(item);
            }
            return term == null ? null : new Expr(sign, term, items);
        }
        

        public string EvalString(Vm vm)
        {
            return EvalInt(vm).ToString();
        }

        public int EvalInt(Vm vm)
        {
            var m = sign == "-" ? -1 : 1;
            var lhs = m*term.EvalInt(vm);
            foreach (var exprCont in cont)
            {
                IntOp intOp;
                if(exprCont.Op == "+")
                {
                    intOp = new Add();
                } else if(exprCont.Op == "-")
                {
                    intOp = new Sub();
                }
                else if (exprCont.Op == "*")
                {
                    intOp = new Mult();
                }
                else
                {
                    intOp = new Div();
                }
                var rhs = exprCont.Term.EvalInt(vm);
                lhs = intOp.Execute(lhs, rhs);
            }
            return lhs;
        }
    }
}