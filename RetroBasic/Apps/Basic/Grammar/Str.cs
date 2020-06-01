using System;
using System.Collections.Generic;

namespace RetroBasic.Grammar
{
    public class Str
    {
        private readonly string body;

        private Str(string body)
        {
            this.body = body;
        }

        public static Str Parse(Input input)
        {
            return NewMethod(input);
        }

        private static Str NewMethod(Input input)
        {
            
                var opening = input.Pop(new List<string> { "\"" });
                if (opening == null)
                {
                    return null;
                }
                var s = "";
                var bodyChar = default(String);
                while ((bodyChar = input.Pop()) != "\"")
                {
                    s += bodyChar;
                }
                return new Str(s);
            
        }

        public string EvalString(Vm vm)
        {
            return body;
        }
    }
}