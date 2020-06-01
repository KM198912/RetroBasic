using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.Drawing;
using RetroBasic.Apps.MainConsole;
using RetroBasic.Apps.Utils;
using Cosmos.System.FileSystem;
using System.IO;
using RetroBasic.Grammar;
using System.Runtime.InteropServices.ComTypes;
using Cosmos.System.Graphics;
using ahci = Cosmos.HAL.BlockDevice.AHCI;
using Cosmos.Core.IOGroup;
using System.Linq;

namespace RetroBasic
{
    public class Kernel : Sys.Kernel
    {
        MainConsole m_log = new MainConsole();
        public static string current_directory = @"0:\";
        public static string file;
        public static bool FileSystemEnabled;
        public static string system_email = "RetroBasic@online.de";
        public static CosmosVFS vFS = new CosmosVFS();
     //   public static Canvas canvas;
        public static BasicBufferScreen canvas;
        public static String font = "NgQDEAAAAAAAZjxmZmY8ZgAAAAAAABgYGBgYAAAYGBgYGAAAAABsbAAAAAAAAAAAAAAAAAAAAHyCmqKiopqCfAAAAAAAAAB8grqqsqqqgnwAAAAAAHwAAAAAAAAAAAAAAAAAAAAcJgwGJhwAAAAAAAAAAAAAAAAAAAAYPDwYAAAAAAAAAAwYMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABgYMAAAGDgYGBg8AAAAAAAAAAAAAGCQIBKWbBgwcNSUHgQEADAYAHzGxsb+xsbGxgAAAAAYMAB8xsbG/sbGxsYAAAAAOGwAfMbGxv7GxsbGAAAAADJMAHzGxsb+xsbGxgAAAAAwGAB+wMDA+MDAwH4AAAAAOGwAfsDAwPjAwMB+AAAAAGxsAH7AwMD4wMDAfgAAAAAwGAB+GBgYGBgYGH4AAAAAAAB+1tbWdhYWFhYWAAAAAAA8ZmAwPGZmZmY8DAZmPAAMGAB+GBgYGBgYGH4AAAAAOGwAfhgYGBgYGBh+AAAAAGZmAH4YGBgYGBgYfgAAAAAAAHxmZmb2ZmZmZnwAAAAAMBgAfMbGxsbGxsZ8AAAAABgwAHzGxsbGxsbGfAAAAAA4bAB8xsbGxsbGxnwAAAAAMkwAfMbGxsbGxsZ8AAAAAAAAAAAAxmw4OGzGAAAAAAAAAnzGzs7W1ubmxnyAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGBgYGBgYGAAYGAAAAAAAZmZmZgAAAAAAAAAAAAAAAABsbP5sbGxs/mxsAAAAAAAQftDQ0HwWFhYW/BAAAAAAAAZmbAwYGDA2ZmAAAAAAAAA4bGxsOHDazMx6AAAAAAAYGBgYAAAAAAAAAAAAAAAADhgwMGBgYGBgYDAwGA4AAHAYDAwGBgYGBgYMDBhwAAAAAABmPBj/GDxmAAAAAAAAAAAAABgYfhgYAAAAAAAAAAAAAAAAAAAAABgYMAAAAAAAAAAAAAB+AAAAAAAAAAAAAAAAAAAAAAAAGBgAAAAAAAMDBgYMDBgYMDBgYMDAAAAAfMbGzt725sbGfAAAAAAAABg4eFgYGBgYGH4AAAAAAAB8xgYGDBgwYMb+AAAAAAAAfMYGBjwGBgbGfAAAAAAAAMDAzMzMzP4MDAwAAAAAAAD+xsDA/AYGBsZ8AAAAAAAAfMbAwPzGxsbGfAAAAAAAAP7GBgYMGDAwMDAAAAAAAAB8xsbGfMbGxsZ8AAAAAAAAfMbGxsZ+BgbGfAAAAAAAAAAAABgYAAAAGBgAAAAAAAAAAAAYGAAAABgYMAAAAAAABgwYMGBgMBgMBgAAAAAAAAAAAH4AAH4AAAAAAAAAAABgMBgMBgYMGDBgAAAAAAAAfMYGDBgwMAAwMAAAAAAAAAB8wtra2trewHwAAAAAAAB8xsbG/sbGxsbGAAAAAAAA/MbGxvzGxsbG/AAAAAAAAH7AwMDAwMDAwH4AAAAAAAD8xsbGxsbGxsb8AAAAAAAAfsDAwPjAwMDAfgAAAAAAAH7AwMD4wMDAwMAAAAAAAAB+wMDA3sbGxsZ+AAAAAAAAxsbGxv7GxsbGxgAAAAAAAH4YGBgYGBgYGH4AAAAAAAB+GBgYGBgYGBjwAAAAAAAAxsbGzPjMxsbGxgAAAAAAAMDAwMDAwMDAwH4AAAAAAADG7v7WxsbGxsbGAAAAAAAAxsbm5tbWzs7GxgAAAAAAAHzGxsbGxsbGxnwAAAAAAAD8xsbG/MDAwMDAAAAAAAAAfMbGxsbGxtbWfBgMAAAAAPzGxsb8xsbGxsYAAAAAAAB+wMDAfAYGBgb8AAAAAAAA/xgYGBgYGBgYGAAAAAAAAMbGxsbGxsbGxn4AAAAAAADGxsbGxsbGbDgQAAAAAAAAxsbGxsbG1v7uxgAAAAAAAMbGxmw4bMbGxsYAAAAAAADGxsbGfgYGBgb8AAAAAAAA/gYGDBgwYMDA/gAAAAAAPjAwMDAwMDAwMDAwMD4AAMDAYGAwMBgYDAwGBgMDAAB8DAwMDAwMDAwMDAwMfAAAEDhsxgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AAAwGAwAAAAAAAAAAAAAAAAAAAAAAHwGfsbGxn4AAAAAAADAwMD8xsbGxsb8AAAAAAAAAAAAfsDAwMDAfgAAAAAAAAYGBn7GxsbGxn4AAAAAAAAAAAB+xsb+wMB+AAAAAAAAHjAwMHwwMDAwMAAAAAAAAAAAAH7GxsbGxnwGBvwAAADAwMD8xsbGxsbGAAAAAAAAGBgAOBgYGBgYHAAAAAAAABgYABgYGBgYGBgYGHAAAADAwMDM2PDw2MzGAAAAAAAAMDAwMDAwMDAwHAAAAAAAAAAAAOzW1tbWxsYAAAAAAAAAAAD8xsbGxsbGAAAAAAAAAAAAfMbGxsbGfAAAAAAAAAAA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VeKUjiSCVtuYX5ws+UQXtzjn2d2u5mEGlQCxi14BgMHYXIe/6/X7IJxs4m0ebagyG1giOo8W6NeZagr5W8br84fNR1dvSdEJ7FmaStMsw6kocGEkHopjjSNqL9KGcJmapAD//6YA//+oAP//qQD//64A//+vAMkC//+zAP//IiDPJf//tAD//7gA//+5AP//vgD//8AA///BAP//wgD//8MA///IAP//ygD//8sAAQT//8wA//+2AP//pwD//80A///OAP//zwAHBP//0AAQAf//0gD//9MA///UAP//1QD//9cA///YAP//IACgAAAgASACIAMgBCAFIAYgByAIIAkgCiAvIP//IQD//yIA//8jAP//JAD//yUAlSL//yYA//8nAP//KAD//ykA//8qAJsi//8rAP//LAD//y0AEiATIBIirQAQIBEgliL//y4AmSL//y8AmCL//zAA6iT//zEAYCT//zIAYST//zMAYiT//zQAYyT//zUAZCT//zYAZST//zcAZiT//zgAZyT//zkAaCT//zoA//87AP//PAD//z0AnCL//z4A//8/AP//QAD//0EAEASRA7Yk//9CABIEkgO3JP//QwAhBLgk//9EALkk//9FABUElQO6JP//RgC7JP//RwC8JP//SAAdBJcDvST//0kABgSZA74k//9KAAgEvyT//0sAGgSaAyohwCT//0wAwST//00AHAScA8Ik//9OAJ0DwyT//08AHgSfA8Qk//9QACAEoQPFJP//UQDGJP//UgDHJP//UwAFBMgk//9UACIEpAPJJP//VQDKJP//VgDLJP//VwDMJP//WAAlBKcDzST//1kArgTOJP//WgCWA88k//9bAP//XAD//10A//9eAP//XwD//2AA//9hADAE0CT//2IA0ST//2MAQQTSJP//ZADTJP//ZQA1BNQk//9mANUk//9nANYk//9oANck//9pAFYE2CT//2oAWATZJP//awDaJP//bADbJP//bQDcJP//bgDdJP//bwA+BN4k//9wAEAE3yT//3EA4CT//3IA4ST//3MAVQRzAOIk//90AOMk//91AOQk//92AOUk//93AOYk//94AEUE5yT//3kAQwToJP//egDpJP//ewD//3wA//99AP//fgD//9kA///HAP///AD//+kA///iAP//5AD//+AA///lAP//5wD//+oA///rAFEE///oAP//7wBXBP//7gD//+wA///EAP//xQArIf//yQD//+YA///GAP//9AD///YA///yAP//+wD///kA////AP//1gD//9wA//+iAP//owD//6UA///aAP//2wD//+EA///tAP//8wD///oA///xAP//0QD//6oA//+6AP//vwD//90A//+sAP//vQD//7wA//+hAP//qwBqIv//uwBrIv//kSX//5Il//+TJf//AiUDJX8lfSV7JXcleSV1Jf//JCUrJSolKSUoJSclJiUlJf//YSX//2Il//9WJf//VSX//2Ml//9RJf//VyX//10l//9cJf//WyX//xAlEyUSJREl//8UJRclFiUVJf//NCU7JTolOSU4JTclNiU1Jf//LCUzJTIlMSUwJS8lLiUtJf//HCUjJSIlISUgJR8lHiUdJf//ACUBJX4lfCV6JXYleCV0Jf//PCVLJUolSSVIJUclRiVFJUQlQyVCJUElQCU/JT4lPSX//14l//9fJf//WiX//1Ql//9pJf//ZiX//2Al//9QJf//bCX//2cl//9oJf//ZCX//2Ul//9ZJf//WCX//1Il//9TJf//ayX//2ol//8YJRslGiUZJf//DCUPJQ4lDSX//4gl//+EJf//jCX//5Al//+AJf//3gD//98A///jAP//8AD///UA///4AP//tQC8A////QD///4A//8AAf//AQH//wIB0AT//wMB0QT//wQB//8FAf//BgH//wcB//+xAP//CAH//wkB//8KAf//CwH///cA//8MAf//sAD//w0B//+3AP//DgH//w8B//+yAP//EQH//xIB//8TAf//FgH//xcB//8YAf//GQH//xoB//8bAf//HAH//x0B//8eAf//HwH//yAB//8hAf//IgH//yMB//8kAf//JQH//yYB//8nAf//KAH//ykB//8qAf//KwH//y4B//8vAf//MAH//zEB//80Af//NQH//zYB//83Af//OAH//zkB//86Af//OwH//zwB//89Af//PgH//0EB//9CAf//QwH//0QB//9FAf//RgH//0cB//9IAf//SgH//0sB//9MAf//TQH//1AB//9RAf//UgH//1MB//9UAf//VQH//1YB//9XAf//WAH//1kB//9aAf//WwH//1wB//9dAf//XgH//18B//9gAf//YQH//2IB//9jAf//ZAH//2UB//9mAf//ZwH//2gB//9pAf//agH//2sB//9sAf//bQH//24B//9vAf//cAH//3EB//9yAf//cwH//3gB//95Af//egH//3sB//98Af//fQH//34B//8YIP//GSD//xwg//8dIP//rCD//yYg/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////w==";
            public static Cosmos.Core.ProcessorInformation cpu = new Cosmos.Core.ProcessorInformation();

