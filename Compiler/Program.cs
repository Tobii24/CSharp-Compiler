using System;
using System.Linq;
using System.IO;
using Compiler.Source;
using Compiler.Source.Syntax;
using Compiler.Source.Lib;

namespace Compiler
{
    internal class Program
    {
        static void Main()
        {
            var contxt = new Context("");

            Console.Write("> ");
            var cmd = Console.ReadLine();

            if (cmd == "help")
            {
                Console.Clear();
                Console.WriteLine(
                    "Commands: " +
                    "help (shows all available commands), " +
                    "loadFile (loads a file and try to interpret it), " +
                    "interpreter (initializes the command line interpreter)"
                );
            }
            else if (cmd == "loadFile")
            {
                //Fix the strange illegal char errors
                Console.Clear();
                Console.Write("Type your file path -> ");
                var filePath = Console.ReadLine();

                string fileText;

                try
                {
                    fileText = File.ReadAllText(filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                var evaluator = new Evaluator("<file>", fileText);
                var results = evaluator.Evaluate(new Context(""));

                var err = evaluator._diagnostics.GetIfError();
                foreach(var e in evaluator._diagnostics.Diagnostics)
                {
                    e.Throw();
                }
                if (err != null)
                {
                    err.Throw();
                }
                else
                {
                    foreach (var res in results)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(res);
                        Console.ResetColor();
                    }
                }
            }
            else if (cmd == "interpreter")
            {
                Console.Clear();
                var context = new Context("");
                _ = new InitCommandlineInterpreter(context);
            }
        }

        public class InitCommandlineInterpreter
        {
            public InitCommandlineInterpreter(Context contxt)
            {
                while (true)
                {
                    Console.Write("> ");
                    var line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        return;

                    var evaluator = new Evaluator("<stdin>", line);
                    var results = evaluator.Evaluate(contxt);

                    var err = evaluator._diagnostics.GetIfError();
                    if (err != null)
                    {
                        err.Throw();
                    }
                    else
                    {
                        foreach (var res in results)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(res);
                            Console.ResetColor();
                        }
                    }

                    contxt = evaluator._context;
                }
            }
        }
    }
}