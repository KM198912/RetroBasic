using System.IO;

namespace RetroBasic.Grammar
{
    public class Return : ICommand
    {
        public const string Keyword = "return";

        public static Return Parse(Input input)
        {
            return input.ArglessSymbol<Return>(Keyword, new Return());
        }

        public void ExecuteIn(Vm vm)
        {
            vm.Return();
        }
    }
}