        public static bool ContainsVolumes()
        {
            var vols = vFS.GetVolumes();
            foreach (var vol in vols)
            {
                return true;
            }
            return false;
        }
        public static void FormatHDD()
            {
           vFS.Format(@"0", "FAT32", true);
            }

        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        protected override void BeforeRun()
        {
            canvas = new BasicBufferScreen(new VGACanvas(new Mode(320,200,ColorDepth.ColorDepth8)),Color.Blue);
            Sys.MouseManager.ScreenHeight = 320;
            Sys.MouseManager.ScreenWidth = 200;
            
           // canvas.Render();
            if (ContainsVolumes())
            {
                FileSystemEnabled = true;
            }
            Sys.MouseManager.ScreenHeight = 200;
            Sys.MouseManager.ScreenWidth = 320;
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(vFS);
            //PCScreenFont fontz = new PCScreenFont(Convert.FromBase64String(font));
            //VGAScreen.SetFont(fontz.CreateVGAFont(), fontz.CharHeight);
               
            StartScreen();

            PS2Controller ps2 = new PS2Controller();


            if (ContainsVolumes())
            {
                m_log.InfoFormat("FILE SYSTEM", "Detected Partitions: " + vFS.GetVolumes().Count);
                int count = vFS.GetVolumes().Count;

                for (int i = 0; i < count; i++)
                {
                    string drive = i.ToString() + ":\\";
                    string label = vFS.GetFileSystemLabel(drive);
                    int UsedSpace = (int)vFS.GetTotalSize(drive) - (int)vFS.GetTotalFreeSpace(drive);
                    long intBytes = UsedSpace;
                    m_log.InfoFormat("FILE SYSTEM", "Detected Partition Label for Partition: " + drive + " is " + label);

                    m_log.InfoFormat("FILE SYSTEM", @"Total Size for Partition " 
                    + drive +
                    " is " + FormatBytes(vFS.GetTotalSize(drive)) + 
                    " Free: "+ FormatBytes(vFS.GetTotalFreeSpace(drive)) + 
                    " Used: "+ FormatBytes((long)intBytes));

                    //Console.WriteLine("Quajak Request: " + Path.GetPathRoot(drive));
                }



            }
        }

