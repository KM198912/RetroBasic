#define COSMOSDEBUG
using Cosmos.Debug.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
using RetroBasic.Grammar;

namespace RetroBasic
{
    public static class InputHelp
    {
        private static string PopSingle(this Input @this, string s)
        {
            var candidate = @this.Peek(s.Length);
            return candidate.Equals(s, StringComparison.OrdinalIgnoreCase) ? @this.Pop(s.Length) : null;
        }

        public static string Pop(this Input @this, List<string> choices)
        {
            while (@this.Peek() == " ")
            {
                @this.Pop();
            }

            foreach (var choice in choices)
            {
                var candidate = @this.PopSingle(choice);
                if (candidate != null)
                {
                    return candidate;
                }
            }
            return null;
        }

        public static T RewindOnNull<T>(this Input @this, T value)
        {
            @this.Mark();
            var result = value;
            if (result == null)
            {
                @this.RewindToMark();
            }
            else
            {
                @this.CommitMark();
            }
            return result;
        }

        public static T ArglessSymbol<T>(this Input @this, string keyword, T newobj) where T : class
        {
            string symbol = @this.Pop(new List<string> { keyword });
            return symbol == null ? null : newobj;
        }

        public static ExprList ParseKeywordThenExprList(this Input @this, string keyword)
        {
            var cmd = @this.Pop(new List<string> { keyword });
            ExprList value = null;
            if (cmd == null)
            {
                value = null;
            }
            else
            {
                ExprList exprList = ExprList.Parse(@this);
                if (exprList == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("expression-list"));
                    value = null;
                }
                else
                {
                    value = exprList;
                }
            }
            var el = @this.RewindOnNull(value);
            return el;
        }
    }
}