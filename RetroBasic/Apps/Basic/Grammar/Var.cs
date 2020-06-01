using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class Var
    {
        public readonly string Name;

        private Var(string name)
        {
            Name = name;
        }

        public static Var Parse(Input input)
        {
            var s = input.Pop(new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" });
            return s != null ? new Var(s) : null;
        }

        public int EvalInt(Vm vm)
        {
            return vm[Name];
        }
    }
}