        protected override void Run()
        {
            try
            {
                bool loop = true; /// Avoid a Stupid Debugger loop
                while (loop)
                {
                    canvas.ClearBuf(Color.Green);
                   // canvas.Render();
                    int X = (int)Sys.MouseManager.X;
                    int Y = (int)Sys.MouseManager.Y;
                   // canvas.DrawFilledRectangle(new Pen(Color.Crimson), X, Y, 20, 20);
                    //     Kernel.helper.Render();
                    //canvas.Render();
                    var input = Console.ReadLine();
                    MainCommands.RunConsoleCommand(input);
                }

            }
            catch (Exception error)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                m_log.WarnFormat("SYSTEM CRASH", MainCommands.UpperCase("System Crashed with exception: " + error.Message + "."));
                m_log.WarnFormat("SYSTEM CRASH", MainCommands.UpperCase("If this keeps happening please Contact the Author of RetroBasic at " + system_email));
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                StartScreen();
            }
        }
        public static void StartScreen()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            string boot = "**** COSMOS BASIC V1 ****";
            Console.SetCursorPosition((Console.WindowWidth - boot.Length) / 2, Console.CursorTop);
            Console.WriteLine(boot);
            string memory = MemoryManager.UsedMemory().ToString() + " MB / " + MemoryManager.TotalMemory().ToString() + " MB (used / total)";
            Console.SetCursorPosition((Console.WindowWidth - memory.Length) / 2, Console.CursorTop + 1);
            Console.WriteLine(MainCommands.UpperCase(memory));
            Console.WriteLine("READY.");
        }
    }
}
