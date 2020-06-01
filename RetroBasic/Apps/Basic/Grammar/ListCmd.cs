using System.IO;

namespace RetroBasic.Grammar
{
    public class ListCmd : ICommand
    {
        public const string Keyword = "list";

        public static ListCmd Parse(Input inputLine)
        {
            return inputLine.ArglessSymbol<ListCmd>(Keyword, new ListCmd());
        }

        public void ExecuteIn(Vm vm)
        {
            vm.List();
        }
    }
}