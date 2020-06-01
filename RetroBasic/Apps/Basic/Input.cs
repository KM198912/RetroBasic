using System;
using System.Collections.Generic;

namespace RetroBasic
{
    public class Input
    {
        public readonly string Body;
        public int Index
        {
            get => index; private set
            {
                index = value;
            }
        }
        public Input(string body)
        {
            Body = body;
        }

        public string Peek(int length = 1)
        {
            return Body.Substring(Index, Math.Min(length, Body.Length - Index));
        }

        public string Pop(int length = 1)
        {
            var result = Peek(length);
            Index += result.Length;
            return result;
        }

        private readonly Stack<int> rewindStack = new Stack<int>();
        private int index = 0;

        public void Mark()
        {
            rewindStack.Push(Index);
        }

        public void RewindToMark()
        {
            Index = rewindStack.Pop();
        }

        public void CommitMark()
        {
            rewindStack.Pop();
        }

        public void FastForward()
        {
            Index = Body.Length;
        }
    }
}