using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RetroBasic.Apps.Basic
{
    class ExtLoad
    {
        public static void ExtLoadFile(string path)
        {
            if (File.Exists(path))
            {
                string e = Path.GetExtension(path);
                if (e == ".txt" || e == ".bas" || e == ".TXT" || e == ".BAS")
                {
                    List<string> read = new List<string>();
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader(@path);
                    while ((line = file.ReadLine()) != null)
                    {
                        read.Add(line);
                    }
                    var Basic = new RetroBasic.Vm(read); 
                    Basic.Start();
                    Basic.Run();
                }
                else
                {
                    Console.WriteLine("Can only read from '.txt' and '.bas' files");
                }
            }
            else
            {
                Console.WriteLine("Cannot find the file you are looking for!");
            }
        }
    }
}
