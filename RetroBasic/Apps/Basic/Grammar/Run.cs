using System.IO;

namespace RetroBasic.Grammar
{
    public class Run : ICommand
    {
        public const string Keyword = "run";

        public static Run Parse(Input input)
        {
            return input.ArglessSymbol<Run>(Keyword, new Run());
        }

        public void ExecuteIn(Vm vm)
        {
            vm.Run();
        }
    }
}