using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class If : ICommand
    {
        private readonly Expr lhExpr;
        private readonly Relop relop;
        private readonly Expr rhExpr;
        private readonly Statement statement;

        private If(Expr lhExpr, Relop relop, Expr rhExpr, Statement statement)
        {
            this.lhExpr = lhExpr;
            this.relop = relop;
            this.rhExpr = rhExpr;
            this.statement = statement;
        }

        public const string KeywordIf = "if";
        private const string KeywordThen = "then";

        public static If Parse(Input input)
        {
            return NewMethod(input);
        }

        private static If NewMethod(Input input)
        {
            
                var cmd = input.Pop(new List<string> { KeywordIf });
                if (cmd == null)
                {
                    return null;
                }
                var lhExpr = Expr.Parse(input);
                if (lhExpr == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("expression"));
                    return null;
                }
                var relop = Relop.Parse(input);
                if (relop == null)
                {
                    Console.WriteLine(string.Join("", MessageFormatter.Expected(Relop.Strings)));
                    return null;
                }
                var rhExpr = Expr.Parse(input);
                if (rhExpr == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("expression"));
                    return null;
                }
                var then = input.Pop(new List<string> { KeywordThen });
                if (then == null)
                {
                    Console.WriteLine(MessageFormatter.Expected(KeywordThen));
                    return null;
                }
                var statement = Statement.Parse(input);
                if (statement == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("statement"));
                    return null;
                }
                return new If(lhExpr, relop, rhExpr, statement);
            
        }

        public void ExecuteIn(Vm vm)
        {
            var lhs = lhExpr.EvalInt(vm);
            RelOp relOp;
            if(relop.s == ">")
            {
                relOp = new GT();
            } else if(relop.s == ">=")
            {
                relOp = new GTE();
            }
            else if (relop.s == "<")
            {
                relOp = new LT();
            }
            else if (relop.s == "<=")
            {
                relOp = new LTE();
            }
            else if (relop.s == "<>")
            {
                relOp = new NEQ();
            }
            else
            {
                relOp = new EQ();
            }
            var rhs = rhExpr.EvalInt(vm);
            var pass = relOp.Execute(lhs, rhs);
            if (pass)
            {
                statement.Command.ExecuteIn(vm);
            }
        }
    }
}