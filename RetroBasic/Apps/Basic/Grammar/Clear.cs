using System.IO;

namespace RetroBasic.Grammar
{
    public class Clear : ICommand
    {
        public const string Keyword = "clear";

        public static Clear Parse(Input input)
        {
            return input.ArglessSymbol<Clear>(Keyword, new Clear());
        }

        public void ExecuteIn(Vm vm)
        {
            vm.Clear();
        }
    }
}