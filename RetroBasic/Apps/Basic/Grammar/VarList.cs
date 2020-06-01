using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class VarList
    {
        public readonly List<Var> Vars;

        private VarList(List<Var> vars)
        {
            Vars = vars;
        }

        public static VarList Parse(Input input)
        {
            var items = new List<Var>();
            Var item = null;
            while ((item = Var.Parse(input)) != null)
            {
                items.Add(item);
            }
            return items.Count > 0 ? new VarList(items) : null;
        }
    }
}