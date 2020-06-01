using Cosmos.Common;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using RetroBasic.Apps.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Sys = Cosmos.System;
namespace RetroBasic.Apps.MainConsole
{
    class MainCommands
    {

  
        public static void RunConsoleCommand(string input)
        {
            MainConsole m_log = new MainConsole();
            input = input.ToUpper();
            if (input.StartsWith("LIST "))
            {
                if (!Kernel.FileSystemEnabled)
                {
                    m_log.WarnFormat("FILE SYSTEM", "NO VALID DRIVES DETECTED, FILE COMMANDS ARE DISABLED");
                    return;
                }
                string list_file = input.Remove(0, 5);
                if(list_file == "") { list_file = ""; }
                ListDirectory.DispDirectories(Kernel.current_directory + list_file);
                ListDirectory.DispFiles(Kernel.current_directory + list_file);
                Console.WriteLine("");
            }
            else if(input == "LIST")
            {
                ListDirectory.DispDirectories(Kernel.current_directory);
                ListDirectory.DispFiles(Kernel.current_directory);
                Console.WriteLine("");
            }
            else if(input == "GUI")
            {
                try
                {
                   
                    while (true)
                    {

                        //Kernel.canvas.Clear(System.Drawing.Color.Blue);
                     //   Kernel.helper.Render();
                    }
                }
                catch(Exception e)
                {
                    m_log.WarnFormat("CRASH", e.Message);
                }
            }
            else if(input == "KBD")
            {
                Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.US_Standard());
                m_log.WarnFormat("KEYBOARD LAYOUT", "NO KEYBOARD LAYOUT PROVIDED, SETTING TO DEFAULT: EN");
            }
            else if(input.StartsWith("KBD "))
            {
                string lang = input.Remove(0, 4);
                switch(lang)
                {
                    case "DE":

                        Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.DE_Standard());
                        break;
                    case "FR":

                        Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.FR_Standard());
                        break;
                    case "TR":
                        Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.TR_StandardQ());
                        break;
                    case "EN":
                        Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.US_Standard());
                        break;
                    case "US":
                        Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.US_Standard());
                        break;
                    default:
                        m_log.SuccessFormat("KEYBOARD LAYOUT", "CANT FIND LAYOUT '" + lang + "' VALID OPTIONS ARE: DE,FR,TR,EN,US");
                    return;
                }
                m_log.SuccessFormat("KEYBOARD LAYOUT", "SETTING KEYBOARD LAYOUT TO: " + lang);
            }
            else if(input == "CALC")
            {
                Calculator.Calculator.Calc();
            }
            else if (input.StartsWith("LOAD "))
            {
                string program = input.Remove(0, 5);
                Apps.Basic.ExtLoad.ExtLoadFile(Kernel.current_directory + program);
            }
            else if(input == "COUNTFILES")
            {
                if (Directory.GetFiles(@"0:").Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[FILESYSTEM]: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Drive contains files");
                    Console.WriteLine(Directory.GetFiles(@"0:").Length);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[FILESYSTEM]: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Drive contains NO files");
                    Console.WriteLine(Directory.GetFiles(@"0:").Length);
                }
            }
            else if (input.StartsWith("READFILE "))
            {
                string file = input.Remove(0, 9);
                Console.WriteLine(File.ReadAllText(Kernel.current_directory + file));
                Console.WriteLine("");
            }
            else if(input == "MOUSETEST")
            {
                while (true)
                {
                    uint X = Cosmos.System.MouseManager.X;
                    uint Y = Cosmos.System.MouseManager.Y;
                    string test = "X = " + X.ToString() + " Y = " + Y.ToString();
                    m_log.InfoFormat("MOUSE TEST", test);
                //    Console.ReadKey();
              //      return;
                }
            }
            else if(input == "FORMAT")
            {
                Kernel.FormatHDD();
            }
            else if(input.StartsWith("RENAME "))
            {
                string file = input.Remove(0, 7);
                string[] moveFile = file.Split(" ");
                List<string> newFile = new List<string>();
                foreach (var word in moveFile)
                {
                    newFile.Add(word);
                }
                if (File.Exists(Kernel.current_directory + newFile[1]))
                {
                  
                    m_log.WarnFormat("FILE EXISTS", UpperCase("The Destination File: " + Kernel.current_directory + newFile[1] + " already Exists, please choose a different name"));
                }
                else
                {
                    File.Copy(Kernel.current_directory + newFile[0], Kernel.current_directory + newFile[1]);
                    File.Delete(Kernel.current_directory + newFile[0]);
                    Console.WriteLine(UpperCase("Renamed " + newFile[0] + " to " + newFile[1]));
                }
                }
            else if(input.StartsWith("MKDIR "))
            {
                string NewDir = input.Remove(0, 6);
                if(Directory.Exists(Kernel.current_directory+NewDir))
                {
                    m_log.WarnFormat("DIRECTORY EXISTS", UpperCase("The Directory " + NewDir + " already exist. Choose a different Name"));
                }
                else
                {
                    Directory.CreateDirectory(Kernel.current_directory + NewDir);
                }
            }
            else if(input == "PWD")
            {
                Console.WriteLine(Kernel.current_directory);
            }
            else if(input == "CHECKROOT")
            {
                Console.WriteLine(Directory.GetDirectoryRoot(Kernel.current_directory));
            }
            else if(input == "GOHOME")
            {
                string Home = Kernel.current_directory;
                Kernel.current_directory = Directory.GetDirectoryRoot(Home);
                Console.WriteLine(UpperCase("back home at: " + Kernel.current_directory));
            }
            else if (input == "LSPCI")
            {
                Apps.Utils.LsPCI.c_Lspci();
            }
            else if(input.StartsWith("EDITOR "))
            {
                m_log.WarnFormat("EDITOR", UpperCase("EDITOR does not take arguments!"));
            }
            else if (input.StartsWith("CHDIR "))
            {
                string newDir = input.Remove(0, 6);
                if (input == "..")
                {
                    string curDir = Kernel.current_directory;
                    Kernel.current_directory = Directory.GetDirectoryRoot(curDir);
                    Console.WriteLine("Current Directory: " + Kernel.current_directory);
                }        
                else if(Directory.Exists(Kernel.current_directory + newDir))
                {
                    Kernel.current_directory = Kernel.current_directory + newDir;
                    Console.WriteLine("Current Directory: " + Kernel.current_directory);
                }
            }
            else if(input.StartsWith("DELETE "))
            {
                string delFile = input.Remove(0, 7);
                if (Directory.Exists(Kernel.current_directory + delFile))
                {
                    Directory.Delete(Kernel.current_directory + delFile,true);
                    Console.WriteLine(UpperCase("Deleted Directory " + delFile));
                }
                else
                {
                    if (File.Exists(Kernel.current_directory + delFile))
                    {

                            File.Delete(Kernel.current_directory + delFile);
                            Console.WriteLine(UpperCase(Kernel.current_directory + delFile + " Deleted!"));
                          
                        
                    }
                    else
                    {
                        m_log.WarnFormat("FILE DELETE", UpperCase("The file " + Kernel.current_directory + delFile + " does not exist!"));
                    }
                }
            }
            else if(input == "CLS")
            {
                Kernel.StartScreen();
            }
            else if(input == "SNAKE")
            {
                Snake.Snake snake = new Snake.Snake();
                snake.ÎnitSnake();
            }
            else if(input == "CREDITS")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("RETROBASIC WAS MADE WITH SEVERAL OPENSOURCE SOLUTIONS");
                Console.WriteLine("MAINSYSTEM WAS BUILD USING COSMOS!");
                Console.WriteLine("URL: https://github.com/CosmosOS/Cosmos");
                Console.WriteLine("AUTHOR AND LINKS ARE LISTED BELOW:");
                Console.WriteLine("BASIC INTERPRETER:");
                Console.WriteLine("ORIGINAL AUTHOR: JMCD");
                Console.WriteLine("URL: https://github.com/jmcd/WeeBas/tree/master/WeeBas");
                Console.WriteLine("PORTED BY: QUAJAK");
                Console.WriteLine("URL:https://github.com/quajak");
                Console.WriteLine("SNAKE BY: "+ UpperCase("Denis Bartashevich"));
                Console.WriteLine("URL: https://github.com/bartashevich/Snake");
                Console.WriteLine("EDITOR BY: " + UpperCase("Denis Bartashevich"));
                Console.WriteLine("URL: https://github.com/bartashevich/MIV");
                Console.WriteLine("OTHER AUTHORS: THE DEV TEAM OF AuraOS and SartoxOS");
                Console.WriteLine("URLS:");
                Console.WriteLine("AURAOS: https://github.com/aura-systems/Aura-Operating-System");
                Console.WriteLine("SARTOXOS: https://github.com/AnErrupTion/Sartox-OS");
                Console.WriteLine("WITHOUT THOSE PROJECTS RETROBASIC WOULD NOT BE WHAT IT IS!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (input == "EDITOR")
            {

                    Apps.MIV.StartMIV();                   

            }
            else if(input == "POWEROFF")
            {
                Cosmos.System.Power.Shutdown();
            }
            else
            {
                Console.WriteLine(input);
            }


        }
        public static string UpperCase(string text)
        {
            return text.ToUpper();
        }
    }
}
