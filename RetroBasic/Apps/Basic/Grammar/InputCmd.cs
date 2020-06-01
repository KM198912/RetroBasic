using System;
using System.Collections.Generic;
using System.IO;

namespace RetroBasic.Grammar
{
    public class InputCmd : ICommand
    {
        private readonly VarList varList;

        private InputCmd(VarList varList)
        {
            this.varList = varList;
        }

        public const string Keyword = "input";

        public static InputCmd Parse(Input input)
        {
            return NewMethod(input);
        }

        private static InputCmd NewMethod(Input input)
        {
            
                var cmd = input.Pop(new List<string> { Keyword });
                if (cmd == null)
                {
                    return null;
                }
                var varList = VarList.Parse(input);
                if (varList == null)
                {
                    Console.WriteLine(MessageFormatter.Expected("var-list"));
                    return null;
                }
                return new InputCmd(varList);
            
        }

        public void ExecuteIn(Vm vm)
        {
            foreach (var @var in varList.Vars)
            {
                var readValue = default(int?);
                while (!readValue.HasValue)
                {
                    vm.Write("? ", newLine: false);
                    var s = vm.ReadLine();
                    var i = 0;
                    if (int.TryParse(s, out i))
                    {
                        readValue = i;
                    }
                }
                vm[@var.Name] = readValue.Value;
            }
        }
    }
}