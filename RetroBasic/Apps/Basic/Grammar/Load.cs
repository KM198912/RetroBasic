using System.IO;

namespace RetroBasic.Grammar
{
    public class Load : ICommand
    {
        private readonly ExprList exprList;

        private Load(ExprList exprList)
        {
            this.exprList = exprList;
        }

        public const string Keyword = "load";

        public static Load Parse(Input input)
        {
            var exprList = input.ParseKeywordThenExprList(Keyword);
            return exprList == null ? null : new Load(exprList);
        }

        public void ExecuteIn(Vm vm)
        {
            var s = exprList.EvalString(vm);
            vm.Load(s);
        }
    }
}