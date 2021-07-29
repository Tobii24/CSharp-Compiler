namespace Compiler.Source.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(ExpressionSyntax[] roots, SyntaxToken endOfFileToken)
        {
            Roots = roots;
            EndOfFileToken = endOfFileToken;
        }

        public ExpressionSyntax[] Roots { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string filename, string text)
        {
            var parser = new Parser(filename, text);
            return parser.Parse();
        }
    }
}
