using Compiler.Source.Lib;
using System.Linq;
using System;

namespace Compiler.Source.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(ExpressionSyntax[] roots, SyntaxToken endOfFileToken, DiagnosticBag diagnostics)
        {
            Roots = roots;
            EndOfFileToken = endOfFileToken;
            Diagnostics = diagnostics;
        }

        public ExpressionSyntax[] Roots { get; }
        public SyntaxToken EndOfFileToken { get; }
        public DiagnosticBag Diagnostics { get; }

        public void Log()
        {
            foreach(var root in Roots)
            {
                PrettyPrint(root);
            }
        }

        private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Type);

            if (node is SyntaxToken st)
            {
                if (st.Value != null)
                {
                    Console.Write(" ");
                    Console.Write(st.Value);
                }
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}
