using Compiler.Source.Lib;

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

        public static SyntaxTree Parse(string filename, string text)
        {
            var parser = new Parser(filename, text);
            return parser.Parse();
        }
    }
}
