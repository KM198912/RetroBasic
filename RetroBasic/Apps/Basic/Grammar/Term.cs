using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Term
    {
        private readonly Factor factor;
        private readonly List<TermCont> cont;

        private Term(Factor factor, List<TermCont> cont)
        {
            this.factor = factor;
            this.cont = cont;
        }

        public static Term Parse(Input input)
        {
            var factor = Factor.Parse(input);
            var items = new List<TermCont>();
            TermCont item = null;
            while ((item = TermCont.Parse(input)) != null)
            {
                items.Add(item);
            }
            return factor == null ? null : new Term(factor, items);
        }

        public int EvalInt(Vm vm)
        {
            var lhs = factor.EvalInt(vm);
            foreach (var termCont in cont)
            {
                IntOp intOp;
                if (termCont.Op == "+")
                {
                    intOp = new Add();
                }
                else if (termCont.Op == "-")
                {
                    intOp = new Sub();
                }
                else if (termCont.Op == "*")
                {
                    intOp = new Mult();
                }
                else
                {
                    intOp = new Div();
                }
                var rhs = termCont.Factor.EvalInt(vm);
                lhs = intOp.Execute(lhs, rhs);
            }
            return lhs;
        }
    }
}