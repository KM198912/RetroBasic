using System.Collections.Generic;

namespace RetroBasic.Grammar
{
    public class Relop
    {
        public readonly string s;

        private Relop(string s)
        {
            this.s = s;
        }

        public static readonly List<string> Strings = new List<string> {"<>", "<=", ">=", "<", ">", "="};

        public static Relop Parse(Input input)
        {
            var s = input.Pop(Strings);
            return s != null ? new Relop(s) : null;
        }
    }
}