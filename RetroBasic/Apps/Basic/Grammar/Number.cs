using System;
using System.Collections.Generic;


namespace RetroBasic.Grammar
{
    public class Number
    {
        public readonly List<Digit> Digits;

        private Number(List<Digit> digits)
        {
            Digits = digits;
        }

        public static Number Parse(Input input)
        {
            var digits = new List<Digit>();
            var digit = default(Digit);
            while ((digit = Digit.Parse(input)) != null)
            {
                digits.Add(digit);
            }
            return digits.Count > 0 ? new Number(digits) : null;
        }

        public int EvalInt(Vm vm)
        {
            string value = "";
            foreach (var item in Digits)
            {
                value += item.Body;
            }
            return int.Parse(value);
        }
    }
}