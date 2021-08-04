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
            var cntxt = new Context("");

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return;

                var Evaluator = new Evaluator("<stdin>", line);
                Evaluator.Evaluate(cntxt, false);
            }
        }
    }
}