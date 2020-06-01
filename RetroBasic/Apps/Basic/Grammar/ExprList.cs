using System.Collections.Generic;
using System.IO;


namespace RetroBasic.Grammar
{
    public class ExprList
    {
        private readonly List<ExprListItem> exprListItems;

        private ExprList(List<ExprListItem> exprListItems)
        {
            this.exprListItems = exprListItems;
        }

        public static ExprList Parse(Input input)
        {
            var items = new List<ExprListItem>();
            ExprListItem item = null;
            while ((item = ExprListItem.Parse(input)) != null)
            {
                items.Add(item);
            }
            return items.Count > 0 ? new ExprList(items) : null;
        }

        public string EvalString(Vm vm)
        {
            string value = "";
            foreach (var item in exprListItems)
            {
                value += item.EvalString(vm);
            }
            return value;
        }
    }
}