using System.IO;

namespace RetroBasic.Grammar
{
    public class End : ICommand
    {
        public const string Keyword = "end";

        public static End Parse(Input input)
        {
            return input.ArglessSymbol<End>(Keyword, new End());
        }

        public void ExecuteIn(Vm vm)
        {
            vm.End();
        }
    }
}