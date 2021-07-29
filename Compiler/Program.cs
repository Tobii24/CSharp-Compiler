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
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                var evaluator = new Evaluator("<stdin>", line);
                var results = evaluator.Evaluate();

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
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Type);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}