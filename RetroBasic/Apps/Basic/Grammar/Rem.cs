using System.IO;

namespace RetroBasic.Grammar
{
    public class Rem : ICommand
    {
        public const string Keyword = "rem";

        public static Rem Parse(Input input)
        {
            var rem = input.ArglessSymbol<Rem>(Keyword, new Rem());
            input.FastForward();
            return rem;
        }

        public void ExecuteIn(Vm vm) {}
    }
}