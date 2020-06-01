using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RetroBasic.Apps.MainConsole
{
    class MainConsole
    {
        public MainConsole()
        {
        
        }
        public void InfoFormat(string area,string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[" + area + "] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            
        }
        public void SuccessFormat(string area, string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[" + area + "] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);

        }
        public void WarnFormat(string area, string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[" + area + "] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
        }
    }
}
