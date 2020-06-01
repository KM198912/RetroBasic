using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RetroBasic.Grammar;

namespace RetroBasic
{
    public class Vm
    {
        List<string> input;

        private readonly Dictionary<int, Line> program = new Dictionary<int, Line>();

        private readonly IDictionary<string, int> variables = new Dictionary<string, int>();

        private int programCounter;
        private bool running;

        private readonly Stack<int> subStack = new Stack<int>();

        public Vm(List<string> input)
        {
            this.input = input;
        }

        public void Start()
        {
            
            Ready();
            ReadLines(input);
        }

        private void ReadLines(List<string> lines)
        {
            int currentLine = 0;
            do
            {
                string readLine = lines[currentLine];
                var trimmedLine = readLine.Trim();

                if (trimmedLine.Length == 0)
                {
                    continue;
                }

                var input = new Input(trimmedLine);
                var line = Line.Parse(input);
                if (line == null)
                {
                    continue;
                }
                if (line.Number != null)
                {
                    var lineNumber = Evaluate(line.Number);
                    if (program.ContainsKey(lineNumber))
                        program.Remove(lineNumber);
                    program.Add(lineNumber, line);
                }
                else
                {
                    Execute(line.Statement.Command);
                    Ready();
                }
            } while (++currentLine < lines.Count);
        }

        private void Execute(ICommand command)
        {
            command.ExecuteIn(this);
        }

        private static int Evaluate(Number n)
        {
            string toParse = "";
            foreach (var item in n.Digits)
            {
                toParse += item.Body;
            }
            return int.Parse(toParse);
        }

        public int this[string variableName]
        {
            get
            {
                var result = 0;
                return variables.TryGetValue(variableName.ToLowerInvariant(), out result) ? result : 0;
            }
            set { variables[variableName.ToLowerInvariant()] = value; }
        }

        public void Goto(int lineNumber)
        {
            var indexOfKey = -1;
            int c = 0;
            List<int> lines = new List<int>();
            foreach (var key in program.Keys)
            {
                lines.Add(key);
            }
            lines = MergeSort(lines);
            foreach (int key in lines)
            {
                if (key == lineNumber)
                {
                    indexOfKey = c;
                    break;
                }
                c++;
            }
            if (indexOfKey == -1)
            {
                End();
                Console.WriteLine(MessageFormatter.Error($"no such line {lineNumber}", LineNumberOfProgramCounter()));
            }
            else
            {
                programCounter = indexOfKey;
                if (!running)
                {
                    Run();
                }
            }
        }

        private int? LineNumberOfProgramCounter()
        {
            return programCounter < program.Count ? GetLineNumber() : default(int?);
        }

        public void Run()
        {
            if (running)
            {
                programCounter = 0;
            }
            running = true;

            while (programCounter < program.Count && running)
            {
                int line = GetLineNumber();
                programCounter += 1;
                program[line].Statement.Command.ExecuteIn(this);
            }

            End();
        }

        private int GetLineNumber()
        {
            List<int> lines = new List<int>();
            foreach (var key in program.Keys)
            {
                lines.Add(key);
            }
            lines = MergeSort(lines);
            var line = lines[programCounter];
            return line;
        }

        public void Gosub(int lineNumber)
        {
            subStack.Push(programCounter);
            Goto(lineNumber);
        }

        public void Return()
        {
            programCounter = subStack.Pop();
        }

        public void Clear()
        {
            End();
            program.Clear();
        }

        public void List()
        {
            WriteProgram();
        }

        private void WriteProgram()
        {
            foreach (var line in program.Values)
            {
                Console.WriteLine(line.ToString());
            }
        }

        public void End()
        {
            programCounter = 0;
            running = false;
        }

        private void Ready()
        {
            
        }

        public void Write(string s, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(s);
            }
            else
            {
                Console.Write(s);
            }
        }

        public string ReadLine()
        {
            return Console.ReadLine(); //not sure about this
        }

        public void Save(string filepath)
        {
            List<string> lines = new List<string>();
            foreach (var value in program.Values)
            {
                lines.Add(value.ToString());
            }
            try
            {
                File.WriteAllLines(filepath, lines.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(MessageFormatter.Error(ex.Message, null));
            }
        }

        public void Load(string filepath)
        {
            Clear();

            try
            {
                var array = File.ReadAllLines(filepath);
                List<string> lines = new List<string>();
                for (int i = 0; i < array.Length; i++)
                {
                    lines.Add(array[i]);
                }
                ReadLines(lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine(MessageFormatter.Error(ex.Message, null));
            }
        }
        internal List<string> GetLines()
        {
            List<string> lines = new List<string>();
            foreach (var value in program.Values)
            {
                lines.Add(value.ToString());
            }
            return lines;
        }
        private readonly Random random = new Random();

        public int Random(int upperLimit)
        {
            return random.Next(upperLimit);
        }

        //Copied from https://www.w3resource.com/csharp-exercises/searching-and-sorting-algorithm/searching-and-sorting-algorithm-exercise-7.php
        private static List<int> MergeSort(List<int> unsorted)
        {
            if (unsorted.Count <= 1)
                return unsorted;

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            int middle = unsorted.Count / 2;
            for (int i = 0; i < middle; i++)  //Dividing the unsorted list
            {
                left.Add(unsorted[i]);
            }
            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }

        private static List<int> Merge(List<int> left, List<int> right)
        {
            List<int> result = new List<int>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left[0] <= right[0])  //Comparing First two elements to see which is smaller
                    {
                        result.Add(left[0]);
                        left.RemoveAt(0);      //Rest of the list minus the first element
                    }
                    else
                    {
                        result.Add(right[0]);
                        right.RemoveAt(0);
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }
                else if (right.Count > 0)
                {
                    result.Add(right[0]);

                    right.Remove(right[0]);
                }
            }
            return result;
        }
    }
}