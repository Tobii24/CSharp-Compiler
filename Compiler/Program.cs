using System;
using System.Linq;
using Compiler.Source;
using Compiler.Source.Syntax;
using Compiler.Source.Lib;

namespace Compiler
{
    internal class Program
    {
        static void Main()
        {
            var contxt = new Context();
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