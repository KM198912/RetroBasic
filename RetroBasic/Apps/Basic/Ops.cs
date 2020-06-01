using System.Collections.Generic;

namespace RetroBasic
{
    abstract class RelOp
    {
        public abstract bool Execute(int a, int b);
    }
    class GT : RelOp
    {
        public override bool Execute(int a, int b)
        {
            return a > b;
        }
    }

    class LT : RelOp
    {
        public override bool Execute(int a, int b)
        {
            return a < b;
        }
    }

    class GTE : RelOp
    {
        public override bool Execute(int a, int b)
        {
            return a >= b;
        }
    }

    class LTE : RelOp
    {
        public override bool Execute(int a, int b)
        {
            return a <= b;
        }
    }

    class NEQ : RelOp
    {
        public override bool Execute(int a, int b)
        {
            return a != b;
        }
    }

    class EQ : RelOp
    {
        public override bool Execute(int a, int b)
        {
            return a == b;
        }
    }

    public abstract class IntOp
    {
        public abstract int Execute(int a, int b);
    }

    class Add : IntOp
    {
        public override int Execute(int a, int b)
        {
            return a + b;
        }
    }
    class Sub : IntOp
    {
        public override int Execute(int a, int b)
        {
            return a - b;
        }
    }
    class Mult : IntOp
    {
        public override int Execute(int a, int b)
        {
            return a * b;
        }
    }
    class Div : IntOp
    {
        public override int Execute(int a, int b)
        {
            return a / b;
        }
    }

}